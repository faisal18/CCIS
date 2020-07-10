<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Email.aspx.cs" Inherits="CCIS.UIComponents.Admin.Email" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">

    <h3>Email Templates</h3>

        <asp:GridView ID="GV_Email" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false" 
            ShowHeaderWhenEmpty="true" 
            showfooter="true"
            DataKeyNames="TemplateID"
          
            OnRowCommand="GV_Email_RowCommand" 
            OnRowEditing="GV_Email_RowEditing"
            OnRowCancelingEdit="GV_Email_RowCancelingEdit" 
            OnRowUpdating="GV_Email_RowUpdating"
            OnRowDeleting="GV_Email_RowDeleting"
            >

            <Columns>
                 <asp:TemplateField Visible="false" HeaderText="Payer Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("TemplateID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_EmailID" Enabled="false" Text='<%# Eval("TemplateID") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>

                   <asp:TemplateField HeaderText="Email Category">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Category") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Category" Text='<%# Eval("Category") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_CategoryFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Notification Type">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("TemplateType") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_NotificationType" Text='<%# Eval("TemplateType") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_NotificationTypeFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Email Subject">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("EmailSubject") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_EmailSubject" Text='<%# Eval("EmailSubject") %>' runat="server" TextMode="MultiLine"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_EmailSubjectFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Email Template">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("EmailBody") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Description" Text='<%# Eval("EmailBody") %>' runat="server" ValidateRequestMode="Disabled" TextMode="MultiLine"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_DescriptionFooter" runat="server"></asp:TextBox>
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

         <br />
        <asp:Label runat="server" ID="lbl_message"></asp:Label>

    </div>


</asp:Content>
