<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminControl.aspx.cs" Inherits="CCIS.UIComponenets.Admin.AdminControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron">
    <h3>Admin Control Utility</h3>

        <div class="list-group">

            <a href="Payers.aspx" class="list-group-item">Payers</a>
            <a href="PayerType.aspx" class="list-group-item">Payer Type</a>

            <a href="Providers.aspx" class="list-group-item">Provider</a>
            <a href="ProviderType.aspx" class="list-group-item">Prvoider Type</a>

            <a href="Nationality.aspx" class="list-group-item">Nationality</a>
            <a href="AddressLocation.aspx" class="list-group-item">Address location</a>
            <a href="PersonInformation.aspx" class="list-group-item">Person Information</a>

            <a href="Application.aspx"class="list-group-item">Application Information</a>
            <a href="ApplicationProperty.aspx" class="list-group-item">Application Property</a>
            <a href="UserApplication.aspx"class="list-group-item">User Application Configuration</a>

            <a href="SystemAdminUser.aspx"class="list-group-item">System Admin User</a>


            <a href="UserGroups.aspx"class="list-group-item disabled">User Groups</a>
            <a href="Groups.aspx"class="list-group-item">Groups Information</a>

            <a href="UserRoles.aspx"class="list-group-item disabled">User Roles</a>
            <a href="Roles.aspx"class="list-group-item">Roles Information</a>

            <a href="ItemType.aspx"class="list-group-item disabled">Item Type Information</a>
            <a href="Statuses.aspx"class="list-group-item disabled">Status Information</a>

           
            
            <a href="SLADeclaration.aspx"class="list-group-item">SLA Declaration</a>
            <a href="Email.aspx"class="list-group-item">Email Templates</a>





            

        </div>
    </div>
</asp:Content>
