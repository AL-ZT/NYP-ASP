<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="auditlog.aspx.cs" Inherits="myShoeRack.auditlog" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .clrbtn, .dwldbtn {
            width: initial;
            padding-bottom: 10px;
        }

        .search {
        }

        .audit-table-wrap {
            overflow-x: scroll;
        }

        .audit-table {
            background: #f9f9ff;
            padding: 15px 0px 30px 0px;
            min-width: 800px;
        }

            .audit-table .code {
                width: 10%;
                padding-left: 30px;
            }

            .audit-table .datetime {
                width: 15%;
            }

            .audit-table .user {
                width: 25%;
            }

            .audit-table .ip {
                width: 15%;
            }

            .audit-table .descript {
                width: 25%;
            }

            .audit-table .details {
                width: 10%;
                padding-right: 50px;
                /*color: transparent;*/
            }

            /*.audit-table .details .detailsBtn {
                color: black !important;                
            }*/

            .audit-table .table-head {
                display: flex;
                color: #222222;
                line-height: 40px;
                text-transform: uppercase;
                font-weight: 500;
            }

            .audit-table .table-row {
                padding: 15px 0;
                border-top: 1px solid #edf3fd;
                display: flex;
                align-items: center;
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
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-10">
                        </div>
                        <div class="col-md-2">
                            <asp:Label runat="server" ID="lbl_onlineusers"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6 clrbtn dwldbtn">
                            <asp:Button runat="server" ID="btn_clear" Text="Clear Log" CssClass="genric-btn primary radius" OnClick="btn_clear_Click" />
                            <asp:Button runat="server" ID="btn_download" Text="Download as Text File" CssClass="genric-btn primary radius" OnClick="btn_download_Click" />
                        </div>
                        <div class="col-md-2">
                        </div>
                        <div class="col-md-4">
                            <div class="search">
                                <asp:TextBox runat="server" ID="tb_search"></asp:TextBox>
                                <asp:Button runat="server" ID="btn_search_submit" Text="Search" CssClass="genric-btn primary radius" OnClick="btn_search_submit_Click" />
                            </div>
                        </div>
                    </div>

                    <asp:UpdatePanel runat="server" ID="updatepanel" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="audit-table-wrap">
                                <div class="audit-table">
                                    <div class="table-head">
                                        <div class="code">#</div>
                                        <div class="datetime">Date/Time</div>
                                        <div class="user">Username</div>
                                        <div class="ip">Source IP</div>
                                        <div class="descript">Description</div>
                                        <div class="details">Details</div>
                                    </div>

                                    <div id="placeholdertablediv">
                                        <asp:PlaceHolder runat="server" ID="audittableph"></asp:PlaceHolder>

                                    </div>
                                    <asp:PlaceHolder runat="server" ID="searchresults"></asp:PlaceHolder>
                                </div>
                            </div>
                        </ContentTemplate>

                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btn_search_submit" EventName="Click" />
                            <%--                            <asp:AsyncPostBackTrigger ControlID="btn_clear" EventName="Click" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
