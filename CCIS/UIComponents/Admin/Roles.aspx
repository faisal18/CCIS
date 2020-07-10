﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


        <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Roles Information</h3>


        <asp:GridView ID="GV_Roles" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="100"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="RolesID"
            OnRowCommand="GV_Roles_RowCommand"
            OnRowUpdating="GV_Roles_RowUpdating"
            OnRowDeleting="GV_Roles_RowDeleting"
            OnRowEditing="GV_Roles_RowEditing"
            OnRowCancelingEdit="GV_Roles_RowCancelingEdit"
            OnPageIndexChanging="GV_Roles_PageIndexChanging">
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("RolesID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_RolesID" Text='<%# Eval("RolesID") %>' runat="server"></asp:Label>
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
