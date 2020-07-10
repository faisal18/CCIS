<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SLADeclaration.aspx.cs" Inherits="CCIS.UIComponenets.Admin.SLADeclaration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


     <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>SLA Declaration</h3>


        <asp:GridView ID="GV_SLADeclaration" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="100"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="SLADeclarationsID"
            OnRowCommand="GV_SLADeclaration_RowCommand"
            OnRowUpdating="GV_SLADeclaration_RowUpdating"
            OnRowDeleting="GV_SLADeclaration_RowDeleting"
            OnRowDataBound="GV_SLADeclaration_RowDataBound"
            OnRowEditing="GV_SLADeclaration_RowEditing"
            OnRowCancelingEdit="GV_SLADeclaration_RowCancelingEdit"
            OnPageIndexChanging="GV_SLADeclaration_PageIndexChanging">
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("SLADeclarationsID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_SLADeclarationsID" Text='<%# Eval("SLADeclarationsID") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                    
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Description") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Description" Text='<%# Eval("Description") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_DescriptionFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IsActive">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("isActive") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Visible="false" ID="txt_IsActive" Text='<%# Eval("isActive") %>'></asp:TextBox>
                        <asp:DropDownList ID="ddl_IsActive" runat="server">
                            <asp:ListItem Value="1">True</asp:ListItem>
                            <asp:ListItem Value="0">False</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_IsActiveFooter" runat="server">
                            <asp:ListItem Value="1">True</asp:ListItem>
                            <asp:ListItem Value="0">False</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Sequence">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("SLASequenceID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_SLASequenceID" Text='<%# Eval("SLASequenceID") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_SLASequenceIDFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Time (min)">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("TimeinMinutes") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_TimeinMinutes" Text='<%# Eval("TimeinMinutes") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_TimeinMinutesFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("StatusDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Status" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_StatusFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="SubStatus">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("SubStatusDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_SubStatus" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_SubStatusFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                
                <asp:TemplateField HeaderText="Priority">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PriorityDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Priority" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_PriorityFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Severity">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("SeverityDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Severity" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_SeverityFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="ActionRequried">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ActionRequiredDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_ActionRequired" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_ActionRequiredFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Application">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ApplicationDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Application" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_ApplicationFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="NotificationType">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("NotificationTypeDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Notification" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_NotificationFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="TicketType">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("TicketTypeDescription") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_TicketType" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_TicketTypeFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:ImageButton ImageUrl="~/Images/Edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" />
                        <asp:ImageButton ImageUrl="~/Images/Delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton ImageUrl="~/Images/Update.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                        <asp:ImageButton ImageUrl="~/Images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:ImageButton ImageUrl="~/Images/Add.png" runat="server" CommandName="AddNew" ToolTip="Add New" Width="20px" Height="20px" />
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

        <asp:Label runat="server" ID="lbl_message"></asp:Label>


    </div>


</asp:Content>
