<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CallerInformation.aspx.cs" Inherits="CCIS.UIComponenets.CallerInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/jquery-latest.min.js" type="text/javascript"></script>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />--%>


    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">


  <%--  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.0/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>--%>







    <script type="text/javascript">  
        $(document).ready(function () {
            SearchLic();
        });
        function SearchLic() {
            console.log("Search Lic called");
            $("#txt_CallerLicense").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",

                        contentType: "application/json; charset=utf-8",
                        url: "../Webservice/WSautomation.asmx/GetAllLicenses",
                        //url: "../Webservice/WSautomation.asmx/GetPayerCodesCached",
                        //url: "../Default.aspx/GetPayerCodesCached",

                        // data: "{" + document.getElementById('txt_CallerLicense').value + "'}",
                        data: "{'LicenseCode':'" + document.getElementById('txt_CallerLicense').value + "'}",

                        dataType: "json",
                        success: function (data) {
                            //alert("match");
                            //alert(data.d);
                            console.log(data.d);
                            response(data.d);

                        },
                        error: function (result) {
                            console.log("Error occured");
                            //   alert("{'Name':'" + document.getElementById('txt_CallerLicense').value + "'}");
                            //   alert("No Match");
                            //   alert(result);
                            //   alert(document.getElementById("txt_CallerLicense").value);
                        }
                    });
                }
            });
        }



    </script>

    <%--Created by Fazeel, now this div is hidden--%>
    <script type="text/javascript">
        function GetCompanies() {
            alert(document.getElementById('txt_CallerLicense').innerText);
            $("#UpdatePanel").html("<div style='text-align:center; background-color:gray; border:1px solid red; padding:3px; width:200px'>Please Wait...</div>");
            $.ajax({
                type: "POST",
                //url: "../Webservice/WSautomation.asmx/GetCallerListbyLicenseID",
                url: "../Default.aspx/GetCallerListbyLicenseID",

                data: "{'PayerCodes':'" + document.getElementById('txt_CallerLicense').value + "'}",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: OnSuccess,
                error: OnError
            });
        }
        function OnSuccess(data) {
            // alert("yes");
            if (data != null) {
                var TableContent = "<table border='1'>" +
                    "<tr>" +
                    "<td>CallerKeyID</td>" +
                    "<td>Name</td>" +
                    "<td>License</td>" +
                    "<td>Email</td>" +
                    "<td>Phone</td>" +
                    "</tr>";
                for (var i = 0; i < data.d.length; i++) {
                    TableContent += "<tr>" +
                        "<td>" + data.d[i].CallerKeyID + "</td>" +
                        "<td>" + data.d[i].Name + "</td>" +
                        "<td>" + data.d[i].CallerLicense + "</td>" +
                        "<td>" + data.d[i].Email + "</td>" +
                        "<td>" + data.d[i].PhoneNumber + "</td>" +
                        "</tr>";
                }
                TableContent += "</table>";

                $("#UpdatePanel").html(TableContent);
            }
            else {
                //   $("#UpdatePanel").html("No Records Found");
            }
        }
        function OnError(data) {
            //alert("no");
        }
    </script>


    <%--Created By Faisal for license population in gridview--%>
    <script type="text/javascript">

        function fromServer() {
            var license_txt = document.getElementById('txt_CallerLicense').value;
            console.log("FromServer called. Sending data " + license_txt);

            if (license_txt.length > 1) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json;charset=utf-8",
                    url: "CallerInformation.aspx/Transformlicense",
                    data: "{'license':'" + license_txt + "'}",
                    dataType: "json",
                    success: function (data) { console.log("received data:" + data.d); setHiddenField(data.d); SetHiddenNameField() },
                    error: function (result) {
                        console.log(result);
                    }
                });
            }
        }

        function setHiddenField(license) {
            console.log("SetHiddenField called.The license is : " + license);
            document.getElementById('<%= txt_hiddenfield.ClientID %>').value = license;
            console.log(document.getElementById("txt_hiddenfield").value);
         //   __doPostBack('txt_hiddenfield')
        }

        function SetHiddenNameField() {
            console.log("SetHiddenNameField called.");
        }

