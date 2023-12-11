using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using myShoeRack.App_Code;

namespace myShoeRack
{
    public partial class auditlogDetails : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Adminclass adminin = new Adminclass();
                if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
                {
                    string email = Session["LoggedIn"].ToString();
                    Boolean checkemail = adminin.checkAdmin(email);
                    if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value) || checkemail != true)
                    {
                        //SHANG WEE'S intrusion logging stuff
                        IntrustionLogging();
                        Response.Redirect("index.aspx", false);
                        System.Diagnostics.Debug.WriteLine("someone try to access the admin page!!");
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            //do stuff
                            int code = int.Parse(Request.QueryString["Code"].ToString());

                            AuditLog audit = null;
                            string date, time, userid, username, ipadd, descript, details, mdetails, hp, address, pname, pbrand, platform;
                            int dbcode, pid, pcost;
                            string queryStr = "SELECT * FROM AuditLog WHERE a_Code = @code";
                            SqlConnection conn = new SqlConnection(_connStr);
                            SqlCommand cmd = new SqlCommand(queryStr, conn);
                            cmd.Parameters.AddWithValue("@code", code);
                            conn.Open();
                            SqlDataReader dr = cmd.ExecuteReader();
                            if (dr.Read())
                            {
                                dbcode = int.Parse(dr["a_Code"].ToString());
                                date = dr["a_Date"].ToString();
                                time = dr["a_Time"].ToString();

                                if (dr["a_Userid"] != DBNull.Value)
                                {
                                    userid = dr["a_Userid"].ToString();
                                }
                                else
                                {
                                    userid = null;
                                }
                                username = dr["a_Username"].ToString();
                                ipadd = dr["a_Ipaddr"].ToString();
                                descript = dr["a_Descript"].ToString();
                                details = dr["a_Details"].ToString();
                                mdetails = dr["a_Mdetails"].ToString();

                                if (dr["register_hp"] != DBNull.Value)
                                {
                                    hp = dr["register_hp"].ToString();
                                }
                                else
                                {
                                    hp = null;
                                }

                                if (dr["register_address"] != DBNull.Value)
                                {
                                    address = dr["register_address"].ToString();
                                }
                                else
                                {
                                    address = null;
                                }

                                if (dr["product_id"] != DBNull.Value)
                                {
                                    pid = int.Parse(dr["product_id"].ToString());
                                }
                                else
                                {
                                    pid = -1;
                                }

                                if (dr["product_name"] != DBNull.Value)
                                {
                                    pname = dr["product_name"].ToString();
                                }
                                else
                                {
                                    pname = null;
                                }

                                if (dr["product_brand"] != DBNull.Value)
                                {
                                    pbrand = dr["product_brand"].ToString();
                                }
                                else
                                {
                                    pbrand = null;
                                }

                                if (dr["product_cost"] != DBNull.Value)
                                {
                                    pcost = int.Parse(dr["product_cost"].ToString());
                                }
                                else
                                {
                                    pcost = -1;
                                }
                                if (dr["platform"] != DBNull.Value)
                                {
                                    platform = dr["platform"].ToString();
                                }
                                else
                                {
                                    platform = null;
                                }

                                audit = new AuditLog(dbcode.ToString(), date, time, userid, username, ipadd, descript,
                                    details, mdetails, hp, address, pid.ToString(), pname, pbrand, pcost, platform);
                            }
                            else
                            {
                                audit = null;
                            }
                            conn.Close();
                            dr.Close();
                            dr.Dispose();

                            lbl_code.Text = audit.a_Code;
                            lbl_userid.Text = audit.a_Userid;
                            lbl_username.Text = audit.a_Username;
                            lbl_date.Text = audit.a_Date;
                            lbl_time.Text = audit.a_Time;
                            lbl_ipadd.Text = audit.a_Ipaddr;
                            lbl_platform.Text = audit.platform;
                            //lbl_add.Text = audit.register_address;
                            //lbl_hp.Text = audit.register_hp;
                            //lbl_pid.Text = audit.product_id;
                            //lbl_pname.Text = audit.product_name;
                            //lbl_pbrand.Text = audit.product_brand;
                            //lbl_pcost.Text = audit.product_cost.ToString();
                            lbl_details.Text = audit.a_Mdetails;
                        }
                    }
                }
                else
                {
                    Response.Redirect("index.aspx", false);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //SHANG WEE'S
        protected void IntrustionLogging()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connStr))
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
    }
}