using System;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class register_activation : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        byte[] Key;
        byte[] IV;
        byte[] tempCode;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
            {
                Response.Redirect("Index.aspx");
            }
            else
            {
                if (!this.IsPostBack)
                {
                    string userid = Server.UrlDecode(Request.QueryString["s"]);

                    if (checkIfCodeInTable(userid) == true)
                    {
                        string activationCodeDB = getActivationCode(userid);
                        string activationCodeParam = Server.UrlDecode(Request.QueryString["a"]);

                        if (checkCodeExpired(activationCodeDB) == false)
                        {
                            if (activationCodeDB == activationCodeParam)
                            {
                                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                                {
                                    using (SqlCommand cmd = new SqlCommand("DELETE FROM UserActivation WHERE UserId = @USERID"))
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
                                                emailAct_msg.Text = "Activation successful. Now you can login.";
                                                //AUDIT -JW
                                                AuditLog audit = new AuditLog();
                                                audit.auditRegisterSuccess(int.Parse(userid));
                                            }
                                            else
                                            {
                                                emailAct_msg.Text = "Invalid Activation code. Please try again.";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                emailAct_msg.Text = "Invalid Activation code. Please try again.";
                            }
                        }

                        else
                        {
                            emailAct_msg.Text = "Activation code has expired.";
                            //AUDIT -JW
                            AuditLog audit = new AuditLog();
                            audit.auditRegisterEmailExpire(int.Parse(userid), activationCodeDB);
                        }
                    }
                    else
                    {
                        Response.Redirect("Index.aspx");
                    }
                }
            }
        }

        protected Boolean checkCodeExpired(string code)
        {
            Boolean expired = false;

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ExpiryTime FROM UserActivation WHERE ActivationCode = @code"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@code", code);
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

        protected string getActivationCode(string user_id)
        {
            string paramKey = Server.UrlDecode(Request.QueryString["k"]);
            string paramIV = Server.UrlDecode(Request.QueryString["i"]);

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
                using (SqlCommand cmd = new SqlCommand("SELECT ActivationCode FROM UserActivation WHERE UserId = @userid"))
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
                                tempCode = Convert.FromBase64String(reader["ActivationCode"].ToString());
                            }
                            con.Close();
                            return decryptData(tempCode);
                        }
                    }
                }
            }
        }

        protected Boolean checkIfCodeInTable(string user_id)
        {
            Boolean inTable = false;

            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(UserId) FROM UserActivation WHERE UserId = @userid"))
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