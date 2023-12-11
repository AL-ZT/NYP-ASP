<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="myShoeRack.Settings" Async="true" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .alertBox {
            background-color: dimgrey;
            padding: 10px 30px;
            color: lavender;
        }

        .mfaBox {
            width : inherit;
        }

        .floatRight {
            float: right;
        }

        .btnupdate.genric-btn{
            padding: 5px;
            text-transform: uppercase;
            border: none;
            background: linear-gradient(90deg, #ffba00 0%, #ff6c00 100%);
        }
    </style>
	<!-- Start Banner Area -->
	<section class="banner-area organic-breadcrumb">
		<div class="container">
			<div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
				<div class="col-first">
					<h1>Account Settings</h1>
					<nav class="d-flex align-items-center">
						<a href="Index.aspx">Home<span class="lnr lnr-arrow-right"></span></a>
						<a href="Settings.aspx">Settings</a>
					</nav>
				</div>
			</div>
		</div>
	</section>

    <section class="features-area section_gap">
        <div class="container">
            <div class="row">
                <div class="col-xl-3 col-lg-4 col-md-5">
                    <div class="sidebar-categories" style="box-shadow: 5px 5px 5px lightgrey;">
                        <ul class="main-categories">
                            <li class="main-nav-list">
                                <asp:LinkButton ID="acctInfo" runat="server" OnClick="acctInfo_Click">
                                    Account Information
                                </asp:LinkButton>
                            </li>
                            <li class="main-nav-list">
                                <asp:LinkButton ID="securityLink" runat="server" OnClick="securityLink_Click">
                                    Security
                                </asp:LinkButton>
                            </li>
<%--                            <li class="main-nav-list">
                                <asp:HyperLink runat="server" NavigateUrl="~/acntPayMethods.aspx">
                                    Payment Methods
                                </asp:HyperLink>
                            </li>
                            <li class="main-nav-list">
                                <asp:HyperLink runat="server" NavigateUrl="~/acntTransacHist.aspx">
                                    Transaction History
                                </asp:HyperLink>
                            </li>--%>
                        </ul>
                    </div>
                </div>
                <div class="col-xl-9 col-lg-8 col-md-7">
                    <div id="AccountParticulars" runat="server">
                        <div class="container">
                            <h1>Account Information</h1>

                <br />
                <br />

                <div class="row">
                    <div class="col-md-6">
                        <h4>Username </h4>
                        <asp:Label runat="server" ID="lbl_username"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <h4>Email </h4>
                        <asp:Label runat="server" ID="lbl_email"></asp:Label>
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <h4>Phone number </h4>
                        <asp:Label runat="server" ID="lbl_hp"></asp:Label>
                    </div>
                    <div class="col-md-6">
                        <h4>Address </h4>
                        <asp:Label runat="server" ID="lbl_address"></asp:Label>
                    </div>
                </div>

                <br />
                <br />

                <div class="row text-right">
                    <div class="col-md-12">
                        <asp:Button runat="server" ID="btn_updateForm" CssClass="btnupdate genric-btn primary radius" Text="Edit Information" OnClick="btn_updateForm_Click" />
                        <%-- rmb to remove this button --%>
                        <%-- <asp:Button runat="server" ID="btn_addproduct" Text="Add Product" OnClick="btn_addproduct_Click" />--%>
                    </div>
                </div>
                        </div>
                    </div>
                    <div id="SecurityFunc" runat="server">
                        <div class="container">
                            <h3 class="mb-20">Current Sessions</h3>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-lg-12">
                                <div id="accordian">
                                    <asp:Repeater ID="loggedInUserQuery" runat="server">
                                        <ItemTemplate>
                                            <div class="card">
                                                <div class="card-header" id="headingOne">
                                                    <h5 class="mb-0">
                                                    <asp:Label ID="collapseHeader" runat="server" Text='<%# Eval("[DateTime]") %>'></asp:Label>
                                                    <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("Id") %>' />
                                                    <asp:Button type="button" class="genric-btn danger floatRight" ID="clearSessionBtn" Text="Clear Session" runat="server" OnClick="clearSessionBtn_Click" />
                                                    </h5>
                                                </div>

                                                <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                                                    <div class="card-body">
                                                        <blockquote class="generic-blockquote">
                                                            <div id="loginDataExist" runat="server">
                                                                <div class="row">
                                                                    <div class="col-3">
                                                                        <p><strong>Country/City:</strong></p>
                                                                        <p><strong>Last Logged In Date/Time:</strong></p>
                                                                        <p><strong>Computer's Public IP:</strong></p>
                                                                        <p><strong>Computer Name:</strong></p>
                                                                    </div>
                                                                    <div class="col-9">
                                                                        <p><asp:Image ID="countryImage" runat="server" ImageUrl='<%# Eval("[Flag]") %>' />   <asp:Label ID="countryLabel" runat="server" Text='<%# Eval("[Country]") %>'></asp:Label>/<asp:Label ID="cityLabel" runat="server" Text='<%# Eval("[City]") %>'></asp:Label></p>
                                                                        <p><asp:Label ID="dateTimeLabel" runat="server" Text='<%# Eval("[DateTime]") %>'></asp:Label> GMT</p>
                                                                        <p><asp:Label ID="ipLabel" runat="server" Text='<%# Eval("[Ip]") %>'></asp:Label></p>
                                                                        <p><asp:Label ID="computerNameLabel" runat="server" Text='<%# Eval("[DeviceName]") %>'></asp:Label></p>
                                                                    </div>
                                                                </div>
                                                        </blockquote>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
            <%--                    <blockquote class="generic-blockquote">
                                    <div id="Div1" runat="server">
                                        <div class="row">
                                            <div class="col-3">
                                                <p><strong>Country/City:</strong></p>
                                                <p><strong>Last Logged In Date/Time:</strong></p>
                                                <p><strong>Computer's Public IP:</strong></p>
                                                <p><strong>Computer Name:</strong></p>
                                            </div>
                                            <div class="col-9">
                                                <p><asp:Image ID="countryImage" runat="server" />   <asp:Label ID="countryLabel" runat="server" Text="Label"></asp:Label>/<asp:Label ID="cityLabel" runat="server" Text=""></asp:Label></p>
                                                <p><asp:Label ID="dateTimeLabel" runat="server" Text="Label"></asp:Label> GMT</p>
                                                <p><asp:Label ID="ipLabel" runat="server" Text="Label"></asp:Label></p>
                                                <p><asp:Label ID="computerNameLabel" runat="server" Text="Label"></asp:Label></p>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="Div2" runat="server" visible="false">
                                        <div class="row">
                                            <strong>    No Recent Login Information Recorded.</strong>
                                        </div>
                                    </div>
                                </blockquote>--%>
                            </div>
                        </div>
                        <br />
                        <div class="container">
                            <div class="row">
                                <h3 class="mb-20">Multi-Factor Authentication</h3>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-4">
                                    <% if (Session["2FA"] != null && Session["2FA"].ToString() == "Enabled")
                                        { %>
                                        <asp:Button type="button" class="genric-btn danger mfaBox" ID="disable2fa" Text="Disable 2FA" runat="server" OnClick="disable2fa_Click" />
                                    <% } %>
                                    <% else if (Session["2FA"] != null && Session["2FA"].ToString() == "Disabled")
                                        { %>
                                        <asp:Button type="button" ID="disabledBtn" Text="Enable 2FA" runat="server" ViewStateMode="Disabled" Enabled="False" />
                                        <div class="alertBox">Google Authenticator 2FA is disabled for Google Sign-In.</div>
                                    <% } %> 
                                    <% else
                                        { %>
                                        <asp:Button type="button" class="genric-btn info mfaBox" ID="google2fa" Text="Enable 2FA" runat="server" OnClick="google2fa_Click" />
                                    <% } %>  
                                </div>
                                <div class="col-4">
                                    <% if (Session["emailAuth"] != null && Session["emailAuth"].ToString() == "Enabled")
                                        { %>
                                        <asp:Button type="button" class="genric-btn danger mfaBox" ID="emailAuthDisable" Text="Disable Email Verification" runat="server" OnClick="emailAuthDisable_Click"/>
                                    <% } %>
                                    <% else if (Session["emailAuth"] != null && Session["emailAuth"].ToString() == "Disabled")
                                        { %>
                                        <asp:Button type="button" ID="disabledEmailAuth" Text="Enable Email Verification" runat="server" ViewStateMode="Disabled" Enabled="False" />
                                        <div class="alertBox">Email Verification is disabled for Google Sign-In.</div>
                                    <% } %> 
                                    <% else
                                        { %>
                                        <asp:Button type="button" class="genric-btn info mfaBox" ID="enableEmailAuth" Text="Enable Email Verification" runat="server" OnClick="enableEmailAuth_Click" />
                                    <% } %> 
                                </div>
                                <div class="col-4">
                                    <% if (Session["smsAuth"] != null && Session["smsAuth"].ToString() == "Enabled")
                                        { %>
                                        <asp:Button type="button" class="genric-btn danger mfaBox" ID="smsAuthDisable" Text="Disable SMS Verification" runat="server" OnClick="smsAuthDisable_Click"/>
                                    <% } %>
                                    <% else if (Session["smsAuth"] != null && Session["smsAuth"].ToString() == "Disabled")
                                        { %>
                                        <asp:Button type="button" ID="disabledSMSAuth" Text="Enable SMS Verification" runat="server" ViewStateMode="Disabled" Enabled="False" />
                                        <div class="alertBox">SMS Verification is disabled for Google Sign-In.</div>
                                    <% } %> 
                                    <% else
                                        { %>
                                        <asp:Button type="button" class="genric-btn info mfaBox" ID="enableSMSAuth" Text="Enable SMS Verification" runat="server" OnClick="enableSMSAuth_Click" />
                                    <% } %> 
                                </div>
                            </div>
                            <br />
                            <div class="row" id="QR2FA" runat="server">
                                <div class="col-lg-12">
                                    <blockquote class="generic-blockquote">
                                        <div class="row">
                                            <div class="col-lg-7 col-sm-12">
					                            <div class="single-defination">
						                            <h4 class="mb-20">Step 1a - Scan Google Authenticator QR Code</h4>
						                            <asp:Image ID="QR" runat="server" />
                                                    <br />
						                            <h4 class="mb-20">OR Step 1b - Enter Key Manually into Google Authenticator</h4>
                                                    <br />
                                                    <asp:Label ID="ManCodeAuth" runat="server" CssClass="alertBox" Text=""></asp:Label>
                                                    <br />
					                            </div>
				                            </div>
                                            <br />
				                            <div class="col-lg-5 col-sm-12">
					                            <div class="single-defination">
                                                    <div class="row">
                                                        <div class="col">
                                                            <h4 class="mb-20">Step 2 - Enter Generated Code</h4>
						                                    <input type="text" class="form-control" id="code2FA" name="code2FA" placeholder="Confirm 2FA Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Confirm 2FA Code'" runat="server">
                                                            <br />
                                                            <asp:Button type="button" class="genric-btn info" ID="check2FA" Text="Confirm Code" runat="server" OnClick="check2FA_Click" />
                                                            <br />
                                                            <br />
                                                            <asp:Label ID="invalidCode" runat="server" ForeColor="Red"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row" id="recoveryCodes" runat="server">
                                                        <div class="col">
                                                            <h4 class="mb-20">Step 3 - Save Recovery Codes</h4>
                                                            <p>Use these Recovery Codes if you ever lose your 2FA Authenticator. Each code can only be used once.</p>
                                                            <br />
                                                            <div class="alertBox">
                                                                testing
                                                                testing
                                                            </div>
                                                            <br />
                                                            <asp:Button type="button" class="genric-btn info" ID="complete2FA" Text="Complete 2FA" runat="server" OnClick="complete2FA_Click" />
                                                        </div>                   
                                                    </div>
					                            </div>
				                            </div>
                                        </div>     
                                    </blockquote>
                                </div>
                            </div>
                            <div class="row" id="SMSAuthStep" runat="server">
                                <div class="col-lg-12">
                                    <blockquote class="generic-blockquote">
                                        <div class="row">
                                            <div class="col-lg-12 col-sm-12">
					                            <div class="single-defination">
						                            <h4 class="mb-20">Check One-Time Password (OTP) in Mobile Phone</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-2">
                                                <img src="https://cdn2.iconfinder.com/data/icons/thin-devices/24/thin-1248_mobile_phone_lock_password-512.png" style="max-width:100%; height:100px;" />
                                            </div>
                                            <div class="col-6">
                                                Please Enter The OTP we have sent over to your phone number (+<asp:Label ID="phoneNumberLabel" runat="server" Text=""></asp:Label>)
                                            </div>
                                            <div class="col-4">
                                                <input type="text" class="form-control" id="codeSMS" name="codeSMS" placeholder="Confirm OTP Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Confirm OTP Code'" runat="server">
                                                <asp:Label ID="invalidCodeSMS" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Button type="button" class="genric-btn info" ID="SMSOTP" Text="Confirm OTP" runat="server" OnClick="SMSOTP_Click" style="float:right;" />
                                            </div>
                                        </div>
                                    </blockquote>
                                </div>
                            </div>
                            <div class="row" id="EmailAuthStep" runat="server">
                                <div class="col-lg-12">
                                    <blockquote class="generic-blockquote">
                                        <div class="row">
                                            <div class="col-lg-12 col-sm-12">
					                            <div class="single-defination">
						                            <h4 class="mb-20">Check One-Time Password (OTP) in Email</h4>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <br />
                                            <div class="col-2">
                                                <img src="https://cdn2.iconfinder.com/data/icons/the-best-for-emails/512/mail24-512.png" style="max-width:100%; height:100px;" />
                                            </div>
                                            <div class="col-6">
                                                Please Enter The OTP we have sent over to your phone number (<asp:Label ID="emailLabel" runat="server" Text=""></asp:Label>)
                                            </div>
                                            <div class="col-4">
                                                <input type="text" class="form-control" id="codeEmail" name="codeEmail" placeholder="Confirm OTP Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Confirm OTP Code'" runat="server">
                                                <asp:Label ID="invalidCodeEmail" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col">
                                                <asp:Button type="button" class="genric-btn info" ID="EMAILOTP" Text="Confirm OTP" runat="server" OnClick="EMAILOTP_Click" style="float:right;" />
                                            </div>
                                        </div>
                                    </blockquote>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <script>
        $(document).ready(function () {
            $("#MainContent_disabledBtn").addClass("genric-btn disable mfaBox");
            $("#MainContent_disabledEmailAuth").addClass("genric-btn disable mfaBox");
            $("#MainContent_disabledSMSAuth").addClass("genric-btn disable mfaBox");
        })
    </script>
</asp:Content>