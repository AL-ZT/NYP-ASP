using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using myShoeRack.App_Code;
using System.Net;
using System.Text.RegularExpressions;

namespace myShoeRack.App_Start
{
    public class HttpModule : IHttpModule
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Init(HttpApplication context)
        //{
        //    throw new NotImplementedException();
        //}

        //private StreamWriter sw;

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            //Occurs when ASP.NET acquires the current state (for example, session state) that is associated with the current request.
            //context.PostAcquireRequestState += (new EventHandler(this.Application_BeginRequest));
            context.PostAcquireRequestState += (new EventHandler(this.Application_BeginRequest));
            //throw new NotImplementedException();
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session != null)    //check if session exists
                {
                    string path = HttpContext.Current.Server.MapPath("~/log.txt");
                    if (HttpContext.Current.Session["LoggedIn"] != null)
                    {
                        if (!File.Exists(path))
                        {
                            using (FileStream fs = File.Create(path))
                            {
                                Debug.WriteLine("File doesnt exist. Creating file. . .");

                                //if error, run visual studio as ADMINISTRATOR
                            }
                            //File.Create(path);                        
                        }
                        else
                        {
                            //Debug.WriteLine("File already exists");
                        }

                        string code, userid, date, time, username, ipadd, platform, description, details, mdetails, action;
                        User theUser = null;

                        //writes audit log in textfile

                        //StreamWriter sw = new StreamWriter(@"C:\Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log.txt", true);
                        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                using (StreamWriter sw = new StreamWriter(fs))
                                {
                                    string existingStr = sr.ReadToEnd();
                                    fs.SetLength(0);

                                    //code = "0";
                                    date = DateTime.Now.ToString("dd/MM/yyyy");
                                    time = DateTime.Now.ToString("HH:mm:ss");
                                    //var username = HttpContext.Current.Session["userLoggedIn"];
                                    username = (string)HttpContext.Current.Session["LoggedIn"];
                                    //ipadd = HttpContext.Current.Request.UserHostAddress;        //ip address
                                    ipadd = getExternalIp();
                                    platform = GetUserEnvironment(HttpContext.Current.Request);
                                    description = "User requested for a page";
                                    details = "auditlogDetails.aspx?code=";

                                    string queryStr = "SELECT * FROM Users WHERE user_email = @useremail";
                                    SqlConnection conn = new SqlConnection(_connStr);
                                    SqlCommand cmd = new SqlCommand(queryStr, conn);
                                    cmd.Parameters.AddWithValue("@useremail", username);
                                    conn.Open();
                                    SqlDataReader dr = cmd.ExecuteReader();

                                    string useremail, userName, userPw, userSalt, userAddress, adminStatus;
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
                                    mdetails = date + " " + time + ": " + username + "(ID: " + userid + ") requested for page, from " + ipadd;
                                    //mdetails = username + "(ID: " + userid + ") requested for page, from " + ipadd + " at " + date + " " + time;
                                    //action = "Request";
                                    sw.Write(existingStr);
                                    sw.WriteLine(mdetails);

                                    //sw.Close();
                                    //sw.Dispose();                                
                                }
                            }
                        }
                        checkDuplicateLine();
                        userid = theUser.User_Id.ToString();
                        AuditLog audlog = new AuditLog();
                        audlog.insertAuditIntoDb(date, time, userid, username, ipadd, description, details, mdetails, null, null, null, null, null, null, platform);
                    }
                    else
                    {

                    }
                }
                else
                {
                    //Debug.WriteLine("No session yet");
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        static void removeDupLines(TextReader reader, TextWriter writer)
        {
            string currentLine;
            string nextLine = null;

            while ((currentLine = reader.ReadLine()) != null)
            {
                if (currentLine != nextLine)
                {
                    writer.WriteLine(currentLine);
                    nextLine = currentLine;
                }
            }
        }

        public void checkDuplicateLine()
        {
            string path = HttpContext.Current.Server.MapPath("~/log.txt");
            string[] lines = File.ReadAllLines(path);
            File.WriteAllLines(path, lines.Distinct().ToArray());

            //string path = @"C:\Users\Jun Wei\source\repos\Albert481\myShoeRack\myShoeRack\log.txt";
            //FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            //StreamWriter sw = new StreamWriter(fs);
            //StreamReader sr = new StreamReader(fs);
            //string currentLine;
            //string nextLine = null;
            //while ((currentLine = sr.ReadLine()) != null)
            //{
            //    Debug.WriteLine(currentLine);
            //    if (currentLine != nextLine)
            //    {
            //        //fs.SetLength();
            //        sw.WriteLine(currentLine);
            //        sw.Flush();
            //        nextLine = currentLine;
            //    }
            //}
            //fs.Close();

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

        public string IpAddress()   //get ip address
        {
            //string strHostName = System.Net.Dns.GetHostName();
            //string clientIPAddress = System.Net.Dns.GetHostAddresses
            //(strHostName).GetValue(1).ToString();
            //string ipadd = clientIPAddress;

            string strIpAddress;
            strIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIpAddress == null)
            {
                strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIpAddress;
        }
    }
}