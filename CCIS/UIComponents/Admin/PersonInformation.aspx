<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PersonInformation.aspx.cs" Inherits="CCIS.UIComponenets.Admin.PersonInformation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Person Information</h3>


        <asp:GridView ID="GV_PersonInformation" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="PersonInformationID"
            
            OnRowCommand="GV_PersonInformation_RowCommand"
            OnRowEditing="GV_PersonInformation_RowEditing"
            OnRowCancelingEdit="GV_PersonInformation_RowCancelingEdit"
            OnRowUpdating="GV_PersonInformation_RowUpdating"
            OnRowDeleting="GV_PersonInformation_RowDeleting"
            OnRowDataBound="GV_PersonInformation_RowDataBound"
            OnPageIndexChanging="GV_PersonInformation_PageIndexChanging"
            
            >
            <Columns>
                <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("PersonInformationID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txt_PersonInformationID" Text='<%# Eval("PersonInformationID") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                    
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Full Name">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("FullName") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_FullName" Text='<%# Eval("FullName") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_FullNameFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Contact Number">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ContactNumber") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_ContactNumber" Text='<%# Eval("ContactNumber") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ContactNumberFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Gender">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Gender") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox runat="server" Visible="false" ID="txt_Gender" Text='<%# Eval("Gender") %>'></asp:TextBox>
                        <asp:DropDownList ID="ddl_Gender" runat="server">
                            <asp:ListItem Value="1">Male</asp:ListItem>
                            <asp:ListItem Value="0">Female</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_GenderFooter" runat="server">
                            <asp:ListItem Value="1">Male</asp:ListItem>
                            <asp:ListItem Value="0">Female</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Nationality">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Nationality") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_Nationality"  runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_NationalityFooter"  runat="server"></asp:DropDownList>
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

                <asp:TemplateField HeaderText="ResidentialLocation">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ResidentialLocation") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_ResidentialLocation" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_ResidentialLocationFooter" runat="server"></asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>

              

                <asp:TemplateField HeaderText="Work Location">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("WorkLocation") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddl_WorkLocation" runat="server"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddl_WorkLocationFooter" runat="server"></asp:DropDownList>
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
