<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Errorpanel.aspx.cs" Inherits="myShoeRack.Admin.Errorpanel" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>Error</h1>
                </div>
            </div>
        </div>
    </section>
    <section class="tracking_box_area section_gap">
        <div class="container">
            <div class="tracking_box_inner">
                <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" Style="color: red"></asp:Label>
                <asp:Panel ID="DetailedErrorPanel" runat="server">
                    <p>&nbsp;</p>


                    <h4>Error ID:</h4>
                    <p>
                        <asp:Label ID="ErrorId" runat="server" Font-Size="Small" /><br />
                    </p>


                    <h4>Date and Time:</h4>
                    <p>
                        <asp:Label ID="DateTime_LB" runat="server" Font-Size="Medium" /><br />
                    </p>

                    <h4>Detailed Error:</h4>
                    <p>
                        <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
                    </p>

                    <h4>Error Handler:</h4>
                    <p>
                        <asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
                    </p>

                    <h4>Detailed Error Message:</h4>
                    <p>
                        <asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /><br />
                    </p>
                    <p>
                        <asp:Label ID="InnerTrace" runat="server" />
                    </p>
                    <br />
                </asp:Panel>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="Stylesheets">
    <link rel="stylesheet" href="../css/linearicons.css">
    <link rel="stylesheet" href="../css/font-awesome.min.css">
    <link rel="stylesheet" href="../css/themify-icons.css">
    <link rel="stylesheet" href="../css/bootstrap.css">
    <link rel="stylesheet" href="../css/owl.carousel.css">
    <link rel="stylesheet" href="../css/nice-select.css">
    <link rel="stylesheet" href="../css/nouislider.min.css">
    <link rel="stylesheet" href="../css/ion.rangeSlider.css" />
    <link rel="stylesheet" href="../css/ion.rangeSlider.skinFlat.css" />
    <link rel="stylesheet" href="../css/magnific-popup.css">
    <link rel="stylesheet" href="../css/main.css">
</asp:Content>
