using System;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Text;

using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class register_check : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        string userid;
        User userTemp = new User();
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
            {
                Response.Redirect("Index.aspx");
            }
            else
            {
                userid = Request.QueryString["d"];

                if (checkIfCodeInTable(userid) == false)
                {
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT COUNT(one_time_password) FROM UserOTP WHERE UserId = @userid"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@userid", userid);
                                cmd.Connection = con;
                                con.Open();
                                int noRows = (int)cmd.ExecuteScalar();
                                con.Close();
                                if (noRows >= 1)
                                {
                                    Response.Redirect("Index.aspx");
                                }
                                else
                                {
                                    User user = userTemp.GetUser(int.Parse(userid));
                                    SendActivationEmail(int.Parse(userid), user.User_Email, user.User_Name);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
        }

        private void SendActivationEmail(int userId, string email, string username)
        {
            string activationCode = Guid.NewGuid().ToString();

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserActivation VALUES(@UserId, @ActivationCode, @ExpiryTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@ActivationCode", Convert.ToBase64String(encryptData(activationCode)));
                            cmd.Parameters.AddWithValue("@ExpiryTime", DateTime.Now.AddHours(24));
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                using (MailMessage mm = new MailMessage("sender@gmail.com", email))
                {
                    string paramkey = Convert.ToBase64String(Key);
                    string paramIV = Convert.ToBase64String(IV);

                    mm.Subject = "Account Activation - myShoeRack";
                    string body = "Hello " + username + ",";
                    body += "<br /><br />Please click the following link to activate your account";
                    body += "<br />Do this before 24 hours!";
                    body += "<br /><a href = '" + 
                        Request.Url.Scheme + "://" + Request.Url.Authority + 
                        "/register_activation.aspx?s=" + Server.UrlEncode(userId.ToString()) + 
                        "&a=" + Server.UrlEncode(activationCode) + 
                        "&k=" + Server.UrlEncode(paramkey) +
                        "&i=" + Server.UrlEncode(paramIV) +
                        "'>Click here to activate your account.</a>";
                    body += "<br /><br />Thanks";
                    mm.Body = body;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("thisisforprojectbro@gmail.com", "projectsarefun123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected Boolean checkIfCodeInTable (string user_id)
        {
            Boolean inTable = false;

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserActivation WHERE UserId = @userid"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@userid", user_id);
                        cmd.Connection = con;
                        con.Open();
                        int noRows = (int)cmd.ExecuteScalar();
                        con.Close();
                        if (noRows >= 1)
                        {
                            inTable = true;
                        }
                        return inTable;
                    }
                }
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

    }
}