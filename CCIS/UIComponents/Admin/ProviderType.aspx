<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProviderType.aspx.cs" Inherits="CCIS.UIComponenets.Admin.ProviderType" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
       <h3>Provider Type Information</h3>


        <asp:GridView ID="GV_ProviderType" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false" 
            ShowHeaderWhenEmpty="true" 
            showfooter="true"
            DataKeyNames="ProviderTypeID"
            
            OnRowCommand="GV_ProviderType_RowCommand" 
            OnRowEditing="GV_ProviderType_RowEditing"
            OnRowCancelingEdit="GV_ProviderType_RowCancelingEdit" 
            OnRowUpdating="GV_ProviderType_RowUpdating"
            OnRowDeleting="GV_ProviderType_RowDeleting"
            
            >
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="Provider Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ProviderTypeID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_ProviderTypeID" Enabled="false" Text='<%# Eval("ProviderTypeID") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    
                </asp:TemplateField>


                <asp:TemplateField HeaderText="Provider Name">
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
