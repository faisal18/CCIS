<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PayerType.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Payer_Type" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron"  style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Payer Type Information</h3>


        <asp:GridView ID="GV_PayerType" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false" 
            ShowHeaderWhenEmpty="true" 
            showfooter="true"
            DataKeyNames="PayerTypeID"
          
            OnRowCommand="GV_PayerType_RowCommand" 
            OnRowEditing="GV_PayerType_RowEditing"
            OnRowCancelingEdit="GV_PayerType_RowCancelingEdit" 
            OnRowUpdating="GV_PayerType_RowUpdating"
            OnRowDeleting="GV_PayerType_RowDeleting"
            
            >
            <Columns>

                <asp:TemplateField Visible="false" HeaderText="Payer Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PayerTypeID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_PayerId" Enabled="false" Text='<%# Eval("PayerTypeID") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                   <%-- <FooterTemplate>
                        <asp:TextBox ID="txt_PayerIdFooter" Enabled="false" runat="server"></asp:TextBox>
                    </FooterTemplate>--%>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Payer Type Description">
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

               <%-- <asp:TemplateField HeaderText="Payer License">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PayerLicense") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_PayerLicense" Text='<%# Eval("PayerLicense") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_PayerLicenseFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>--%>

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
