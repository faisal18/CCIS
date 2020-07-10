<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewTicket.aspx.cs" Inherits="CCIS.UIComponenets.ViewTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<h2>View Ticket</h2>--%>



    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/js/select2.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#<%= ddl_JiraAssignee.ClientID %>").select2({
                placeholder: "Select Assignee",
                allowClear: false
            });

            $("#<%= ddl_assignee.ClientID %>").select2({
                placeholder: "Select Assignee",
                allowClear: false
            });

            $("#<%= ddl_JiraUsers.ClientID %>").select2({
                placeholder: "Select Assignees",
                allowClear: true,
                tags: false,
                selectOnClose: false
            });

             $("#<%= ddl_JiraApplication.ClientID %>").select2({
                placeholder: "Select Application",
                allowClear: false
            });

               $("#<%= ddl_JiraPriority.ClientID %>").select2({
                placeholder: "Select Priority",
                allowClear: false
            });

               $("#<%= ddl_JiraTicketStatus.ClientID %>").select2({
                placeholder: "Select Status",
                allowClear: false
            });

               $("#<%= ddl_JiraTicketType.ClientID %>").select2({
                placeholder: "Select Type",
                allowClear: false
            });

        });

    </script>



    <script type="text/javascript">
        function showCloseTicketModal() {
            $("#CloseTicketModal").modal('show');
        }

        function showAwaitingModal() {
            $("#AwaitInfoModal").modal('show');
        }
    </script>
    <div class="jumbotron">

        <div>
            <h3 runat="server" id="h3_Summary"></h3>
        </div>

        <div class="btn-group">
            <asp:Button runat="server" ID="btn_EditTicket" CssClass="btn btn-primary" Text="Edit" OnClick="btn_EditTicket_Click" />
        </div>

        <div class="btn-group">
            <button runat="server" type="button" class="btn btn-primary" id="btn_assign" data-toggle="modal" data-target="#myModal">Assign To</button>
        </div>
        <div class="btn-group">
            <button runat="server" type="button" class="btn btn-primary" id="btn_AddComments" data-toggle="modal" data-target="#CommentTemplate">Add Comment</button>
        </div>

        <%--MODAL ASSIGN TO--%>
        <div id="myModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Select Assignee</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Table runat="server" CssClass="table table-bordered" ID="tbl_Assignee">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Assignee"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList CssClass="dropdown" ID="ddl_assignee" runat="server"></asp:DropDownList>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button runat="server" ID="btn_AssigneeSelect" Text="Update Internal" CssClass="btn btn-primary" OnClick="btn_AssigneeSelect_Click" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="JiraAssigneeRow">
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Jira Assignee"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList CssClass="dropdown" AppendDataBoundItems="true" ID="ddl_JiraAssignee" runat="server">
                                        <asp:ListItem Value="">Select an Item</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button runat="server" ID="btn_JiraAssignee" Enabled="false" Text="Update JIRA" CssClass="btn btn-primary" OnClick="btn_JiraAssignee_Click" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <%--<asp:Button runat="server" ID="btn_AssigneeSelect" Text="Update Internal" CssClass="btn btn-primary" OnClick="btn_AssigneeSelect_Click" />--%>
                        <%--<asp:Button runat="server" ID="btn_JiraAssignee" Enabled="false" Text="Update JIRA" CssClass="btn btn-primary" OnClick="btn_JiraAssignee_Click" />--%>

                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- Close Ticket -->
        <div runat="server" id="div_buttonClose" class="btn-group">
            <button type="button" class="btn btn-primary" runat="server" id="btn_closeTicketMain" data-toggle="modal" data-target="#CloseTicketModal">Resolve Ticket with KB</button>
        </div>
        <%--MODAL Close Ticket--%>
        <div id="CloseTicketModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Knowledge Base</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">

                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Version"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_Version" AppendDataBoundItems="true">
                                        <asp:ListItem Text="">Select a Value</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Module"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_Module" AppendDataBoundItems="true">
                                        <asp:ListItem Text="">Select a Value</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Component"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_Component" AppendDataBoundItems="true">
                                        <asp:ListItem Text="">Select a Value</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Resolution Category"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_Category" AppendDataBoundItems="true">
                                        <asp:ListItem Text="">Select a Value</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Steps Taken"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_StepsTaken" TextMode="MultiLine"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                            <asp:Label runat="server" Text="Root Cause"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_RootCause" TextMode="MultiLine"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>

                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_CloseTicket" Text="Submit" CssClass="btn btn-primary" OnClick="btn_CloseTicket_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!--Awaiting Customer Information-->
        <div id="AwaitInfoModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Awaiting Customer Information</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">

                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Required Information"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_AwaitInfor"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>

                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_AwaitInfor" Text="Submit" CssClass="btn btn-primary" OnClick="btn_AwaitInfor_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- CommenTemplate -->
        <div id="CommentTemplate" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Comment Template</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">

                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Scenario"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_scenario">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_CommentTemplate1" Text="Next" CssClass="btn btn-primary" OnClick="btn_CommentTemplate1_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- CommenTemplate 2-->
        <div id="CommentTemplate2" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Comment Template</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">
                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Comment"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <textarea class="form-control" runat="server" id="txt_Comments" style="resize: both; height: 300px; width: 400px; margin: 0px;"></textarea>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_CommentTemplate" Text="Submit" CssClass="btn btn-primary" OnClick="btn_AddCommetns_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- Statuses -->
        <div class="btn-group">
            <div class="dropdown">
                <button runat="server" class="btn btn-primary dropdown-toggle" type="button" id="btn_ChangeStatus" data-toggle="dropdown">
                    Change Status
                    <span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu" aria-labelledby="menu1">
                    <asp:PlaceHolder runat="server" ID="PH_Statuses"></asp:PlaceHolder>
                </ul>
            </div>
        </div>

        <!-- Reports -->
        <div class="btn-group">
            <div class="dropdown">
                <button runat="server" class="btn btn-primary dropdown-toggle" type="button" id="btn_Report" data-toggle="dropdown">
                    Generate Report
                    <span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu" aria-labelledby="menu1">
                    <li>
                        <asp:LinkButton runat="server" ID="link_pdf" Text="Incident Report" OnClick="link_pdf_Click"></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton runat="server" Visible="false" ID="link_word_report" Text="WORD Format" OnClick="link_word_report_Click"></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton runat="server" ID="link_Business_Form" Text="Business Incident Report" OnClick="link_Business_Form_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>

        <!-- Business Form -->
        <div id="Report_BusinessForm" class="modal" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Business Form</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Issue Description"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txt_issueDesc"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Resolution ActionTaken"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txt_RActionTaken"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Resolution Summary"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" TextMode="MultiLine" ID="txt_RSummary"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_BusinessForm" Text="Submit" CssClass="btn btn-primary" OnClick="btn_BusinessForm_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Create Jira -->
        <div runat="server" id="div_CreateJira" class="btn-group">
            <button type="button" class="btn btn-primary" runat="server" id="btn_CreateJira" data-toggle="modal" data-target="#CreateJiraModal">Create Jira</button>
        </div>
        <!-- MODAL Create Jira Part1-->
        <div id="CreateJiraModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Select Jira Application</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">
                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Application"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_JiraApplication" Width="200px" ></asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_CreateJiraPage2" Text="Next" CssClass="btn btn-primary" OnClick="btn_CreateJiraPage2_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- MODAL Create Jira Part2-->
        <div id="CreateJiraModal2" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Create Ticket</h4>
                    </div>
                    <div class="modal-body" style="overflow: auto;">
                        <asp:Table runat="server" CssClass="table">
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Ticket Type"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_JiraTicketType" ></asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Ticket Status"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_JiraTicketStatus"></asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Ticket Priority"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_JiraPriority" ></asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell><asp:Label runat="server" Text="Assign To"></asp:Label></asp:TableCell>
                                <asp:TableCell>
                                    <asp:DropDownList runat="server" ID="ddl_JiraUsers" ></asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>

                        </asp:Table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btn_CreateJiraSubmit" Text="Submit" CssClass="btn btn-primary" OnClick="btn_CreateJiraSubmit_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>

        <div class="btn-group">
            <asp:Button runat="server" ID="btn_SendtoInfra" CssClass="btn btn-primary" Text="Send to Infrastructure" OnClick="btn_SendtoInfra_Click" />
        </div>


        <script type="text/javascript">
            function showModal() {
                $("#CreateJiraModal2").modal('show');
            }
        </script>

        <script>
            function showComment() {
                $("#CommentTemplate2").modal('show');
            }
        </script>

        <script type="text/javascript">
            function showBusinessModal() {
                $("#Report_BusinessForm").modal('show');
            }
        </script>

        <br />
        <br />

        <asp:Table runat="server" CssClass="table" Width="100%">

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Ticket Number"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_TicketNumber"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Jira Number"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:HyperLink runat="server" ID="link_JiraNumber"></asp:HyperLink>
                    <asp:Label runat="server" Visible="false" ID="lbl_JiraNumber"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Caller Name"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_callerKey"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Caller Facility"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_callerfacility"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Application Name"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lbl_Application" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Due Date"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_DueDate"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Contigency Plan"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_ContigencyPlan"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Client Severity"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_Severity"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Internal Severity"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_IntSeverity"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Client Priority"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_Priority"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Internal Priority"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_IntPriority"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Ticket Status"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_TicketStatus"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Ticket Type"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_TicketType"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

             <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Support Type"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_SupportType"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Routing Status"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_SubStatus"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Description"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="form-group">
                        <textarea disabled runat="server" id="txt_Description" style="overflow: auto; resize: none; height: 100px" class="form-control"></textarea>
                    </div>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Action Taken"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="txt_ActionTaken"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>



            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Attachments"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:PlaceHolder runat="server" ID="Place_Attachment"></asp:PlaceHolder>
                </asp:TableCell>
            </asp:TableRow>


            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Reported By"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_ReportedBy"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Reported To"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_ReportedTo"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>


        </asp:Table>


        <ul class="nav nav-tabs">
            <%-- <li>
                <a class="tablinks" data-toggle="tab" href="#Paris">All</a>
            </li>--%>
            <li>
                <a class="tablinks" data-toggle="tab" id="CommentsTab" href="#CommentsTest">Comments</a>
            </li>
            <li>
                <a class="tablinks" data-toggle="tab" href="#Activity" id="JqueryTest">Activity</a>
            </li>
            <li>
                <a class="tablinks" data-toggle="tab" id="ResolutionTab" href="#Resolution">Resolution</a>
            </li>

            <%--<li>
                <a class="tablinks" data-toggle="tab" href="#StaticTab">Activity</a>
            </li>--%>
        </ul>

        <div class="tab-content">
            <div id="CommentsTest" class="tab-pane fade in active">
                <asp:Panel runat="server" ID="CommentsContainer">
                </asp:Panel>
            </div>
            <div id="Resolution" class="tab-pane fade in">
                <asp:Panel runat="server" ID="Pnl_Resolution"></asp:Panel>
            </div>



            <div id="Activity" class="tab-pane fade in">
                <asp:Panel runat="server" ID="Pnl_ActivityComments">
                </asp:Panel>
            </div>

            <%--  <div id="StaticTab" class="tab-pane fade">
                <p>This is not yet Implemented</p>--%>
        </div>

    </div>


    <div>
        <asp:Label runat="server" ID="lbl_message"></asp:Label>
    </div>






</asp:Content>
