<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutConfirmation.aspx.cs" Inherits="myShoeRack.CheckoutConfirmation" %>
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
			<h3 class="title_confirmation">Thank you. Your order has been received.</h3>
            <h3 class="title_confirmation">
                <asp:Label ID="Label1" runat="server" ForeColor="Black" Text="Payment Transaction ID: "></asp:Label>
                <asp:Label ID="lbl_TransactionId" runat="server" ForeColor="Black"></asp:Label>
            </h3>

            <div class="checkout_btn_inner d-flex align-items-center">
                <a class="gray_btn" href="Index.aspx">Continue Shopping</a>
            </div>
			<%--<div class="row order_d_inner">
				<div class="col-lg-4">
					<div class="details_item">
						<h4>Order Info</h4>
						<ul class="list">
							<li><a href="#"><span>Order number</span> : 
                                <asp:Label ID="lbl_orderid" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>Date</span> :</a> <a href="#">
                                <asp:Label ID="lbl_orderdate" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>Total</span> :</a> <a href="#">
                                <asp:Label ID="lbl_total" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>Payment method</span> :</a> <a href="#">
                                <asp:Label ID="lbl_payment" runat="server">Paypal</asp:Label>
                                </a></li>
						</ul>
					</div>
				</div>
				<div class="col-lg-4">
					<div class="details_item">
						<h4>Billing Address</h4>
						<ul class="list">
							<li><a href="#"><span>Street</span> :</a> <a href="#">
                                <asp:Label ID="lbl_address" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>City</span> : 
                                <asp:Label ID="lbl_city" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>Country</span> :</a> <a href="#">
                                <asp:Label ID="lbl_country" runat="server"></asp:Label>
                                </a></li>
							<li><a href="#"><span>Postcode </span> : 
                                <asp:Label ID="lbl_postalcode" runat="server"></asp:Label>
                                </a></li>
						</ul>
					</div>
				</div>
				<div class="col-lg-4">
					<div class="details_item">
						<h4>Shipping Address</h4>
						<ul class="list">
							<li><a href="#"><span>Street</span> : 180 Ang Mo Kio Ave 8</a></li>
							<li><a href="#"><span>City</span> : Singapore</a></li>
							<li><a href="#"><span>Country</span> : Singapore</a></li>
							<li><a href="#"><span>Postcode </span> : 569830</a></li>
						</ul>
					</div>
				</div>
			</div>
			<div class="order_details_table">
				<h2>Order Details</h2>
				<div class="table-responsive">
					<table class="table">
						<thead>
							<tr>
								<th scope="col">Product</th>
								<th scope="col">Quantity</th>
								<th scope="col">Total</th>
							</tr>
						</thead>
						<tbody>
							<tr>
								<td>
									<p>Pixelstore fresh Blackberry</p>
								</td>
								<td>
									<h5>x 01</h5>
								</td>
								<td>
									<p>SGD 129.00</p>
								</td>
							</tr>
							<tr>
								<td>
									<p>Pixelstore dated Blueberry</p>
								</td>
								<td>
									<h5>x 01</h5>
								</td>
								<td>
									<p>SGD 126.00</p>
								</td>
							</tr>
							<tr>
								<td>
									<h4>Subtotal</h4>
								</td>
								<td>
									<h5></h5>
								</td>
								<td>
									<p>$225.00</p>
								</td>
							</tr>
							<tr>
								<td>
									<h4>Shipping</h4>
								</td>
								<td>
									<h5></h5>
								</td>
								<td>
									<p>Flat rate: $50.00</p>
								</td>
							</tr>
							<tr>
								<td>
									<h4>Total</h4>
								</td>
								<td>
									<h5></h5>
								</td>
								<td>
									<p>$275.00</p>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>--%>
		</div>
	</section>
	<!--================End Order Details Area =================-->
</asp:Content>
