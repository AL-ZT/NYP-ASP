using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace myShoeRack
{
    public partial class CheckoutStart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NVPAPICaller payPalCaller = new NVPAPICaller();
            string retMsg = "";
            string token = "";

            try
            {
                if (Session["totalPayable"] != null)
                {
                    string amt = Session["totalPayable"].ToString();

                    Response.Write("<script>alert('" + amt + "');</script>");

                    bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
                    if (ret)
                    {
                        Session["token"] = token;
                        
                        Response.Redirect(retMsg, false);
                    }
                    else
                    {
                        Response.Write("<script>alert('" + retMsg + "');</script>");
                    }
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?ErrorCode=AmtMissing");
                }
            } catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            
        }
    }
}