using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace myShoeRack
{
    public partial class Cart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This is false when the first time the page is loaded and is true when the page is submitted and processed. 
            // This enables users to write the code depending on if the PostBack is true or false (with the use of the function Page.IsPostBack()).
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


                String mycon = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
                SqlConnection scon = new SqlConnection(mycon);

                if (Request.QueryString["id"] != null)
                {
                    if (Session["Buyitems"] == null)
                    {

                        dr = dt.NewRow();
                        String myquery = "SELECT * from Products WHERE product_id=" + Request.QueryString["id"];
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = myquery;
                        cmd.Connection = scon;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dr["sno"] = 1;
                        dr["product_id"] = ds.Tables[0].Rows[0]["product_id"].ToString();
                        dr["product_name"] = ds.Tables[0].Rows[0]["product_name"].ToString();
                        dr["product_brand"] = ds.Tables[0].Rows[0]["product_brand"].ToString();
                        dr["product_image"] = ds.Tables[0].Rows[0]["product_image"].ToString();
                        //dr["quantity"] = Request.QueryString["quantity"];
                        dr["product_cost"] = ds.Tables[0].Rows[0]["product_cost"].ToString();
                        int price = Convert.ToInt16(ds.Tables[0].Rows[0]["product_cost"].ToString());
                        //int quantity = Convert.ToInt16(Request.QueryString["quantity"].ToString());
                        //int totalprice = price * quantity;
                        //dr["totalprice"] = price;

                        dt.Rows.Add(dr);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();

                        Session["buyitems"] = dt;
                        lbl_subtotal.Text = grandtotal().ToString();
                        //Response.Redirect("AddToCart.aspx");

                    }
                    else
                    {

                        dt = (DataTable)Session["buyitems"];
                        int sr;
                        sr = dt.Rows.Count;

                        dr = dt.NewRow();
                        String myquery = "SELECT * from Products WHERE product_id=" + Request.QueryString["id"];
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = myquery;
                        cmd.Connection = scon;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        dr["sno"] = sr + 1;
                        dr["product_id"] = ds.Tables[0].Rows[0]["product_id"].ToString();
                        dr["product_name"] = ds.Tables[0].Rows[0]["product_name"].ToString();
                        dr["product_image"] = ds.Tables[0].Rows[0]["product_image"].ToString();
                        dr["product_brand"] = ds.Tables[0].Rows[0]["product_brand"].ToString();
                        //dr["quantity"] = Request.QueryString["quantity"];
                        dr["product_cost"] = ds.Tables[0].Rows[0]["product_cost"].ToString();
                        int price = Convert.ToInt16(ds.Tables[0].Rows[0]["product_cost"].ToString());
                        //int quantity = Convert.ToInt16(Request.QueryString["quantity"].ToString());
                        //int totalprice = price * quantity;
                        //dr["totalprice"] = price;
                        dt.Rows.Add(dr);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();

                        Session["buyitems"] = dt;
                        lbl_subtotal.Text = grandtotal().ToString();
                        //Response.Redirect("AddToCart.aspx");

                    }
                }
                else
                {
                    dt = (DataTable)Session["buyitems"];
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    if (GridView1.Rows.Count > 0)
                    {
                        lbl_subtotal.Text = grandtotal().ToString();

                    }


                }

            }
        }

        public int grandtotal()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["buyitems"];
            int nrow = dt.Rows.Count;
            int i = 0;
            int gtotal = 0;
            while (i < nrow)
            {
                gtotal = gtotal + Convert.ToInt32(dt.Rows[i]["product_cost"].ToString());

                i = i + 1;
            }
            return gtotal;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["buyitems"];


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                int sr;
                int sr1;
                string qdata;
                string dtdata;
                sr = Convert.ToInt32(dt.Rows[i]["sno"].ToString());
                TableCell cell = GridView1.Rows[e.RowIndex].Cells[0];
                qdata = cell.Text;
                dtdata = sr.ToString();
                sr1 = Convert.ToInt32(qdata);

                if (sr == sr1)
                {
                    dt.Rows[i].Delete();
                    dt.AcceptChanges();
                    //Label1.Text = "Item Has Been Deleted From Shopping Cart";
                    break;

                }
            }

            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                dt.Rows[i - 1]["sno"] = i;
                dt.AcceptChanges();
            }

            Session["buyitems"] = dt;
            Response.Redirect("Cart.aspx");
        }

        protected void btnCheckout(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                dt = (DataTable)Session["buyitems"];
                int sr;
                sr = dt.Rows.Count;
                if (sr == 0)
                {
                    Response.Write("<script>alert('Cart empty');</script>");
                }
                else
                {
                    Response.Redirect("Checkout.aspx");
                }
            } catch (Exception ex)
            {
                Response.Write("<script>alert('Cart empty');</script>");
            }
            
                
        }
    }
}