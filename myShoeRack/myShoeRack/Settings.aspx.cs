using Google.Authenticator;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;

namespace myShoeRack
{
    public partial class Settings : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString;
        SqlDataAdapter da;
        DataSet ds;
        const string accountSid = "ACf752021e1d086cbbd54e342846a508c0";
        const string authToken = "0bd91b35492032d87722cbdf2ceb958c";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                getUserParticulars(Session["LoggedIn"].ToString());
                if (!IsPostBack)
                {
                    dataRepeat();
                    QR2FA.Visible = false;
                    SecurityFunc.Visible = false;
                    SMSAuthStep.Visible = false;
                    EmailAuthStep.Visible = false;
                    recoveryCodes.Visible = false;
                    if (Request.Cookies["TFA"] != null)
                    {
                        Response.Cookies["TFA"].Value = string.Empty;
                        Response.Cookies["TFA"].Expires = DateTime.Now.AddMonths(-20);
                    }
                }
                if (Request.Cookies["RedirectSecurity"] != null)
                {
                    SecurityFunc.Visible = true;
                    AccountParticulars.Visible = false;
                    Response.Cookies["RedirectSecurity"].Value = string.Empty;
                    Response.Cookies["RedirectSecurity"].Expires = DateTime.Now.AddMonths(-20);
                }
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void getUserParticulars(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lbl_username.Text = reader["user_name"].ToString();
                        lbl_hp.Text = "+" + reader["phone_number"].ToString();
                        lbl_email.Text = reader["user_email"].ToString();
                        lbl_address.Text = reader["user_address"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected void google2fa_Click(object sender, EventArgs e)
        {
            QR2FA.Visible = true;
            SMSAuthStep.Visible = false;
            EmailAuthStep.Visible = false;
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string randomString = RandomString(10);
            Response.Cookies.Add(new HttpCookie("TFA", randomString));
            var setupInfo = tfa.GenerateSetupCode("MyShoeRack", Session["LoggedIn"].ToString(), randomString, 300, 300); //the width and height of the Qr Code in pixels
            string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl; //  assigning the Qr code information + URL to string
            string manualEntrySetupCode = setupInfo.ManualEntryKey; // show the Manual Entry Key for the users that don't have app or phone
            QR.ImageUrl = qrCodeImageUrl;// showing the qr code on the page "linking the string to image element"
            ManCodeAuth.Text = manualEntrySetupCode;
        }

        public void dataRepeat()
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM LoggedInUsers WHERE Username=@USERNAME ORDER BY Id DESC";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERNAME", Session["LoggedIn"].ToString());
            connection.Open();
            DataSet ds = new DataSet();
            da = new SqlDataAdapter(command);
            da.Fill(ds);
            loggedInUserQuery.DataSource = ds;
            loggedInUserQuery.DataBind();
        }

        protected void check2FA_Click(object sender, EventArgs e)
        {
            string user_enter = code2FA.Value.ToString().Trim();
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string strToken = Request.Cookies["TFA"].Value;
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(strToken, user_enter);
            if (isCorrectPIN == true)
            {
                int affectedRows = update2FAToken(Session["LoggedIn"].ToString(), strToken);
                if (Request.Cookies["TFA"] != null)
                {
                    Response.Cookies["TFA"].Value = string.Empty;
                    Response.Cookies["TFA"].Expires = DateTime.Now.AddMonths(-20);
                }
                invalidCode.Text = "";
                Session["2FA"] = "Enabled";
                recoveryCodes.Visible = true;
            }
            else
            {
                invalidCode.Text = "Invalid Code, Try Again.";
            }
        }

