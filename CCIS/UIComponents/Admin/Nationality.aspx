<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Nationality.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Nationality" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


     <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Nationality Information</h3>


        <asp:GridView ID="GV_Nationality" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
             AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="NationalityId"
            
            OnRowCommand="GV_Nationality_RowCommand"
            OnRowEditing="GV_Nationality_RowEditing"
            OnRowCancelingEdit="GV_Nationality_RowCancelingEdit"
            OnRowUpdating="GV_Nationality_RowUpdating"
            OnRowDeleting="GV_Nationality_RowDeleting"
            
            >
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="Nationality Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("NationalityID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txt_NationalityId" Text='<%# Eval("NationalityID") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                    
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Nationality Description">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Description") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_NationalityDesc" Text='<%# Eval("Description") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_NationalityDescFooter" runat="server"></asp:TextBox>
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
