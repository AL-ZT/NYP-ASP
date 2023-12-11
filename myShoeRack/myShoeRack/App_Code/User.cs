using System;
using System.Collections.Generic;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace myShoeRack.App_Code
{
    public class User
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
        private string _phoneNumber = string.Empty;
        private string _enable2fa = string.Empty;
        private string _iv = string.Empty;
        private string _key = string.Empty;
        private int _bannedUser = 0;

        // Default constructor
        public User()
        {
        }

        // Constructor that take in all data required to build a Product object
        public User(int user_id, string user_email, string user_name,
                       string user_passhash, string user_hashsalt, int user_counter, string user_address, string admin_status, string phoneNumber, string enable2fa, string iv, string key, int banned_user)
        {
            _userId = user_id;
            _userEmail = user_email;
            _username = user_name;
            _userPasshash = user_passhash;
            _userHashsalt = user_hashsalt;
            _userCounter = user_counter;
            _userAddress = user_address;
            _adminStatus = admin_status;
            _phoneNumber = phoneNumber;
            _enable2fa = enable2fa;
            _iv = iv;
            _key = key;
            _bannedUser = banned_user;
        }

        // Constructor that take in all except product ID -- *change this comment*
        public User(string user_email, string user_name,
                       string user_passhash, string user_hashsalt, int user_counter, string user_address, string admin_status, string phoneNumber, string enable2fa, string iv, string key, int banned_user)
            : this(0, user_email, user_name, user_passhash, user_hashsalt, user_counter, user_address, admin_status, phoneNumber, enable2fa, iv, key, banned_user)
        {
        }

        // Constructor that take in only Product ID. The other attributes will be set to 0 or empty. -- *change this comment*
        public User(int userId)
            : this(userId, "", "", "", "", 0, "", "", "", "", "", "", 0)
        {
        }

        // Constructor without IV, Key, phone number and enable2fa -JW
        public User(int userid, string useremail, string username, string userpw, string usersalt, int usercounter, string useraddress, string adminstatus, int banneduser)
        {
            _userId = userid;
            _userEmail = useremail;
            _username = username;
            _userPasshash = userpw;
            _userHashsalt = usersalt;
            _userCounter = usercounter;
            _userAddress = useraddress;
            _adminStatus = adminstatus;
            _bannedUser = banneduser;
        }

        // Get/Set the attributes of the User object.
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
        public string Phone_Number
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }
        public string Enable_2FA
        {
            get { return _enable2fa; }
            set { _enable2fa = value; }
        }
        public string IV
        {
            get { return _iv; }
            set { _iv = value; }
        }
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        public int banned_user
        {
            get { return _bannedUser; }
            set { _bannedUser = value; }
        }

        public User GetUser(int userId)
        {
            User userInfo = null;
            string user_email, username, user_passhash, user_hashsalt, user_address, twofa_enabled;
            string admin_status, ivValue, ivKey, phone_number;
            int user_counter, user_Id, banned_user;
            string queryStr = "SELECT * FROM Users WHERE user_Id = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", userId);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            //Check if there are any resultsets
            if (dr.Read())
            {
                user_Id = int.Parse(dr["user_Id"].ToString());
                user_email = dr["user_email"].ToString();
                username = dr["user_name"].ToString();
                user_passhash = dr["user_passhash"].ToString();
                user_hashsalt = dr["user_hashsalt"].ToString();
                user_address = dr["user_address"].ToString();
                user_counter = int.Parse(dr["user_counter"].ToString());
                admin_status = dr["admin_status"].ToString();
                phone_number = dr["phone_number"].ToString();
                twofa_enabled = dr["enable2FA"].ToString();
                ivValue = dr["IV"].ToString();
                ivKey = dr["Key"].ToString();
                banned_user = int.Parse(dr["banned_user"].ToString());
                userInfo = new User(user_Id, user_email, username, user_passhash, user_hashsalt, user_counter, user_address, admin_status, phone_number, twofa_enabled, ivValue, ivKey, banned_user);
            }
            else
            {
                userInfo = null;
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return userInfo;
        }

        public List<User> GetUserAll()
        {
            List<User> userlist = new List<User>();
            string user_email, username, user_passhash, user_hashsalt, user_address, twofa_enabled;
            string admin_status, ivValue, ivKey, phone_number;
            int user_counter, user_Id, banned_user;
            string queryStr = "Select * from Users Order by user_name";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                user_Id = int.Parse(dr["user_Id"].ToString());
                user_email = dr["user_email"].ToString();
                username = dr["user_name"].ToString();
                user_passhash = dr["user_passhash"].ToString();
                user_hashsalt = dr["user_hashsalt"].ToString();
                user_address = dr["user_address"].ToString();
                user_counter = int.Parse(dr["user_counter"].ToString());
                admin_status = dr["admin_status"].ToString();
                phone_number = dr["phone_number"].ToString();
                twofa_enabled = dr["enable2FA"].ToString();
                ivValue = dr["IV"].ToString();
                ivKey = dr["Key"].ToString();
                banned_user = int.Parse(dr["banned_user"].ToString());
                User u = new User(user_Id, user_email, username, user_passhash, user_hashsalt, user_counter, user_address, admin_status, phone_number, twofa_enabled, ivValue, ivKey, banned_user);
                userlist.Add(u);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return userlist;
        }
        //public User verifyUser(string userName, string passWord)
        //{
        //    User userInfo = null;
        //    string user_email, username, user_passhash, user_address;
        //    Boolean admin_status;
        //    int user_counter;
        //    string queryStr = "SELECT * FROM User WHERE user_Id = @userId";
        //    SqlConnection conn = new SqlConnection(_connStr);
        //    SqlCommand cmd = new SqlCommand(queryStr, conn);
        //    cmd.Parameters.AddWithValue("@userId", userId);
        //    conn.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();
        //    //Check if there are any resultsets
        //    if (dr.Read())
        //    {
        //        user_email = dr["user_email"].ToString();
        //        username = dr["user_name"].ToString();
        //        user_passhash = dr["user_passhash"].ToString();
        //        user_address = dr["user_address"].ToString();
        //        user_counter = int.Parse(dr["user_counter"].ToString());
        //        admin_status = Boolean.Parse(dr["admin_status"].ToString());
        //        userInfo = new User(userId, user_email, username, user_passhash, user_counter, user_address, admin_status);
        //    }
        //    else
        //    {
        //        userInfo = null;
        //    }
        //    conn.Close();
        //    dr.Close();
        //    dr.Dispose();
        //    return userInfo;
        //}

        //get userid with username in Session[LoggedIn]     -JW
        public int getUserId(string username)
        {
            int userid;
            string queryStr = "SELECT user_Id FROM Users WHERE user_email= @useremail";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@useremail", username);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                userid = int.Parse(dr["user_Id"].ToString());
            }
            else
            {
                userid = -1;
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return userid;
        }
    }
}