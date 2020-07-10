<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllSearch.ascx.cs" Inherits="CCIS.UIComponenets.UserControl.AllSearch" %>


<script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>

<!-- <script src="https://code.jquery.com/jquery-1.12.4.js"></script> -->
<script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
<!-- <script src="https://code.jquery.com/jquery-2.1.1.js"></script> -->


<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
<!-- <script src="https://code.jquery.com/jquery-3.3.1.js"></script> -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>

<link href="http://automation.dimensions-healthcare.com/CCIS/Content/Jquery-UI.css" rel="stylesheet" type="text/css" />
<link href="http://automation.dimensions-healthcare.com/CCIS/Content/bootstrap-theme.css" rel="stylesheet" type="text/css" />



<script type="text/javascript">  
        $(document).ready(function () {
            // alert(document.getElementById('txt_SearchAll').value);
            SearchText();
        });
        function SearchText() {
            $("#txt_SearchAll").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",

                        contentType: "application/json; charset=utf-8",
                        url: "http://automation.dimensions-healthcare.com/CCIS/Webservice/WSautomation.asmx/GetTicketsCached",
                        // url: "../Default.aspx/GetPayerCodesCached",

                        // data: "{" + document.getElementById('txt_SearchAll').value + "'}",
                        data: "{'TicketSubject':'" + document.getElementById('txt_SearchAll').value + "'}",

                        dataType: "json",
                        success: function (data) {
                            //alert("match");
                            //alert(data.d);
                            response(data.d);
                        },
                        error: function (result) {
                        //    alert("{'Name':'" + document.getElementById('txt_SearchAll').value + "'}");
                            // alert("No Match");
                         //   alert(document.getElementById("txt_SearchAll").value);
                        }
                    });
                }
            });
        }
</script>

<div class="form-group">
    <asp:TextBox runat="server" ClientIDMode="Static" placeholder="Search Ticket" CssClass="form-control" AutoCompleteType="Disabled" Visible="true" ID="txt_SearchAll"></asp:TextBox>
</div>
