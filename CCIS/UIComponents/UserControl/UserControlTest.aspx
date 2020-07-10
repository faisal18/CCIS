<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserControlTest.aspx.cs" Inherits="CCIS.UIComponenets.UserControl.UserControlTest" %>

<%--<%@ Register Src="~/UIComponenets/UserControl/AllSearch.ascx" TagPrefix="uc1" TagName="AllSearch" %>--%>

<%@ Register Src="~/UIComponents/UserControl/jQuery_GriedCtrl.ascx" TagPrefix="uc1" TagName="jQuery_GriedCtrl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>User control test</h2>

    <div style="height: auto">
        <uc1:jQuery_GriedCtrl runat="server" ID="jQuery_GriedCtrl" />
    </div>
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
</asp:Content>
