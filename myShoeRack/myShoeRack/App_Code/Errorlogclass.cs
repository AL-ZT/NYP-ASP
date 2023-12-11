using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace myShoeRack.App_Code
{
    public class Errorlogclass
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        private int _ErrorId = 0;
        private string _ErrorDetailedMsg = string.Empty;
        private string _Errorhandler = string.Empty;
        private string _InnerMessage = string.Empty;
        private string _InnerTrace = string.Empty;
        private string _DateTime = string.Empty;

        public Errorlogclass()
        {
        }

        public Errorlogclass(int error_id, string error_detailmsg, string error_handler, string inner_message, string inner_trace, string date_time)
        {
            _ErrorId = error_id;
            _ErrorDetailedMsg = error_detailmsg;
            _Errorhandler = error_handler;
            _InnerMessage = inner_message;
            _InnerTrace = inner_trace;
            _DateTime = date_time;
        }

        public Errorlogclass(int id) : this ( id, "", "", "", "", "")
        {
        }

        public int Error_Id
        {
            get { return _ErrorId; }
            set { _ErrorId = value; }
        }

        public string Error_DetailedMsg
        {
            get { return _ErrorDetailedMsg; }
            set { _ErrorDetailedMsg = value; }
        }

        public string Error_Handler
        {
            get { return _Errorhandler; }
            set { _Errorhandler = value; }
        }

        public string Inner_Message
        {
            get { return _InnerMessage; }
            set { _InnerMessage = value; }
        }

        public string Inner_Trace
        {
            get { return _InnerTrace; }
            set { _InnerTrace = value; }
        }

        public string Date_Time
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }

        public Errorlogclass GetErrorLog(int Error_id)
        {
            Errorlogclass errorinfo = null;
            string ErrorDetailedMsg, ErrorHandler, InnerMessage, InnerTrace, DateTime;

            string queryStr = "SELECT * FROM ErrorLog WHERE Error_Id = @ErrorId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@ErrorId", Error_id);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                ErrorDetailedMsg = dr["Error_DetailedMsg"].ToString();
                ErrorHandler = dr["Error_Handler"].ToString();
                InnerMessage = dr["Inner_Message"].ToString();
                InnerTrace = dr["Inner_Trace"].ToString();
                DateTime = dr["Date_Time"].ToString();
                errorinfo = new Errorlogclass(Error_id, ErrorDetailedMsg, ErrorHandler, InnerMessage, InnerTrace, DateTime);
            }
            else
            {
                errorinfo = null;
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return errorinfo;
        }

        public List<Errorlogclass> getErrorLoglist()
        {
            List<Errorlogclass> allerrorlist = new List<Errorlogclass>();
            int Id;
            string ErrorDetailedMsg, ErrorHandler, InnerMessage, InnerTrace, DateTime;
            String queryStr = "Select * from ErrorLog Order by Error_Id";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Id = int.Parse(dr["Error_Id"].ToString());
                ErrorDetailedMsg = dr["Error_DetailedMsg"].ToString();
                ErrorHandler = dr["Error_Handler"].ToString();
                InnerMessage = dr["Inner_Message"].ToString();
                InnerTrace = dr["Inner_Trace"].ToString();
                DateTime = dr["Date_Time"].ToString();
                Errorlogclass e = new Errorlogclass(Id, ErrorDetailedMsg, ErrorHandler, InnerMessage, InnerTrace, DateTime);
                allerrorlist.Add(e);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return allerrorlist;
        }
    }
}