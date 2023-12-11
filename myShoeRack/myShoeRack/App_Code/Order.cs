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
    public class Order
    {
        string _connStr = ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        private int _OrderId = 0;
        private string _UserId = string.Empty;
        private string _transactionID = string.Empty;
        private string _date = string.Empty;

        public Order()
        {
        }

        public Order(int order_Id, string user_Id, string transaction_ID, string date_)
        {
            _OrderId = order_Id;
            _UserId = user_Id;
            _transactionID = transaction_ID;
            _date = date_;
        }

        public Order(string userId)
            :this(0, userId, "", "" )
        {
        }

        public int Order_Id
        {
            get { return _OrderId; }
            set { _OrderId = value; }
        }

        public string userId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }

        public string transactionID
        {
            get { return _transactionID; }
            set { _transactionID = value; }
        }

        public string date
        {
            get { return _date; }
            set { _date = value; }
        }


        public List<Order> GetTransactionAll(string email)
        {
            List<Order> allorderlist = new List<Order>();
            int order_id;
            string transaction_id, date_time;

            //Preparing the SQL statement
            string queryStr = "Select * from Orders where userId = @userId";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@userId", email);
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            //Check if there are any resultsets
            if (dr.Read())
            {
                order_id = int.Parse(dr["Order_Id"].ToString());
                transaction_id = Decrypt(dr["transactionID"].ToString());
                date_time = dr["date"].ToString();
                Order o = new Order(order_id, email, transaction_id, date_time);
                allorderlist.Add(o);
            }
            conn.Close();
            dr.Close();
            dr.Dispose();
            return allorderlist;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "5MBSJGOQLQL5H8E8QI83JV3CJLQJEV07";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}