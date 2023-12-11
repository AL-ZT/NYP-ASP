<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="myShoeRack.ErrorPage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Start Banner Area -->
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>Error</h1>
                </div>
            </div>
        </div>
    </section>
    <!-- End Banner Area -->

    <!--================Error Page Area =================-->
    <section class="tracking_box_area section_gap">
        <div class="container">
            <div class="tracking_box_inner">
                <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" Style="color: red"></asp:Label>
                <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
                    <p>&nbsp;</p>
                    <h4>Detailed Error:</h4>
                    <p>
                        <asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /><br />
                    </p>

                    
                    <p>
                        <asp:Label Visible="false" ID="ErrorHandler" runat="server" Font-Size="Small" /><br />
                    </p>

                   
                    <p>
                        <asp:Label Visible="false" ID="InnerMessage" runat="server" Font-Size="Small" /><br />
                    </p>
                    <p>
                        <asp:Label Visible="false" ID="InnerTrace" runat="server" />
                    </p>
                </asp:Panel>
            </div>
        </div>
    </section>
    <!--================End Error Page Area =================-->
</asp:Content>
