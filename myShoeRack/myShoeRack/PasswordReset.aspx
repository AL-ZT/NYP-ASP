<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="myShoeRack.PasswordReset" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>Reset Password</h1>
                    <nav class="d-flex align-items-center">
                        <a href="Index.aspx">Home<span class="lnr lnr-arrow-right"></span></a>
                        <a href="PasswordReset.aspx">Reset Password</a>
                    </nav>
                </div>
            </div>
        </div>
    </section>

    <section class="tracking_box_area section_gap">
        <div class="container">
            <div class="tracking_box_inner">
                <p>Enter your new password for <asp:Label ID="emailLabel" runat="server" Text=""></asp:Label>.</p>
                <form class="row tracking_form" action="#" method="post">
                    <div class="col-md-12 form-group">
                        <input type="password" class="form-control" id="newPassword" name="newPassword" placeholder="New Password" onfocus="this.placeholder = ''" onblur="this.placeholder = 'New Password'" runat="server">
                    </div>
                    <div class="col-md-12 form-group">
                        <input type="password" class="form-control" id="cfmNewPassword" name="cfmNewPassword" placeholder="Confirm New Password" onfocus="this.placeholder = ''" onblur="this.placeholder = 'Confirm New Password'" runat="server">
                    </div>
                    <asp:CompareValidator ID="passwordMatchValidator" runat="server" ErrorMessage="Passwords Do Not Match!" ControlToCompare="newPassword" ForeColor="Red" ControlToValidate="cfmNewPassword" Display="Dynamic"></asp:CompareValidator>
                    <div class="col-md-12 form-group">
                        <asp:Button type="button" class="genric-btn primary e-large" ID="passwordResetBtn" Text="Reset Password" runat="server" OnClick="passwordResetBtn_Click" />
                    </div>
                    <br />
                    <div class="g-recaptcha" data-sitekey="6Le0zaMUAAAAACnXtOskF-3qN-eAbuS86dTPVp_F"></div>
                </form>
            </div>
        </div>
    </section>

    <script src="https://www.google.com/recaptcha/api.js" async defer></script>
</asp:Content>