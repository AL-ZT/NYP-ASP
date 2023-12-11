using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _172026H_Lim_ZhengTing
{
    public partial class Login : System.Web.UI.Page
    {
        string _connStr = ConfigurationManager.ConnectionStrings["UserDBContext"].ConnectionString;
        int failureCounter;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string getDBSalt(string email)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(_connStr);
            string sql = "select salt FROM Membership WHERE userId=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["salt"] != null)
                        {
                            if (reader["salt"] != DBNull.Value)
                            {
                                s = reader["salt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(_connStr);
            string sql = "select hashPassword FROM Membership WHERE userId=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["hashPassword"] != null)
                        {
                            if (reader["hashPassword"] != DBNull.Value)
                            {
                                h = reader["hashPassword"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected string getDBAge(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(_connStr);
            string sql = "select age FROM Membership WHERE userId=@EMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["age"] != null)
                        {
                            if (reader["age"] != DBNull.Value)
                            {
                                h = reader["age"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string pwd = loginPassword.Text.ToString().Trim();
                string email = loginEmail.Text.ToString().Trim();
                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);
                string dbAge = getDBAge(email);

                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt + dbAge;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                    if (userHash.Equals(dbHash))
                    {
                        Session["LoggedIn"] = email;
                        string guid = Guid.NewGuid().ToString();
                        Session["AuthToken"] = guid;
                        Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                        Response.Redirect("MemberHome.aspx", false);
                    }
                    else
                    {
                        failureCounter += 1;
                        if (failureCounter == 3)
                        {
                            int rows = deleteUser(email);
                            Response.Write("<script>alert('Your account is disabled.')</script>");
                        }
                        else
                        {
                            errorMsg.Text = "Invalid userid or password";
                            //Response.Cookies.Add(new HttpCookie("Counter", failureCounter.ToString()));
                        }

                    }
                }
                else
                {
                    failureCounter += 1;
                    if (failureCounter == 3)
                    {
                        int rows = deleteUser(email);
                        Response.Write("<script>alert('Your account is disabled.')</script>");
                    }
                    else
                    {
                        errorMsg.Text = "Invalid userid or password";
                        //Response.Cookies.Add(new HttpCookie("counter", failureCounter.ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

            }
        }

        protected int deleteUser(string email)
        {
            string sql = "DELETE FROM Membership WHERE userId=@EMAIL";
            SqlConnection conn = new SqlConnection(_connStr);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@EMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}