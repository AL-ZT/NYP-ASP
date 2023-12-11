using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.UI.HtmlControls;
using myShoeRack.App_Code;
using System.Data;
using System.IO;
using System.Text;

namespace myShoeRack
{
    public partial class auditlog : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
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
                    //if (!IsPostBack)
                    //{
                    //    //do stuff
                    //}
                    checkOnlineUsers();
                }
            }
            else
            {
                Response.Redirect("index.aspx", false);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            //loadTable();
            loadTableData();    //load audit table data from db
        }

        protected void loadTableData()
        {
            try
            {
                string code, date, time, username, ipadd, desc, details, mdetails, pid, pname, pbrand;
                int pcost;
                string queryStr = "SELECT * FROM AuditLog ORDER BY a_Code DESC";

                SqlConnection conn = new SqlConnection(_connStr);
                SqlCommand cmd = new SqlCommand(queryStr, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    code = dr["a_Code"].ToString();
                    date = dr["a_Date"].ToString();
                    time = dr["a_Time"].ToString();
                    username = dr["a_Username"].ToString();
                    ipadd = dr["a_Ipaddr"].ToString();
                    desc = dr["a_Descript"].ToString();
                    details = dr["a_Details"].ToString();
                    mdetails = dr["a_Mdetails"].ToString();
                    pid = dr["product_id"].ToString();
                    pname = dr["product_name"].ToString();
                    pbrand = dr["product_brand"].ToString();
                    if (!Convert.IsDBNull(dr["product_cost"]))      //check if pcost db is !null
                    {
                        pcost = int.Parse(dr["product_cost"].ToString());
                    }
                    else
                    {
                        pcost = 0;
                    }
                    AuditLog audlog = new AuditLog(code, date, time, username, ipadd, desc, details, mdetails, pid, pname, pbrand, pcost);

                    //generate table structure
                    HtmlGenericControl tableRow = new HtmlGenericControl("div");
                    tableRow.Attributes["class"] = "table-row";
                    audittableph.Controls.Add(tableRow);    //audittableph = placeholder @ .aspx
                                                            //Controls.Add(audittableph);

                    HtmlGenericControl tableDivCode = new HtmlGenericControl("div");
                    tableDivCode.Attributes["class"] = "code";
                    tableDivCode.InnerText = code;
                    tableRow.Controls.Add(tableDivCode);

                    HtmlGenericControl tableDivDateTime = new HtmlGenericControl("div");
                    tableDivDateTime.Attributes["class"] = "datetime";
                    HtmlGenericControl tableSubDivDate = new HtmlGenericControl("div");
                    tableSubDivDate.Attributes["class"] = "date";
                    tableSubDivDate.InnerText = date;
                    HtmlGenericControl tableSubDivTime = new HtmlGenericControl("div");
                    tableSubDivDate.Attributes["class"] = "time";
                    tableSubDivTime.InnerText = time;

                    tableDivDateTime.Controls.Add(tableSubDivDate);
                    tableDivDateTime.Controls.Add(tableSubDivTime);
                    tableRow.Controls.Add(tableDivDateTime);

                    HtmlGenericControl tableDivUser = new HtmlGenericControl("div");
                    tableDivUser.Attributes["class"] = "user";
                    tableDivUser.InnerText = username;
                    tableRow.Controls.Add(tableDivUser);

                    HtmlGenericControl tableDivIp = new HtmlGenericControl("div");
                    tableDivIp.Attributes["class"] = "ip";
                    tableDivIp.InnerText = ipadd;
                    tableRow.Controls.Add(tableDivIp);

                    HtmlGenericControl tableDivDescript = new HtmlGenericControl("div");
                    tableDivDescript.Attributes["class"] = "descript";
                    tableDivDescript.InnerText = desc;
                    tableRow.Controls.Add(tableDivDescript);

                    HtmlGenericControl tableDivDetails = new HtmlGenericControl("div");
                    tableDivDetails.Attributes["class"] = "details";
                    //tableDivDetails.ID = details;
                    //tableDivDetails.InnerText = details;

                    HtmlGenericControl tableDivDetailsBtn = new HtmlGenericControl("div");
                    tableDivDetailsBtn.Attributes["class"] = "detailsBtn";
                    tableDivDetailsBtn.ID = "details" + code;
                    tableDivDetails.Controls.Add(tableDivDetailsBtn);

                    Button btnDetails = new Button();
                    btnDetails.Text = "Details";
                    btnDetails.CommandArgument = details + code;
                    btnDetails.Command += new CommandEventHandler(redirectmdetails);
                    //btnDetails.Click += new EventHandler(this.redirect);

                    tableDivDetailsBtn.Controls.Add(btnDetails);
                    tableDivDetails.Controls.Add(tableDivDetailsBtn);
                    tableRow.Controls.Add(tableDivDetails);
                }
                conn.Close();
                dr.Close();
                dr.Dispose();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void redirectmdetails(Object sender, CommandEventArgs e)
        {
            Button btn = (Button)sender;
            Response.Redirect(btn.CommandArgument);
        }

        protected void redirect(object sender, EventArgs e)     //NOT USED
        {
            string code, details;
            string queryStr = "SELECT a_Code, a_Details FROM AuditLog";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                code = dr["a_Code"].ToString();
                details = dr["a_Details"].ToString();
                Debug.WriteLine(code);
                //HtmlGenericControl divdetails = FindControl("details");
                //Control divdetails = FindControl("MainContent_details" + code);
                //HtmlGenericControl divdetails = (HtmlGenericControl)Page.FindControl("MainContent_details" + code);
                //Control divdetailschild = divdetails.Parent;

                //HtmlGenericControl test = (HtmlGenericControl)Page.FindControl("audittableph");
                //Control divdetails = this.Master.FindControl("MainContent").FindControl("MainContent_details" + code) as Control;

                //var checkBoxesInContainer = .Controls.OfType<CheckBox>();
                //Debug.WriteLine(Request.Form["MainContent_details" + code].ToString());

                //Debug.WriteLine(divdetails.Parent.ID);
                Response.Redirect(details + code, false);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
        }

        protected void loadTable()  //NOT USED
        {
            //generate table
            HtmlGenericControl tableRow = new HtmlGenericControl("div");
            tableRow.Attributes["class"] = "table-row";
            audittableph.Controls.Add(tableRow);    //audittableph = placeholder @ .aspx file

            HtmlGenericControl tableDivCode = new HtmlGenericControl("div");
            tableDivCode.Attributes["class"] = "code";
            tableRow.Controls.Add(tableDivCode);

            HtmlGenericControl tableDivDateTime = new HtmlGenericControl("div");
            tableDivDateTime.Attributes["class"] = "datetime";
            HtmlGenericControl tableSubDivDate = new HtmlGenericControl("div");
            tableSubDivDate.Attributes["class"] = "date";
            HtmlGenericControl tableSubDivTime = new HtmlGenericControl("div");
            tableSubDivDate.Attributes["class"] = "time";

            tableDivDateTime.Controls.Add(tableSubDivDate);
            tableDivDateTime.Controls.Add(tableSubDivTime);
            tableRow.Controls.Add(tableDivDateTime);

            HtmlGenericControl tableDivUser = new HtmlGenericControl("div");
            tableDivUser.Attributes["class"] = "user";
            tableRow.Controls.Add(tableDivUser);

            HtmlGenericControl tableDivIp = new HtmlGenericControl("div");
            tableDivIp.Attributes["class"] = "ip";
            tableRow.Controls.Add(tableDivIp);

            HtmlGenericControl tableDivDescript = new HtmlGenericControl("div");
            tableDivDescript.Attributes["class"] = "descript";
            tableRow.Controls.Add(tableDivDescript);

            HtmlGenericControl tableDivDetails = new HtmlGenericControl("div");
            tableDivDetails.Attributes["class"] = "details";
            tableRow.Controls.Add(tableDivDetails);
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

        protected void btn_clear_Click(object sender, EventArgs e)      //clear audit db
        {
            try
            {
                AuditLog audit = new AuditLog();
                audit.clearAuditDb();
                Response.Redirect("auditlog.aspx", false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void btn_download_Click(object sender, EventArgs e)       //download txt file
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/log.txt");
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.Flush();
                    Response.TransmitFile(file.FullName);
                    //Response.End();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void checkOnlineUsers()
        {
            string queryStr = "SELECT COUNT (DISTINCT Username) FROM LoggedInUsers";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            int onlineusers = Convert.ToInt32(cmd.ExecuteScalar());
            conn.Close();
            lbl_onlineusers.Text = "Logged on users: " + onlineusers.ToString();
            Debug.WriteLine(onlineusers);
        }

        protected void btn_search_submit_Click(object sender, EventArgs e)      //search
        {
            try
            {
                string code, date, time, username, ipadd, desc, details, mdetails, pid, pname, pbrand;
                int pcost;
                string searchinput = tb_search.Text;

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "script", "$('#masterdiv div').empty();");
                StringBuilder strScript = new StringBuilder();
                strScript.Append("console.log('TEST');");
                strScript.Append("$(document).ready(function(){");
                //strScript.Append("alert('TEST I SHOULD BE ABLE TO SEE THIS.');");
                strScript.Append("$('#placeholdertablediv div').remove();");
                strScript.Append("});");

                ScriptManager.RegisterStartupScript(updatepanel, updatepanel.GetType(), "Script", strScript.ToString(), true);

                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM AuditLog WHERE (a_Code LIKE @search) OR" +
                        " (a_Date LIKE @search) OR (a_Time LIKE @search) OR (a_Username LIKE @search) OR" +
                        "(a_Ipaddr LIKE @search) OR (a_Descript LIKE @search) ORDER BY a_Code DESC"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@search", string.Format("%{0}%", searchinput));
                            cmd.Connection = conn;
                            conn.Open();
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    code = dr["a_Code"].ToString();
                                    date = dr["a_Date"].ToString();
                                    time = dr["a_Time"].ToString();
                                    username = dr["a_Username"].ToString();
                                    ipadd = dr["a_Ipaddr"].ToString();
                                    desc = dr["a_Descript"].ToString();
                                    details = dr["a_Details"].ToString();
                                    mdetails = dr["a_Mdetails"].ToString();
                                    pid = dr["product_id"].ToString();
                                    pname = dr["product_name"].ToString();
                                    pbrand = dr["product_brand"].ToString();
                                    if (!Convert.IsDBNull(dr["product_cost"]))      //check if pcost db is !null
                                    {
                                        pcost = int.Parse(dr["product_cost"].ToString());
                                    }
                                    else
                                    {
                                        pcost = 0;
                                    }
                                    AuditLog audlog = new AuditLog(code, date, time, username, ipadd, desc, details, mdetails, pid, pname, pbrand, pcost);

                                    //generate table structure
                                    HtmlGenericControl tableRow = new HtmlGenericControl("div");
                                    tableRow.Attributes["class"] = "table-row";
                                    searchresults.Controls.Add(tableRow);    //audittableph = placeholder @ .aspx
                                                                             //Controls.Add(audittableph);

                                    HtmlGenericControl tableDivCode = new HtmlGenericControl("div");
                                    tableDivCode.Attributes["class"] = "code";
                                    tableDivCode.InnerText = code;
                                    tableRow.Controls.Add(tableDivCode);

                                    HtmlGenericControl tableDivDateTime = new HtmlGenericControl("div");
                                    tableDivDateTime.Attributes["class"] = "datetime";
                                    HtmlGenericControl tableSubDivDate = new HtmlGenericControl("div");
                                    tableSubDivDate.Attributes["class"] = "date";
                                    tableSubDivDate.InnerText = date;
                                    HtmlGenericControl tableSubDivTime = new HtmlGenericControl("div");
                                    tableSubDivDate.Attributes["class"] = "time";
                                    tableSubDivTime.InnerText = time;

                                    tableDivDateTime.Controls.Add(tableSubDivDate);
                                    tableDivDateTime.Controls.Add(tableSubDivTime);
                                    tableRow.Controls.Add(tableDivDateTime);

                                    HtmlGenericControl tableDivUser = new HtmlGenericControl("div");
                                    tableDivUser.Attributes["class"] = "user";
                                    tableDivUser.InnerText = username;
                                    tableRow.Controls.Add(tableDivUser);

                                    HtmlGenericControl tableDivIp = new HtmlGenericControl("div");
                                    tableDivIp.Attributes["class"] = "ip";
                                    tableDivIp.InnerText = ipadd;
                                    tableRow.Controls.Add(tableDivIp);

                                    HtmlGenericControl tableDivDescript = new HtmlGenericControl("div");
                                    tableDivDescript.Attributes["class"] = "descript";
                                    tableDivDescript.InnerText = desc;
                                    tableRow.Controls.Add(tableDivDescript);

                                    HtmlGenericControl tableDivDetails = new HtmlGenericControl("div");
                                    tableDivDetails.Attributes["class"] = "details";
                                    //tableDivDetails.ID = details;
                                    //tableDivDetails.InnerText = details;

                                    HtmlGenericControl tableDivDetailsBtn = new HtmlGenericControl("div");
                                    tableDivDetailsBtn.Attributes["class"] = "detailsBtn";
                                    tableDivDetailsBtn.ID = "details" + code;
                                    tableDivDetails.Controls.Add(tableDivDetailsBtn);

                                    Button btnDetails = new Button();
                                    btnDetails.Text = "Details";
                                    btnDetails.ClientIDMode = ClientIDMode.Static;
                                    btnDetails.Attributes["class"] = "resultbtn";
                                    btnDetails.ID = code;
                                    //btnDetails.CommandArgument = details + code;
                                    //btnDetails.Command += new CommandEventHandler(redirectmdetails);

                                    tableDivDetailsBtn.Controls.Add(btnDetails);
                                    tableDivDetails.Controls.Add(tableDivDetailsBtn);
                                    tableRow.Controls.Add(tableDivDetails);

                                    StringBuilder strScript2 = new StringBuilder();
                                    strScript2.Append("console.log('TEST');");
                                    strScript2.Append("$(document).ready(function(){");
                                    strScript2.AppendFormat("$('.detailsBtn').on('click', 'input', function ()");
                                    //strScript2.AppendFormat("$('.detailsBtn').on('click', .resultbtn, function ()");
                                    //strScript2.AppendFormat("$('.detailsBtn').on('click', #{0}, function ()", code);
                                    strScript2.Append("{");
                                    strScript2.AppendFormat("window.location.replace('auditlogDetails.aspx?code={0}')", code);
                                    strScript2.Append("});");

                                    ScriptManager.RegisterStartupScript(updatepanel, updatepanel.GetType(), "Script", strScript2.ToString(), true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        //protected override void Render(HtmlTextWriter writer)     /overrides default render method
        //{
        //    //1st method
        //    StringWriter output = new StringWriter();
        //    base.Render(new HtmlTextWriter(output));
        //    //This is the rendered HTML of the page
        //    string outputAsString = output.ToString();
        //    Debug.WriteLine(outputAsString);          

        //    //writer.Write(outputAsString); ;   uncomment at the end pls
        //    writer.Dispose();

        //}
    }
}