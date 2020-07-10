<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotificationService.aspx.cs" Inherits="CCIS.UIComponents.Notification.NotificationService" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h2>Notification Updater</h2>
    </div>
    <div class="jumbotron">
        <asp:Label runat="server"  ID="lbl_message"></asp:Label>
    </div>
</asp:Content>
