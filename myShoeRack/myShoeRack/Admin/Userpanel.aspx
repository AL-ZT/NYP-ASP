<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Userpanel.aspx.cs" Inherits="myShoeRack.Admin.Userpanel" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>User Profile</h1>
                </div>
            </div>
        </div>
    </section>
    <section>
        <div class="col-sm-8" align="center">
            <h4>Users Profile</h4>
        </div>
        <div class="col-sm-8" align="center">
            <table style="margin: 0 auto">
                <tr>
                    <td>User Id:</td>
                    <td>
                        <asp:Label runat="server" ID="userid"></asp:Label></td>
                </tr>
                <tr>
                    <td>Username:</td>
                    <td>
                        <asp:Label runat="server" ID="username"></asp:Label></td>
                </tr>
                <tr>
                    <td>Email:</td>
                    <td>
                        <asp:Label runat="server" ID="useremail"></asp:Label></td>
                </tr>
                <tr>
                    <td>Address:</td>
                    <td>
                        <asp:Label runat="server" ID="useraddress"></asp:Label></td>
                </tr>
                <tr>
                    <td>status:</td>
                    <td>
                        <asp:Label runat="server" ID="adminstatus"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="banbtn" Text="Ban" class="primary-btn" OnClick="banbtn_Click" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button runat="server" ID="unbanbtn" Text="Unban" class="primary-btn" Visible="False" OnClick="unbanbtn_Click" /></td>
                </tr>
            </table>
        </div>
         <div class="col-sm-8" align="center">
            <h4>User transactions</h4>
        </div>
        <div class="col-sm-10" align="center">
            <asp:GridView ID="gv_transaction" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1067px">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Order_Id" HeaderText="Order ID" />
                    <asp:BoundField DataField="userId" HeaderText="user ID" />
                    <asp:BoundField DataField="transactionID" HeaderText="Transaction ID" />
                    <asp:BoundField DataField="date" HeaderText="Date Time" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
        </div>
         <div class="col-sm-8" align="center">
            <h4>Intrusion detail</h4>
        </div>
        <div class="col-sm-10" align="center">
            <asp:GridView ID="gv_intrusion" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1067px">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="Intrusion_Id" HeaderText="Intrusion ID" />
                    <asp:BoundField DataField="Intrusion_Email" HeaderText="Intruder Email" />
                    <asp:BoundField DataField="Intrusion_Detail" HeaderText="Detail" />
                    <asp:BoundField DataField="Intrusion_Date_Time" HeaderText="Date Time" />
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
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
