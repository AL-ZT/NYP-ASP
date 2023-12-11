<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="myShoeRack.Checkout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headerChange" runat="server">
</asp:Content>

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
									<li class="nav-item active"><a class="nav-link" href="checkout.html">Product Checkout</a></li>
									<li class="nav-item"><a class="nav-link" href="cart.html">Shopping Cart</a></li>
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
                    <h1>Checkout</h1>
                    <nav class="d-flex align-items-center">
                        <a href="index.html">Home<span class="lnr lnr-arrow-right"></span></a>
                        <a href="single-product.html">Checkout</a>
                    </nav>
                </div>
            </div>
        </div>
    </section>
    <!-- End Banner Area -->

    <!--================Checkout Area =================-->
    <section class="checkout_area section_gap">
        <div class="container">
            <div class="billing_details">
                <div class="row">
                    <div class="col-lg-8">
                        <h3>Billing Details</h3>
                        <form class="row contact_form" action="#" method="post" novalidate="novalidate">
                            <div class="col-md-6 form-group p_star">
                                First Name
                                <input type="text" class="form-control" id="first" name="name">
                            </div>
                            <div class="col-md-6 form-group p_star">
                                Last Name
                                <input type="text" class="form-control" id="last" name="name">
                            </div>
                            <div class="col-md-12 form-group">
                                Company Name
                                <input type="text" class="form-control" id="company" name="company">
                            </div>
                            <div class="col-md-6 form-group p_star">
                                Phone Number
                                <input type="text" class="form-control" id="number" name="number">
                            </div>
                            <div class="col-md-6 form-group p_star">
                                Email
                                <input type="text" class="form-control" id="email" name="compemailany">
                            </div>
                            <div class="col-md-12 form-group p_star">
                                Country
                                <select class="country_select">
                                    <option value="1">China</option>
                                    <option value="2">Malaysia</option>
                                    <option value="3">Philippines</option>
                                    <option server="" value="4<asp:ListView runat=">Singapore</option>
                                </select>
                            </div>
                            <div class="col-md-12 form-group p_star">
                                Address line 1
                                <input type="text" class="form-control" id="add1" name="add1">
                            </div>
                            <div class="col-md-12 form-group p_star">
                                Address line 2
                                <input type="text" class="form-control" id="add2" name="add2">
                            </div>
                            <div class="col-md-12 form-group p_star">
                                City
                                <input type="text" class="form-control" id="city" name="city">
                            </div>
                            <div class="col-md-12 form-group">
                                Postal Code
                                <input type="text" class="form-control" id="zip" name="zip">
                            </div>
                        </form>
                    </div>
                    <div class="col-lg-4">
                        <div class="order_box">
                            <h2>Your Order</h2>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%" CssClass="table" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="product_id" HeaderText="Product ID" Visible="False" />
                                    <asp:BoundField DataField="product_name" HeaderText="Product">
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="product_cost" HeaderText="Total">
                                    <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <ul class="list list_2">
                                <li><a href="#">Total <span>
                                    <asp:Label ID="lbl_subtotal" runat="server" Text="Label"></asp:Label>
                                    </span></a></li>
                            </ul>
                        </div>
                        <br />
                        <asp:Button style="background-image:url('https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif')" class="btn" Width="145" Height="43" BackColor="Transparent" BorderWidth="0" ID="btnPaypalCheckout" OnClick="PayPalBtn_Click" CausesValidation="False" runat="server" UseSubmitBehavior="false"/>
                        <%--<asp:ImageButton ID="CheckoutImageBtn" runat="server" 
                      ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" 
                      Width="145" AlternateText="Check out with PayPal" 
                      OnClick="PayPalBtn_Click"
                      BackColor="Transparent" BorderWidth="0" CausesValidation="False" UseSubmitBehavior="false"/>
                    </div>--%>
                </div>
            </div>
        </div>
    </section>
    <!--================End Checkout Area =================-->
</asp:Content>
