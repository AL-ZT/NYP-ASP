using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _172026H_Lim_ZhengTing
{
    public partial class MemberHome : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;
        byte[] IV;
        byte[] Key;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    SqlConnection connection = new SqlConnection(_connStr);
                    string sql = "SELECT * FROM Membership WHERE userId=@EMAIL";
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@EMAIL", Session["LoggedIn"].ToString());
                    byte[] encryptedCreditCardNo = null;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["memberName"] != null)
                                {
                                    if (reader["memberName"] != DBNull.Value)
                                    {
                                        homeName.Text = reader["memberName"].ToString();
                                    }
                                    if (reader["creditCardNo"] != DBNull.Value)
                                    {
                                        encryptedCreditCardNo = Convert.FromBase64String(reader["creditCardNo"].ToString());
                                    }
                                    if (reader["dateTime"] != DBNull.Value)
                                    {
                                        homeDateTime.Text = reader["dateTime"].ToString();
                                    }
                                    if (reader["IV"] != DBNull.Value)
                                    {
                                        IV = Convert.FromBase64String(reader["IV"].ToString());
                                    }
                                    if (reader["Key"] != DBNull.Value)
                                    {
                                        Key = Convert.FromBase64String(reader["Key"].ToString());
                                    }
                                }
                            }
                        }

                        RijndaelManaged cipher = new RijndaelManaged();
                        cipher.IV = IV;
                        cipher.Key = Key;
                        ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                        using (MemoryStream msDecrypt = new MemoryStream(encryptedCreditCardNo))
                        {
                            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                            {
                                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                                {
                                    homeCreditCardNumber.Text = srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { connection.Close(); }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

            Response.Redirect("Login.aspx");
        }
    }
}