        protected string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected int update2FAToken(string email, string token)
        {
            string queryStr = "UPDATE Users SET" +
     " enable2FA = @enable2FA" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@enable2FA", token);
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected int remove2FAToken(string email)
        {
            string queryStr = "UPDATE Users SET" +
     " enable2FA = NULL" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected void disable2fa_Click(object sender, EventArgs e)
        {
            int affectedRows = remove2FAToken(Session["LoggedIn"].ToString());
            Session["2FA"] = null;
            Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
            Response.Redirect("Settings.aspx");
        }

        protected void complete2FA_Click(object sender, EventArgs e)
        {
            Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
            Response.Redirect("Settings.aspx");
        }

        protected string getDBHash(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select user_passhash FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["user_passhash"] != null)
                        {
                            if (reader["user_passhash"] != DBNull.Value)
                            {
                                h = reader["user_passhash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return h;
        }

        protected string getDBPhoneNo(string email)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select phone_number FROM Users WHERE user_email=@USEREMAIL";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USEREMAIL", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["phone_number"] != null)
                        {
                            if (reader["phone_number"] != DBNull.Value)
                            {
                                h = reader["phone_number"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return h;
        }

        //protected Boolean RetrieveLastUserSession()
        //{
        //    SqlConnection connection = new SqlConnection(MYDBConnectionString);
        //    string userhash = null;
        //    if (Session["sub"] == null)
        //    {
        //        userhash = getDBHash(Session["LoggedIn"].ToString());
        //    }
        //    else
        //    {
        //        userhash = Session["sub"].ToString();
        //    }
        //    string sql = "select * FROM LoggedInUsers WHERE Username=@USERNAME AND Passhash=@PASSHASH";
        //    SqlCommand command = new SqlCommand(sql, connection);
        //    command.Parameters.AddWithValue("@USERNAME", Session["LoggedIn"].ToString());
        //    command.Parameters.AddWithValue("@PASSHASH", userhash);
        //    try
        //    {
        //        connection.Open();
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                ipLabel.Text = reader["IP"].ToString();
        //                countryLabel.Text = reader["Country"].ToString();
        //                cityLabel.Text = reader["City"].ToString();
        //                dateTimeLabel.Text = reader["DateTime"].ToString();
        //                countryImage.ImageUrl = reader["Flag"].ToString();
        //                computerNameLabel.Text = reader["DeviceName"].ToString();
        //                return true;
        //            }
        //            else { return false; }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        protected int SessionDelete(string ID)
        {
            string queryStr = "DELETE FROM LoggedInUsers WHERE Id=@ID";
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@ID", ID);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Delete

        protected void clearSessionBtn_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            var item = (RepeaterItem)btn.NamingContainer;
            HiddenField hiddenID = (HiddenField)item.FindControl("hiddenID");
            string id = hiddenID.Value;
            SessionDelete(id);
            Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
            Response.Redirect("Settings.aspx", false);
        }

        protected void enableEmailAuth_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(AsyncEmailAuth));
        }

        protected async Task AsyncEmailAuth()
        {
            QR2FA.Visible = false;
            EmailAuthStep.Visible = true;
            SMSAuthStep.Visible = false;
            emailLabel.Text = Session["LoggedIn"].ToString();
            string randomString = RandomString(6);
            Response.Cookies.Add(new HttpCookie("EmailAuth", randomString));
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("admin@MyShoeRack.com", "MyShoeRack-Admin");
            var subject = "MyShoeRack - Email OTP";
            var to = new EmailAddress(Session["LoggedIn"].ToString(), "You");
            var plainTextContent = "";
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            var htmlContent = "<p>Hi,</p>" +
                "<p>Your Email OTP for MyShoeRack Email Authentication Activation is : " + randomString +
                "<br />" +
                "<br />" +
                "<p>Regards, </p>" +
                "<p>MyShoeRack Team</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        protected void enableSMSAuth_Click(object sender, EventArgs e)
        {
            QR2FA.Visible = false;
            EmailAuthStep.Visible = false;
            SMSAuthStep.Visible = true;
            string phoneNo = getDBPhoneNo(Session["LoggedIn"].ToString());
            phoneNumberLabel.Text = phoneNo;
            string randomString = RandomString(6);
            Response.Cookies.Add(new HttpCookie("SMSAuth", randomString));
            TwilioClient.Init(accountSid, authToken);
            try
            {
                var message = MessageResource.Create(
                    body: "Enter the OTP for MyShoeRack Website : " + randomString,
                    from: new Twilio.Types.PhoneNumber("+14439799177"),
                    to: new Twilio.Types.PhoneNumber("+" + phoneNo)
                );

                Console.WriteLine(message.Sid);
            }
            catch (ApiException te)
            {
                Console.WriteLine(te.Message);
                Console.WriteLine($"Twilio Error {te.Code} - {te.MoreInfo}");
            }
        }

        protected void securityLink_Click(object sender, EventArgs e)
        {
            SecurityFunc.Visible = true;
            AccountParticulars.Visible = false;
        }

        protected void btn_updateForm_Click(object sender, EventArgs e)
        {

        }

        protected void acctInfo_Click(object sender, EventArgs e)
        {
            SecurityFunc.Visible = false;
            AccountParticulars.Visible = true;
        }

        protected void SMSOTP_Click(object sender, EventArgs e)
        {
            string entered_code = codeSMS.Value;
            string OTP = Request.Cookies["SMSAuth"].Value;
            if (entered_code == OTP)
            {
                int affectedRows = updateSMSAuth(Session["LoggedIn"].ToString());
                if (Request.Cookies["SMSAuth"] != null)
                {
                    Response.Cookies["SMSAuth"].Value = string.Empty;
                    Response.Cookies["SMSAuth"].Expires = DateTime.Now.AddMonths(-20);
                }
                Session["smsAuth"] = "Enabled";
                Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
                Response.Redirect("Settings.aspx");
            }
            else
            {
                invalidCodeSMS.Text = "Invalid Code, Try Again.";
            }
        }

        protected void EMAILOTP_Click(object sender, EventArgs e)
        {
            string entered_code = codeEmail.Value;
            string OTP = Request.Cookies["EmailAuth"].Value;
            if (entered_code == OTP)
            {
                int affectedRows = updateEmailAuth(Session["LoggedIn"].ToString());
                if (Request.Cookies["EmailAuth"] != null)
                {
                    Response.Cookies["EmailAuth"].Value = string.Empty;
                    Response.Cookies["EmailAuth"].Expires = DateTime.Now.AddMonths(-20);
                }
                Session["emailAuth"] = "Enabled";
                Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
                Response.Redirect("Settings.aspx");

            }
            else
            {
                invalidCodeEmail.Text = "Invalid Code, Try Again.";
            }
        }

        protected int updateSMSAuth(string email)
        {
            string queryStr = "UPDATE Users SET" +
     " enableSMSAuth = @ENABLESMSAUTH" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@ENABLESMSAUTH", "Enabled");
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected int updateEmailAuth(string email)
        {
            string queryStr = "UPDATE Users SET" +
     " enableEmailAuth = @ENABLEEMAILAUTH" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@ENABLEEMAILAUTH", "Enabled");
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected int removeSMSAuth(string email)
        {
            string queryStr = "UPDATE Users SET" +
     " enableSMSAuth = NULL" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected int removeEmailAuth(string email)
        {
            string queryStr = "UPDATE Users SET" +
     " enableEmailAuth = NULL" +
     " WHERE user_email = @USEREMAIL";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyShoeRackContext"].ConnectionString);
            SqlCommand cmd = new SqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@USEREMAIL", email);
            conn.Open();
            int nofRow = 0;
            nofRow = cmd.ExecuteNonQuery();
            conn.Close();
            return nofRow;
        }//end Update

        protected void emailAuthDisable_Click(object sender, EventArgs e)
        {
            int affectedRows = removeEmailAuth(Session["LoggedIn"].ToString());
            Session["emailAuth"] = null;
            Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
            Response.Redirect("Settings.aspx");
        }

        protected void smsAuthDisable_Click(object sender, EventArgs e)
        {
            int affectedRows = removeSMSAuth(Session["LoggedIn"].ToString());
            Session["smsAuth"] = null;
            Response.Cookies.Add(new HttpCookie("RedirectSecurity", "true"));
            Response.Redirect("Settings.aspx");
        }
    }
}