using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace myShoeRack
{
    public partial class Index : System.Web.UI.Page
    {
        protected void addToCart(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string prodId = btn.CommandArgument;
            Response.Redirect("Cart.aspx?id=" + prodId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Cookies.Add(new HttpCookie("AuthToken", Session["AuthToken"].ToString()));
                    if (Request.Cookies["Location"] != null)
                    {
                        Response.Cookies["Location"].Value = string.Empty;
                        Response.Cookies["Location"].Expires = DateTime.Now.AddMonths(-20);
                    }
                }
                else
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
                }
            }
        }
    }
}