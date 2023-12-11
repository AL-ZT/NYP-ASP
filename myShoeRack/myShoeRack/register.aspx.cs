using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class register : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        string UserId;
        string otp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    bind();
                }
            }
        }

        protected void bind()
        {
            Country aCountry = new Country();

            List<Country> cList = new List<Country>();
            cList = aCountry.getCountryAll();
            countryDropdown.DataSource = cList;
            countryDropdown.DataTextField = "country";
            countryDropdown.DataValueField = "code";
            countryDropdown.DataBind();
        }

        protected void submitBtn_Click(object sender, EventArgs e)
        {
            if (captchaValidate())
            {
                if (checkForDuplicateEmail(email.Text.ToString().Trim()) == false)
                {
                    string pass = pwd.Text.ToString().Trim();

                    //Generate random "salt"
                    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                    byte[] saltByte = new byte[8];

                    //Fills array of bytes with a cryptographically strong sequence of random values.
                    rng.GetBytes(saltByte);
                    salt = Convert.ToBase64String(saltByte);
                    SHA512Managed hashing = new SHA512Managed();
                    string passWithSalt = pass + salt;
                    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pass));
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(passWithSalt));

                    finalHash = Convert.ToBase64String(hashWithSalt);
                    RijndaelManaged cipher = new RijndaelManaged();

                    cipher.GenerateKey();

                    Key = cipher.Key;
                    IV = cipher.IV;

                    registerUser();

                    string paramkey = Convert.ToBase64String(Key);
                    string paramIV = Convert.ToBase64String(IV);

                    Response.Redirect("register_otp.aspx?i=" + UserId + 
                        "&k=" + Server.UrlEncode(paramkey) +
                        "&v=" + Server.UrlEncode(paramIV));
                }

                else
                {
                    lblmsg.Text = "Email is registered to another user. Please use another one.";
                    lblmsg.ForeColor = System.Drawing.Color.Red;
                }
            }

            else
            {
                lblmsg.Text = "Not Valid Recaptcha. Try Again!";
                lblmsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void registerUser()
        {
            int userId = 0;
            string address =
                address1.Text.Trim() + "," +
                address2.Text.Trim() + "," +
                postalCode.Text.Trim() + "," +
                city.Text.Trim() + "," +
                countryDropdown.SelectedItem.Text.Trim();
            string phoneNumber = countryCode.Text.Trim() + phoneNo.Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = 
                        new SqlCommand("INSERT INTO Users VALUES(@user_email, @user_name, @user_passhash, @user_hashsalt, @user_counter, @user_address, @admin_status, @phone_number, @enable2FA, @enableEmailAuth, @enableSMSAuth, @IV, @Key, @banned_user)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@user_email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@user_name", username.Text.Trim());
                            cmd.Parameters.AddWithValue("@user_passhash", finalHash);
                            cmd.Parameters.AddWithValue("@user_hashsalt", salt);
                            cmd.Parameters.AddWithValue("@user_counter", 0);
                            cmd.Parameters.AddWithValue("@user_address", address);
                            cmd.Parameters.AddWithValue("@admin_status", "normal");
                            cmd.Parameters.AddWithValue("@phone_number", phoneNumber);
                            cmd.Parameters.AddWithValue("@enable2FA", DBNull.Value);
                            cmd.Parameters.AddWithValue("@enableEmailAuth", DBNull.Value);
                            cmd.Parameters.AddWithValue("@enableSMSAuth", DBNull.Value);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@banned_user", 0);

                            cmd.Connection = con;

                            con.Open();
                            cmd.ExecuteNonQuery();
                            userId = getUserId(email.Text.Trim());
                            con.Close();
                            
                            sendOTP(userId);
                            UserId = userId.ToString();
                        }
                    }
                }     
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private int getUserId(string email)
        {
            int userId_temp = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd =
                        new SqlCommand("SELECT * FROM Users WHERE user_email = @user_email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@user_email", email);

                            cmd.Connection = con;

                            con.Open();
                            userId_temp = Convert.ToInt32(cmd.ExecuteScalar());
                            con.Close();

                            return userId_temp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private bool captchaValidate()
        {

            string Response = Request["g-recaptcha-response"]; //Getting Response String Appned to Post Method
            bool Valid = false;

            //Request to Google Server
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6Ld_KK0UAAAAAHyhN0__tLAy1GQjaLeSll_wb54S&response=" + Response);

            try
            {
                //Google recaptcha Responce 
                using (WebResponse wResponse = req.GetResponse())

                {

                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject data = js.Deserialize<MyObject>(jsonResponse);// Deserialize Json 

                        Valid = Convert.ToBoolean(data.success);


                    }
                }

                return Valid;

            }
            catch (WebException ex)
            {
                throw ex;
            }

        
        }

        public class MyObject
        {

            public string success { get; set; }

        }

        private void sendOTP(int userId)
        {
            // Find your Account Sid and Token at twilio.com/console
            const string accountSid = "AC7bcbf95ad93115cb8df0c945f99389e2";
            const string authToken = "35ed91aab6aaf0f0c61d8f8b4b269249";
            otp = generateOTP();

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO UserOTP VALUES(@UserId, @one_time_password, @ExpiryTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@one_time_password", Convert.ToBase64String(encryptData(otp)));
                            cmd.Parameters.AddWithValue("@ExpiryTime", DateTime.Now.AddHours(24));
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = MessageResource.Create(
                    body: "From myShoeRack,\nPlease enter the following on our website!\n" + otp,
                    from: new Twilio.Types.PhoneNumber("+18065471693"),
                    to: new Twilio.Types.PhoneNumber("+" + countryCode.Text.Trim() + phoneNo.Text.Trim())
                );

                Console.WriteLine(message.Sid);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Twilio Error {e.Code} - {e.MoreInfo}");
            }
        }

        protected string generateOTP()
        {
            // declare array string to generate random string with combination of small,capital letters and numbers
            char[] charArr = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string otp = string.Empty;
            Random objran = new Random();
            int noofcharacters = 6;
            for (int i = 0; i < noofcharacters; i++)
            {
                //It will not allow Repetation of Characters
                int pos = objran.Next(1, charArr.Length);
                if (!otp.Contains(charArr.GetValue(pos).ToString()))
                    otp += charArr.GetValue(pos);
                else
                    i--;
            }
            return otp;
        }

        private Boolean checkForDuplicateEmail(string email)
        {
            Boolean exists = false;

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE user_email = @email"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Connection = con;
                            con.Open();
                            int noRows = (int) cmd.ExecuteScalar();
                            con.Close();
                            if (noRows >= 1)
                            {
                                exists = true;
                            }
                            return exists;
                        }
                    }
                }
            }
            catch (Exception ex)
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