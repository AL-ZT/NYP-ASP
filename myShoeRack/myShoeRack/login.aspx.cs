using myShoeRack.App_Code;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using IpData;
using System.Threading.Tasks;
using System.Net.Http;
using Google.Authenticator;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace myShoeRack
{
    public class LoggedInUser
    {
        private int _userId = 0;
        private string _username = string.Empty;
        private string _passhash = string.Empty;
        private string _ip = string.Empty;
        private string _country = string.Empty;
        private string _city = string.Empty;
        private string _datetime = string.Empty;
        private string _flag = string.Empty;
        private string _devicename = string.Empty;

        public LoggedInUser()
        {
        }

        public LoggedInUser(int userId, string username, string passhash, string ip, string country, string city, string datetime, string flag, string devicename)
        {
            _userId = userId;
            _username = username;
            _passhash = passhash;
            _ip = ip;
            _country = country;
            _city = city;
            _datetime = datetime;
            _flag = flag;
            _devicename = devicename;
        }

        public LoggedInUser(string username, string passhash, string ip, string country, string city, string datetime, string flag, string devicename)
        {
            _username = username;
            _passhash = passhash;
            _ip = ip;
            _country = country;
            _city = city;
            _datetime = datetime;
            _flag = flag;
            _devicename = devicename;
        }

        public int User_Id
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string User_Name
        {
            get { return _username; }
            set { _username = value; }
        }
        public string Pass_Hash
        {
            get { return _passhash; }
            set { _passhash = value; }
        }
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string Date_Time
        {
            get { return _datetime; }
            set { _datetime = value; }
        }

        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        public string Device_Name
        {
            get { return _devicename; }
            set { _devicename = value; }
        }
    }

    public partial class login : System.Web.UI.Page
    {
        Adminclass adminin = new Adminclass();
        User userClass = new User();
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        const string accountSid = "ACf752021e1d086cbbd54e342846a508c0";
        const string authToken = "0bd91b35492032d87722cbdf2ceb958c";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
            {
                Response.Redirect("Index.aspx");
            }
        }

        //protected void addAccount(object sender, EventArgs e)
        //{
        //    string pwd = loginPassword.Value.ToString().Trim();

        //    //Generate random "salt"
        //    RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        //    byte[] saltByte = new byte[8];

        //    //Fills array of bytes with a cryptographically strong sequence of random values.
        //    rng.GetBytes(saltByte);
        //    salt = Convert.ToBase64String(saltByte);
        //    SHA512Managed hashing = new SHA512Managed();
        //    string pwdWithSalt = pwd + salt;
        //    byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
        //    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
        //    finalHash = Convert.ToBase64String(hashWithSalt);
        //    RijndaelManaged cipher = new RijndaelManaged();
        //    cipher.GenerateKey();
        //    Key = cipher.Key;
        //    IV = cipher.IV;

        //    //// Set a variable to the Documents path.
        //    //string docPath =
        //    //  Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //    //// Write the string array to a new file named "WriteLines.txt".
        //    //using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "PassHash.txt")))
        //    //{
        //    //    outputFile.WriteLine(finalHash);
        //    //}

        //    //using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "HashSalt.txt")))
        //    //{
        //    //    outputFile.WriteLine(salt);
        //    //}

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(MYDBConnectionString))
        //        {
        //            using (SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES(@user_email, @user_name, @user_passhash, @user_hashsalt, @user_counter, @user_address, @admin_status)"))
        //            {
        //                using (SqlDataAdapter sda = new SqlDataAdapter())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.Parameters.AddWithValue("@user_email", "lmao@email.com");
        //                    cmd.Parameters.AddWithValue("@user_name", "zhengtingl");
        //                    cmd.Parameters.AddWithValue("@user_passhash", finalHash);
        //                    cmd.Parameters.AddWithValue("@user_hashsalt", salt);
        //                    cmd.Parameters.AddWithValue("@user_counter", 0);
        //                    cmd.Parameters.AddWithValue("@user_address", "Yishun Street 32");
        //                    cmd.Parameters.AddWithValue("@admin_status", "normal");
        //                    cmd.Connection = con;
        //                    con.Open();
        //                    cmd.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //}

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select user_passhash FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["user_passhash"] != null)
                        {
                            if (reader["user_passhash"] != DBNull.Value)
                            {
                                h = reader["user_passhash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return h;
        }

        protected Boolean checkValidEmail(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select user_email FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_email"] != null)
                        {
                            if (reader["user_email"] != DBNull.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select user_hashsalt FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["user_hashsalt"] != null)
                        {
                            if (reader["user_hashsalt"] != DBNull.Value)
                            {
                                s = reader["user_hashsalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return s;
        }

        protected string get2FA(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select enable2FA FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["enable2FA"] != null)
                        {
                            if (reader["enable2FA"] != DBNull.Value)
                            {
                                s = reader["enable2FA"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return s;
        }

        protected Boolean check2FA(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select enable2FA FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["enable2FA"] != null)
                        {
                            if (reader["enable2FA"] != DBNull.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

        protected Boolean checkEmailAuth(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select enableEmailAuth FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["enableEmailAuth"] != null)
                        {
                            if (reader["enableEmailAuth"] != DBNull.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
        }

        protected Boolean checkSMSAuth(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select enableSMSAuth FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["enableSMSAuth"] != null)
                        {
                            if (reader["enableSMSAuth"] != DBNull.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return false;
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

        protected void modalLogin_Click(object sender, EventArgs e)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string strToken = get2FA(loginName.Value.ToString().Trim());
            string user_enter = login2fa.Value.ToString().Trim();
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(strToken, user_enter);
            if (isCorrectPIN == true)
            {
                RegisterAsyncTask(new PageAsyncTask(PageLoadAsync));
            }
            else
            {
                invalidCode.Text = "Invalid Code, Try Again.";
            }
        }

        protected async Task PageLoadAsync()
        {
            string n = loginName.Value.ToString().Trim();
            string dbHash = getDBHash(n);
            var client = new IpDataClient("59b6c43c2d79045ed7fb80e00cd3d782d4510e87338164214e413e97");
            var myIpInfo = await client.Lookup();
            string deviceName = HttpContext.Current.Server.MachineName;
            if (!CheckUserLastLogIn(n, dbHash))
            {
                AddLoggedInUser(n, dbHash, myIpInfo.Ip, myIpInfo.CountryName, myIpInfo.City, myIpInfo.TimeZone.CurrentTime.ToString(), myIpInfo.Flag.ToString(), deviceName);
            }
            else
            {
                LoggedInUser retrievedUser = RetrieveLastUserSession(n, dbHash);
                Response.Cookies["Location"]["Country"] = retrievedUser.Country;
                Response.Cookies["Location"]["City"] = retrievedUser.City;
                Response.Cookies["Location"]["Ip"] = retrievedUser.Ip;
                Response.Cookies["Location"]["Flag"] = retrievedUser.Flag;
                Response.Cookies["Location"]["CurrentTime"] = retrievedUser.Date_Time;
                Response.Cookies["Location"]["DeviceName"] = deviceName;
                AddLoggedInUser(n, dbHash, myIpInfo.Ip, myIpInfo.CountryName, myIpInfo.City, myIpInfo.TimeZone.CurrentTime.ToString(), myIpInfo.Flag.ToString(), deviceName);
            }

            Session["LoggedIn"] = n;
            Session["sessionId"] = getRecentSessionId(n, dbHash);
            Session["profilePic"] = "https://png.pngtree.com/svg/20170602/user_circle_1048392.png";
            if (check2FA(n))
            {
                Session["2FA"] = "Enabled";
            }
            if (checkEmailAuth(n))
            {
                Session["emailAuth"] = "Enabled";
            }
            if (checkSMSAuth(n))
            {
                Session["smsAuth"] = "Enabled";
            }
            string gUID = Guid.NewGuid().ToString();
            Session["AuthToken"] = gUID;
            DateTime currentTime = DateTime.Now;
            Response.Cookies["AuthToken"].Value = gUID;
            Response.Cookies.Add(new HttpCookie("AuthToken", gUID));

            //AUDIT -JW
            AuditLog audit = new AuditLog();
            audit.auditLoginSuccess(n);
            audit.auditResetLoginAttempt(n);

            // Logic for Checkout
            if (Session["checkoutProcess"] != null)
            {
                if (((bool)(Session["checkoutProcess"])))
                {
                    Response.Redirect("CheckoutStart.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Index.aspx", false);
            }

        }

        protected int UpdateLastUserSession(string passhash, string ip, string country, string city, string dateTime, string flag)
        {
            string queryStr = "UPDATE LoggedInUsers SET" +
             " IP = @IP, " +
             "Country = @COUNTRY, " +
             "City = @CITY, " +
             "DateTime = @DATETIME, " +
             "Flag = @FLAG" +
             " WHERE Passhash = @PASSHASH";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@IP", ip);
            cmd.Parameters.AddWithValue("@COUNTRY", country);
            cmd.Parameters.AddWithValue("@CITY", city);
            cmd.Parameters.AddWithValue("@DATETIME", dateTime);
            cmd.Parameters.AddWithValue("@FLAG", flag);
            cmd.Parameters.AddWithValue("@PASSHASH", passhash);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }

        protected void AddLoggedInUser(string name, string pass, string ip, string country, string city, string datetime, string flag, string devicename)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO LoggedInUsers VALUES(@USERNAME, @PASSHASH, @IP, @COUNTRY, @CITY, @DATETIME, @FLAG, @DEVICENAME)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@USERNAME", name);
                            cmd.Parameters.AddWithValue("@PASSHASH", pass);
                            cmd.Parameters.AddWithValue("@IP", ip);
                            cmd.Parameters.AddWithValue("@COUNTRY", country);
                            cmd.Parameters.AddWithValue("@CITY", city);
                            cmd.Parameters.AddWithValue("@DATETIME", datetime);
                            cmd.Parameters.AddWithValue("@FLAG", flag);
                            cmd.Parameters.AddWithValue("@DEVICENAME", devicename);
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
        }

        protected string getRecentSessionId(string name, string pass)
        {
            string id = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Id FROM LoggedInUsers WHERE Username=@USERNAME AND Passhash=@PASSHASH";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERNAME", name);
            command.Parameters.AddWithValue("@PASSHASH", pass);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = reader["Id"].ToString();
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected LoggedInUser RetrieveLastUserSession(string name, string pass)
        {
            LoggedInUser user = null;
            string username, passhash, ip, country, city, datetime, flag, devicename;
            int id;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Id, Username, Passhash, IP, Country, City, DateTime, Flag, DeviceName FROM LoggedInUsers WHERE Username=@USERNAME AND Passhash=@PASSHASH";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERNAME", name);
            command.Parameters.AddWithValue("@PASSHASH", pass);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = int.Parse(reader["Id"].ToString());
                        username = reader["Username"].ToString();
                        passhash = reader["Passhash"].ToString();
                        ip = reader["IP"].ToString();
                        country = reader["Country"].ToString();
                        city = reader["City"].ToString();
                        datetime = reader["DateTime"].ToString();
                        flag = reader["Flag"].ToString();
                        devicename = reader["DeviceName"].ToString();
                        user = new LoggedInUser(id, username, passhash, ip, country, city, datetime, flag, devicename);
                        return user;
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected Boolean CheckUserLastLogIn(string name, string pass)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Username, Passhash FROM LoggedInUsers WHERE Username=@USERNAME AND Passhash=@PASSHASH";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERNAME", name);
            command.Parameters.AddWithValue("@PASSHASH", pass);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            if (checkCaptcha())
            {
                string n = loginName.Value.ToString().Trim();
                string p = loginPassword.Value.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(n);
                string dbSalt = getDBSalt(n);

                if (checkValidUser(n) == true)
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = p + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);
                        if (userHash.Equals(dbHash))
                        {
                            Boolean checkban = adminin.checkban(n);
                            if (checkban == false)
                            {
                                if (check2FA(n))
                                {
                                    if (checkEmailAuth(n) && checkSMSAuth(n))
                                    {
                                        orLabel.Visible = true;
                                        smsLogin.Visible = true;
                                        otherAuthMethods.Visible = true;
                                        emailLogin.Visible = true;
                                    }
                                    else
                                    {
                                        if (checkEmailAuth(n))
                                        {
                                            orLabel.Visible = false;
                                            smsLogin.Visible = false;
                                        }
                                        else if (checkSMSAuth(n))
                                        {
                                            orLabel.Visible = false;
                                            emailLogin.Visible = false;
                                        }
                                        else
                                        {
                                            otherAuthMethods.Visible = false;
                                        }

                                    }
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "loginCheck();", true);
                                }
                                else
                                {
                                    if (checkEmailAuth(n))
                                    {
                                        if (!checkSMSAuth(n))
                                        {
                                            emailAlternativeAuth.Visible = false;
                                        }
                                        else
                                        {
                                            emailAlternativeAuth.Visible = true;
                                        }
                                        RegisterAsyncTask(new PageAsyncTask(AsyncEmailAuth));
                                    }
                                    else if (checkSMSAuth(n))
                                    {
                                        string phoneNo = getDBPhoneNo(loginName.Value.ToString().Trim());
                                        phoneNoLabel.Text = phoneNo;
                                        string randomString = RandomString(6);
                                        Response.Cookies.Add(new HttpCookie("SMSAuth", randomString));
                                        TwilioClient.Init(accountSid, authToken);
                                        try
                                        {
                                            var message = MessageResource.Create(
                                                body: "Enter the OTP for MyShoeRack Website : " + randomString,
                                                from: new Twilio.Types.PhoneNumber("+14439799177"),
                                                to: new Twilio.Types.PhoneNumber("+" + phoneNo)
                                            );

                                            Console.WriteLine(message.Sid);
                                        }
                                        catch (ApiException te)
                                        {
                                            Console.WriteLine(te.Message);
                                            Console.WriteLine($"Twilio Error {te.Code} - {te.MoreInfo}");
                                        }
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "loginCheckSMS();", true);
                                    }
                                    else
                                    {
                                        RegisterAsyncTask(new PageAsyncTask(PageLoadAsync));
                                    }
                                }
                            }
                            else
                            {
                                emailPasswordFailLabel.Text = "You are ban";
                                //AUDIT -JW
                                AuditLog audit = new AuditLog();
                                audit.bannedUserLogin(n);
                                audit.auditCountLoginAttempts(n);
                            }
                        }
                        else
                        {
                            emailPasswordFailLabel.Text = "Email/Password is invalid, Please Try Again.";
                            //AUDIT     -JW 
                            AuditLog audit = new AuditLog();
                            audit.auditLoginFail(n);
                            audit.auditCountLoginAttempts(n);
                        }
                    }
                    else
                    {
                        emailPasswordFailLabel.Text = "Email/Password is invalid, Please Try Again.";
                        //AUDIT     -JW 
                        AuditLog audit = new AuditLog();
                        audit.auditLoginFail(n);
                        audit.auditCountLoginAttempts(n);
                    }
                }
                else
                {
                    emailPasswordFailLabel.Text = "Account has not been activated yet.";
                    //AUDIT     -JW 
                    AuditLog audit = new AuditLog();
                    audit.auditLoginFail(n);
                    audit.auditCountLoginAttempts(n);
                }
            }
            else
            {
                Response.Write("<script>alert('Please Complete the Captcha Test.');</script>");
            }
        }

        protected async Task SendForgotPasswordEmail()
        {
            if (checkValidEmail(forgotPasswordEmail.Value.ToString().Trim()))
            {
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("admin@MyShoeRack.com", "MyShoeRack-Admin");
                var subject = "MyShoeRack - Forgot Password";
                var to = new EmailAddress(forgotPasswordEmail.Value.ToString().Trim(), "You");
                var plainTextContent = "";
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                var htmlContent = "<p>Hi,</p>" +
                    "<p>You have requested a change in password, and thus receiving this email. Please reset your password by clicking on the link <a href='https://localhost:44347/PasswordReset?email=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(forgotPasswordEmail.Value.ToString().Trim())) + "&tk=" + token + "'>here</a>. This URL Token expires in 24 Hours.</p>" +
                    "<br />" +
                    "<strong>If This Isn\'t You, Please Report it at This link here.</strong>" +
                    "<br />" +
                    "<br />" +
                    "<p>Regards, </p>" +
                    "<p>MyShoeRack Team</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                Response.Write("<div class='modal fade' id='successModal' tabindex='-1' role='dialog' aria-labelledby='successLabel' aria-hidden='true'><div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'><h5 class='modal-title' id='successLabel'>Reset Password Email</h5><button type='button' class='close' data-dismiss='♦modal' aria-label='Close' id='successmodalClose'><span aria-hidden='true'>&times;</span></button></div><div class='modal-body' align='center'><div class='row'><div class='col'><div class='col'><img src='https://mastertrader.com/wp-content/uploads/2017/06/check-mark-icon.png' style='height:100px;' /></div><h4>Successfully Sent!</h4><p>Please Check Your Email and Follow the Instructions.</p></div></div></div></div></div></div>");
            }
            else
            {
                Response.Write("<div class='modal fade' id='successModal' tabindex='-1' role='dialog' aria-labelledby='successLabel' aria-hidden='true'><div class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'><h5 class='modal-title' id='successLabel'>Reset Password Email</h5><button type='button' class='close' data-dismiss='♦modal' aria-label='Close' id='successmodalClose'><span aria-hidden='true'>&times;</span></button></div><div class='modal-body' align='center'><div class='row'><div class='col'><div class='col'><img src='http://www.sclance.com/pngs/red-x-mark-png/red_x_mark_png_1158633.png' style='height:100px;' /></div><br /><h4>Couldn't Find the Email!</h4><p>Please Make Sure You Enter a Registered Email!</p></div></div></div></div></div></div>");
            }
        }

        protected void forgotPasswordSubmit_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(SendForgotPasswordEmail));
        }

        protected Boolean checkValidUser(string email)
        {
            Boolean valid = false;
            int userid = 0;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select user_id FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userid = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }

            SqlConnection connection2 = new SqlConnection(MYDBConnectionString);
            string sql2 = "SELECT * FROM UserActivation WHERE UserId=@user_id";
            SqlCommand command2 = new SqlCommand(sql2, connection2);
            command2.Parameters.AddWithValue("@user_id", userid.ToString());
            try
            {
                connection2.Open();
                using (SqlDataReader reader2 = command2.ExecuteReader())
                {
                    if (reader2.HasRows)
                    {
                        SqlConnection connection3 = new SqlConnection(MYDBConnectionString);
                        string sql3 = "SELECT * FROM UserOTP WHERE UserId=@user_id";
                        SqlCommand command3 = new SqlCommand(sql3, connection3);
                        command3.Parameters.AddWithValue("@user_id", userid.ToString());
                        try
                        {
                            connection3.Open();
                            using (SqlDataReader reader3 = command3.ExecuteReader())
                            {
                                if (reader3.HasRows)
                                {
                                    return valid;
                                }
                                else
                                {
                                    return valid = true;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                        finally
                        {
                            connection3.Close();
                        }
                    }
                    else
                    {
                        SqlConnection connection4 = new SqlConnection(MYDBConnectionString);
                        string sql4 = "SELECT * FROM UserOTP WHERE UserId=@user_id";
                        SqlCommand command4 = new SqlCommand(sql4, connection4);
                        command4.Parameters.AddWithValue("@user_id", userid.ToString());
                        try
                        {
                            connection4.Open();
                            using (SqlDataReader reader4 = command4.ExecuteReader())
                            {
                                if (reader4.HasRows)
                                {
                                    return valid;
                                }
                                else
                                {
                                    return valid = true;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            throw new Exception(ex.ToString());
                        }
                        finally
                        {
                            connection4.Close();
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection2.Close();
            }
        }


        protected async Task AsyncEmailAuth()
        {
            emailAddressLabel.Text = loginName.Value.ToString().Trim();
            string randomString = RandomString(6);
            Response.Cookies.Add(new HttpCookie("EmailAuth", randomString));
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("admin@MyShoeRack.com", "MyShoeRack-Admin");
            var subject = "MyShoeRack - Email OTP";
            var to = new EmailAddress(loginName.Value.ToString().Trim(), "You");
            var plainTextContent = "";
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            var htmlContent = "<p>Hi,</p>" +
                "<p>Your Email OTP for MyShoeRack Email Verification is : " + randomString +
                "<br />" +
                "<br />" +
                "<p>Regards, </p>" +
                "<p>MyShoeRack Team</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "loginCheckEmail();", true);
        }

        protected void emailLogin_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(AsyncEmailAuth));
        }

        protected void smsLogin_Click(object sender, EventArgs e)
        {
            string phoneNo = getDBPhoneNo(loginName.Value.ToString().Trim());
            phoneNoLabel.Text = phoneNo;
            string randomString = RandomString(6);
            Response.Cookies.Add(new HttpCookie("SMSAuth", randomString));
            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                    body: "Enter the OTP for MyShoeRack Website : " + randomString,
                    from: new Twilio.Types.PhoneNumber("+14439799177"),
                    to: new Twilio.Types.PhoneNumber("+" + phoneNo)
                );

                Console.WriteLine(message.Sid);
            }
            catch (ApiException te)
            {
                Console.WriteLine(te.Message);
                Console.WriteLine($"Twilio Error {te.Code} - {te.MoreInfo}");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "loginCheckSMS();", true);
        }

        protected void email2fa_Click(object sender, EventArgs e)
        {
            string entered_code = emailOTP.Value;
            string OTP = Request.Cookies["EmailAuth"].Value;
            if (entered_code == OTP)
            {
                if (Request.Cookies["EmailAuth"] != null)
                {
                    Response.Cookies["EmailAuth"].Value = string.Empty;
                    Response.Cookies["EmailAuth"].Expires = DateTime.Now.AddMonths(-20);
                }
                invalidCodeEmail.Text = "";
                RegisterAsyncTask(new PageAsyncTask(PageLoadAsync));

            }
            else
            {
                invalidCodeEmail.Text = "Invalid Code, Try Again.";
            }
        }

        protected void sms2fa_Click(object sender, EventArgs e)
        {
            string entered_code = smsOTP.Value;
            string OTP = Request.Cookies["SMSAuth"].Value;
            if (entered_code == OTP)
            {
                if (Request.Cookies["SMSAuth"] != null)
                {
                    Response.Cookies["SMSAuth"].Value = string.Empty;
                    Response.Cookies["SMSAuth"].Expires = DateTime.Now.AddMonths(-20);
                }
                invalidCodeSMS.Text = "";
                RegisterAsyncTask(new PageAsyncTask(PageLoadAsync));
            }
            else
            {
                invalidCodeSMS.Text = "Invalid Code, Try Again.";
            }
        }

        protected string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected string getDBPhoneNo(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select phone_number FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["phone_number"] != null)
                        {
                            if (reader["phone_number"] != DBNull.Value)
                            {
                                h = reader["phone_number"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return h;
        }

        protected void emailAltSMSLogin_Click(object sender, EventArgs e)
        {
            string phoneNo = getDBPhoneNo(loginName.Value.ToString().Trim());
            phoneNoLabel.Text = phoneNo;
            string randomString = RandomString(6);
            Response.Cookies.Add(new HttpCookie("SMSAuth", randomString));
            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                    body: "Enter the OTP for MyShoeRack Website : " + randomString,
                    from: new Twilio.Types.PhoneNumber("+14439799177"),
                    to: new Twilio.Types.PhoneNumber("+" + phoneNo)
                );

                Console.WriteLine(message.Sid);
            }
            catch (ApiException te)
            {
                Console.WriteLine(te.Message);
                Console.WriteLine($"Twilio Error {te.Code} - {te.MoreInfo}");
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "loginCheckSMS();", true);
        }
    }
}