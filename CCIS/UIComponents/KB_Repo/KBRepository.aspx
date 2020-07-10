<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KBRepository.aspx.cs" Inherits="CCIS.UIComponents.KB_Repo.KBRepository" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

   
    <div class="jumbotron">
        <h3>Knowledge Base Repository</h3>
        <a href="https://quintiles.sharepoint.com/sites/AMESA-PP/IT/SitePages/AMESA-PP-IT.aspx" target="_blank">Sharepoint Knowledge Base Repository</a>
       
        <br />
        <asp:Table runat="server" CssClass="table table-responsive">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" Text="Application"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Application" CssClass="form-control" OnSelectedIndexChanged="ddl_Application_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            
            <asp:TableRow>
                    <asp:TableCell>
                    <asp:Label runat="server" Text="Search"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <div>
                            <div>
                                <asp:TextBox runat="server" ID="txt_searchbox"></asp:TextBox>
                            
                                <asp:DropDownList runat="server" ID="ddl_search" >
                                    <asp:ListItem Value="TicketNumber">Ticket Number</asp:ListItem>
                                    <asp:ListItem Value="Subject">Subject</asp:ListItem>
                                    <asp:ListItem Value="Description">Description</asp:ListItem>
                                    <asp:ListItem Value="ActionTaken">ActionTaken</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <div>
            <asp:Button runat="server" CssClass="btn-default" Text="Search" ID="btn_search" OnClick="btn_search_Click" />
        </div>

        <div style="overflow: auto">
            <asp:Repeater runat="server" ID="repeater_assignedtosession">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">TicketNumber</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Subject</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(3)')">Description</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(4)')">ActionTaken</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(5)')">Category</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:LinkButton runat="server" ID="lb_Ticket" Text='<%# Eval("TicketNumber") %>' OnCommand="lb_Ticket_Command" CommandName='<%# Eval("TicketInformationID") %>' CommandArgument='<%# Eval("TicketInformationID") %>'></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_ticketsubject" Text='<%# Eval("Subject") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label1" Text='<%# Eval("Description") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label2" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="Label3" Text='<%# Eval("Value") %>'></asp:Label>
                        </td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>


        <div id="myModal" class="modal fade" style="overflow: auto;" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Resolution</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel runat="server" ID="pnl_ResolDetail" Visible="false">
                            <asp:Table runat="server" CssClass="table table-bordered" ID="tbl_Popup">

                                <asp:TableRow>
                                    <asp:TableCell>
                             <asp:Label runat="server" Text="Created By: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_CreatedBy"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                            <asp:Label runat="server" Text="Resolved By: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_ResolvedBy"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                            <asp:Label runat="server" Text="Description: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_Description"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                            <asp:Label runat="server" Text="Action Taken: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_ActionTaken"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                            <asp:Label runat="server" Text="Root Cause: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_RootCause"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                            <asp:Label runat="server" Text="Steps Taken: "></asp:Label>

                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Label runat="server" ID="lbl_StepsTaken"></asp:Label>

                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>

                        </asp:Panel>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <script type="text/javascript">
            function showModal() {
                $("#myModal").modal('show');
            }


        </script>

        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>
    </div>

</asp:Content>
