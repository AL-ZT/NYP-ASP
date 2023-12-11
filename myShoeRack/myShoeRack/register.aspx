<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="myShoeRack.register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

	<!-- Start Banner Area -->
	<section class="banner-area organic-breadcrumb">
		<div class="container">
			<div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
				<div class="col-first">
					<h1>Registration</h1>
				</div>
			</div>
		</div>
	</section>
	<!-- End Banner Area -->

    <br />

	<!--================ Registration Area =================-->
	<section>
        <div align="center">
            <asp:Label ID="lblmsg" runat="server"></asp:Label>

            <h4>Enter your basic information here!</h4>

            <hr width="40%"/>
        </div>
        <table style="margin:0 auto">  
            <tr>  
                <td>Username</td>  
                <td>  
                    <asp:TextBox ID="username" runat="server" Height="20px" Width="345px"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        runat="server" ID="requiredUsername"
                        ErrorMessage="Please enter a username" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="username" />
                </td>
  
            </tr>  
            <tr>  
                <td>Password</td>  
                <td> 
                    <asp:TextBox ID="pwd" runat="server" TextMode="Password" Height="20px" Width="345px"></asp:TextBox>
                    <ajaxToolkit:PasswordStrength 
                        ID="pwd_PasswordStrength" runat="server" 
                        StrengthIndicatorType="BarIndicator" 
                        BehaviorID="txtPassword_PasswordStrength" 
                        TargetControlID="pwd" 
                        MinimumLowerCaseCharacters="2" 
                        MinimumNumericCharacters="2" 
                        MinimumUpperCaseCharacters="1"
                        PreferredPasswordLength="10" 
                        MinimumSymbolCharacters="1" 
                        CalculationWeightings="35;25;20;20"
                        StrengthStyles="barInternal;barInternalYellow;barInternalGreen"
                        BarBorderCssClass="barBorder"/>
                    <asp:RequiredFieldValidator
                        runat="server" ID="requiredPwd"
                        ControlToValidate="pwd" Display="Dynamic" ForeColor="Red"
                        ErrorMessage="Please enter a password" />
                    
                </td>
            </tr>  
            <tr>  
                <td>Confirm Password</td>  
                <td>  
                    <asp:TextBox ID="confPwd" runat="server" TextMode="Password" Height="20px" Width="345px"></asp:TextBox> 
                    <asp:CompareValidator 
                        ID="comparePwd" runat="server" 
                        ControlToValidate="confPwd"
                        ControlToCompare="pwd" Display="Dynamic" ForeColor="Red"
                        ErrorMessage="Passwords do not match" />
                    <asp:RequiredFieldValidator 
                        ErrorMessage="Please confirm your password" 
                        runat="server" ID="requiredConfPwd" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="confPwd" />
                </td>
            </tr>
            <tr>
                <td>Email</td>
                <td>
                    <asp:TextBox ID="email" runat="server" Height="20px" Width="345px"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="requiredEmail" runat="server"
                        ErrorMessage="Please enter an email" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="email" />
                    <asp:RegularExpressionValidator
                        runat="server" ID="regexEmail"
                        ControlToValidate="email" Display="Dynamic" 
                        ErrorMessage="Enter the appropriate email format" 
                        ForeColor="Red" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                </td>
            </tr>
            <tr>
                <td>Phone Number</td>
                <td>
                    +
                    <asp:TextBox ID="countryCode" runat="server" Height="20px" Width="20px"></asp:TextBox>
                    <asp:TextBox ID="phoneNo" runat="server" Height="20px" Width="310px"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="requiredCountryCode" runat="server"
                        ErrorMessage="Please enter the country code" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="countryCode" />
                    <asp:RequiredFieldValidator
                        ID="requiredPhoneNo" runat="server"
                        ErrorMessage="Please enter a phone number" Display="Dynamic" ForeColor="Red"
                        ControlToValidate="phoneNo" />
                </td>
            </tr>
            
        </table>

        <br />

        <div align="center">
            <h4>Enter your address information here!</h4>
            <hr width="40%"/>
        </div>

        <table style="margin:0 auto">  
            <tr>  
                <td>Address Line 1</td>  
            </tr>  
            <tr>
                <td>
                    <asp:TextBox ID="address1" runat="server" Height="20px" Width="460px"></asp:TextBox> 
                </td>
            </tr>
            <tr>  
                <td>Address Line 2</td>  
            </tr>  
            <tr>
                <td>
                    <asp:TextBox ID="address2" runat="server" Height="20px" Width="460px"></asp:TextBox> 
                </td>
            </tr>
        </table>

        <table style="margin:0 auto">
            <tr>  
                <td>ZIP / Postal Code</td>  
                <td>City</td>
            </tr>  
            <tr>
                <td>
                    <asp:TextBox ID="postalCode" runat="server" Height="20px" Width="200px" TextMode="Number"></asp:TextBox> 
                </td>
                <td>
                    <asp:TextBox ID="city" runat="server" Height="20px" Width="260px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Country</td>
            </tr>
            <tr>
                <td>
                    <asp:dropdownlist runat="server" ID="countryDropdown"> 
                         <asp:listitem text="America" value="USA"></asp:listitem>
                         <asp:listitem text="China" value="CHI"></asp:listitem>
                         <asp:listitem text="Singapore" value="SG"></asp:listitem>
                    </asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
            </tr>
        </table>

        <br />

        <div align="center">
            <div class="g-recaptcha" data-sitekey="6Ld_KK0UAAAAAA8tr_ED0WIbWxWvra3k8jaUlWYB"></div>
            <br />
            <asp:Button ID="submitBtn" class="primary-btn" runat="server" Text="Submit" OnClick="submitBtn_Click" />
        </div>
        
	</section>
	<!--================ End Registration Area =================-->

</asp:Content>
