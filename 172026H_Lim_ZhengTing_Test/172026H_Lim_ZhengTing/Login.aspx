<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="_172026H_Lim_ZhengTing.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <h1>Login Form</h1>
    </div>
    <br />
    <div>
        <p>Email</p>
        <asp:TextBox ID="loginEmail" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="loginEmailRequiredValidator" runat="server" ErrorMessage="Email cannot be empty!" ControlToValidate="loginEmail" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>Password</p>
        <asp:TextBox ID="loginPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="loginPasswordRequiredValidator" runat="server" ErrorMessage="Password cannot be empty!" ControlToValidate="loginPassword" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <br />
    <div>
        <asp:Button ID="loginBtn" runat="server" Text="Login" OnClick="loginBtn_Click" />
    </div>
        

    <br />

    <asp:Label ID="errorMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>
