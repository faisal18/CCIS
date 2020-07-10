<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Jira_ViewApi.aspx.cs" Inherits="CCIS.UIComponents.Jira.Jira_ViewApi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    <div class="jumbotron">
        <h3>JIRA Rest API</h3>

        <asp:Table runat="server" CssClass="table">
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" Text="Method"></asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_method">
                        <asp:ListItem Value="GET">Get</asp:ListItem>
                        <asp:ListItem Value="POST">Post</asp:ListItem>
                        <asp:ListItem Value="PUT">Update</asp:ListItem>
                        <asp:ListItem Value="DELETE">Delete</asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" Text="URL"></asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" Text="http://jira.dimensions-healthcare.org:8080/" ID="txt_URL" Width="500px"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" Text="Resource" Width="200px"></asp:Label></asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_resource" Width="500px"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Jira Username"></asp:Label>

                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" Text="FAnsari" ID="txt_username"></asp:TextBox>

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Jira Password"></asp:Label>

                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" Text="ansari.COM" ID="txt_Password"></asp:TextBox>

                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" Text="Input"></asp:Label></asp:TableCell>
                <asp:TableCell>
                    <div style="overflow: auto">
                        <textarea runat="server" id="txt_richbox_postFile" style="resize: both; height: 200px; width: 500px; margin: 0px;"></textarea>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell><asp:Label runat="server" Text="Output"></asp:Label></asp:TableCell>
                <asp:TableCell>
                    <div style="overflow: auto">
                        <textarea runat="server" id="txt_richbox_output" style="resize: both; height: 200px; width: 500px; margin: 0px;"></textarea>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>


        <div>
            <asp:Button runat="server" ID="btn_Submit" Text="Submit" OnClick="btn_Submit_Click" />
        </div>
        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>

    </div>


</asp:Content>
