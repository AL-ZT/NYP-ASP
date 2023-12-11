using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace myShoeRack
{
    public partial class PasswordReset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string token = Request.QueryString["tk"];
                byte[] data = Convert.FromBase64String(token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                if (when < DateTime.UtcNow.AddHours(-24))
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    byte[] emaildecode = Convert.FromBase64String(Request.QueryString["email"]);
                    string emailStr = System.Text.Encoding.Default.GetString(emaildecode);
                    emailLabel.Text = emailStr;
                }
            }
            catch
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected Boolean checkCaptcha()
        {
            string response = Request["g-recaptcha-response"];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Le0zaMUAAAAAKXSw-EgPBthnfLuCDAl3jNphC3X&response=" + response);

            try
            {
                using (WebResponse wResponse = request.GetResponse())
                {
                    using (StreamReader readerStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readerStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        JObject jsonContent = JObject.Parse(jsonResponse);
                        if (jsonContent.Value<Boolean>("success") == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void passwordResetBtn_Click(object sender, EventArgs e)
        {
            if (checkCaptcha())
            {
                string pwd = newPassword.Value.ToString().Trim();

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                string salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                string finalHash = Convert.ToBase64String(hashWithSalt);
                string email = emailLabel.Text;
                string queryStr = "UPDATE Users SET" +
                 " user_passhash = @user_passhash, " +
                 "user_hashsalt = @user_hashsalt" +
                 " WHERE user_email = @user_email";
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@user_passhash", finalHash);
                cmd.Parameters.AddWithValue("@user_hashsalt", salt);
                cmd.Parameters.AddWithValue("@user_email", email);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                Response.Redirect("login.aspx");
                Response.Write("<div class='modal fade' id='successModal' tabindex='-1' role='dialog' aria-labelledby='successLabel' aria-hidden='true'><div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'><h5 class='modal-title' id='successLabel'>Reset Password Email</h5><button type='button' class='close' data-dismiss='♦modal' aria-label='Close' id='successmodalClose'><span aria-hidden='true'>&times;</span></button></div><div class='modal-body' align='center'><div class='row'><div class='col'><div class='col'><img src='https://mastertrader.com/wp-content/uploads/2017/06/check-mark-icon.png' style='height:100px;' /></div><h4>Password Has Been Reset!</h4></div></div></div></div></div></div>");
            }
            else
            {
                Response.Write("<script>alert('Please Complete the Captcha Test.');</script>");
            }
        }
    }
}