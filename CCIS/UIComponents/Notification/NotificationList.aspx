<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotificationList.aspx.cs" Inherits="CCIS.UIComponents.Notification.NotificationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div>
        <h2>Notification List</h2>
    </div>
    <div class="jumbotron">



        <div style="overflow: auto">
            <asp:Repeater runat="server" ID="repeater_NotificationList">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table table table-hover">
                        <tr>

                            <%--<th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">Description</th>--%>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <%--<asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'></asp:HyperLink>--%>
                            <%--<asp:Label runat="server" ID="lbl_Description" Text='<%# Eval("Description") %>'></asp:Label>--%>
                            <asp:Panel runat="server" ID="CommentsContainer">
                            </asp:Panel>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Panel runat="server" CssClass="table table-bordered table-condensed table-responsive" ID="CommentsContainer">
            </asp:Panel>


        </div>




        <asp:Label runat="server" ID="lbl_message"></asp:Label>
    </div>


</asp:Content>
