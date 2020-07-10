<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketsReport.aspx.cs" Inherits="CCIS.UIComponents.Notification.TicketsReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script>
        $(function () {
            $("[id$=txt_FromDate]").datepicker({
                //showOn: 'button',
                buttonImageOnly: true,
                changeMonth: true,
                dateFormat: "yy-mm-dd 00:00",
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            });
        });

         $(function () {
            $("[id$=txt_ToDate]").datepicker({
                //showOn: 'button',
                buttonImageOnly: true,
                changeMonth: true,
                dateFormat: "yy-mm-dd 00:00",
                buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            });
        });

    </script>


    <div class="jumbotron" style="overflow: auto">
        <h3>Generate Ticket Report</h3>

        <asp:Table runat="server" CssClass="table" Width="100%">
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="From Date (YYYY-MM-DD)" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_FromDate" CssClass="control-label"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="To Date (YYYY-MM-DD)" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_ToDate" CssClass="control-label"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

                 <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Application" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Application" CssClass="control-label"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

             <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Ticket Type" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_TicketType" CssClass="control-label">
                    </asp:DropDownList>
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