</script>

    <script src="https://www.w3schools.com/lib/w3.js"></script>

    <div class="container-fluid">
        <div id="CallerGrid" class="row">

            <div class="col-sm-9 col-md-6 col-lg-8">
                <div class="jumbotron">
                    <h3>Caller Information</h3>

                    <div id="CallerInformation">
                        <asp:Table runat="server" CssClass="table" Width="100%">

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="License" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" CssClass="form-control col-sm-4" ClientIDMode="Static" AutoCompleteType="Enabled" Visible="true" ID="txt_CallerLicense" Text="CWL001"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txt_hiddenfield" ClientIDMode="Static" AutoPostBack="true"  Style="display: none;"></asp:TextBox>
                                    <%--<asp:TextBox runat="server" ID="txt_hiddenfield" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="txt_hiddenfield_TextChanged" Style="display: none;"></asp:TextBox>--%>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Name *" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control col-sm-4" AutoCompleteType="Enabled" AutoPostBack="false" ID="txt_Name"></asp:TextBox>
                                    <%--<asp:TextBox runat="server" ClientIDMode="Static" CssClass="form-control col-sm-4" AutoCompleteType="Enabled" OnTextChanged="txt_Name_TextChanged" AutoPostBack="true" ID="txt_Name"></asp:TextBox>--%>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Contact Number *" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_PhoneNumber" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Email Address" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_Email" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button runat="server" ID="btn_Search" Text ="Search" OnClick="btn_Search_Click" CssClass="form-control col-sm-4"></asp:Button>
                                </asp:TableCell>

                            </asp:TableRow>

                             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Department" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_Department" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow> 

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Location" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_Location" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                           
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="MachineName" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_MachineName" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>

                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label runat="server" Text="Mobile/Laptop/OS" CssClass="control-label col-sm-8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox runat="server" ID="txt_OperatingSystem" CssClass="form-control col-sm-4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>



                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:CheckBox runat="server" Checked="true" ID="chkBoxContactPerson" Text=" Is Contact Person "></asp:CheckBox>

                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:CheckBox runat="server" Checked="false" ID="CheckBoxOwner" Text=" Is Owner "></asp:CheckBox>

                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </div>

                    <div id="savecaller">
                        <asp:Button class="btn btn-default" Text="Save Caller" ID="btnSaveCaller" runat="server" OnClick="btnSaveCaller_Click" />
                        <br />
                        <br />

                    </div>

                   


                    <asp:Label runat="server" ID="lbl_message"></asp:Label>

                </div>
            </div>

           


            <div class="col-sm-3 col-md-6 col-lg-4">

                <div class="jumbotron">
                    <h4>Last 5 reported by you</h4>

                    <div style="overflow:auto">
                        <asp:Repeater runat="server" ID="repeater_assignedtosession" >
                            <HeaderTemplate>
                                <table id="tbl_Session" class="table-bordered table table-hover">
                                    <tr>
                                        <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(1)')">TicketNumber</th>
                                        <th onclick="w3.sortHTML('#tbl_Session','.item','td:nth-child(2)')">Description</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="item">
                                    <td>
                                <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>

                                        <%--<asp:LinkButton runat="server" ID="lbl_Name_SessionGrid" Text='<%# Eval("TicketNumber") %>' OnCommand="lbl_Name_SessionGrid_Command" CommandName='<%# Eval("TicketInformationID") %>' CommandArgument='<%# Container.DataItem %>'></asp:LinkButton>--%>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_Description" Text='<%# Eval("Subject") %>'></asp:Label>
                                    </td>

                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="row">
             <div class="col-sm-12 col-md-12 col-lg-12">
                <div class="jumbotron">
                    <h3>Caller Directory</h3>

                     <div id="callergrid" style="overflow:auto">
                        <asp:GridView ID="DBGrid" runat="server"
                            CssClass="table table-hover table table-bordered pagination-ys"
                            
                            Width="100%"
                            AutoGenerateColumns="false"
                            AllowPaging="true"
                            PageSize="10"
                            DataKeyNames="CallerInformationID"
                            OnRowEditing="DBGrid_RowEditing"
                            OnRowCancelingEdit="DBGrid_RowCancelingEdit"
                            OnRowUpdating="DBGrid_RowUpdating"
                            OnRowDeleting="DBGrid_RowDeleting"
                            OnPageIndexChanging="DBGrid_PageIndexChanging">

                            <Columns>
                                <asp:TemplateField HeaderText="Callery Key">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" Text='<%# Eval("CallerKeyID") %>' ID="link_Callerkey" OnCommand="link_Callerkey_Command" CommandName="CallerKey" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                        <asp:HiddenField runat="server" Value='<%# Eval("CallerInformationID") %>' />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_CallerKeyID" Text='<%# Eval("CallerKeyID") %>'></asp:TextBox>
                                        <asp:TextBox runat="server" ID="txt_CallerInformationID" Visible="false" Text='<%# Eval("CallerInformationID") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_Name" Text='<%# Eval("Name") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Caller License">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("CallerLicense") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_CallerLicense" Text='<%# Eval("CallerLicense") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Phone Number">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_PhoneNumber" Text='<%# Eval("PhoneNumber") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="isOwner">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("isOwner") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="txt_isOwner" Checked='<%# Eval("isOwner") %>'></asp:CheckBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="isContactPerson">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("isContactPerson") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox runat="server" ID="txt_isContactPerson" Checked='<%# Eval("isContactPerson") %>'></asp:CheckBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_Email" Text='<%# Eval("Email") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Department">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_Department" Text='<%# Eval("Department") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Location">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Location") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_Location" Text='<%# Eval("Location") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="MachineName">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("MachineName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_MachineName" Text='<%# Eval("MachineName") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="OperatingSystem">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("OperatingSystem") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox runat="server" ID="txt_OperatingSystem" Text='<%# Eval("OperatingSystem") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/Edit.png" runat="server" CommandName="Edit" ToolTip="Edit" Width="20px" Height="20px" />
                                        <asp:ImageButton ImageUrl="~/Images/Delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="20px" Height="20px" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ImageUrl="~/Images/Update.png" runat="server" CommandName="Update" ToolTip="Update" Width="20px" Height="20px" />
                                        <asp:ImageButton ImageUrl="~/Images/Cancel.png" runat="server" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>


