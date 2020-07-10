<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CreateTicket.aspx.cs" Inherits="CCIS.UIComponenets.Tickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/js/select2.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/css/select2.min.css" />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= ddl_Status.ClientID %>").select2({
                placeholder: "Select Status",
                allowClear: false
            });
            $("#<%= ddl_SubStatus.ClientID %>").select2({
                placeholder: "Select Sub-Status",
                allowClear: false
            });
            $("#<%= ddl_tickettype.ClientID %>").select2({
                placeholder: "Select Ticket Type",
                allowClear: false
            });
            $("#<%= ddl_Severity.ClientID %>").select2({
                placeholder: "Select Severity",
                allowClear: false
            });
            $("#<%= ddl_priority.ClientID %>").select2({
                placeholder: "Select Priority",
                allowClear: false
            });
            $("#<%= ddl_Assignee.ClientID %>").select2({
                placeholder: "Select Assignee",
                allowClear: false
            });
            $("#<%= ddl_LinkRelation.ClientID %>").select2({
                placeholder: "Select Ticket Relation",
                allowClear: false
            });
            $("#<%= ddl_LinkTicket.ClientID %>").select2({
                placeholder: "Select Ticket",
                allowClear: false
            });
            $("#<%= ddl_Summary.ClientID %>").select2({
                placeholder: "Enter Summary/Subject",
                allowClear: true,
                tags: true,
                selectOnClose: true
            });

            //ddl_tickettype
            var select2 = $("#<%= ddl_Summary.ClientID %>");
            select2.on('select2:select', onSelect);

            var select3 = $("#<%= ddl_tickettype.ClientID %>");
            select3.on('select2:select', onSummary);

        });

        function onSelect(evt) {
            var data = $(this).val();
            console.log("actual: " + data);
            document.getElementById('<%= txt_hidden_summary.ClientID %>').value = data;
            console.log("hidden: " + document.getElementById("txt_hidden_summary").value);
        }
    </script>

    <script>
        function onSummary(evt) {
            var data = $(this).val();
            console.log("Summary actual: " + data);
            if (data == 41) {
                $("#Modal_Problem").modal('show');
            }
        }
    </script>

    <script>

    $(document).ready(function(){

      $('[data-toggle="tooltip"]').tooltip();  

    });

</script>

    <%--<script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        $(function () {
            $("[id$=txt_DueDate]").datepicker({
                //showOn: 'button',
                buttonImageOnly: true,
                changeMonth: true,
                dateFormat: "d/m/yy",
                //buttonImage: 'http://jqueryui.com/demos/datepicker/images/calendar.gif'
            }).noConflict();
        });
    </script>--%>








    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Modal Methods</h4>
                </div>
                <div class="modal-body">
                    <p>The <strong>show</strong> method shows the modal and the <strong>hide</strong> method hides the modal.</p>
                </div>
            </div>

        </div>
    </div>



    <div id="Modal_Problem" class="modal" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Problem Justification</h4>
                </div>
                <div class="modal-body">
                    <asp:Table runat="server" CssClass="table">
                        <asp:TableRow>
                            <asp:TableCell><asp:Label runat="server" Text="Description"></asp:Label></asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txt_Justification"></asp:TextBox>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btn_modal_problem" Text="Submit" CssClass="btn btn-primary" OnClick="btn_modal_problem_Click" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



    <div class="jumbotron" style="overflow: auto">
        <h3>Create Ticket</h3>
        <asp:Table runat="server" CssClass="table" Width="100%">
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Application" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label runat="server" ID="lbl_Project" Text="EClaimLink" CssClass="control-label"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Summary *" CssClass="control-label" title="Tooltip Text" data-toggle="tooltip" ></asp:Label>
                </asp:TableCell>

                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Summary" AppendDataBoundItems="true" CssClass="form-control">
                        <asp:ListItem Text=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="txt_hidden_summary" ClientIDMode="Static" AutoPostBack="true" Style="display: none;"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>


            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" Text="Status" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Status" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" Text="Routing Status" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_SubStatus" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server"  Text="Issue Type" CssClass="control-label"></asp:Label>
                </asp:TableCell>

                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_tickettype" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server"  Text="Support Type" CssClass="control-label"></asp:Label>
                </asp:TableCell>

                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_SupportType" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server"  Text="Client Severity" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Severity" CssClass="form-control">
                    </asp:DropDownList>
                </asp:TableCell>
                <asp:TableCell><a href="PriorityGuidelines.html" target="_blank">Impact Guidelines</a></asp:TableCell>
            </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server"  Text="Internal Severity" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Internal_Severity" CssClass="form-control">
                    </asp:DropDownList>
                </asp:TableCell>
                <%--<asp:TableCell><a href="PriorityGuidelines.html" target="_blank">Impact Guidelines</a></asp:TableCell>--%>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Client Priority" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_priority" CssClass="form-control">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Internal Priority" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Internal_priority" CssClass="form-control">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Assignee" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Assignee" CssClass="form-control">
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Due Date" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_DueDate" CssClass="form-control"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>


            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server" Text="Link Relation" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_LinkRelation" AppendDataBoundItems="true" CssClass="form-control">
                        <asp:ListItem Text="Select a Relation" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>

                <asp:TableCell>
                    <asp:Label runat="server" Text="Link Ticket" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_LinkTicket" AppendDataBoundItems="true" CssClass="form-control">
                        <asp:ListItem Text="Select a Ticket" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Contigency Plan" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_ContigencyPlan" CssClass="form-control"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>


            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Description" CssClass="control-label" ></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <div class="form-group">
                        <textarea runat="server" id="txt_rich_description" class="form-control" rows="7" cssclass="form-control"></textarea>
                    </div>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="ActionTaken" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <textarea runat="server" class="form-control" id="txt_rich_actiontake" style="resize: both; height: 200px; width: 900px; margin: 0px;"></textarea>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Attachment" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:FileUpload runat="server" AllowMultiple="true" ID="file_Attachment"
                        accept=".png,.jpg,.jpeg,.xls,.xlsx,.xml,.txt,.docx,.doc" />
                    <p style="font-size: x-small">The maximum file upload size is 10.00 MB.</p>
                </asp:TableCell>
            </asp:TableRow>

        </asp:Table>
        <div>
            <asp:Button runat="server" CssClass="btn btn-default" ID="btn_submit" OnClick="btn_submit_Click" Text="Submit" />
        </div>
        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>
    </div>




</asp:Content>
