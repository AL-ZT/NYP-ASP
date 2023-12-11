using System;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class register_otp : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] tempOTP;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
            {
                Response.Redirect("Index.aspx");
            }

            string userid = Request.QueryString["i"];

            if (checkIfOtpInTable(userid) == false)
            {
                Response.Redirect("Index.aspx");
            }
        }

        protected void otpBtn_Click(object sender, EventArgs e)
        {
            string userid = Request.QueryString["i"];

            if (checkOtpExpired(userid) == false)
            {
                string otpDB = getOTP(userid);

                if (otpDB == otpText.Text.ToString().Trim())
                {
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM UserOTP WHERE UserId = @USERID"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@USERID", userid);
                                cmd.Connection = con;
                                con.Open();
                                int rowsAffected = cmd.ExecuteNonQuery();
                                con.Close();
                                if (rowsAffected == 1)
                                {
                                    Response.Redirect("register_check.aspx?d=" + userid);
                                }
                                else
                                {
                                    otpLbl.Text = "Invalid OTP";
                                    otpLbl.ForeColor = System.Drawing.Color.Red;
                                    //AUDIT -JW
                                    AuditLog audit = new AuditLog();
                                    audit.auditRegisterOtpFail(int.Parse(userid));
                                }
                            }
                        }
                    }
                }
                else
                {
                    otpLbl.Text = "Invalid OTP";
                    otpLbl.ForeColor = System.Drawing.Color.Red;
                    //AUDIT -JW
                    AuditLog audit = new AuditLog();
                    audit.auditRegisterOtpFail(int.Parse(userid));
                }
            }
            else
            {
                otpLbl.Text = "OTP has expired";
                otpLbl.ForeColor = System.Drawing.Color.Red;
                //AUDIT -JW
                AuditLog audit = new AuditLog();
                audit.auditRegisterOtpExpire(int.Parse(userid));
            }
        }

        protected Boolean checkOtpExpired (string user_id)
        {
            Boolean expired = false;

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ExpiryTime FROM UserOTP WHERE UserId = @userid"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@userid", user_id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (DateTime.Parse(reader["ExpiryTime"].ToString()) < DateTime.Now)
                                {
                                    expired = true;
                                }     
                            }
                            con.Close();
                            return expired;
                        }
                    }
                }
            }
        }

        protected Boolean checkIfOtpInTable (string user_id)
        {
            Boolean inTable = false;

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserOTP WHERE UserId = @USERID"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@USERID", user_id);
                        System.Diagnostics.Debug.WriteLine("LOG" + user_id);
                        cmd.Connection = con;
                        con.Open();
                        int noRows = Convert.ToInt32(cmd.ExecuteScalar());
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

        protected string getOTP(string user_id)
        {
            string paramKey = Server.UrlDecode(Request.QueryString["k"]);
            string paramIV = Server.UrlDecode(Request.QueryString["v"]);

            paramKey = paramKey.Replace(" ", "+");
            int mod1 = paramKey.Length % 4;
            if (mod1 > 0)
            {
                paramKey += new string('=', 4 - mod1);
            }

            paramIV = paramIV.Replace(" ", "+");
            int mod2 = paramIV.Length % 4;
            if (mod2 > 0)
            {
                paramIV += new string('=', 4 - mod2);
            }

            Key = Convert.FromBase64String(paramKey);
            IV = Convert.FromBase64String(paramIV);

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT one_time_password FROM UserOTP WHERE UserId = @USERID"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@USERID", user_id);
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tempOTP = Convert.FromBase64String(reader["one_time_password"].ToString());
                            }
                            con.Close();
                            return decryptData(tempOTP);
                        }
                    }
                }
            }
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            finally { }
            return plainText;
        }

    }
}