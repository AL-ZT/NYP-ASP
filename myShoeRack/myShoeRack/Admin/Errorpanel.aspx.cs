using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using myShoeRack.App_Code;

namespace myShoeRack.Admin
{
    public partial class Errorpanel : System.Web.UI.Page
    {
        Errorlogclass error = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Errorlogclass errorin = new Errorlogclass();
            Adminclass adminin = new Adminclass();
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                string email = Session["LoggedIn"].ToString();
                Boolean checkadmin = adminin.checkAdmin(email);

                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value) || checkadmin == false)
                {
                    Response.Redirect("../index.aspx", false);
                }
                else if (checkadmin == true)
                {
                    string error_id = Server.UrlDecode(Request.QueryString["error_id"].ToString());

                    FriendlyErrorMsg.Text = "A problem has occurred on this web site. Please try again. " + "If this error continues, please contact support.";
                    
                    error = errorin.GetErrorLog(int.Parse(error_id));
                    ErrorId.Text = error.Error_Id.ToString();
                    DateTime_LB.Text = error.Date_Time;
                    ErrorDetailedMsg.Text = error.Error_DetailedMsg;
                    ErrorHandler.Text = error.Error_Handler;
                    InnerMessage.Text = error.Inner_Message;
                    InnerTrace.Text = error.Inner_Trace;
                }
            }
            else
            {
                Response.Redirect("../index.aspx", false);
            }

        }
    }
}