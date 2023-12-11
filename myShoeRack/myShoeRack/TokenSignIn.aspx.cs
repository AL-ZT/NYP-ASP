using IpData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace myShoeRack
{
    public partial class TokenSignIn : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(GoogleSession));
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

        protected async Task GoogleSession()
        {
            string receiveContent;
            NameValueCollection formData = Request.Form;
            String idToken = formData.Get("idtoken");
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create("https://oauth2.googleapis.com/tokeninfo?id_token=" + idToken);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = myHttpWebResponse.GetResponseStream();

                if (responseStream != null)
                {
                    var reader = new StreamReader(responseStream);
                    receiveContent = reader.ReadToEnd();
                    reader.Close();
                    JObject jsonContent = JObject.Parse(receiveContent);
                    string email = jsonContent.Value<String>("email");
                    string profilePic = jsonContent.Value<String>("picture");
                    string sub = jsonContent.Value<String>("sub");
                    Session["LoggedIn"] = email;
                    Session["sub"] = sub;
                    Session["profilePic"] = profilePic;
                    Session["2FA"] = "Disabled";
                    Session["emailAuth"] = "Disabled";
                    Session["smsAuth"] = "Disabled";
                    string gUID = Guid.NewGuid().ToString();
                    Session["AuthToken"] = gUID;
                    Response.Cookies["AuthToken"].Value = gUID;
                    Response.Cookies.Add(new HttpCookie("AuthToken", gUID));
                    var client = new IpDataClient("59b6c43c2d79045ed7fb80e00cd3d782d4510e87338164214e413e97");
                    var myIpInfo = await client.Lookup();
                    if (!CheckUserLastLogIn(Session["LoggedIn"].ToString(), Session["sub"].ToString()))
                    {
                        AddLoggedInUser(Session["LoggedIn"].ToString(), Session["sub"].ToString(), myIpInfo.Ip, myIpInfo.CountryName, myIpInfo.City, myIpInfo.TimeZone.CurrentTime.ToString(), myIpInfo.Flag.ToString());
                    }
                    Session["sessionId"] = getRecentSessionId(email, sub);
                }
                myHttpWebResponse.Close();
                Response.Redirect("Index.aspx", false);
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

        protected void AddLoggedInUser(string name, string pass, string ip, string country, string city, string datetime, string flag)
        {
            try
            {
                string deviceName = HttpContext.Current.Server.MachineName;

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
                            cmd.Parameters.AddWithValue("@DEVICENAME", deviceName);
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
    }
}