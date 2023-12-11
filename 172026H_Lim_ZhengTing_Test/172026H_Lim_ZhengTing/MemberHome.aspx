<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MemberHome.aspx.cs" Inherits="_172026H_Lim_ZhengTing.MemberHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <p>
        Your Name : <asp:Label ID="homeName" runat="server" Text=""></asp:Label>
    </p>
    <p>
        Credit Card Number : <asp:Label ID="homeCreditCardNumber" runat="server" Text=""></asp:Label>
    </p>
    <p>
        Date & Time Of Registration : <asp:Label ID="homeDateTime" runat="server" Text=""></asp:Label>
    </p>

    <br />
    <asp:Button ID="logoutBtn" runat="server" Text="Log Out" OnClick="logoutBtn_Click" />
</asp:Content>
