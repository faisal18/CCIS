<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SystemAdminUser.aspx.cs" Inherits="CCIS.UIComponenets.Admin.SystemAdminUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>System Admin User</h3>



        <asp:GridView ID="GV_SysAdminUser" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="200"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="SystemUserID"
            OnRowCommand="GV_SysAdminUser_RowCommand"
            OnRowUpdating="GV_SysAdminUser_RowUpdating"
            OnRowDeleting="GV_SysAdminUser_RowDeleting"
            OnRowDataBound="GV_SysAdminUser_RowDataBound"
            OnRowEditing="GV_SysAdminUser_RowEditing"
            OnRowCancelingEdit="GV_SysAdminUser_RowCancelingEdit"
            OnPageIndexChanging="GV_SysAdminUser_PageIndexChanging">
            <Columns>

                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("SystemUserID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_SystemUserID" Text='<%# Eval("SystemUserID") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Username">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("username") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_username" Text='<%# Eval("username") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_usernameFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Password">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("password") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_password" Text='<%# Eval("password") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_passwordFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Person Name">
                    <ItemTemplate>
                        <asp:Label ID="lbl_FullName" Text='<%# Eval("FullName") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_FullName" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_FullNameFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="isAdmin">
                    <ItemTemplate>
                        <asp:Label ID="lbl_isAdmin" Text='<%# Eval("isAdmin") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_isAdmin" runat="server">
                            <asp:ListItem Value="true">Admin</asp:ListItem>
                            <asp:ListItem Value="false">Staff</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_isAdminFooter" runat="server">
                             <asp:ListItem Value="true">Admin</asp:ListItem>
                            <asp:ListItem Value="false">Staff</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>


                   <asp:TemplateField HeaderText="isActive">
                    <ItemTemplate>
                        <asp:Label ID="lbl_isActive" Text='<%# Eval("isActive") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_isActive" runat="server">
                            <asp:ListItem Value="true">True</asp:ListItem>
                            <asp:ListItem Value="false">False</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_isActiveFooter" runat="server">
                             <asp:ListItem Value="true">True</asp:ListItem>
                            <asp:ListItem Value="false">False</asp:ListItem>
                        </asp:DropDownList>
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
    </div>

    <asp:Label runat="server" ID="lbl_message"></asp:Label>

</asp:Content>
