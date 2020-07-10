<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Providers.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Provider" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Providers Information</h3>


        <asp:GridView ID="GV_Provider" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" 
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false" 
            ShowHeaderWhenEmpty="true" 
            showfooter="true"
            DataKeyNames="ProviderID"
           
            OnRowCommand="GV_Provider_RowCommand"
            OnRowEditing="GV_Provider_RowEditing"
            OnRowCancelingEdit="GV_Provider_RowCancelingEdit" 
            OnRowUpdating="GV_Provider_RowUpdating"
            OnRowDeleting="GV_Provider_RowDeleting"
            OnRowDataBound="GV_Provider_RowDataBound"
            OnPageIndexChanging="GV_Provider_PageIndexChanging"
            
            >
            <Columns>

                <asp:TemplateField Visible="false" HeaderText="Provider Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ProviderId") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txt_ProviderId" Enabled="false" Text='<%# Eval("ProviderId") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                   
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Provider UId">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ProviderUId") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txt_ProviderUId" Text='<%# Eval("ProviderUId") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Provider License">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ProviderLicense") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_ProviderLicense" Text='<%# Eval("ProviderLicense") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ProviderLicenseFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Provider Name">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ProviderName") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_ProviderName" Text='<%# Eval("ProviderName") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ProviderNameFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Provider Type">
                    <ItemTemplate>
                        <asp:Label  Text='<%# Eval("Description") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_ProviderType"  runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_ProviderTypeFooter" runat="server"></asp:DropDownList>
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

                  <asp:TemplateField HeaderText="Emirate">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Emirate") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Emirate" Text='<%# Eval("Emirate") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_EmirateFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Source">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Source") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Source" Text='<%# Eval("Source") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_SourceFooter" runat="server"></asp:TextBox>
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
