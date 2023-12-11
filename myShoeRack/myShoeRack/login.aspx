<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="myShoeRack.login" Async="true" MaintainScrollPositionOnPostback="true" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Start Banner Area -->
	<section class="banner-area organic-breadcrumb">
		<div class="container">
			<div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
				<div class="col-first">
					<h1>Login/Register</h1>
					<nav class="d-flex align-items-center">
						<a href="Index.aspx">Home<span class="lnr lnr-arrow-right"></span></a>
						<a href="login.aspx">Login/Register</a>
					</nav>
				</div>
			</div>
		</div>
	</section>
	<!-- End Banner Area -->

	<!--================Login Box Area =================-->
	<section class="login_box_area section_gap">
		<div class="container">
            <div class="row">
                <div class="col-lg-12"></div>
            </div>
			<div class="row">
				<div class="col-lg-6">
					<div class="login_box_img">
						<img class="img-fluid" src="img/login.jpg" alt="">
						<div class="hover">
							<h4>New to our website?</h4>
                            <br />
							<a class="primary-btn" href="register.aspx">Create an Account</a>
                            <br />
                            <br />
                            <br />
                            <h4>OR</h4>
                            <br />
                            <div align="center" class="g-signin2" data-onsuccess="onSignIn" data-width="200" data-height="40" data-longtitle="true" data-theme="dark"></div>
						</div>
					</div>
				</div>
				<div class="col-lg-6">
					<div class="login_form_inner">
						<h3>Log in to enter</h3>
						<div class="col-md-12 form-group">
							<input type="text" class="form-control" id="loginName" name="loginName" placeholder="User E-Mail" onfocus="this.placeholder = ''" onblur="this.placeholder = 'E-Mail'" runat="server">
						</div>
						<div class="col-md-12 form-group">
                            <input type="password" class="form-control" id="loginPassword" name="loginPassword" placeholder="Password" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Password'" runat="server">
						</div>
                        <div class="col-md-12 form-group">
                            <asp:Label ID="emailPasswordFailLabel" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                        <div class="col-md-12 form-group">
                            <a id="forgotPasswordLink" class="nav-link" style="cursor:pointer;" onclick="forgetPasswordModal();">Forgot Password?</a>
                        </div>
						<div class="col-md-12 form-group">
							<asp:Button type="button" class="genric-btn primary e-large"  ID="loginBtn" Text="Log In" runat="server" OnClick="loginBtn_Click" />
						</div>        
                        <br />
                        <div class="col-md-12 form-group" align="center">
                            <div class="g-recaptcha" data-sitekey="6Le0zaMUAAAAACnXtOskF-3qN-eAbuS86dTPVp_F"></div>
                        </div>
                        <br/>
					</div>
                </div>
			</div>
		</div>

        <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Two-Factor Authentication</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="modalClose">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <div class="row">
                    <div class="col-3">
                        <img src="https://png.pngtree.com/svg/20170329/9b025f409c.svg" style="max-width:100%; height:100px;" />
                    </div>
                    <div class="col-9">
                        <p>Enter Your Google Authenticator Code Below.</p>
                        <input type="text" class="form-control" id="login2fa" name="login2fa" placeholder="Enter 2FA Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter 2FA Code'" runat="server">
                        <br />
                        <br />
                        <asp:Label ID="invalidCode" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div id="otherAuthMethods" class="row" runat="server">
                    <div class="col" align="center">
                        Can't use Google Authenticator? Use <asp:LinkButton ID="emailLogin" runat="server" OnClick="emailLogin_Click">Email</asp:LinkButton> <asp:Label ID="orLabel" runat="server" Text="OR"></asp:Label> <asp:LinkButton ID="smsLogin" runat="server" OnClick="smsLogin_Click">SMS</asp:LinkButton>
                    </div>
                </div>
              </div>
              <div class="modal-footer">
                <asp:Button type="button" class="btn btn-primary" ID="modalLogin" OnClick="modalLogin_Click" Text="Submit" runat="server" />
              </div>
            </div>
          </div>
        </div>

        <div class="modal fade" id="emailModal" tabindex="-1" role="dialog" aria-labelledby="emailModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="emailModalLabel">Email OTP</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="emailModalClose">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <div class="row">
                    <div class="col-3">
                        <img src="https://cdn2.iconfinder.com/data/icons/the-best-for-emails/512/mail24-512.png" style="max-width:100%; height:100px;" />
                    </div>
                    <div class="col-9">
                        <p>Enter The OTP we have sent over to your Email Address (<asp:Label ID="emailAddressLabel" runat="server" Text=""></asp:Label>)</p>
                        <input type="text" class="form-control" id="emailOTP" name="emailOTP" placeholder="Enter OTP Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter OTP Code'" runat="server">
                        <br />
                        <br />
                        <asp:Label ID="invalidCodeEmail" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
                <div id="emailAlternativeAuth" class="row" runat="server">
                    <div class="col" align="center">
                        Can't use Email? Use <asp:LinkButton ID="emailAltSMSLogin" runat="server" OnClick="emailAltSMSLogin_Click">SMS</asp:LinkButton>
                    </div>
                </div>
              </div>
              <div class="modal-footer">
                <asp:Button type="button" class="btn btn-primary" ID="email2fa" OnClick="email2fa_Click" Text="Submit" runat="server" />
              </div>
            </div>
          </div>
        </div>

        <div class="modal fade" id="smsModal" tabindex="-1" role="dialog" aria-labelledby="smsModalLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="smsModalLabel">SMS OTP</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="smsModalClose">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <div class="row">
                    <div class="col-3">
                        <img src="https://cdn2.iconfinder.com/data/icons/thin-devices/24/thin-1248_mobile_phone_lock_password-512.png" style="max-width:100%; height:100px;" />
                    </div>
                    <div class="col-9">
                        <p>Enter The OTP we have sent over to your Phone Number (+<asp:Label ID="phoneNoLabel" runat="server" Text=""></asp:Label>)</p>
                        <input type="text" class="form-control" id="smsOTP" name="smsOTP" placeholder="Enter OTP Code" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter OTP Code'" runat="server">
                        <br />
                        <br />
                        <asp:Label ID="invalidCodeSMS" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
              </div>
              <div class="modal-footer">
                <asp:Button type="button" class="btn btn-primary" ID="sms2fa" OnClick="sms2fa_Click" Text="Submit" runat="server" />
              </div>
            </div>
          </div>
        </div>

        <div class="modal fade" id="forgotPasswordModal" tabindex="-1" role="dialog" aria-labelledby="forgotPasswordLabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="forgotPasswordLabel">Forgot Password?</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close" id="secondmodalClose">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                <div class="row">
                    <div class="col">
                        <p>Enter Your Registered Email Below.</p>
                        <input type="text" class="form-control" id="forgotPasswordEmail" name="forgotPasswordEmail" placeholder="Enter Email" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Enter Email'" runat="server">
                        <br />
                        <asp:RegularExpressionValidator ID="regexEmailValidator" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="forgotPasswordEmail" runat="server" ErrorMessage="Please Enter a valid Email Address!" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                </div>
              </div>
                <div class="modal-footer">
                <asp:Button type="button" class="btn btn-primary" ID="forgotPasswordSubmit" OnClick="forgotPasswordSubmit_Click" Text="Submit" runat="server" />
              </div>
            </div>
          </div>
        </div>
	</section>
    <script>
        $(document).ready(function () {
            $("#MainContent_loginBtn").css("font-size", "15px");
            $('#successModal').modal('show');
            //$("li.nav-item a.nav-link[href='login.aspx']").addClass("active");
        })

        var captchaWidget;

        function forgetPasswordModal() {
            $('#forgotPasswordModal').modal('show');
        }

        function loginCheck() {
            $('#exampleModal').modal('show');
        }

        function loginCheckEmail() {
            $('#emailModal').modal('show');
        }

        function loginCheckSMS() {
            $('#smsModal').modal('show');
        }

        function onSignIn(googleUser) {
            var token = googleUser.getAuthResponse().id_token;
            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'https://localhost:44347/TokenSignIn');
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr.onload = function() {
                if (xhr.responseURL == "https://localhost:44347/Index") {
                    window.location.href = "Index.aspx";
                }
            };
            xhr.send('idtoken=' + token);
        }
    </script>
    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
	<!--================End Login Box Area =================-->
</asp:Content>