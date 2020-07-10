<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="CCIS.UIComponents.KPI.UserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
        <div>
            <h3 runat="server" id="h3_title"></h3>
        </div>

        <div style="overflow: auto">
            <h3>Inquiries Count</h3>
            <asp:Repeater runat="server" ID="rptr_InqCount">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">Name</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Count</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>

                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("ApplicationDesc") %>' NavigateUrl='<%# Eval("ApplicationDesc", "~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID={ApplicationDesc}")%>'></asp:HyperLink>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Count" Text='<%# Eval("Count") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>



        <div>
            <h4>Open Issues Count</h4>
        </div>
        <div>
            <asp:GridView runat="server" ID="gv_OIC"
                CssClass="table table-hover table table-bordered pagination-ys"
                Width="100%"
                OnPageIndexChanging="gv_OIC_PageIndexChanging"
                AutoGenerateColumns="false"
                AllowPaging="true"
                PageSize="10">
                <Columns>
                    <asp:TemplateField HeaderText="Application Name">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("Name") %>' NavigateUrl='<%# Eval("ApplicationID", "~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID={0}")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>


        <div>
            <h4>Closed Issues Count</h4>
        </div>
        <div>
            <asp:GridView runat="server" ID="gv_CIC"
                CssClass="table table-hover table table-bordered pagination-ys"
                Width="100%"
                OnPageIndexChanging="gv_CIC_PageIndexChanging"
                AutoGenerateColumns="false"
                AllowPaging="true"
                PageSize="10">
                <Columns>
                    <asp:TemplateField HeaderText="Application Name">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("Name") %>' NavigateUrl='<%# Eval("ApplicationID", "~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID={0}")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Count">
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Eval("Count") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>


        <h4>Open Issues</h4>
        <div style="overflow: auto">
            <asp:GridView runat="server" ID="gv_AssignedOpenIssues"
                CssClass="table table-hover table table-bordered pagination-ys"
                Width="100%"
                OnPageIndexChanging="gv_AssignedOpenIssues_PageIndexChanging"
                AutoGenerateColumns="false"
                AllowPaging="true"
                PageSize="10">

                <Columns>
                    <asp:TemplateField HeaderText="Ticket Number">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'></asp:HyperLink>
                            <%--<asp:LinkButton runat="server" ID="lb_Ticket" Text='<%# Eval("TicketNumber") %>' OnCommand="lb_Ticket_Command" CommandArgument='<%# Container.DataItemIndex %>' CommandName='<%# Eval("TicketInformationID") %>'></asp:LinkButton>--%>
                        </ItemTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Status" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subject">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_ticketsubject" Text='<%# Eval("Subject") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Label1" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action Taken">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Label2" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Application Name">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="LB_Application" Text='<%# Eval("Application") %>' OnCommand="LB_Application_Command" CommandArgument='<%# Container.DataItemIndex %>' CommandName='<%# Eval("ApplicationID") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </div>







        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>

    </div>

</asp:Content>
