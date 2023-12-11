using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace myShoeRack
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
        string _connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                DataRow dr;
                dt.Columns.Add("sno");
                dt.Columns.Add("product_id");
                dt.Columns.Add("product_image");
                dt.Columns.Add("product_name");
                dt.Columns.Add("product_brand");
                dt.Columns.Add("product_cost");
                dt.Columns.Add("totalprice");

                NVPAPICaller payPalCaller = new NVPAPICaller();

                string retMsg = "";
                string token = "";
                string PayerID = "";
                NVPCodec decoder = new NVPCodec();
                token = Session["token"].ToString();

                bool ret = payPalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    SqlConnection scon = new SqlConnection(_connStr);

                    //DataTable dt = new DataTable();
                    //DataRow dr;
                    //dt = (DataTable)Session["buyitems"];
                    //int sr;
                    //sr = dt.Rows.Count;

                    //dr = dt.NewRow();
                    //String myquery = "SELECT * from Products WHERE product_id=" + Request.QueryString["id"];
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.CommandText = myquery;
                    //cmd.Connection = scon;
                    //SqlDataAdapter da = new SqlDataAdapter();
                    //da.SelectCommand = cmd;
                    //DataSet ds = new DataSet();
                    //da.Fill(ds);
                    //dr["sno"] = sr + 1;
                    //dr["productid"] = ds.Tables[0].Rows[0]["productid"].ToString();
                    //dr["productname"] = ds.Tables[0].Rows[0]["productname"].ToString();
                    //dr["productimage"] = ds.Tables[0].Rows[0]["productimage"].ToString();
                    //dr["quantity"] = Request.QueryString["quantity"];
                    //dr["price"] = ds.Tables[0].Rows[0]["price"].ToString();
                    //int price = Convert.ToInt16(ds.Tables[0].Rows[0]["price"].ToString());
                    //int quantity = Convert.ToInt16(Request.QueryString["quantity"].ToString());
                    //int totalprice = price * quantity;
                    //dr["totalprice"] = totalprice;
                    //dt.Rows.Add(dr);
                    //GridViewReview.DataSource = dt;
                    //GridViewReview.DataBind();

                    dt = (DataTable)Session["buyitems"];
                    GridViewReview.DataSource = dt;
                    GridViewReview.DataBind();

                    Session["buyitems"] = dt;
                    lbl_subtotal.Text = Session["totalPayable"].ToString();


                    Session["payerId"] = PayerID;


                    lbl_orderdate.Text = DateTime.Now.ToString();
                    lbl_total.Text = Convert.ToDecimal(decoder["AMT"].ToString()).ToString();
                    lbl_address.Text = decoder["SHIPTOSTREET"].ToString();
                    lbl_city.Text = decoder["SHIPTOCITY"].ToString();
                    lbl_country.Text = decoder["SHIPTOCOUNTRYCODE"].ToString();
                    lbl_postalcode.Text = decoder["SHIPTOZIP"].ToString();

                    // Verify total payment amount as set on CheckoutStart.aspx.
                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["totalPayable"].ToString());
                        decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());

                        //if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
                        //{
                        //    Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                        //}
                    }
                    catch (Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                    }

                    // Get DB context.
                    //ProductContext _db = new ProductContext();

                    // Add order to DB.
                    //_db.Orders.Add(myOrder);
                    //_db.SaveChanges();

                    // Get the shopping cart items and process them.
                    //using (WingtipToys.Logic.ShoppingCartActions usersShoppingCart = new WingtipToys.Logic.ShoppingCartActions())
                    //{
                    //    List<CartItem> myOrderList = usersShoppingCart.GetCartItems();

                    //    // Add OrderDetail information to the DB for each product purchased.
                    //    for (int i = 0; i < myOrderList.Count; i++)
                    //    {
                    //        // Create a new OrderDetail object.
                    //        var myOrderDetail = new OrderDetail();
                    //        myOrderDetail.OrderId = myOrder.OrderId;
                    //        myOrderDetail.Username = User.Identity.Name;
                    //        myOrderDetail.ProductId = myOrderList[i].ProductId;
                    //        myOrderDetail.Quantity = myOrderList[i].Quantity;
                    //        myOrderDetail.UnitPrice = myOrderList[i].Product.UnitPrice;

                    //        // Add OrderDetail to DB.
                    //        //_db.OrderDetails.Add(myOrderDetail);
                    //        //_db.SaveChanges();
                    //    }

                    //    // Set OrderId.
                    //    Session["currentOrderId"] = myOrder.OrderId;

                        // Display Order information.
                        //List<Order> orderList = new List<Order>();
                        //orderList.Add(myOrder);
                        //ShipInfo.DataSource = orderList;
                        //ShipInfo.DataBind();

                        // Display OrderDetails.
                        //OrderItemList.DataSource = myOrderList;
                        //OrderItemList.DataBind();

                    
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
        }

        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("CheckoutConfirmation.aspx", false);
        }
    }
}