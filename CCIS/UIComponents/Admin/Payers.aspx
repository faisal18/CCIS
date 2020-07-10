<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Payers.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Payers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Payer Information</h3>


        <asp:GridView ID="GV_Payer" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="PayerID"

            OnRowCommand="GV_Payer_RowCommand"
            OnRowEditing="GV_Payer_RowEditing"
            OnRowCancelingEdit="GV_Payer_RowCancelingEdit"
            OnRowUpdating="GV_Payer_RowUpdating"
            OnRowDeleting="GV_Payer_RowDeleting"
            OnRowDataBound="GV_Payer_RowDataBound"
            OnPageIndexChanging="GV_Payer_PageIndexChanging"
            
            >
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="Payer Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PayerId") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_PayerId" Enabled="false" Text='<%# Eval("PayerId") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Payer Code">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PayerCode") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_PayerCode" Text='<%# Eval("PayerCode") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_PayerCodeFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Payer Name">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PayerName") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_PayerName" Text='<%# Eval("PayerName") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_PayerNameFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Payer Type">
                    <ItemTemplate>
                        <asp:Label ID="lbl_PayerTypeDesc" Text='<%# Eval("Description") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_PayerType" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_PayerTypeFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="IsActive">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("IsActive") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Visible="false" ID="txt_IsActive" Text='<%# Eval("IsActive") %>'></asp:TextBox>
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

                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Email") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Email" Text='<%# Eval("Email") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_EmailFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Login Key">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("LoginKey") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_LoginKey" Text='<%# Eval("LoginKey") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_LoginKeyFooter" runat="server"></asp:TextBox>
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
