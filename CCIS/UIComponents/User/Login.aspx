<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CCIS.UIComponents.User.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
  <%--  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>--%>

    
    <div class="container">
          <div class="glyphicon-align-center">
        <p align="middle"><img    width ="300px" height ="200px" src="../../Content/IQVIA_Service_Desk_Login_v1.0.png" alt="IQ Login">
           </p>            
            </div>
           
        <h2>User Login</h2>
        <div class="form-group">
            <label for="email">Email:</label>
            <input runat="server" type="text" class="form-control" id="UserName" placeholder="Enter UserName" name="UserName">
        </div>
        <div class="form-group">
            <label for="pwd">Password:</label>
            <input runat="server" type="password" class="form-control" id="Password" placeholder="Enter password" name="Password" >
        </div>

        <div>
            <%--<button type="submit" runat="server" OnServerClick="Button_Submit()" name="Submit" title="Submit" class="btn btn-default">Submit</button>--%>
            <asp:Button runat="server" ID="submit" Text="Submit" CssClass="btn btn-default" OnClick="submit_Click" />
        </div>

        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>
    </div>


</asp:Content>
