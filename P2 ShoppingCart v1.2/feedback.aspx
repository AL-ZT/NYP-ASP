<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="feedback.aspx.cs" Inherits="feedback" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .auto-style3 {
            width: 159px;
        }
        .auto-style4 {
            width: 115px;
        }
        .auto-style5 {
            width: 268px;
        }
        .auto-style6 {
            width: 159px;
            height: 46px;
        }
        .auto-style7 {
            width: 268px;
            height: 46px;
        }
        .auto-style8 {
            height: 46px;
        }
        .auto-style9 {
            width: 159px;
            height: 26px;
        }
        .auto-style10 {
            width: 268px;
            height: 26px;
        }
        .auto-style11 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <p>
        <table style="width:100%;">
            <tr>
                <td class="auto-style3">Comments</td>
                <td class="auto-style5">
                    <asp:TextBox ID="TextBox1" runat="server" Height="57px" TextMode="MultiLine" Width="243px"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator runat="server" ErrorMessage="RequiredFieldValidator"></asp:RequiredFieldValidator>
                    &nbsp;<asp:RequiredFieldValidator ID="rfv_Comments" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please enter your comments." ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style9">Email</td>
                <td class="auto-style10">
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style11">
                    <asp:RequiredFieldValidator ID="rfv_Comments0" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please enter your email." ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Confirm Email</td>
                <td class="auto-style5">
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="rfv_Comments1" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please enter your email." ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style6">How would you grade our web site?</td>
                <td class="auto-style7">
                    <select id="Select1" class="auto-style4" name="D1">
                        <option></option>
                    </select></td>
                <td class="auto-style8">
                    <asp:RequiredFieldValidator ID="rfv_Comments2" runat="server" ControlToValidate="TextBox1" ErrorMessage="Please select a choice." ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Send" />
                    <asp:Button ID="Button2" runat="server" Text="Cancel" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </p>
</asp:Content>

