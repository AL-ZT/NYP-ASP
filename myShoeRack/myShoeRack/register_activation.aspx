<%@ Page Title="Register" Language="C#"  MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register_activation.aspx.cs" Inherits="myShoeRack.register_activation" %>

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

    <br />

    <section>
        <div align="center" style="margin-bottom:125px;margin-top:125px">
            <h1><asp:Literal ID="emailAct_msg" runat="server" /></h1>
        </div>
    </section>
    
</asp:Content>
