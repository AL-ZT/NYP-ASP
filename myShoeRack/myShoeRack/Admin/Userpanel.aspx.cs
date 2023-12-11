using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using myShoeRack.App_Code;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Text;


namespace myShoeRack.Admin
{
    public partial class Userpanel : System.Web.UI.Page
    {
        string PDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        Adminclass user = null;
        byte[] Key;
        byte[] IV;
        byte[] U_email = null;
        Order order = new Order();
        Intrusionlog intrustion = new Intrusionlog();

        protected void Page_Load(object sender, EventArgs e)
        {
            Adminclass userin = new Adminclass();
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                string email = Session["LoggedIn"].ToString();
                Boolean checkadmin = userin.checkAdmin(email);

                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value) || checkadmin == false)
                {
                    Response.Redirect("../index.aspx", false);
                }
                else if (checkadmin == true)
                {
                    string paramKey = Server.UrlDecode(Request.QueryString["Key"]);
                    string paramIV = Server.UrlDecode(Request.QueryString["IV"]);
                    string encryptemail = Server.UrlDecode(Request.QueryString["encryptemail"]);

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

                    encryptemail = encryptemail.Replace(" ", "+");
                    int mod3 = encryptemail.Length % 4;
                    if (mod3 > 0)
                    {
                        encryptemail += new string('=', 4 - mod3);
                    }

                    Key = Convert.FromBase64String(paramKey);
                    IV = Convert.FromBase64String(paramIV);
                    U_email = Convert.FromBase64String(encryptemail);

                    string decrypt_email = decryptData(U_email);

                    user = userin.GetUser(decrypt_email);
                    userid.Text = user.User_Id.ToString();
                    username.Text = user.User_Name;
                    useremail.Text = user.User_Email;
                    useraddress.Text = user.User_Address;
                    adminstatus.Text = user.Admin_Status;

                    int banned_status = user.banned_user;

                    if (email == user.User_Email)
                    {
                        banbtn.Visible = false;
                        unbanbtn.Visible = false;
                    }
                    else if (banned_status == 1)
                    {
                        banbtn.Visible = false;
                        unbanbtn.Visible = true;
                    }
                    else if (banned_status == 0)
                    {
                        banbtn.Visible = true;
                        unbanbtn.Visible = false;
                    }

                    if (!IsPostBack)
                    {
                        bind();
                    }
                }
                else
                {
                    Response.Redirect("../index.aspx", false);
                }
            }
        }

        protected void bind()
        {
            Adminclass userin = new Adminclass();
            string paramKey = Server.UrlDecode(Request.QueryString["Key"]);
            string paramIV = Server.UrlDecode(Request.QueryString["IV"]);
            string encryptemail = Server.UrlDecode(Request.QueryString["encryptemail"]);

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

            encryptemail = encryptemail.Replace(" ", "+");
            int mod3 = encryptemail.Length % 4;
            if (mod3 > 0)
            {
                encryptemail += new string('=', 4 - mod3);
            }

            Key = Convert.FromBase64String(paramKey);
            IV = Convert.FromBase64String(paramIV);
            U_email = Convert.FromBase64String(encryptemail);

            string decrypt_email = decryptData(U_email);            

            List<Order> orderlist = new List<Order>();
            orderlist = order.GetTransactionAll(decrypt_email);
            System.Diagnostics.Debug.WriteLine(decrypt_email + " transaction log here!!!");
            gv_transaction.DataSource = orderlist;
            gv_transaction.DataBind();

            List<Intrusionlog> intrusionlist = new List<Intrusionlog>();
            intrusionlist = intrustion.GetIntrusionAllByUsers(decrypt_email);
            gv_intrusion.DataSource = intrusionlist;
            gv_intrusion.DataBind();
        }

        protected void banbtn_Click(object sender, EventArgs e)
        {
            Adminclass userin = new Adminclass();
            string paramKey = Server.UrlDecode(Request.QueryString["Key"]);
            string paramIV = Server.UrlDecode(Request.QueryString["IV"]);
            string encryptemail = Server.UrlDecode(Request.QueryString["encryptemail"]);

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

            encryptemail = encryptemail.Replace(" ", "+");
            int mod3 = encryptemail.Length % 4;
            if (mod3 > 0)
            {
                encryptemail += new string('=', 4 - mod3);
            }

            Key = Convert.FromBase64String(paramKey);
            IV = Convert.FromBase64String(paramIV);
            U_email = Convert.FromBase64String(encryptemail);

            string decrypt_email = decryptData(U_email);
            userin.banUser(decrypt_email);
            Response.Redirect("Userpanel.aspx?encryptemail=" + Server.UrlEncode(encryptemail) + "&Key=" + Server.UrlEncode(paramKey) + "&IV=" + Server.UrlEncode(paramIV), false);
        }

        protected void unbanbtn_Click(object sender, EventArgs e)
        {
            Adminclass userin = new Adminclass();
            string paramKey = Server.UrlDecode(Request.QueryString["Key"]);
            string paramIV = Server.UrlDecode(Request.QueryString["IV"]);
            string encryptemail = Server.UrlDecode(Request.QueryString["encryptemail"]);

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

            encryptemail = encryptemail.Replace(" ", "+");
            int mod3 = encryptemail.Length % 4;
            if (mod3 > 0)
            {
                encryptemail += new string('=', 4 - mod3);
            }

            Key = Convert.FromBase64String(paramKey);
            IV = Convert.FromBase64String(paramIV);
            U_email = Convert.FromBase64String(encryptemail);

            string decrypt_email = decryptData(U_email);
            userin.unbanUser(decrypt_email);
            Response.Redirect("Userpanel.aspx?encryptemail=" + Server.UrlEncode(encryptemail) + "&Key=" + Server.UrlEncode(paramKey) + "&IV=" + Server.UrlEncode(paramIV), false);
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