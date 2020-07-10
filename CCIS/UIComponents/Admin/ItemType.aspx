<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemType.aspx.cs" Inherits="CCIS.UIComponenets.Admin.ItemType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Item Type Information</h3>


        <asp:GridView ID="GV_ItemTypes" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="ItemTypesID"
            OnRowCommand="GV_ItemTypes_RowCommand"
            OnRowUpdating="GV_ItemTypes_RowUpdating"
            OnRowDeleting="GV_ItemTypes_RowDeleting"
            OnRowEditing="GV_ItemTypes_RowEditing"
            OnRowCancelingEdit="GV_ItemTypes_RowCancelingEdit"
            OnPageIndexChanging="GV_ItemTypes_PageIndexChanging">
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ItemTypesID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_ItemTypesID" Text='<%# Eval("ItemTypesID") %>' runat="server"></asp:Label>
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

                <asp:TemplateField HeaderText="Categories">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Categories") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Categories" Text='<%# Eval("Categories") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_CategoriesFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

               
<asp:TemplateField HeaderText="Role">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Role") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Role" Text='<%# Eval("Role") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_RoleFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

<asp:TemplateField HeaderText="Scenario">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Scenario") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Scenario" Text='<%# Eval("Scenario") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ScenarioFooter" runat="server"></asp:TextBox>
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
