<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Application.aspx.cs" Inherits="CCIS.UIComponenets.Admin.Application" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">
    <h3>Application Informaiton</h3>

        <asp:GridView ID="GV_Application" runat="server"
            CssClass="table table-striped table-bordered table-condensed pagination-ys"
            AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true"
            ShowFooter="true"
            DataKeyNames="ApplicationsID"
            OnRowCommand="GV_Application_RowCommand"
            OnRowUpdating="GV_Application_RowUpdating"
            OnRowDeleting="GV_Application_RowDeleting"
            OnRowEditing="GV_Application_RowEditing"
            OnRowDataBound="GV_Application_RowDataBound"
            OnRowCancelingEdit="GV_Application_RowCancelingEdit"
            OnPageIndexChanging="GV_Application_PageIndexChanging">
            <Columns>

               <asp:TemplateField Visible="false" HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ApplicationsID") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="lbl_AplicationID" Text='<%# Eval("ApplicationsID") %>' Visible="false" runat="server"></asp:Label>
                    </EditItemTemplate>
                   
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Name") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Name" Text='<%# Eval("Name") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_NameFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                 <%--<asp:TemplateField HeaderText="Owner">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Owner") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Owner" Text='<%# Eval("Owner") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_OwnerFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>--%>

               <%--  <asp:TemplateField HeaderText="Owner Email">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Owner_Email") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Owner_Email" Text='<%# Eval("Owner_Email") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_Owner_EmailFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>--%>

                 <asp:TemplateField HeaderText="Contact Number">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Contact_Number") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Contact_Number" Text='<%# Eval("Contact_Number") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_Contact_NumberFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                 <asp:TemplateField HeaderText="Contact Person">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Contact_Person") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_Contact_Person" Text='<%# Eval("Contact_Person") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_Contact_PersonFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="URL">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("URL") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_URL" Text='<%# Eval("URL") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_URLFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <%-- <asp:TemplateField HeaderText="Group Email">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("GroupEmailDesc") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:DropDownList runat="server" ID="ddl_GroupEmail"></asp:DropDownList>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList runat="server" ID="ddl_GroupEmailFooter"></asp:DropDownList>
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

        <asp:Label runat="server" ID="lbl_message"></asp:Label>


    </div>


</asp:Content>
