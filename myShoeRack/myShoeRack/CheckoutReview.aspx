<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutReview.aspx.cs" Inherits="myShoeRack.CheckoutReview" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start Header Area -->
	<header class="header_area sticky-header">
		<div class="main_menu">
			<nav class="navbar navbar-expand-lg navbar-light main_box">
				<div class="container">
					<!-- Brand and toggle get grouped for better mobile display -->
					<a class="navbar-brand logo_h" href="index.html"><img src="img/logo.png" alt=""></a>
					<button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent"
					 aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
						<span class="icon-bar"></span>
					</button>
					<!-- Collect the nav links, forms, and other content for toggling -->
					<div class="collapse navbar-collapse offset" id="navbarSupportedContent">
						<ul class="nav navbar-nav menu_nav ml-auto">
							<li class="nav-item"><a class="nav-link" href="index.html">Home</a></li>
							<li class="nav-item submenu dropdown active">
								<a href="#" class="nav-link dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true"
								 aria-expanded="false">Shop</a>
								<ul class="dropdown-menu">
									<li class="nav-item"><a class="nav-link" href="category.html">Shop Category</a></li>
									<li class="nav-item"><a class="nav-link" href="single-product.html">Product Details</a></li>
									<li class="nav-item"><a class="nav-link" href="checkout.html">Product Checkout</a></li>
									<li class="nav-item active"><a class="nav-link" href="cart.html">Shopping Cart</a></li>
									<li class="nav-item"><a class="nav-link" href="confirmation.html">Confirmation</a></li>
								</ul>
							</li>
							<li class="nav-item submenu dropdown">
								<a href="#" class="nav-link dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true"
								 aria-expanded="false">Blog</a>
								<ul class="dropdown-menu">
									<li class="nav-item"><a class="nav-link" href="blog.html">Blog</a></li>
									<li class="nav-item"><a class="nav-link" href="single-blog.html">Blog Details</a></li>
								</ul>
							</li>
							<li class="nav-item submenu dropdown">
								<a href="#" class="nav-link dropdown-toggle" data-toggle="dropdown" role="button" aria-haspopup="true"
								 aria-expanded="false">Pages</a>
								<ul class="dropdown-menu">
									<li class="nav-item"><a class="nav-link" href="login.html">Login</a></li>
									<li class="nav-item"><a class="nav-link" href="tracking.html">Tracking</a></li>
									<li class="nav-item"><a class="nav-link" href="elements.html">Elements</a></li>
								</ul>
							</li>
							<li class="nav-item"><a class="nav-link" href="contact.html">Contact</a></li>
						</ul>
						<ul class="nav navbar-nav navbar-right">
							<li class="nav-item"><a href="#" class="cart"><span class="ti-bag"></span></a></li>
							<li class="nav-item">
								<button class="search"><span class="lnr lnr-magnifier" id="search"></span></button>
							</li>
						</ul>
					</div>
				</div>
			</nav>
		</div>
		<div class="search_input" id="search_input_box">
			<div class="container">
				<form class="d-flex justify-content-between">
					<input type="text" class="form-control" id="search_input" placeholder="Search Here">
					<button type="submit" class="btn"></button>
					<span class="lnr lnr-cross" id="close_search" title="Close Search"></span>
				</form>
			</div>
		</div>
	</header>
	<!-- End Header Area -->

    <!-- Start Banner Area -->
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>Review Order</h1>
                    <nav class="d-flex align-items-center">
                        <a href="index.html">Home<span class="lnr lnr-arrow-right"></span></a>
                        <a href="category.html">Cart</a>
                    </nav>
                </div>
            </div>
        </div>
    </section>
    <!-- End Banner Area -->

    <!--================Cart Area =================-->
    <section class="cart_area">
        <div class="container">
            <div class="cart_inner">
                <div class="table-responsive">
                    <asp:GridView ID="GridViewReview" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="sno" HeaderText="SNo" />
                            <asp:BoundField DataField="product_id" HeaderText="Product ID" Visible="False" />
                            <asp:ImageField DataImageUrlField="product_image" HeaderText="Image">
                                <ItemStyle HorizontalAlign="Center" Height="150px" Width="100px"/>
                            </asp:ImageField>
                            <asp:BoundField DataField="product_name" HeaderText="Product Name">
                            </asp:BoundField>
                            <asp:BoundField DataField="product_brand" HeaderText="Brand">
                            </asp:BoundField>
                            <asp:BoundField DataField="product_cost" HeaderText="Price">
                            <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>

                    <!--================Order Details Area =================-->
	                <section class="order_details section_gap">
		                <div class="container">
			                <div class="row order_d_inner">
				                <div class="col-lg-6">
					                <div class="details_item">
						                <h4>Order Info</h4>
						                <ul class="list">
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
				                <div class="col-lg-6">
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
			                </div>
		                </div>
	                </section>
	                <!--================End Order Details Area =================-->

                    <table class="table">
                        <tbody>
                            <tr>
                                <td>

                                </td>
                                <td>

                                </td>
                                <td>
                                    <h5>Subtotal</h5>
                                </td>
                                <td>
                                    $<asp:Label ID="lbl_subtotal" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                           
                            <tr class="out_button_area">
                                <td>

                                </td>
                                <td>

                                </td>
                                <td>

                                </td>
                                <td>
                                    <div class="checkout_btn_inner d-flex align-items-center">
                                        &nbsp;<asp:LinkButton ID="LinkButton1" runat="server" CssClass="gray_btn" OnClick="CheckoutConfirm_Click">Complete Order</asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </section>
    <!--================End Cart Area =================-->
</asp:Content>
