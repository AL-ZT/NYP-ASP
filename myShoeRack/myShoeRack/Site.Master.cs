using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class SiteMaster : MasterPage
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            Adminclass adminin = new Adminclass();
            if (Session["LoggedIn"] != null)
            {
                string email = Session["LoggedIn"].ToString();
                Boolean checkadmin = adminin.checkAdmin(email);
                if (checkadmin == true)
                {
                    admin_link.Visible = true;
                    audit_link.Visible = true;
                }
            }
            if (Session["LoggedIn"] != null || Session["AuthToken"] != null)
            {
                if (!CheckUserSession(Session["sessionId"].ToString()))
                {
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();

                    if (Request.Cookies["ASP.NET_SessionId"] != null)
                    {
                        Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                    }
                    if (Request.Cookies["AuthToken"] != null)
                    {
                        Response.Cookies["AuthToken"].Value = string.Empty;
                        Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
                    }
                    if (Request.Cookies["Location"] != null)
                    {
                        Response.Cookies["Location"].Value = string.Empty;
                        Response.Cookies["Location"].Expires = DateTime.Now.AddMonths(-20);
                    }
                    Response.Redirect("Index.aspx", false);
                }
            }
        }

        protected void logout_Click(object sender, EventArgs e)
        {
            //AUDIT -JW 
            string username = Session["LoggedIn"].ToString();
            AuditLog audit = new AuditLog();
            audit.auditLogout(username);

            SessionDelete(Session["sessionId"].ToString());
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["Location"] != null)
            {
                Response.Cookies["Location"].Value = string.Empty;
                Response.Cookies["Location"].Expires = DateTime.Now.AddMonths(-20);
            }
            Response.Redirect("Index.aspx", false);
        }


        protected Boolean CheckUserSession(string id)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Username FROM LoggedInUsers WHERE Id=@ID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ID", id);
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

        protected int SessionDelete(string ID)
        {
            string queryStr = "DELETE FROM LoggedInUsers WHERE Id=@ID";
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@ID", ID);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Delete

        protected void link_btn_admin(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/adminpanel.aspx", false);
        }

        protected void link_btn_audit(object sender, EventArgs e)
        {
            Response.Redirect("/auditlog.aspx", false);
        }
    }
}