using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using myShoeRack.App_Code;

namespace myShoeRack.Admin
{
    public partial class Adminpanel : System.Web.UI.Page
    {
        string PDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        Adminclass user = new Adminclass();
        Errorlogclass error = new Errorlogclass();
        Intrusionlog intrusion = new Intrusionlog();
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {
            Adminclass adminin = new Adminclass();
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                string email = Session["LoggedIn"].ToString();
                Boolean checkemail = adminin.checkAdmin(email);
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value) || checkemail != true)
                {
                    IntrustionLogging();
                    Response.Redirect("../index.aspx", false);
                    System.Diagnostics.Debug.WriteLine("someone try to access the admin page!!");
                }
                else
                {
                    if (!IsPostBack)
                    {
                        bind();
                    }
                }
            }
            else
            {
                Response.Redirect("../index.aspx", false);
            }
        }

        protected void bind()
        {
            List<Adminclass> userlist = new List<Adminclass>();
            List<Adminclass> banneduser = new List<Adminclass>();
            List<Errorlogclass> errorlist = new List<Errorlogclass>();
            List<Intrusionlog> intrustionlist = new List<Intrusionlog>();
            banneduser = user.GetBanListUsers();
            userlist = user.GetUserAll();
            errorlist = error.getErrorLoglist();
            intrustionlist = intrusion.GetIntrusionAll();
            gv_banneduser.DataSource = banneduser;
            gv_banneduser.DataBind();
            gv_userlist.DataSource = userlist;
            gv_userlist.DataBind();
            gv_Errorlog.DataSource = errorlist;
            gv_Errorlog.DataBind();
            gv_intrusion.DataSource = intrustionlist;
            gv_intrusion.DataBind();
        }

        protected void IntrustionLogging()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(PDBConnectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("INSERT into intrusionLog VALUES(@Intrusion_Email, @Intrusion_Detail, @Intrusion_Date_Time)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Intrusion_Email", Session["LoggedIn"].ToString());
                            cmd.Parameters.AddWithValue("@Intrusion_Detail", "User trying to access Admin page");
                            cmd.Parameters.AddWithValue("@Intrusion_Date_Time", DateTime.Now);

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
        protected void gvuserlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gv_userlist.SelectedRow;
            string testemail = row.Cells[2].Text;
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            string paramkey = Convert.ToBase64String(Key);
            string paramIV = Convert.ToBase64String(IV);
            string encryptemail = Convert.ToBase64String(encryptData(testemail));
            System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(Key) + " " + Convert.ToBase64String(IV) + " " + Convert.ToBase64String(encryptData(testemail)) + " logging Key and IV here");
            Response.Redirect("Userpanel.aspx?encryptemail=" + Server.UrlEncode(encryptemail) + "&Key=" + Server.UrlEncode(paramkey) + "&IV=" + Server.UrlEncode(paramIV), false);
        }

        protected void gvbanneduser_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gv_banneduser.SelectedRow;
            string testemail = row.Cells[2].Text;
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            string paramkey = Convert.ToBase64String(Key);
            string paramIV = Convert.ToBase64String(IV);
            string encryptemail = Convert.ToBase64String(encryptData(testemail));
            System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(Key) + " " + Convert.ToBase64String(IV) + " " + Convert.ToBase64String(encryptData(testemail)) + " logging Key and IV here");
            Response.Redirect("Userpanel.aspx?encryptemail=" + Server.UrlEncode(encryptemail) + "&Key=" + Server.UrlEncode(paramkey) + "&IV=" + Server.UrlEncode(paramIV), false);
        }

        protected void gvIntrusionLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gv_intrusion.SelectedRow;
            string testemail = row.Cells[1].Text;
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;

            string paramkey = Convert.ToBase64String(Key);
            string paramIV = Convert.ToBase64String(IV);
            string encryptemail = Convert.ToBase64String(encryptData(testemail));
            System.Diagnostics.Debug.WriteLine(Convert.ToBase64String(Key) + " " + Convert.ToBase64String(IV) + " " + Convert.ToBase64String(encryptData(testemail)) + " logging Key and IV here");
            Response.Redirect("Userpanel.aspx?encryptemail=" + Server.UrlEncode(encryptemail) + "&Key=" + Server.UrlEncode(paramkey) + "&IV=" + Server.UrlEncode(paramIV), false);
        }

        protected void gvErrorLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gv_Errorlog.SelectedRow;
            string errorID = row.Cells[0].Text;
            Response.Redirect("Errorpanel.aspx?error_id=" + Server.UrlEncode(errorID), false);
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