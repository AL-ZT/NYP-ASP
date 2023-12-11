using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace myShoeRack.App_Code
{
    public class Adminclass
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        private int _userId = 0;
        private string _userEmail = string.Empty;
        private string _username = string.Empty;
        private string _userPasshash = string.Empty;
        private string _userHashsalt = string.Empty;
        private int _userCounter = 0;
        private string _userAddress = string.Empty;
        private string _adminStatus;
        private int _bannedUser = 0;

        //default
        public Adminclass()
        {
        }

        //Getting all data from the object
        public Adminclass(int user_id, string user_email, string user_name,
                       string user_passhash, string user_hashsalt, int user_counter, string user_address, string admin_status, int banned_user)
        {
            _userId = user_id;
            _userEmail = user_email;
            _username = user_name;
            _userPasshash = user_passhash;
            _userHashsalt = user_hashsalt;
            _userCounter = user_counter;
            _userAddress = user_address;
            _adminStatus = admin_status;
            _bannedUser = banned_user;
        }

        //Getting data with only userID
        public Adminclass(int userId)
          : this(userId, "", "", "", "", 0, "", "", 0)
        {
        }

        // Get/Set the attributes
        // Note the attribute name (e.g. User_Id) is same as the actual database field name.
        public int User_Id
        {
            get { return _userId; }
            set { _userId = value; }
        }
        public string User_Email
        {
            get { return _userEmail; }
            set { _userEmail = value; }
        }
        public string User_Name
        {
            get { return _username; }
            set { _username = value; }
        }
        public string User_Passhash
        {
            get { return _userPasshash; }
            set { _userPasshash = value; }
        }
        public string User_Hashsalt
        {
            get { return _userHashsalt; }
            set { _userHashsalt = value; }
        }
        public int User_Counter
        {
            get { return _userCounter; }
            set { _userCounter = value; }
        }
        public string User_Address
        {
            get { return _userAddress; }
            set { _userAddress = value; }
        }
        public string Admin_Status
        {
            get { return _adminStatus; }
            set { _adminStatus = value; }
        }
        public int banned_user
        {
            get { return _bannedUser; }
            set { _bannedUser = value; }
        }

        public Adminclass GetUser(string email)
        {
            //create a null object
            Adminclass userinfo = null;

            //define the attribute list
            string user_email, user_name, user_passhash, user_hashsalt, user_address;
            string admin_status;
            int user_counter, banned_user, user_Id;

            //Preparing the SQL statement
            string queryStr = "SELECT * FROM Users WHERE user_email = @userEmail";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userEmail", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            //Check if there are any resultsets
            if (dr.Read())
            {
                user_Id = int.Parse(dr["user_Id"].ToString());
                user_name = dr["user_name"].ToString();
                user_passhash = dr["user_passhash"].ToString();
                user_hashsalt = dr["user_hashsalt"].ToString();
                user_address = dr["user_address"].ToString();
                user_counter = int.Parse(dr["user_counter"].ToString());
                admin_status = dr["admin_status"].ToString();
                banned_user = int.Parse(dr["banned_user"].ToString());
                userinfo = new Adminclass(user_Id, email, user_name, user_passhash, user_hashsalt, user_counter, user_address, admin_status, banned_user);
            }
            else
            {
                userinfo = null;
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return userinfo;
        }

        public List<Adminclass> GetUserAll()
        {
            //create a lsit of users that are active.
            List<Adminclass> alluserlist = new List<Adminclass>();

            //define the attribute list
            String user_email, user_name, user_passhash, user_hashsalt, user_address, admin_status;
            int user_counter, user_Id, banned_user;

            //Preparing the SQL statement
            string queryStr = "Select * from Users Order by user_name";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                user_Id = int.Parse(dr["user_Id"].ToString());
                user_email = dr["user_email"].ToString();
                user_name = dr["user_name"].ToString();
                user_counter = int.Parse(dr["user_counter"].ToString());
                user_address = dr["user_address"].ToString();
                admin_status = dr["admin_status"].ToString();
                user_passhash = dr["user_passhash"].ToString();
                user_hashsalt = dr["user_hashsalt"].ToString();
                banned_user = int.Parse(dr["banned_user"].ToString());
                Adminclass a = new Adminclass(user_Id, user_email, user_name, user_passhash, user_hashsalt, user_counter, user_address, admin_status, banned_user);
                alluserlist.Add(a);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return alluserlist;
        }

        public List<Adminclass> GetBanListUsers()
        {
            //create a lsit of users that are banned.
            List<Adminclass> banuserlist = new List<Adminclass>();

            //define the attribute list
            String user_email, user_name, user_passhash, user_hashsalt, user_address, admin_status;
            int user_counter, user_Id, banned_user;

            //Preparing the SQL statement
            string queryStr = "Select * from Users where banned_user = 1";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                user_Id = int.Parse(dr["user_Id"].ToString());
                user_email = dr["user_email"].ToString();
                user_name = dr["user_name"].ToString();
                user_counter = int.Parse(dr["user_counter"].ToString());
                user_address = dr["user_address"].ToString();
                admin_status = dr["admin_status"].ToString();
                user_passhash = dr["user_passhash"].ToString();
                user_hashsalt = dr["user_hashsalt"].ToString();
                banned_user = int.Parse(dr["banned_user"].ToString());
                Adminclass a = new Adminclass(user_Id, user_email, user_name, user_passhash, user_hashsalt, user_counter, user_address, admin_status, banned_user);
                banuserlist.Add(a);
            }

            conn.Close();
            dr.Close();
            dr.Dispose();
            return banuserlist;
        }

        public int banUser(string userEmail)
        {
            int result = 0;

            //creating insert statement
            string queryStr = "UPDATE Users SET banned_user = @banned_users where user_email = @user_email";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@banned_users", 1);
            cmd.Parameters.AddWithValue("@user_email", userEmail);
            conn.Open();
            result += cmd.ExecuteNonQuery(); // Returns no. of rows affected. Must be > 0

            //close sql connection when done
            conn.Close();
            return result;
        }

        public int unbanUser(string userEmail)
        {
            int result = 0;
            //creating insert statement
            string queryStr = "UPDATE Users SET banned_user = @banned_users where user_email = @user_email";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@banned_users", 0);
            cmd.Parameters.AddWithValue("@user_email", userEmail);
            conn.Open();
            result += cmd.ExecuteNonQuery(); // Returns no. of rows affected. Must be > 0

            //close sql connection when done
            conn.Close();
            return result;
        }

        public Boolean checkban(string email)
        {
            //define the attribute list
            int banned_user;
            Boolean result = false;

            string queryStr = "SELECT * FROM Users WHERE user_email = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                banned_user = int.Parse(dr["banned_user"].ToString());
                if (banned_user == 0)
                {
                    result = false;
                }
                else if (banned_user == 1)
                {
                    result = true; 
                }
            }
            conn.Close();
            dr.Close();
            dr.Dispose();

            return result;
        }

        public Boolean checkcounter(string email)
        {
            //define the attribute list
            int user_counter;
            Boolean result = false;

            string queryStr = "SELECT user_counter  FROM Users WHERE user_email = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                user_counter = int.Parse(dr["user_counter"].ToString());
                if (user_counter >= 0 && user_counter < 3 )
                {
                    result = false;
                }
                else if ( user_counter >= 3)
                {
                    result = true;
                }
            }
            conn.Close();
            dr.Close();
            dr.Dispose();

            return result;
        }

        public Boolean checkAdmin(string email)
        {
            string admin_status = string.Empty;
            Boolean result = false;
            string queryStr = "SELECT admin_status FROM Users WHERE user_email = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                admin_status = dr["admin_status"].ToString();
                if (admin_status == "normal")
                {
                    result = false;
                }
                else if (admin_status == "admin")
                {
                    result = true;
                }
            }
            return result;
        }

        public int resetcounter(int userID)
        {
            int result = 0;
            string queryStr = "UPDATE Users SET user_counter = @user_counter WHERE user_Id = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@user_counter", 0);
            cmd.Parameters.AddWithValue("@userId", userID);
            conn.Open();
            result += cmd.ExecuteNonQuery(); // Returns no. of rows affected. Must be > 0

            //close sql connection when done
            conn.Close();
            return result;
        }
    }
}