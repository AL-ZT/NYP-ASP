using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _172026H_Lim_ZhengTing
{
    public partial class Register : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected Boolean checkDuplicateEmail(string email)
        {
            SqlConnection connection = new SqlConnection(_connStr);
            string sql = "select userId FROM Membership WHERE userId=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["passhash"] != null)
                        {
                            if (reader["passhash"] != DBNull.Value)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return false;
        }

        protected void registerSubmitBtn_Click(object sender, EventArgs e)
        {
            string registerEmail = email.Text.ToString().Trim();
            if (!checkDuplicateEmail(registerEmail))
            {
                string CreditCardNumber = creditCardNo.Text.ToString().Trim();
                string name = memberName.Text.ToString().Trim();
                string pwd = password.Text.ToString().Trim();
                string userAge = age.Text.ToString().Trim();
                //Generate random "salt"
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];
                //Fills array of bytes with a cryptographically strong sequence of random values.
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt + userAge;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                byte[] encryptedCreditCardNo = encryptData(CreditCardNumber);

                UserInsert(registerEmail, name, finalHash, salt, encryptedCreditCardNo, DateTime.Now.ToString(), userAge, IV, Key);
                Session["LoggedIn"] = registerEmail;
                string guid = Guid.NewGuid().ToString();
                Session["AuthToken"] = guid;
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                Response.Write("<script>alert('Registration is successful.')</script>");
            } 
            else
            {
                Response.Write("<script>alert('Registration is NOT successful.')</script>");
            }
            
        }

        protected void UserInsert(string email, string memberName, string passhash, string hashsalt, byte[] creditCardNo, string datetime, string age, byte[] iv, byte[] key)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Membership VALUES(@USERID, @MEMBERNAME, @HASHPASSWORD, @SALT, @CREDITCARDNO, @DATETIME, @AGE, @IV, @KEY)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@USERID", email);
                            cmd.Parameters.AddWithValue("@MEMBERNAME", memberName);
                            cmd.Parameters.AddWithValue("@HASHPASSWORD", passhash);
                            cmd.Parameters.AddWithValue("@SALT", hashsalt);
                            cmd.Parameters.AddWithValue("@CREDITCARDNO", Convert.ToBase64String(creditCardNo));
                            cmd.Parameters.AddWithValue("@DATETIME", datetime);
                            cmd.Parameters.AddWithValue("@AGE", age);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(iv));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(key));
                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery(); // Returns no. of rows affected. Must be > 0
                            conn.Close();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
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