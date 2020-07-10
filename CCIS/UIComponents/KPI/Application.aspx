<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Application.aspx.cs" Inherits="CCIS.UIComponents.KPI.Application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Application</h2>




        <div class="jumbotron">

            <div style="overflow: auto">
                <asp:Repeater runat="server" ID="rptr_Applications">
                    <HeaderTemplate>
                        <table id="tbl_Session" class="table table-bordered table table-hover">
                            <tr>
                                <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">Application Name</th>
                                <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Owner</th>
                                <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(3)')">URL</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="item">
                            <td>
                                <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("Name") %>' NavigateUrl='<%# Eval("ApplicationsID", "~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID={0}")%>'  ></asp:HyperLink>
                                <%--<asp:LinkButton runat="server" ID="LB_Project" Text='<%# Eval("Name") %>' OnCommand="LB_Project_Command" CommandName='<%# Eval("ApplicationsID") %>' CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>--%>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" ID="lbl_OwnerName" Text='<%# Eval("Owner") %>' OnCommand="lbl_OwnerName_Command"  CommandArgument='<%# Container.DataItem %>' ></asp:LinkButton>
                            </td>
                            <td>
                                <asp:HyperLink runat="server" ID="lbl_URL" Text='<%# Eval("URL") %>'></asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            <div>
                <asp:Label runat="server" ID="lbl_message"></asp:Label>
            </div>
        </div>



</asp:Content>
