<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="L2_TicketQueue.aspx.cs" Inherits="CCIS.UIComponents.Ticket.L2.L2_TicketQueue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h3>L2 Tickets Queue</h3>






        <ul class="nav nav-tabs">

            <li>
                <a class="tablinks" data-toggle="tab" id="CommentsTab" href="#TechSupport">TechSupport</a>
            </li>

            <li>
                <a class="tablinks" data-toggle="tab" id="ResolutionTab" href="#TechGuys">TechSupportTeam</a>
            </li>

            <li>
                <a class="tablinks" data-toggle="tab" id="NonIncidentTab" href="#NonIncident">Non-Incident</a>
            </li>


        </ul>

        <div class="tab-content">
            <div id="TechSupport" class="tab-pane fade in active">
                <asp:Panel runat="server" ID="PNL_TechSupport">
                    <div id="grid" style="overflow: auto">

                        <div>
                            <asp:Label runat="server" CssClass="info" Text="Search"></asp:Label>
                            <asp:TextBox runat="server" CssClass="form-control" ID="txt_searchbox"></asp:TextBox>

                            <asp:DropDownList runat="server" ID="ddl_search">

                                <asp:ListItem Value="TicketNumber">Ticket Number</asp:ListItem>
                                <asp:ListItem Value="Subject">Summary</asp:ListItem>

                                <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                                <asp:ListItem Value="Status">Status</asp:ListItem>

                                <asp:ListItem Value="ReportedBy">Reported By</asp:ListItem>
                                <asp:ListItem Value="Assignee">Assignee</asp:ListItem>

                            </asp:DropDownList>
                            <asp:Button runat="server" CssClass="btn-default" Text="Search" ID="btn_search" OnClick="btn_search_Click" />
                        </div>
                        <br />
                        <br />




                        <asp:GridView ID="GV_ListTickets" CssClass="table  table-hover table-bordered  pagination-ys" Width="100%" runat="server" CellPadding="10" CellSpacing="5"
                            AllowPaging="true" PageSize="10"
                            AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true"
                            ShowFooter="true"
                            DataKeyNames="TicketInformationID"
                            OnPageIndexChanging="GV_ListTickets_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Assign">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" Text="Assign to me" ID="link_AssignToMe" CommandArgument='<%# Eval("TicketInformationID") %>' CommandName="AssignToMe" OnCommand="link_AssignToMe_Command"> </asp:LinkButton>
                                        <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ticket Number">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Summary">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Caller">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CallerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported By">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("ReportedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported To">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Assignee") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CreationDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Status" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div id="TechGuys" class="tab-pane fade in">
                <asp:Panel runat="server" ID="PNL_TechGuys" Style="overflow: auto">

                    <div>
                        <asp:Label runat="server" CssClass="info" Text="Search"></asp:Label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txt_SearchBox2"></asp:TextBox>

                        <asp:DropDownList runat="server" ID="ddl_search2">

                            <asp:ListItem Value="TicketNumber">Ticket Number</asp:ListItem>
                            <asp:ListItem Value="Subject">Summary</asp:ListItem>

                            <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>

                            <asp:ListItem Value="ReportedBy">Reported By</asp:ListItem>
                            <asp:ListItem Value="Assignee">Assignee</asp:ListItem>

                        </asp:DropDownList>
                        <asp:Button runat="server" CssClass="btn-default" Text="Search" ID="btn_Search2" OnClick="btn_Search2_Click" />
                    </div>
                    <br />
                    <br />

                    <div style="overflow: auto">
                        <asp:GridView ID="GV_TechGuys" CssClass="table  table-hover table-bordered  pagination-ys"  Width="100%" runat="server" CellPadding="10" CellSpacing="5"
                            AllowPaging="true" PageSize="50"
                            AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true"
                            ShowFooter="true"
                            DataKeyNames="TicketInformationID"
                            OnPageIndexChanging="GV_TechGuys_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Ticket Number">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Summary">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Caller">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CallerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported By">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("ReportedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported To">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Assignee") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CreationDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Status" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
            <div id="NonIncident" class="tab-pane fade in">
                <asp:Panel runat="server" ID="Panel1">

                    <div>
                        <asp:Label runat="server" CssClass="info" Text="Search"></asp:Label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="TextBox1"></asp:TextBox>

                        <asp:DropDownList runat="server" ID="ddl_nonincident">

                            <asp:ListItem Value="TicketNumber">Ticket Number</asp:ListItem>
                            <asp:ListItem Value="Subject">Summary</asp:ListItem>

                            <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                            <asp:ListItem Value="Status">Status</asp:ListItem>

                            <asp:ListItem Value="ReportedBy">Reported By</asp:ListItem>
                            <asp:ListItem Value="Assignee">Assignee</asp:ListItem>

                        </asp:DropDownList>
                        <asp:Button runat="server" CssClass="btn-default" Text="Search" ID="Button1" OnClick="Button1_Click" />
                    </div>
                    <br />
                    <br />

                    <div style="overflow: auto">
                        <asp:GridView ID="GV_NonIncident" CssClass="table  table-hover table-bordered  pagination-ys"  Width="100%" runat="server" CellPadding="10" CellSpacing="5"
                            AllowPaging="true" PageSize="100"
                            AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true"
                            ShowFooter="true"
                            DataKeyNames="TicketInformationID"
                            OnPageIndexChanging="GV_NonIncident_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Ticket Number">
                                    <ItemTemplate>
                                        <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
								
								 <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Status" Text='<%# Eval("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_Status" Text='<%# Eval("Type") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
								
								<asp:TemplateField HeaderText="Created Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CreationDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Summary">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Caller">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CallerName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported By">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("ReportedBy") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Reported To">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Assignee") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                

                               

                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </div>
        </div>







        <asp:Label runat="server" ID="lbl_message"></asp:Label>

    </div>

</asp:Content>

