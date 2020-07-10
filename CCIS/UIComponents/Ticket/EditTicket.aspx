<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditTicket.aspx.cs" Inherits="CCIS.UIComponenets.EditTicket" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/js/select2.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/css/select2.min.css"  />

    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= ddl_Project_List.ClientID %>").select2({
                placeholder: "Select Project",
                allowClear: false
            });

            $("#<%= ddl_Summary.ClientID %>").select2({
                placeholder: "Enter Summary/Subject",
                allowClear: true,
                tags: true,
                selectOnClose: true
            });

             $("#<%= ddl_ActionTaken.ClientID %>").select2({
                placeholder: "Enter Action Taken",
                allowClear: true,
                tags: true,
                selectOnClose: true
            });

            $("#<%= ddl_Status.ClientID %>").select2({
                placeholder: "Select Status",
                allowClear: false
            });

            $("#<%= ddl_SubStatus.ClientID %>").select2({
                placeholder: "Select Sub-Status",
                allowClear: false
            });

            $("#<%= ddl_TicketType.ClientID %>").select2({
                placeholder: "Select Ticket Type",
                allowClear: false
            });

            $("#<%= ddl_Client_Severity.ClientID %>").select2({
                placeholder: "Select Client Severity",
                allowClear: false
            });

            $("#<%= ddl_Severity.ClientID %>").select2({
                placeholder: "Select Severity",
                allowClear: false
            });

            $("#<%= ddl_Client_priority.ClientID %>").select2({
                placeholder: "Select Client Priority",
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

            var select2 = $("#<%= ddl_Summary.ClientID %>");
        select2.on('select2:select', onSummary);

        var select3 = $("#<%= ddl_ActionTaken.ClientID %>");
        select3.on('select2:select', onAction);

        var select4 = $("#<%= ddl_TicketType.ClientID %>");
        select4.on('select2:select', onTicketType);
           
        })

        

        function onSummary(evt) {
            var data = $(this).val();
            console.log("Summary: " + data);
            document.getElementById('<%= txt_hidden_summary.ClientID %>').value = data;
            console.log("hidden: " + document.getElementById("txt_hidden_summary").value);
        }

        function onAction(evt) {
            var data = $(this).val();
            console.log("ActionTaken: " + data);
            document.getElementById('<%= txt_hdn_ActionTaken.ClientID %>').value = data;
            console.log("hidden: " + document.getElementById("txt_hdn_ActionTaken").value);
        }

        function onTicketType(evt) {
            var data = $(this).val();
            console.log("TicketType: " + data);
            if (data == 41) {
                $("#Modal_Problem").modal('show');
            }
        }

        
    </script>


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
                    <button type="button" class="btn btn-default" data-dismiss="modal">Submit</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>




    <div class="jumbotron">
        <h3>Edit Incident</h3>

        <asp:Table runat="server" CssClass="table" Width="100%">

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Application" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Project_List" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Jira Number" CssClass="control-label" ></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_JiraNumbers" CssClass="form-control"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Summary" CssClass="control-label"></asp:Label>
                </asp:TableCell>

                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Summary" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:TextBox runat="server" ID="txt_hidden_summary" ClientIDMode="Static" AutoPostBack="true" Style="display: none;"></asp:TextBox>
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
                <asp:Label runat="server" Text="Contigency Plan" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox runat="server" ID="txt_ContigencyPlan" CssClass="form-control"></asp:TextBox>
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
                <asp:Label runat="server" Text="Issue Type" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_TicketType" CssClass="form-control"></asp:DropDownList>
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
                <asp:Label runat="server" Text="Client Severity" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Client_Severity" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Internal Severity" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Severity" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Client Priority" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_Client_priority" CssClass="form-control"></asp:DropDownList>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Internal Priority" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList runat="server" ID="ddl_priority" CssClass="form-control">
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
                <asp:Label runat="server" Text="Description" CssClass="control-label"></asp:Label>
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
                    <asp:DropDownList runat="server" ID="ddl_ActionTaken" CssClass="form-control"></asp:DropDownList>
                    <asp:TextBox runat="server" ID="txt_hdn_ActionTaken" ClientIDMode="Static" AutoPostBack="true" Style="display: none;"></asp:TextBox>

                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow>
                <asp:TableCell>
                <asp:Label runat="server" Text="Attachment" CssClass="control-label"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:FileUpload runat="server" AllowMultiple="true" ID="file_Attachment" />
                    <p style="font-size: x-small">The maximum file upload size is 10.00 MB.</p>
                    <asp:PlaceHolder runat="server" ID="Place_Attachment"></asp:PlaceHolder>
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
