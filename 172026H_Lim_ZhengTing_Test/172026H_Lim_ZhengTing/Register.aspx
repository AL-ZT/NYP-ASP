<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="_172026H_Lim_ZhengTing.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div>
        <h1>Registration Form</h1>
    </div>
    <br />
    <div>
        <p>
            Email
        </p>
        <asp:TextBox ID="email" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="registerEmailValidator" runat="server" ErrorMessage="Email must be valid!" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="email" Display="Dynamic"></asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ID="registerEmailRequiredValidator" runat="server" ErrorMessage="Email Cannot Be Empty!" ControlToValidate="email" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>
            Password
        </p>
        <asp:TextBox ID="password" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="passwordRequiredValidator" runat="server" ErrorMessage="Password Cannot Be Empty!" ControlToValidate="password" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>
            Confirm Password
        </p>
        <asp:TextBox ID="cfmPassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:CompareValidator ID="passwordMatchValidator" runat="server" ErrorMessage="Passwords Do Not Match!" ControlToCompare="password" ForeColor="Red" ControlToValidate="cfmPassword" Display="Dynamic"></asp:CompareValidator>
        <asp:RequiredFieldValidator ID="cfmPasswordRequiredValidator" runat="server" ErrorMessage="Please ReType your Password!" ForeColor="Red" ControlToValidate="cfmPassword" Display="Dynamic"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>
            Credit Card Number
        </p>
        <asp:TextBox ID="creditCardNo" runat="server" TextMode="Number"></asp:TextBox>
        <asp:RequiredFieldValidator ID="creditCardRequiredValidator" runat="server" ErrorMessage="Credit Card Number Cannot Be Empty!" ControlToValidate="creditCardNo" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>
            Name
        </p>
        <asp:TextBox ID="memberName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="registerNameValidator" runat="server" ErrorMessage="Name cannot be empty!" ForeColor="Red" ControlToValidate="memberName"></asp:RequiredFieldValidator>
    </div>
    <div>
        <p>
            Age
        </p>
        <asp:TextBox ID="age" runat="server" TextMode="Number"></asp:TextBox>
        <asp:RequiredFieldValidator ID="ageRequiredValidator" runat="server" ErrorMessage="Age Cannot Be Empty!" ControlToValidate="age" ForeColor="Red"></asp:RequiredFieldValidator>
    </div>
    <br />
    <asp:Button ID="registerSubmitBtn" runat="server" Text="Register" OnClick="registerSubmitBtn_Click" />
</asp:Content>
