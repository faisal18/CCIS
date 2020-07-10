<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="InquiryDetails.aspx.cs" Inherits="CCIS.UIComponenets.RecieveCall" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.7/js/select2.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $("#<%= ddl_Application.ClientID %>").select2({
                placeholder: "Select Application",
                allowClear: false
            });

            $("#<%= ddl_InquirySource.ClientID %>").select2({
                placeholder: "Select Source",
                allowClear: false
            });

            $("#<%= ddl_ActionTaken.ClientID %>").select2({
                placeholder: "Enter Action Taken",
                allowClear: false,
                tags: true,
                selectOnClose: true
            });

            var select2 = $("#<%= ddl_ActionTaken.ClientID %>");
            select2.on('select2:select', onSelect);

        });

        function onSelect(evt) {

            var data = $(this).val();
            console.log("actual: " + data);

            document.getElementById('<%= txt_hidden_actiontaken.ClientID %>').value = data;
            console.log("hidden: " + document.getElementById("txt_hidden_actiontaken").value);

        }

    </script>



    <div class="jumbotron">
        <h3>Caller Inquiry</h3>


        <div id="TableDiv" style="overflow: auto">
            <asp:Table runat="server" CssClass="table">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Caller"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label runat="server" ID="lbl_CallerId"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Application"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddl_Application"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Description *"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <div class="form-group">
                            <textarea runat="server" id="txt_rich_description" class="form-control" rows="7" cssclass="form-control">
                   
Issue Details: 






Impact Analysis: 
1. What is the scope of user impact ? Is it growing or limited/stagnant?

2. How many departments are affected?

3. Are both critical internal users (E.g. -  Senior Management, VPs etc.) and external users? 

4. How many are affected?

5. Does this incident cause major risks to the organization?

6. Are both critical internal users (E.g. -  Senior Management, VPs etc.) and external users? How many are affected?

7. Is there a financial impact?

8. Does this incident cause a major financial loss or damage to reputation?


URGENCY:
1. Is the urgency because of the IMPACT (above mentioned reasons), or because of other reasons? (This will be a comments section)

                        </textarea>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>

                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="ActionTaken *"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddl_ActionTaken" CssClass="" AppendDataBoundItems="true">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txt_hidden_actiontaken" ClientIDMode="Static" AutoPostBack="true" Style="display: none"></asp:TextBox>
                        <%--<textarea class="form-control" runat="server" id="txt_rich_actiontaken" style="resize: both; height: 100px; width: 900px; margin: 0px;"></textarea>--%>
                    </asp:TableCell>
                </asp:TableRow>



                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Call Type"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RadioButtonList runat="server" ID="rdl_InquiryType">
                            <asp:ListItem>Inquiry</asp:ListItem>
                            <asp:ListItem Selected="True">Issue</asp:ListItem>

                        </asp:RadioButtonList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label runat="server" Text="Inquiry Source"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList runat="server" ID="ddl_InquirySource"></asp:DropDownList>
                    </asp:TableCell>
                </asp:TableRow>

            </asp:Table>

            <div>
                <asp:Button runat="server" CssClass="btn btn-default" ID="btn_submit" OnClick="btn_submit_Click" Text="Submit" />
            </div>




        </div>

        <div id="TicketHistoryDiv">

            <div>
                <h3>Open Ticket History</h3>
            </div>
            <div style="overflow: auto">
                <asp:GridView ID="GV_CallerHistory" runat="server"
                    AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    CssClass="table table-striped table-bordered table-condensed pagination-ys"
                    AllowPaging="true"
                    PageSize="5"
                    OnPageIndexChanging="GV_CallerHistory_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Follow Up">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Text="Send Reminder" ID="link_FollowUp" CommandArgument='<%# Container.DataItemIndex %>' CommandName="FollowUp" OnCommand="link_TicketNumber_Command"> </asp:LinkButton>
                                <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Ticket Number">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>
                                <%--<asp:LinkButton runat="server" Text='<%# Eval("TicketNumber") %>' ID="link_TicketNumber" CommandArgument='<%# Container.DataItemIndex %>' CommandName="TicketNumber" OnCommand="link_TicketNumber_Command"> </asp:LinkButton>
                                <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Application">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Application") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Taken">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Assigned To">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Created Date">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div id="CloseTicketHistoryDiv">

            <div>
                <h3>Closed Ticket History</h3>
            </div>
            <div style="overflow: auto">
                <asp:GridView ID="GV_CloseTicketHistory" runat="server"
                    AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    CssClass="table table-striped table-bordered table-condensed pagination-ys"
                    AllowPaging="true"
                    PageSize="5"
                    OnPageIndexChanging="GV_CloseTicketHistory_PageIndexChanging">
                    <Columns>
                        <%--<asp:TemplateField HeaderText="Follow Up">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Text="Send Reminder" ID="link_FollowUp" CommandArgument='<%# Container.DataItemIndex %>' CommandName="FollowUp" OnCommand="link_TicketNumber_Command"> </asp:LinkButton>
                                <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>


                        <asp:TemplateField HeaderText="Ticket Number">
                             <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hyp_TicketNumber" Text='<%# Eval("TicketNumber") %>' NavigateUrl='<%# Eval("TicketInformationID", "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID={0}")%>'  ></asp:HyperLink>
                                <%--<asp:LinkButton runat="server" Text='<%# Eval("TicketNumber") %>' ID="link_TicketNumber" CommandArgument='<%# Container.DataItemIndex %>' CommandName="TicketNumber" OnCommand="link_TicketNumber_Command"> </asp:LinkButton>
                                <asp:HiddenField runat="server" Value='<%# Eval("TicketInformationID") %>' />--%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Application">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Application") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Taken">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Assigned To">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("AssignedTo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Created Date">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div id="InquiryHistoryDiv">
            <div>
                <h3>Inquiry History</h3>
            </div>
            <div style="overflow: auto">
                <asp:GridView ID="GV_CallerInquiryHistory" runat="server"
                    AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="true"
                    ShowFooter="true"
                    AllowPaging="true"
                    CssClass="table table-striped table-bordered table-condensed pagination-ys"
                    OnPageIndexChanging="GV_CallerInquiryHistory_PageIndexChanging"
                    PageSize="5">

                    <Columns>

                        <asp:TemplateField HeaderText="Application">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Application") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%-- <asp:TemplateField HeaderText="Subject">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="ActionTaken">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("ActionTaken") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Source">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CallerSource") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created By">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CreatedBy") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Created Date">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("CreatedDate") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div>
            <asp:Label runat="server" ID="lbl_message"></asp:Label>
        </div>
    </div>





</asp:Content>
