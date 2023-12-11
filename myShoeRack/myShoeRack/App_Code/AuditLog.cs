using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using IpData;
using System.Net;
using System.Text.RegularExpressions;

namespace myShoeRack.App_Code
{
    public class AuditLog
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        public static List<string> inputnames = new List<string>();

        private string aCode = "";
        private string aDate = "";
        private string aTime = "";
        private string aUsername = "";
        private string aUserid = "";
        private string aIpaddr = "";
        private string aDescript = "";
        private string aDetails = "";
        private string aAction = "";
        private string aMdetails = "";

        private string _reg_hp = "";
        private string _reg_address = "";

        private string _product_id = "";
        private string _product_name = "";
        private string _product_brand = "";
        private decimal _product_cost = 0;

        private string _platform = "";

        //default constructor
        public AuditLog()
        {
            this.aCode = "";
        }

        public AuditLog(string code, string date, string time, string username,
            string ipaddr, string descript, string details, string action)
        {
            aCode = code;
            aDate = date;
            aTime = time;
            aUsername = username;
            aIpaddr = ipaddr;
            aDescript = descript;
            aDetails = details;
            aAction = action;
        }

        //without id (code)
        public AuditLog(string date, string time, string username,
            string ipaddr, string descript, string details, string action)
        {
            aDate = date;
            aTime = time;
            aUsername = username;
            aIpaddr = ipaddr;
            aDescript = descript;
            aDetails = details;
            aAction = action;
        }

        //for table
        public AuditLog(string code, string date, string time, string username,
            string ipaddr, string descript, string details, string action,
            string pid, string pname, string pbrand, int pcost)
        {
            aCode = code;
            aDate = date;
            aTime = time;
            aUsername = username;
            aIpaddr = ipaddr;
            aDescript = descript;
            aDetails = details;
            aAction = action;
            _product_id = pid;
            _product_name = pname;
            _product_brand = pbrand;
            _product_cost = pcost;
        }

        //for more details
        public AuditLog(string code, string date, string time, string userid, string username,
            string ipaddr, string descript, string details, string mdetails, string hp, string address,
            string pid, string pname, string pbrand, int pcost, string platform)
        {
            aCode = code;
            aDate = date;
            aTime = time;
            aUsername = username;
            aUserid = userid;
            aIpaddr = ipaddr;
            aDescript = descript;
            aDetails = details;
            aMdetails = mdetails;
            _reg_hp = hp;
            _reg_address = address;
            _product_id = pid;
            _product_name = pname;
            _product_brand = pbrand;
            _product_cost = pcost;
            _platform = platform;
        }

        //with product details without pri key     
        public AuditLog(string date, string time, string username,
            string ipaddr, string descript, string details, string action,
            string pid, string pname, string pbrand, decimal pcost)
        {
            aDate = date;
            aTime = time;
            aUsername = username;
            aIpaddr = ipaddr;
            aDescript = descript;
            aDetails = details;
            aAction = action;
            _product_id = pid;
            _product_name = pname;
            _product_brand = pbrand;
            _product_cost = pcost;
        }

        public string a_Code
        {
            get { return aCode; }
            set { aCode = value; }
        }

        public string a_Date
        {
            get { return aDate; }
            set { aDate = value; }
        }

        public string a_Time
        {
            get { return aTime; }
            set { aTime = value; }
        }

        public string a_Username
        {
            get { return aUsername; }
            set { aUsername = value; }
        }

        public string a_Userid
        {
            get { return aUserid; }
            set { aUserid = value; }
        }

        public string a_Ipaddr
        {
            get { return aIpaddr; }
            set { aIpaddr = value; }
        }

        public string a_Descript
        {
            get { return aDescript; }
            set { aDescript = value; }
        }

        public string a_Details
        {
            get { return aDetails; }
            set { aDetails = value; }
        }

        public string a_Action
        {
            get { return aAction; }
            set { aAction = value; }
        }

