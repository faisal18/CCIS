<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationProperty.aspx.cs" Inherits="CCIS.UIComponents.Admin.ApplicationProperty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
        <h3>Application Property Information</h3>
        <asp:GridView ID="GV_ApplicationProperty" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="ApplicationPropID"
            OnRowCommand="GV_ApplicationProperty_RowCommand"
            OnRowUpdating="GV_ApplicationProperty_RowUpdating"
            OnRowDeleting="GV_ApplicationProperty_RowDeleting"
            OnRowEditing="GV_ApplicationProperty_RowEditing"
            OnRowDataBound="GV_ApplicationProperty_RowDataBound"
            OnRowCancelingEdit="GV_ApplicationProperty_RowCancelingEdit"
            OnPageIndexChanging="GV_ApplicationProperty_PageIndexChanging">

            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ApplicationPropID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_ApplicationPropID" Text='<%# Eval("ApplicationPropID") %>' Visible="false" runat="server"></asp:Label>
                    </EditItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Application">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ApplicationName") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddl_application"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList runat="server" ID="ddl_applicationFooter"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Property">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Property") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddl_Property"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList runat="server" ID="ddl_PropertyFooter"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Value">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Value") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Value" Text='<%# Eval("Value") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ValueFooter" runat="server"></asp:TextBox>
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
