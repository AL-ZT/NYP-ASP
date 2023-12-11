<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="auditlogDetails.aspx.cs" Inherits="myShoeRack.auditlogDetails" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .section-gap {
            padding: 50px 0;
        }

        .lblh{
            text-transform: uppercase;
            text-align: left;
            font-weight: 700;
            font-size: 14px;
            /*align-content: flex-start;*/
        }

        .inner {
            box-shadow: 0px 10px 30px 0px rgba(0, 0, 0, 0.07);
            height: 100%;
            text-align: center;
            padding-top: 80px;
        }

            .inner h3 {
                text-align: center;
                color: #222222;
                text-transform: uppercase;
                font-size: 18px;
            }

        .control_group {
            text-align: center;
            align-content: center;
            padding-bottom: 80px;
            padding-left: 10px;
            padding-right: 10px;
        }

        .btn1 {
            text-align: center;
            padding-top: 30px;
        }

        .btn1 .genric-btn {
            width: 100%;
            padding: 5px;
            text-transform: uppercase;
            border: none;
            background: linear-gradient(90deg, #ffba00 0%, #ff6c00 100%);
        }
    </style>

    <!-- Start Banner Area -->
    <section class="banner-area organic-breadcrumb">
        <div class="container">
            <div class="breadcrumb-banner d-flex flex-wrap align-items-center justify-content-end">
                <div class="col-first">
                    <h1>Audit Log</h1>
                </div>
            </div>
        </div>
    </section>
    <!-- End Banner Area -->

    <div class="section_gap">
        <div class="container">
            <div class="row">
                <div class="col-md-2">
                </div>
                <div class="col-md-8">
                    <div class="inner">
                        <h3>Audit Details</h3>

                        <div class="row control_group">
                            <div class="col-md-12 control1">
                                <div class="col-md-12 control1">
                                    <asp:Label runat="server" CssClass="lblh">Code: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_code"></asp:Label>
                                </div>
                                <div class="col-md-12 control2">
                                    <asp:Label runat="server" CssClass="lblh">User ID: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_userid"></asp:Label>
                                </div>
                                <div class="col-md-12 control3">
                                    <asp:Label runat="server" CssClass="lblh">Username: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_username"></asp:Label>
                                </div>
                                <div class="col-md-12 control4">
                                    <asp:Label runat="server" CssClass="lblh">Date: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_date"></asp:Label>
                                </div>
                                <div class="col-md-12 control5">
                                    <asp:Label runat="server" CssClass="lblh">Time: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_time"></asp:Label>
                                </div>
                                <div class="col-md-12 control6">
                                    <asp:Label runat="server" CssClass="lblh">IP Address: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_ipadd"></asp:Label>
                                </div>
                                <div class="col-md-12 control7">
                                    <asp:Label runat="server" CssClass="lblh">Platform: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_platform"></asp:Label>
                                </div>
                                <%--<div class="col-md-12 control8">
                                    <asp:Label runat="server" CssClass="lblh">Address: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_add"></asp:Label>
                                </div>
                                <div class="col-md-12 control9">
                                    <asp:Label runat="server" CssClass="lblh">Phone Number: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_hp"></asp:Label>
                                </div>
                                <div class="col-md-12 control10">
                                    <asp:Label runat="server" CssClass="lblh">Product ID: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_pid"></asp:Label>
                                </div>
                                <div class="col-md-12 control11">
                                    <asp:Label runat="server" CssClass="lblh">Product Name: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_pname"></asp:Label>
                                </div>
                                <div class="col-md-12 control12">
                                    <asp:Label runat="server" CssClass="lblh">Product Brand: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_pbrand"></asp:Label>
                                </div>
                                <div class="col-md-12 control13">
                                    <asp:Label runat="server" CssClass="lblh">Product Cost: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_pcost"></asp:Label>
                                </div>--%>
                                <div class="col-md-12 control14">
                                    <asp:Label runat="server" CssClass="lblh">Details: </asp:Label>
                                    <asp:Label runat="server" CssClass="lbl" ID="lbl_details"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