        public string a_Mdetails
        {
            get { return aMdetails; }
            set { aMdetails = value; }
        }

        public string register_hp
        {
            get { return _reg_hp; }
            set { _reg_hp = value; }
        }

        public string register_address
        {
            get { return _reg_address; }
            set { _reg_address = value; }
        }

        public string product_id
        {
            get { return _product_id; }
            set { _product_id = value; }
        }

        public string product_name
        {
            get { return _product_name; }
            set { _product_name = value; }
        }

        public string product_brand
        {
            get { return _product_brand; }
            set { _product_brand = value; }
        }

        public decimal product_cost
        {
            get { return _product_cost; }
            set { _product_cost = value; }
        }

        public string platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        public string getExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }

        public string GetUserEnvironment(HttpRequest request)
        {
            var browser = request.Browser;
            var platform = GetUserPlatform(request);
            return string.Format("{0} {1} / {2}", browser.Browser, browser.Version, platform);
        }

        public string GetUserPlatform(HttpRequest request)
        {
            var ua = request.UserAgent;

            if (ua.Contains("Android"))
                return string.Format("Android {0}", GetMobileVersion(ua, "Android"));

            if (ua.Contains("iPad"))
                return string.Format("iPad OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("iPhone"))
                return string.Format("iPhone OS {0}", GetMobileVersion(ua, "OS"));

            if (ua.Contains("Linux") && ua.Contains("KFAPWI"))
                return "Kindle Fire";

            if (ua.Contains("RIM Tablet") || (ua.Contains("BB") && ua.Contains("Mobile")))
                return "Black Berry";

            if (ua.Contains("Windows Phone"))
                return string.Format("Windows Phone {0}", GetMobileVersion(ua, "Windows Phone"));

            if (ua.Contains("Mac OS"))
                return "Mac OS";

            if (ua.Contains("Windows NT 5.1") || ua.Contains("Windows NT 5.2"))
                return "Windows XP";

            if (ua.Contains("Windows NT 6.0"))
                return "Windows Vista";

            if (ua.Contains("Windows NT 6.1"))
                return "Windows 7";

            if (ua.Contains("Windows NT 6.2"))
                return "Windows 8";

            if (ua.Contains("Windows NT 6.3"))
                return "Windows 8.1";

            if (ua.Contains("Windows NT 10"))
                return "Windows 10";

            return request.Browser.Platform + (ua.Contains("Mobile") ? " Mobile " : "");
        }

        public String GetMobileVersion(string userAgent, string device)
        {
            var temp = userAgent.Substring(userAgent.IndexOf(device) + device.Length).TrimStart();
            var version = string.Empty;

            foreach (var character in temp)
            {
                var validCharacter = false;
                int test = 0;

                if (Int32.TryParse(character.ToString(), out test))
                {
                    version += character;
                    validCharacter = true;
                }

                if (character == '.' || character == '_')
                {
                    version += '.';
                    validCharacter = true;
                }

                if (validCharacter == false)
                    break;
            }

            return version;
        }

        public void auditLoginSuccess(string username)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string useremail, userid;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User sucessfully logged in";
                string details = "auditlogDetails.aspx?code=";
                //string action = "Login";

                //get userid
                string queryStr = "SELECT * FROM Users WHERE user_email = @useremail";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@useremail", username);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                User theUser = null;
                string userName, userPw, userSalt, userAddress, adminStatus;
                int userId, userCounter, bannedUser;
                if (dr.Read())
                {
                    userId = int.Parse(dr["user_Id"].ToString());
                    useremail = dr["user_email"].ToString();
                    if (useremail != username)
                    {
                        Debug.WriteLine("Usernames do not match!!");
                    }
                    userName = dr["user_name"].ToString();
                    userPw = dr["user_passhash"].ToString();
                    userSalt = dr["user_hashsalt"].ToString();
                    userCounter = int.Parse(dr["user_counter"].ToString());
                    userAddress = dr["user_address"].ToString();
                    adminStatus = dr["admin_status"].ToString();
                    bannedUser = int.Parse(dr["banned_user"].ToString());
                    theUser = new User(userId, useremail, userName, userPw, userSalt, userCounter, userAddress, adminStatus, bannedUser);
                }
                conn.Close();
                dr.Close();
                dr.Dispose();

                userid = theUser.User_Id.ToString();

                string mdetails = date + " " + time + ": " + username + "(ID: " + userid + ") successfully logged in, from " + ipadd;
                //string mdetails = "User logged in successfully into " + username + " account (ID: " + userid + "), from " + ipadd + " at " + date + " " + time;
                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid, username, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
            }
        }

        public void auditLoginFail(string username)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User failed to log in";
                string details = "auditlogDetails.aspx?code=";
                //string action = "Login";
                string mdetails = date + " " + time + ": " + username + " failed to login, from " + ipadd;
                //string mdetails = "User failed to login using " + username + " account, from " + ipadd + " at " + date + " " + time;

                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, null, username, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
            }
        }

        public void auditLogout(string username)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string useremail, userid;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User sucessfully logged out";
                string details = "auditlogDetails.aspx?code=";

                //get userid
                string queryStr = "SELECT * FROM Users WHERE user_email = @useremail";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@useremail", username);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                User theUser = null;
                string userName, userPw, userSalt, userAddress, adminStatus;
                int userId, userCounter, bannedUser;
                if (dr.Read())
                {
                    userId = int.Parse(dr["user_Id"].ToString());
                    useremail = dr["user_email"].ToString();
                    if (useremail != username)
                    {
                        Debug.WriteLine("Usernames do not match!!");
                    }
                    userName = dr["user_name"].ToString();
                    userPw = dr["user_passhash"].ToString();
                    userSalt = dr["user_hashsalt"].ToString();
                    userCounter = int.Parse(dr["user_counter"].ToString());
                    userAddress = dr["user_address"].ToString();
                    adminStatus = dr["admin_status"].ToString();
                    bannedUser = int.Parse(dr["banned_user"].ToString());
                    theUser = new User(userId, useremail, userName, userPw, userSalt, userCounter, userAddress, adminStatus, bannedUser);
                }
                conn.Close();
                dr.Close();
                dr.Dispose();

                userid = theUser.User_Id.ToString();

                string mdetails = date + " " + time + ": " + username + "(ID: " + userid + ") successfully logged out, from " + ipadd;
                //string mdetails = "User logged in successfully into " + username + " account (ID: " + userid + "), from " + ipadd + " at " + date + " " + time;
                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid, username, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
            }
        }

        public void bannedUserLogin(string username)       //log when banned user try to log in
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string useremail, userid;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "Banned user attempted to log in";
                string details = "auditlogDetails.aspx?code=";

                //get userid
                string queryStr = "SELECT * FROM Users WHERE user_email = @useremail";
                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("@useremail", username);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                User theUser = null;
                string userName, userPw, userSalt, userAddress, adminStatus;
                int userId, userCounter, bannedUser;
                if (dr.Read())
                {
                    userId = int.Parse(dr["user_Id"].ToString());
                    useremail = dr["user_email"].ToString();
                    if (useremail != username)
                    {
                        Debug.WriteLine("Usernames do not match!!");
                    }
                    userName = dr["user_name"].ToString();
                    userPw = dr["user_passhash"].ToString();
                    userSalt = dr["user_hashsalt"].ToString();
                    userCounter = int.Parse(dr["user_counter"].ToString());
                    userAddress = dr["user_address"].ToString();
                    adminStatus = dr["admin_status"].ToString();
                    bannedUser = int.Parse(dr["banned_user"].ToString());
                    theUser = new User(userId, useremail, userName, userPw, userSalt, userCounter, userAddress, adminStatus, bannedUser);
                }
                conn.Close();
                dr.Close();
                dr.Dispose();

                userid = theUser.User_Id.ToString();

                string mdetails = date + " " + time + ": Banned user " + username + "(ID: " + userid + ") attempted to log into account, from " + ipadd;
                //string mdetails = "Banned user: " + username + " (ID: " + userid + ") attempted to log into account, from " + ipadd + " at " + date + " " + time;
                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid, username, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
            }
        }

        public void auditRegisterSuccess(int userid)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                //string useremail;
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User successfully registered and activated account";
                string details = "auditlogDetails.aspx?code=";

                User theUser = new User();
                theUser = theUser.GetUser(userid);

                string mdetails = date + " " + time + ": " + theUser.User_Email + "(ID: " + userid + ") successfully registered and activated account, from " + ipadd;
                //string mdetails = "User: " + theUser.User_Email + " ID: " + userid + " successfully registered and activated an account from " + ipadd + " at " + date + " " + time;

                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid.ToString(), theUser.User_Email, ipadd, description, details, mdetails, theUser.Phone_Number, theUser.User_Address, null, null, null, null, platform);
            }
        }

        public void auditRegisterOtpFail(int userid)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                //DateTime currenttime = DateTime.Now;
                //TimeSpan timediff = DateTime.Now - DateTime.Now;
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User entered wrong OTP for register";
                string details = "auditlogDetails.aspx?code=";

                User theUser = new User();
                theUser = theUser.GetUser(userid);

                string mdetails = date + " " + time + ": " + theUser.User_Email + "entered the wrong SMS OTP for register, from " + ipadd;
                //string mdetails = "User: " + theUser.User_Email + " ID: " + userid + " entered wrong OTP for register " + timediff.ToString() + " from " + ipadd + " at " + date + " " + time;

                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid.ToString(), theUser.User_Email, ipadd, description, details, mdetails, theUser.Phone_Number, theUser.User_Address, null, null, null, null, platform);
            }
        }

        public void auditRegisterOtpExpire(int userid)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime currenttime = DateTime.Now;
                TimeSpan timediff = DateTime.Now - DateTime.Now;
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User entered expired OTP for register";
                string details = "auditlogDetails.aspx?code=";

                User theUser = new User();
                theUser = theUser.GetUser(userid);

                //using (SqlConnection conn = new SqlConnection(_connStr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("SELECT ExpiryTime FROM UserOTP WHERE UserId = @userid"))
                //    {
                //        using (SqlDataAdapter sda = new SqlDataAdapter())
                //        {
                //            cmd.CommandType = CommandType.Text;
                //            cmd.Parameters.AddWithValue("@userid", userid);
                //            cmd.Connection = conn;
                //            conn.Open();
                //            using (SqlDataReader reader = cmd.ExecuteReader())
                //            {
                //                while (reader.Read())
                //                {
                //                    DateTime otptime = DateTime.Parse(reader["ExpiryTime"].ToString());
                //                    if (DateTime.Parse(reader["ExpiryTime"].ToString()) < DateTime.Now)
                //                    {
                //                        timediff = currenttime.Subtract(otptime);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                string mdetails = date + " " + time + ": " + theUser.User_Email + "(ID: " + userid + ") entered expired SMS OTP for register, from " + ipadd;
                //string mdetails = "User: " + theUser.User_Email + " ID: " + userid + " entered expired OTP for register " + timediff.ToString() + " from " + ipadd + " at " + date + " " + time;

                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid.ToString(), theUser.User_Email, ipadd, description, details, mdetails, theUser.Phone_Number, theUser.User_Address, null, null, null, null, platform);
            }
        }

        public void auditRegisterEmailExpire(int userid, string code)
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime currenttime = DateTime.Now;
                TimeSpan timediff = DateTime.Now - DateTime.Now;
                string time = DateTime.Now.ToString("HH:mm:ss");
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "User clicked expired activation link for register";
                string details = "auditlogDetails.aspx?code=";

                User theUser = new User();
                theUser = theUser.GetUser(userid);

                //using (SqlConnection conn = new SqlConnection(_connStr))
                //{
                //    using (SqlCommand cmd = new SqlCommand("SELECT ExpiryTime FROM UserActivation WHERE ActivationCode = @code"))
                //    {
                //        using (SqlDataAdapter sda = new SqlDataAdapter())
                //        {
                //            cmd.CommandType = CommandType.Text;
                //            cmd.Parameters.AddWithValue("@code", code);
                //            cmd.Connection = conn;
                //            conn.Open();
                //            using (SqlDataReader reader = cmd.ExecuteReader())
                //            {
                //                while (reader.Read())
                //                {
                //                    DateTime emailtime = DateTime.Parse(reader["ExpiryTime"].ToString());
                //                    if (DateTime.Parse(reader["ExpiryTime"].ToString()) < DateTime.Now)
                //                    {
                //                        timediff = currenttime.Subtract(emailtime);
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}

                string mdetails = date + " " + time + ": " + theUser.User_Email + "(ID: " + userid + ") clicked expired email for register, from " + ipadd;
                //string mdetails = "User: " + theUser.User_Email + " ID: " + userid + " entered expired activation code for register " + timediff.ToString() + " from " + ipadd + " at " + date + " " + time;

                sw.WriteLine(mdetails);

                insertAuditIntoDb(date, time, userid.ToString(), theUser.User_Email, ipadd, description, details, mdetails, theUser.Phone_Number, theUser.User_Address, null, null, null, null, platform);
            }
        }

        public void auditCheckout()
        {

        }

        public bool insertAuditIntoDb(string date, string time, string userid, string username,
            string ipadd, string description, string details, string mdetails, string reghp,
            string regaddress, string pid, string pname, string pbrand, string pcost, string platform)      //insert audit into db
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO AuditLog (a_Date, a_Time, a_Userid, " +
                    "a_Username, a_Ipaddr, a_Descript, a_Details, a_Mdetails, register_hp, " +
                    "register_address, product_id, product_name, product_brand, product_cost, platform) " +
                    "VALUES(@a_Date, @a_Time, @a_Userid," +
                    "@a_Username, @a_Ipaddr, @a_Descript, @a_Details, @a_Mdetails, @register_hp, @register_address, @product_id, " +
                    "@product_name, @product_brand, @product_cost, @platform)"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@a_Date", date);
                        cmd.Parameters.AddWithValue("@a_Time", time);
                        cmd.Parameters.AddWithValue("@a_Username", username);

                        //userid
                        if (userid == null)
                        {
                            cmd.Parameters.AddWithValue("@a_Userid", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@a_Userid", userid);
                        }
                        cmd.Parameters.AddWithValue("@a_Ipaddr", ipadd);
                        cmd.Parameters.AddWithValue("@a_Descript", description);
                        cmd.Parameters.AddWithValue("@a_Details", details);
                        cmd.Parameters.AddWithValue("@a_Mdetails", mdetails);

                        //reghp
                        if (reghp == null)
                        {
                            cmd.Parameters.AddWithValue("@register_hp", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@register_hp", reghp);
                        }

                        //regaddress
                        if (regaddress == null)
                        {
                            cmd.Parameters.AddWithValue("@register_address", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@register_address", regaddress);
                        }

                        //pid
                        if (pid == null)
                        {
                            cmd.Parameters.AddWithValue("@product_id", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@product_id", pid);
                        }

                        //pname
                        if (pname == null)
                        {
                            cmd.Parameters.AddWithValue("@product_name", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@product_name", pname);
                        }

                        //pbrand
                        if (pbrand == null)
                        {
                            cmd.Parameters.AddWithValue("@product_brand", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@product_brand", pbrand);
                        }

                        //pcost
                        if (pcost == null)
                        {
                            cmd.Parameters.AddWithValue("@product_cost", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@product_cost", pcost);
                        }

                        //platform
                        if (platform == null)
                        {
                            cmd.Parameters.AddWithValue("@platform", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@platform", platform);
                        }

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            return true;
        }

        public void auditResetLoginAttempt(string inputname)
        {
            if (inputnames.Find(x => x == inputname) == null)
            {
                //do nothing
            }
            if (inputnames.Find(x => x == inputname) != null)
            {                
                HttpContext.Current.Request.Cookies[inputname].Value = string.Empty;
                HttpContext.Current.Request.Cookies[inputname].Expires = DateTime.Now.AddMonths(-20);
                HttpContext.Current.Request.Cookies[inputname]["count"] = "0";
                HttpContext.Current.Request.Cookies[inputname]["time"] = "0";
            }
        }

        public void auditCountLoginAttempts(string inputname)
        {
            //List<string> inputnames = new List<string>();
            string currenttry, name, time;
            int count = 0;
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                if (inputnames.Count >= 0)
                {
                    currenttry = inputnames.Find(x => x == inputname);
                    if (currenttry != null)
                    {
                        //HttpContext.Current.Request.Cookies[currenttry]["time"] = DateTime.Now.ToString();
                        //HttpContext.Current.Request.Cookies[currenttry]["count"] = HttpContext.Current.Request.Cookies[currenttry]["count"].ToString();
                        HttpCookie username = HttpContext.Current.Request.Cookies[currenttry];
                        //int tempcount = int.Parse(username.Values["count"]);
                        //tempcount = tempcount + 1;
                        //username.Values["count"] = tempcount.ToString();
                        //username.Values["time"] = DateTime.Now.ToString();
                        //HttpContext.Current.Request.Cookies[currenttry].Values["time"] = DateTime.Now.ToString();
                        name = currenttry;
                        count = int.Parse(HttpContext.Current.Request.Cookies[currenttry].Values["count"].ToString()) + 1;
                        //time = DateTime.Now.ToString();
                        string date = DateTime.Now.ToString("dd/MM/yyyy");
                        time = DateTime.Now.ToString("HH:mm:ss");
                        string ipadd = getExternalIp();
                        string platform = GetUserPlatform(HttpContext.Current.Request);
                        string description = "Login fail counter: " + count;
                        string details = "auditlogDetails.aspx?code=";
                        string mdetails = date + " " + time + ": " + currenttry +  ": login fail count is: " + count + ", from " + ipadd;

                        HttpContext.Current.Response.Cookies[currenttry]["count"] = count.ToString();
                        HttpContext.Current.Response.Cookies[currenttry]["time"] = time;

                        sw.WriteLine("Login fail count for " + name + ": " + count + ", at " + time);
                        insertAuditIntoDb(date, time, null, currenttry, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
                    }
                    else if (currenttry == null)
                    {
                        inputnames.Add(inputname);
                        HttpCookie username = new HttpCookie(inputname);
                        username.Values["count"] = 1.ToString();
                        username.Values["time"] = DateTime.Now.ToString();
                        HttpContext.Current.Response.Cookies.Add(username);
                        name = inputname;
                        count = count + 1;
                        //time = DateTime.Now.ToString();


                        string date = DateTime.Now.ToString("dd/MM/yyyy");
                        time = DateTime.Now.ToString("HH:mm:ss");
                        string ipadd = getExternalIp();
                        string platform = GetUserPlatform(HttpContext.Current.Request);
                        string description = "Login fail counter: " + count;
                        string details = "auditlogDetails.aspx?code=";
                        string mdetails = date + " " + time + ": " + currenttry + ": login fail count is: " + count + ", from " + ipadd;

                        sw.WriteLine("Login fail count for " + name + ": " + count + ", at " + time);

                        insertAuditIntoDb(date, time, null, inputname, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
                    }
                }
                else
                {
                    inputnames.Add(inputname);
                    HttpCookie username = new HttpCookie(inputname);
                    username.Values["count"] = 1.ToString();
                    username.Values["time"] = DateTime.Now.ToString();
                    HttpContext.Current.Response.Cookies.Add(username);
                    name = inputname;
                    count = count + 1;
                    time = DateTime.Now.ToString();
                    sw.WriteLine("Login fail count for " + name + ": " + count + ", at " + time);
                }
            }

            //HttpCookie loginattempt = HttpContext.Current.Request.Cookies["loginattempt"];
            //HttpCookie loginattempt = new HttpCookie("loginattempt");
            //loginattempt.Values["inputname"] = null;
            //loginattempt.Values["count"] = null;
            //loginattempt.Values["time"] = null;
            //if (loginattempt != null)
            //{
            //    if (HttpContext.Current.Request.Cookies["loginattempt"]["inputname"] == inputname)
            //    {

            //    }
            //    string name = (HttpContext.Current.Request.Cookies["loginattempt"].Values["inputname"].ToString());
            //    int count = int.Parse(HttpContext.Current.Request.Cookies["loginattempt"].Values["count"].ToString());
            //    string time = (HttpContext.Current.Request.Cookies["loginattempt"].Values["time"].ToString());

            //    using (StreamWriter sw = new StreamWriter(@"C:\Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log.txt", true))
            //    {
            //        sw.WriteLine("Login fail count for " + name + ": " + count + ", at " + time);
            //    }
            //}
            //else
            //{
            //    loginattempt = new HttpCookie("loginattempt");
            //    loginattempt.Values["inputname"] = inputname;
            //    loginattempt.Values["count"] = 1.ToString();
            //    loginattempt.Values["time"] = DateTime.Now.ToString();
            //}
        }

        public void auditAddProduct(string pid, string pname, string pbrand, int pcost)
        {
            //StreamWriter sw = File.AppendText(@"C:\Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log_add.txt");
            //if (!File.Exists(@"C:\Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log_add.txt"))
            //{
            //    Debug.WriteLine("File doesnt exist. Creating file. . .");
            //    File.Create(@"C: \Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log_add.txt");
            //}
            //else
            //{
            //    //File.AppendText("log_add.txt");
            //    Debug.WriteLine("File already exists");
            //}

            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                string time = DateTime.Now.ToString("HH:mm:ss");
                string username = (string)HttpContext.Current.Session["LoggedIn"];
                //string ipadd = HttpContext.Current.Request.UserHostAddress;
                string ipadd = getExternalIp();
                string platform = GetUserEnvironment(HttpContext.Current.Request);
                string description = "New product added to database";
                string details = "auditlogDetails.aspx?code=";
                //string action = "Add product";
                string mdetails = "";

                sw.WriteLine("{0} added new {1} {2}, with ID: {3}, Cost: ${4} from {5} at {6} {7}", username, pbrand, pname, pid, pcost, ipadd, date, time);

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO AuditLog VALUES(@a_Date, @a_Time, @a_Userid," +
                        "@a_Username, @a_Ipaddr, @a_Descript, @a_Details, @a_Mdetails, @product_id, " +
                        "@product_name, @product_brand, @product_cost)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@a_Date", date);
                            cmd.Parameters.AddWithValue("@a_Time", time);
                            cmd.Parameters.AddWithValue("@a_Username", username);
                            cmd.Parameters.AddWithValue("@a_Userid", username);
                            cmd.Parameters.AddWithValue("@a_Ipaddr", ipadd);
                            cmd.Parameters.AddWithValue("@a_Descript", description);
                            cmd.Parameters.AddWithValue("@a_Details", details);
                            cmd.Parameters.AddWithValue("@a_Action", mdetails);
                            cmd.Parameters.AddWithValue("@product_id", pid);
                            cmd.Parameters.AddWithValue("@product_name", pname);
                            cmd.Parameters.AddWithValue("@product_brand", pbrand);
                            cmd.Parameters.AddWithValue("@product_cost", pcost);

                            cmd.Connection = conn;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                }
            }
        }

        public void clearAuditDb()
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM AuditLog"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = conn;
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsAffected > 1)
                        {
                            Debug.WriteLine("Audit Log Cleared");
                        }
                        else
                        {
                            Debug.WriteLine("Audit Log NOT Cleared");
                        }
                    }
                }
            }
        }
    }
}