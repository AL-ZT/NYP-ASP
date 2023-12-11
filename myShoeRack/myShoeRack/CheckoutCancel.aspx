<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutCancel.aspx.cs" Inherits="myShoeRack.Confirmation" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="headerChange" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Stylesheets" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start Banner Area -->
	<section class="banner-area organic-breadcrumb">
		<div class="container">
			<div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
				<div class="col-first">
					<h1>Confirmation</h1>
				</div>
			</div>
		</div>
	</section>
	<!-- End Banner Area -->

    <!--================Order Details Area =================-->
	<section class="order_details section_gap">
		<div class="container">
			<h3 class="title_confirmation" style="color:brown;">Checkout Cancelled.</h3>
			<p></p>
			<h3>Your purchase has been cancelled.</h3>
		</div>
	</section>
	<!--================End Order Details Area =================-->
</asp:Content>
