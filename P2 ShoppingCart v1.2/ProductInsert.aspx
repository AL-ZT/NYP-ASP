<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductInsert.aspx.cs" Inherits="ProductInsert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .auto-style3 {
            width: 138px;
        }
        .auto-style5 {
            width: 138px;
            height: 21px;
        }
        .auto-style6 {
            height: 21px;
        }
    .auto-style7 {
        height: 21px;
        width: 471px;
    }
    .auto-style8 {
        width: 471px;
    }
    .auto-style9 {
        width: 138px;
        height: 38px;
    }
    .auto-style10 {
        width: 471px;
        height: 38px;
    }
    .auto-style11 {
        height: 38px;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <table style="width:100%;">
        <tr>
            <td class="auto-style5">Product ID</td>
            <td class="auto-style7">
                <asp:TextBox ID="tb_ProductID" runat="server"></asp:TextBox>
            </td>
            <td class="auto-style6">
                <asp:RequiredFieldValidator ID="rfv_ProductID" runat="server" ControlToValidate="tb_ProductID" ErrorMessage="Please Enter a Product ID" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            &nbsp;<td class="auto-style3">Product Name</td>
            &nbsp;<td class="auto-style8">
                <asp:TextBox ID="tb_ProductName" runat="server" Width="420px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfv_ProductName" runat="server" ControlToValidate="tb_ProductName" ErrorMessage="Please Enter a Name for the product." ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            &nbsp;</tr>
        <tr>
            &nbsp;<td class="auto-style3">Product Desc</td>
            &nbsp;<td class="auto-style8">
                <asp:TextBox ID="tb_ProductDesc" runat="server" Height="137px" TextMode="MultiLine" Width="438px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfv_ProductDesc" runat="server" ControlToValidate="tb_ProductDesc" ErrorMessage="Please enter a description for the product." ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
            &nbsp;</tr>
        <tr>
            <td class="auto-style3">Unit Price</td>
            <td class="auto-style8">
                <asp:TextBox ID="tb_UnitPrice" runat="server" Width="420px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="rfv_UnitPrice" runat="server" ControlToValidate="tb_UnitPrice" ErrorMessage="Please enter a Unit Price for the product." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cv_UnitPrice" runat="server" ControlToValidate="tb_UnitPrice" ErrorMessage="Only Numeric value is allowed" ForeColor="Red" Operator="DataTypeCheck" Display="Dynamic" Type="Integer"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style9">Stock Level</td>
            <td class="auto-style10">
                <asp:TextBox ID="tb_StockLevel" runat="server" Width="419px"></asp:TextBox>
            </td>
            <td class="auto-style11">
                <asp:RequiredFieldValidator ID="rfv_StockLevel" runat="server" ControlToValidate="tb_StockLevel" EnableTheming="True" ErrorMessage="Please enter a value for the Stock Level." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cv_StockLevel" runat="server" ControlToValidate="tb_StockLevel" ErrorMessage="Only Numeric value is allowed" ForeColor="Red" Operator="DataTypeCheck" Display="Dynamic" Type="Integer"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style3">Product Image</td>
            <td class="auto-style8">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td>
                <asp:RequiredFieldValidator ID="fileupload_Image" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Please select a Product Image" ForeColor="Red"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="auto-style3">
                <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            </td>
            <td class="auto-style8">
                <asp:Label ID="lbl_Result" runat="server" Text="&lt;&lt; &gt;&gt;"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style8">
                <asp:Button ID="btn_Insert" runat="server" Text="Insert" OnClick="btn_Insert_Click" />
                <asp:Button ID="btn_ProductView" runat="server" Text="View Product List" OnClick="btn_ProductView_Click" CausesValidation="False" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style5"></td>
            <td class="auto-style7">
                </td>
            <td class="auto-style6">
                </td>
        </tr>
        <tr>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style8">
                Validation error message:</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style3">&nbsp;</td>
            <td class="auto-style8">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

