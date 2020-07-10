<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationDetails.aspx.cs" Inherits="CCIS.UIComponents.KPI.ApplicationDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 runat="server" id="h2_Title">Application Name</h2>

    <div class="jumbotron">
        <div style="overflow: auto;">
            <h3>Issues Due</h3>
            <asp:Repeater runat="server" ID="rptr_issuesDue">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">TicketNumber</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Description</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(3)')">Assigned To</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(4)')">Assigned Date</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>
                            <%--<asp:LinkButton runat="server" ID="lbl_TicketNumber_IssuesDue" Text='<%# Eval("TicketNumber") %>' OnCommand="lbl_TicketNumber_IssuesDue_Command" CommandName='<%# Eval("TicketInformationID") %>' CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>--%>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Description" Text='<%# Eval("Subject") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_AssignedTo" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_AssignedDate" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                        </td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div style="overflow: auto;">
            <h3>Issues Updated</h3>
            <asp:Repeater runat="server" ID="rptr_issuesUpdated">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">TicketNumber</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Description</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(3)')">Updated By</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(4)')">Updated Date</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Description" Text='<%# Eval("Subject") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_UpdatedBy" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_UpdatedDate" Text='<%# Eval("AssignedDate") %>'></asp:Label>
                        </td>

                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div style="overflow: auto">
            <h3>Highest Scoring Agent</h3>
            <asp:Repeater runat="server" ID="rptr_Agent">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">Name</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Count</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:LinkButton runat="server" ID="lbl_Agent" Text='<%# Eval("AgentName") %>' OnCommand="lbl_Agent_Command" CommandName='<%# Eval("AgentID") %>' CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Count" Text='<%# Eval("Count") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

        <div style="overflow: auto">
            <h3>Inquiries Count</h3>
            <asp:Repeater runat="server" ID="rptr_InqCount">
                <HeaderTemplate>
                    <table id="tbl_Session" class="table-bordered table table-hover">
                        <tr>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">Name</th>
                            <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Count</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="item">
                        <td>
                            <asp:LinkButton runat="server" ID="lbl_Agent" Text='<%# Eval("AgentName") %>' OnCommand="lbl_Agent_Command" CommandName='<%# Eval("AgentID") %>' CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lbl_Count" Text='<%# Eval("Count") %>'></asp:Label>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>

       

        <script src="https://code.jquery.com/jquery-3.1.1.min.js"></script>
        <script src="https://code.highcharts.com/highcharts.js"></script>
        <script src="https://code.highcharts.com/modules/exporting.js"></script>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>


        <div id="chart_div4" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
        <script id="Google Bar Graph Status">
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(DrawStatus);
            var arr_Status = null;
            var myVar4;
            var Status_CheckInTime = null;

            function DrawStatus() {
                var data = google.visualization.arrayToDataTable(GetDataStatus());
                var options = {
                    annotations: {
                        textStyle: {
                            color: 'black',
                            fontSize: 11,
                        },
                        alwaysOutside: true
                    },
                    title: 'Tickets By Status',
                    colors: ['#325C74', '#5995B7', '#A3C4D7'],
                    bar: { groupWidth: "95%" },
                    vAxis: { title: 'Count' },
                    hAxis: { gridlines: { color: 'none' } },
                    seriesType: 'bars',
                    series: { 1: { type: 'line' } }
                };
                var chart = new google.visualization.ComboChart(document.getElementById('chart_div4'));
                chart.draw(data, options);

                myVar4 = setTimeout(DrawStatus, 300000);
            }
            function GetDataStatus() {
                var array_Status = [];
                var license_txt = window.location.search.substring(1).split('=')[1];
                $.ajax({
                    type: "Post",
                    contentType: "application/json;charset=utf-8",
                    async: false,

                    url: "ApplicationDetails.aspx/GetStatus",
                    data: "{'ApplicationID':'" + license_txt + "'}",

                    success: function (data) {
                        arr_Status = data.d;
                        console.log(data.d);
                        for (i = 0; i < arr_Status.length; i++) {

                            Status_CheckInTime = arr_Status[i].CheckingTime;

                            array_Status.push(['Text', 'New', { type: 'number', role: 'annotation' }, 'InProgress', { type: 'number', role: 'annotation' }, 'Resolved', { type: 'number', role: 'annotation' }, 'ReOpened', { type: 'number', role: 'annotation' }, 'Close', { type: 'number', role: 'annotation' }])
                            array_Status.push(['Status', arr_Status[i].New, arr_Status[i].New, arr_Status[i].InProgress, arr_Status[i].InProgress, arr_Status[i].Resolved, arr_Status[i].Resolved, arr_Status[i].ReOpened, arr_Status[i].ReOpened, arr_Status[i].Close, arr_Status[i].Close]);
                        }
                    },
                    error: function (error) {
                        console.log("GetStatus ERROR !\n" + error);
                    }
                });

                return array_Status;
            }
            function stopFunction() {
                console.log("StopFunction:\n");
                clearTimeout(myVar4);
            }
        </script>

        <div id="chart_div3" style="min-width: 310px; height: 400px; margin: 0 auto"></div>
        <script id="Google Bar Graph Priority">

            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(DrawPriority);
            var arr_Priority = null;
            var myVar3;
            var Priority_CheckInTime = null;

            function DrawPriority() {
                var data = google.visualization.arrayToDataTable(GetDataPriority());
                var options = {
                    annotations: {
                        textStyle: {
                            color: 'black',
                            fontSize: 11,
                        },
                        alwaysOutside: true
                    },
                    title: 'Tickets By Priority',
                    colors: ['#325C74', '#5995B7', '#A3C4D7'],
                    bar: { groupWidth: "95%" },
                    vAxis: { title: 'Count' },
                    hAxis: { gridlines: { color: 'none' } },
                    seriesType: 'bars',
                    series: { 5: { type: 'line' } }
                };
                var chart = new google.visualization.ComboChart(document.getElementById('chart_div3'));
                chart.draw(data, options);

                myVar3 = setTimeout(DrawPriority, 300000);
            }
            function GetDataPriority() {
                var array_Priority = [];
                var license_txt = window.location.search.substring(1).split('=')[1];

                $.ajax({
                    type: "Post",
                    contentType: "application/json;charset=utf-8",
                    async: false,

                    url: "ApplicationDetails.aspx/GetPriority",
                    data: "{'ApplicationID':'" + license_txt + "'}",

                    success: function (data) {
                        arr_Priority = data.d;
                        for (i = 0; i < arr_Priority.length; i++) {

                            Priority_CheckInTime = arr_Priority[i].CheckingTime;

                            array_Priority.push(['Text', 'SuperCrtical', { type: 'number', role: 'annotation' }, 'Critical', { type: 'number', role: 'annotation' }, 'High', { type: 'number', role: 'annotation' }, 'Medium', { type: 'number', role: 'annotation' }, 'Low', { type: 'number', role: 'annotation' }])
                            array_Priority.push(['Priority', arr_Priority[i].SuperUrgent, arr_Priority[i].SuperUrgent, arr_Priority[i].Urgent, arr_Priority[i].Urgent, arr_Priority[i].High, arr_Priority[i].High, arr_Priority[i].Medium, arr_Priority[i].Medium, arr_Priority[i].Low, arr_Priority[i].Low]);
                        }
                    },
                    error: function (error) {
                        console.log("GetPriority ERROR !\n" + error);
                    }
                });

                return array_Priority;
            }
            function stopFunction() {
                console.log("StopFunction:\n");
                clearTimeout(myVar3);
            }

        </script>


        

        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>
    </div>
</asp:Content>
