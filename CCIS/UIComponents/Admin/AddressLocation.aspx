<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddressLocation.aspx.cs" Inherits="CCIS.UIComponenets.Admin.AddressLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Address Location</h2>

    <div class="jumbotron" style="overflow-x: scroll; padding: 10px 10px 10px 10px">

        <asp:GridView ID="GV_AddressLocation" CssClass="table table-striped table-bordered table-condensed pagination-ys" runat="server" CellPadding="10" CellSpacing="5"
           AllowPaging="true" PageSize="10"
            AutoGenerateColumns="false" 
            ShowHeaderWhenEmpty="true" 
            showfooter="true"
            DataKeyNames="AddressLocationID"

            OnRowCommand="GV_AddressLocation_RowCommand"
            OnRowEditing="GV_AddressLocation_RowEditing"
            OnRowCancelingEdit="GV_AddressLocation_RowCancelingEdit"
            OnRowUpdating="GV_AddressLocation_RowUpdating"
            OnRowDeleting="GV_AddressLocation_RowDeleting">

            <Columns>
                <asp:TemplateField Visible="false" HeaderText="AddressLocation Id">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("AddressLocationId") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:Label ID="txt_AddressLocationId" Text='<%# Eval("AddressLocationId") %>' runat="server"></asp:Label>
                    </EditItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Address Description">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("Description") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_AddressDesc" Text='<%# Eval("Description") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_AddressDescFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Address Description Arabic">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("DescriptionAR") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_AddressARDesc" Text='<%# Eval("DescriptionAR") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_AddressARDescFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

            <%--    <asp:TemplateField HeaderText="Residence Location">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("ResidenceLocation") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_ResidenceLocation" Text='<%# Eval("ResidenceLocation") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_ResidenceLocationFooter" runat="server"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>

                  <asp:TemplateField HeaderText="Work Location">
                    <ItemTemplate>
                        <asp:Label Text='<%# Eval("WorkLocation") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txt_WorkLocation" Text='<%# Eval("WorkLocation") %>' runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt_WorkLocationFooter" runat="server"></asp:TextBox>
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
