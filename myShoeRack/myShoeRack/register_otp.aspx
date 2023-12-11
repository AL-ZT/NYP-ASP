<%@ Page Title="Register" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="register_otp.aspx.cs" Inherits="myShoeRack.register_otp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Start Banner Area -->
	<section class="banner-area organic-breadcrumb">
		<div class="container">
			<div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
				<div class="col-first">
					<h1>Registration</h1>
				</div>
			</div>
		</div>
	</section>
	<!-- End Banner Area -->

    <section>
        <div align="center" style="margin-bottom:125px;margin-top:125px">
            <h1>We have sent an OTP to your phone number.</h1>
            <h3>Enter it here within 24 hours!</h3>
            <br />
            <br />
            <asp:TextBox ID="otpText" runat="server" Width="345px"></asp:TextBox>
            <asp:Button runat="server" ID="otpBtn" OnClick="otpBtn_Click" Text="Submit"/>
            <asp:Label ID="otpLbl" runat="server"></asp:Label>
        </div>
    </section>
    
</asp:Content>

