using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;


namespace myShoeRack.App_Code
{
    public class Intrusionlog
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        private int _IntrusionId = 0;
        private string _IntrusionEmail = string.Empty;
        private string _IntrusionDetail = string.Empty;
        private string _IntrusionDateTime = string.Empty;

        public Intrusionlog()
        {
        }

        public Intrusionlog(int intrusion_id, string intrusion_email, string intrusion_detail, string intrusion_date_time)
        {
            _IntrusionId = intrusion_id;
            _IntrusionEmail = intrusion_email;
            _IntrusionDetail = intrusion_detail;
            _IntrusionDateTime = intrusion_date_time;
        }

        public Intrusionlog(string email)
            :this(0, email, "", "")
        {
        }

        public int Intrusion_Id
        {
            get { return _IntrusionId; }
            set { _IntrusionId = value; }
        }

        public string Intrusion_Email
        {
            get { return _IntrusionEmail; }
            set { _IntrusionEmail = value; }
        }

        public string Intrusion_Detail
        {
            get { return _IntrusionDetail; }
            set { _IntrusionDetail = value; }
        }

        public string Intrusion_Date_Time
        {
            get { return _IntrusionDateTime; }
            set { _IntrusionDateTime = value; }
        }

        public List<Intrusionlog> GetIntrusionAllByUsers(string email)
        {
            List<Intrusionlog> allintrusionlist = new List<Intrusionlog>();
            int intrusion_id;
            string intrusion_detail, intrusion_date_time;

            //Preparing the SQL statement
            string queryStr = "Select * from intrusionLog where Intrusion_Email = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            //Check if there are any resultsets
            if (dr.Read())
            {
                intrusion_id = int.Parse(dr["Intrusion_Id"].ToString());
                intrusion_detail = dr["Intrusion_Detail"].ToString();
                intrusion_date_time = dr["Intrusion_Date_Time"].ToString();
                Intrusionlog o = new Intrusionlog(intrusion_id, email, intrusion_detail , intrusion_date_time);
                allintrusionlist.Add(o);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return allintrusionlist;
        }

        public List<Intrusionlog> GetIntrusionAll()
        {
            List<Intrusionlog> allintrusionlist = new List<Intrusionlog>();
            int intrusion_id;
            string intrusion_email, intrusion_detail, intrusion_date_time;
            //Preparing the SQL statement
            string queryStr = "Select * from intrusionLog Order by Intrusion_Date_Time";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                intrusion_id = int.Parse(dr["Intrusion_Id"].ToString());
                intrusion_email = dr["Intrusion_Email"].ToString();
                intrusion_detail = dr["Intrusion_Detail"].ToString();
                intrusion_date_time = dr["Intrusion_Date_Time"].ToString();
                Intrusionlog o = new Intrusionlog(intrusion_id, intrusion_email, intrusion_detail, intrusion_date_time);
                allintrusionlist.Add(o);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return allintrusionlist;
        }
    }
}