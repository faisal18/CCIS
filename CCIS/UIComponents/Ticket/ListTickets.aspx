<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListTickets.aspx.cs" Inherits="CCIS.UIComponenets.ListTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" >
        <h3>Tickets list</h3>


        <div>
            <asp:Label runat="server" CssClass="info" Text="Search"></asp:Label>
            <asp:TextBox runat="server" CssClass="form-control-static" ID="txt_searchbox"></asp:TextBox>

            <asp:DropDownList runat="server" ID="ddl_search">

                <asp:ListItem Value="TicketNumber">Ticket Number</asp:ListItem>
                <asp:ListItem Value="Subject">Summary</asp:ListItem>

                <asp:ListItem Value="CallerName">Caller Name</asp:ListItem>
                <asp:ListItem Value="Status">Status</asp:ListItem>
                <asp:ListItem Value="Type">Type</asp:ListItem>
                
                <asp:ListItem Value="ReportedBy">Reported By</asp:ListItem>
                <asp:ListItem Value="Assignee">Assignee</asp:ListItem>
                <asp:ListItem Value="CallerLicense">CallerFacility</asp:ListItem>


            </asp:DropDownList>
            <asp:Button runat="server" CssClass="btn-default" Text="Search" ID="btn_search" OnClick="btn_search_Click" />
        </div>

        <div id="grid" style="overflow:auto">
            <asp:GridView ID="GV_ListTickets" CssClass="table  table-hover table-bordered  pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
                AllowPaging="true" PageSize="10"
                AutoGenerateColumns="false"
                ShowHeaderWhenEmpty="true"
                ShowFooter="true"
                DataKeyNames="TicketInformationID"
                OnPageIndexChanging="GV_ListTickets_PageIndexChanging">

                <Columns>
                    <asp:TemplateField HeaderText="Ticket Number">
                        <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>
                                <%--<asp:LinkButton runat="server" Text='<%# Eval("TicketNumber") %>' ID="link_TicketNumber" CommandArgument='<%# Container.DataItemIndex %>' CommandName="TicketNumber" OnCommand="link_TicketNumber_Command"> </asp:LinkButton>
                                <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />--%>
                            </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Summary">
                        <ItemTemplate>
                      <%--      <asp:LinkButton runat="server" ID="Link_Summary" CommandArgument='<%# Container.DataItemIndex %>' CommandName="Subject" OnCommand="Link_Button_Command" Text='<%# Eval("Subject") %>'></asp:LinkButton>
                            <asp:HiddenField runat="server" ID="hdn_SummaryID" Visible="true" Value='<%#Eval("TicketInformationID") %>' />
                    --%>
                               <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                       

                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Caller">
                        <ItemTemplate>
                          <asp:Label runat="server" Text='<%# Eval("CallerName") %>'></asp:Label>
                          </ItemTemplate>
                    </asp:TemplateField>

                          <asp:TemplateField HeaderText="CallerFacility">
                        <ItemTemplate>
                          <asp:Label runat="server" Text='<%# Eval("CallerLicense") %>'></asp:Label>
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

                    <%--<asp:TemplateField HeaderText="Severity">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Severity") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Priority">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Eval("Priority") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>


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

                     <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Type" Text='<%# Eval("Type") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
        </div>

        <asp:Label runat="server" ID="lbl_message"></asp:Label>

    </div>

</asp:Content>
