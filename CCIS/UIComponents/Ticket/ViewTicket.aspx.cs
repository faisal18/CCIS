using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json.Linq;
using DataTable = System.Data.DataTable;

namespace CCIS.UIComponenets
{
    public partial class ViewTicket : System.Web.UI.Page
    {

        #region Initiaters
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                string guid = CheckQueryStringGuidId();
                if (guid.Length > 1)
                {
                    if (true)
                    {
                        Guid g = new Guid(guid);
                        logUserIP("Ticket being viewed using guid " + g.ToString());
                        int ticketid = DAL.Operations.OpTicketInformation.GetAll().Where(x => x.TicketGUIDKey == g).Select(x => x.TicketInformationID).FirstOrDefault();
                        Loader(ticketid, true);
                    }
                }
                else
                {

                    if (Session.Keys.Count > 0)
                    {

                    }
                    else
                    {
                        System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                    }
                    if (!IsPostBack)
                    {
                        logUserIP("Ticket Viewed using TicketID " + CheckQueryString());
                        Loader(CheckQueryString());
                    }
                    else
                    {
                        LoadPostBack(CheckQueryString());
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private int CheckQueryString()
        {
            int TicketInformationID = 0;
            try
            {
                if (Request.QueryString.HasKeys())
                {
                    TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return TicketInformationID;
        }
        private string CheckQueryStringGuidId()
        {
            
            string guidkey = string.Empty;
            try
            {
                if (Request.QueryString["TicketGKey"]!=null)
                {
                    guidkey = Request.QueryString["TicketGKey"].ToString();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return guidkey;
        }
        #endregion


       

        #region Loaders
        private void Loader(int TicketInformationID)
        {
            try
            {
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID)).Tables[0];

                //Entities.TicketHistory LatestTicketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID_New(TicketInformationID);

                Entities.CallerInformation callerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(ticketInformation.CallerKeyID);



                string ApplicationName = DAL.Operations.OpApplications.GetRecordbyID(ticketInformation.ApplicationID).Name;
                string CallerInformation = callerInformation.Name;
                string CallerFacility = LoadFacilityName(callerInformation.CallerLicense);

                string Severity = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.SeverityID).Description;
                string Priority = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.PriorityID).Description;

                //string Severity_Internal = DAL.Operations.OpItemTypes.GetRecordbyID(LatestTicketHistory.SeverityID).Description;
                //string Priority_INternal = DAL.Operations.OpItemTypes.GetRecordbyID(LatestTicketHistory.PriorityID).Description;
                //string SupportType = DAL.Operations.OpItemTypes.GetRecordbyID(LatestTicketHistory.SupportTypeID).Description;

                string Severity_Internal = DAL.Operations.OpItemTypes.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["SeverityID"].ToString())).Description;
                string Priority_INternal = DAL.Operations.OpItemTypes.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["PriorityID"].ToString())).Description;
                string SupportType = DAL.Operations.OpItemTypes.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["SupportTypeID"].ToString())).Description;


                string ReportedBy = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketInformation.ReporterID).FullName;

                string ReportedTo = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["AssignedTOID"].ToString())).FullName;
                string status = DAL.Operations.OpStatuses.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentStatusID"].ToString())).Description;
                string sub_status = DAL.Operations.OpStatuses.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentSubStatusID"].ToString())).Description;
                string JiraNumber = dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["JiraNumber"].ToString();
                string TicketType = DAL.Operations.OpStatuses.GetRecordbyID(ticketInformation.TicketType).Description;

                h3_Summary.InnerText = ticketInformation.Subject;
                lbl_TicketNumber.Text = ticketInformation.TicketNumber;
                lbl_DueDate.Text = ticketInformation.DueDate.ToString();
                lbl_ContigencyPlan.Text = ticketInformation.ContingencyPlan;
                lbl_Application.Text = ApplicationName;
                lbl_callerKey.Text = CallerInformation;
                lbl_callerfacility.Text = CallerFacility;
                txt_Description.InnerText = ticketInformation.Description;
                txt_ActionTaken.Text = ticketInformation.ActionTaken;
                lbl_Severity.Text = Severity;
                lbl_IntSeverity.Text = Severity_Internal;
                lbl_IntPriority.Text = Priority_INternal;
                lbl_Priority.Text = Priority;
                lbl_ReportedBy.Text = ReportedBy;
                lbl_ReportedTo.Text = ReportedTo;
                lbl_TicketStatus.Text = status;
                lbl_SubStatus.Text = sub_status;
                lbl_TicketType.Text = TicketType;
                lbl_JiraNumber.Text = JiraNumber;
                lbl_SupportType.Text = SupportType;

                LoadButtons(false);
                LoadAttachment(TicketInformationID);
                
                LoadComments(dt_TicketHistory);
                LoadActivityComments(dt_TicketHistory);

                LoadCommentTemplate(false);
                LoadStatuses(false);
                LoadAssignee();
                LoadResolutionProperties(TicketInformationID);
                LoadButton(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentStatusID"].ToString()));
                LoadJiraDropDowns();
                LoadJiraButton(JiraNumber);
                LoadResoltuionPanel(TicketInformationID);
                LoadBusinessComments(ticketInformation);
                LoadSynchJiraComments(TicketInformationID);
                if (JiraNumber.Length > 0)
                {
                    LoadJiraAssignee(JiraNumber);
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        private void Loader(int TicketInformationID,bool isGuid)
        {
            try
            {
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                Entities.CallerInformation callerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(ticketInformation.CallerKeyID);
                Entities.TicketHistory LatestTicketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID)).Tables[0];


                string ApplicationName = DAL.Operations.OpApplications.GetRecordbyID(ticketInformation.ApplicationID).Name;
                string CallerInformation = callerInformation.Name;
                string CallerFacility = LoadFacilityName(callerInformation.CallerLicense);

                string Severity = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.SeverityID).Description;
                string Priority = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.PriorityID).Description;
                string Severity_Internal = DAL.Operations.OpItemTypes.GetRecordbyID(LatestTicketHistory.SeverityID).Description;
                string Priority_INternal = DAL.Operations.OpItemTypes.GetRecordbyID(LatestTicketHistory.PriorityID).Description;

                string ReportedBy = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketInformation.ReporterID).FullName;

                string ReportedTo = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["AssignedTOID"].ToString())).FullName;
                string status = DAL.Operations.OpStatuses.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentStatusID"].ToString())).Description;
                string sub_status = DAL.Operations.OpStatuses.GetRecordbyID(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentSubStatusID"].ToString())).Description;
                string JiraNumber = dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["JiraNumber"].ToString();
                string TicketType = DAL.Operations.OpStatuses.GetRecordbyID(ticketInformation.TicketType).Description;

                h3_Summary.InnerText = ticketInformation.Subject;
                lbl_TicketNumber.Text = ticketInformation.TicketNumber;
                lbl_DueDate.Text = ticketInformation.DueDate.ToString();
                lbl_ContigencyPlan.Text = ticketInformation.ContingencyPlan;
                lbl_Application.Text = ApplicationName;
                lbl_callerKey.Text = CallerInformation;
                lbl_callerfacility.Text = CallerFacility;
                txt_Description.InnerText = ticketInformation.Description;
                txt_ActionTaken.Text = ticketInformation.ActionTaken;
                lbl_Severity.Text = Severity;
                lbl_IntSeverity.Text = Severity_Internal;
                lbl_IntPriority.Text = Priority_INternal;
                lbl_Priority.Text = Priority;
                lbl_ReportedBy.Text = ReportedBy;
                lbl_ReportedTo.Text = ReportedTo;
                lbl_TicketStatus.Text = status;
                lbl_SubStatus.Text = sub_status;
                lbl_TicketType.Text = TicketType;
                lbl_JiraNumber.Text = JiraNumber;

                LoadButtons(isGuid);
                LoadAttachment(TicketInformationID);
                
                //LoadComments(TicketInformationID);
                LoadComments(dt_TicketHistory);

                LoadActivityComments(TicketInformationID);
                LoadCommentTemplate(isGuid);
                LoadStatuses(isGuid);
                LoadAssignee();
                LoadResolutionProperties(TicketInformationID);
                LoadButton(int.Parse(dt_TicketHistory.Rows[dt_TicketHistory.Rows.Count - 1]["IncidentStatusID"].ToString()));
                LoadJiraDropDowns();
                LoadJiraButton(JiraNumber);
                LoadResoltuionPanel(TicketInformationID);
                LoadBusinessComments(ticketInformation);
                if (JiraNumber.Length > 0)
                {
                    LoadJiraAssignee(JiraNumber);
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        private void LoadPostBack(int TicketInformationID)
        {
            try
            {
                LoadStatuses(false);

                //Need TicketInformationID
                LoadAttachment(TicketInformationID);

                //can work with DT
                LoadComments(TicketInformationID);
                //can work with DT
                LoadActivityComments(TicketInformationID);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region Populaters
        private void LoadButtons(bool isGuid)
        {
            try
            {
                if (!isGuid)
                {
                    if (Session["Rank"].ToString() == "Admin")
                    {
                        btn_EditTicket.Visible= true;
                        btn_closeTicketMain.Visible = true;
                        btn_assign.Visible = true;
                        btn_ChangeStatus.Visible = true;
                        btn_Report.Visible = true;
                        btn_CreateJira.Visible = true;
                        btn_AddComments.Visible = true;
                        btn_SendtoInfra.Visible = true;
                        btn_EditTicket.Visible = true;


                    }
                    else
                    {
                        switch (Session["Role"].ToString().ToUpper())
                        {
                            case "L1":

                                btn_EditTicket.Visible = true;
                                btn_closeTicketMain.Visible = true;
                                btn_assign.Visible = true;
                                btn_ChangeStatus.Visible = true;
                                btn_AddComments.Visible = true;

                                btn_Report.Visible = false;
                                btn_CreateJira.Visible = false;
                                btn_SendtoInfra.Visible = false;



                                break;
                            case "L2":

                                btn_EditTicket.Visible = true;
                                btn_closeTicketMain.Visible = false;
                                btn_Report.Visible = false;

                                btn_AddComments.Visible = true;
                                btn_assign.Visible = true;
                                btn_ChangeStatus.Visible = true;
                                btn_CreateJira.Visible = true;
                                btn_SendtoInfra.Visible = true;

                                break;
                            case "L3":

                                btn_EditTicket.Visible = false;
                                btn_closeTicketMain.Visible = false;
                                btn_assign.Visible = false;
                                btn_ChangeStatus.Visible = false;
                                btn_Report.Visible = false;
                                btn_CreateJira.Visible = false;
                                btn_SendtoInfra.Visible = false;
                                btn_AddComments.Visible = false;


                                break;
                            case "BUSINESS":

                                btn_EditTicket.Visible = false;
                                btn_closeTicketMain.Visible = false;
                                btn_assign.Visible = false;
                                btn_ChangeStatus.Visible = false;
                                btn_Report.Visible = true;
                                btn_CreateJira.Visible = false;
                                btn_SendtoInfra.Visible = false;
                                btn_AddComments.Visible = true;

                                break;


                        }

                    }
                }
                else if(isGuid)
                {
                    btn_closeTicketMain.Visible = false;
                    btn_assign.Visible = false;
                    btn_ChangeStatus.Visible = false;
                    btn_Report.Visible = false;
                    btn_CreateJira.Visible = false;
                    btn_AddComments.Visible = false;
                    btn_SendtoInfra.Visible = false;
                    btn_EditTicket.Visible = false;
                }
                
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("This user rank has problem " + Session["Name"]);
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadAttachment(int TicketInformationID)
        {
            try
            {
                DataTable dt_TicketAttachment = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketAttachment.GetTicketAttachmentbyTicketID(TicketInformationID)).Tables[0];

                Place_Attachment.Dispose();
                Place_Attachment.Controls.Clear();
                int count = 0;
                foreach (DataRow row in dt_TicketAttachment.AsEnumerable())
                {
                    string filename = row["FileName"].ToString();
                    string AttachmentID = row["TicketAttachmentID"].ToString();
                    count++;

                    LinkButton LB_Attachment = new LinkButton();
                    LB_Attachment.Text = filename + "<br/>";
                    LB_Attachment.ID = AttachmentID+ count;
                    LB_Attachment.CommandArgument = AttachmentID;
                    LB_Attachment.CommandName = AttachmentID;
                    LB_Attachment.Click += LB_Attachment_Click;
                    Place_Attachment.Controls.Add(LB_Attachment);
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
        private void LoadCommentTemplate(bool isGuid)
        {
            try
            {
                if (!isGuid)
                {
                    var list = new List<Entities.ItemTypes>();
                    if (Session["Rank"].ToString() != "Admin")
                    {
                        string Role = Session["Role"].ToString();
                        list = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Comment Template").Where(x => x.Role == Role).ToList();
                    }
                    else if (Session["Rank"].ToString() == "Admin")
                    {
                        list = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Comment Template").ToList();
                    }
                    DataTable dt = DAL.Helper.ListToDataset.ToDataSet(list).Tables[0];

                    //populate dropdown
                    ddl_scenario.Items.Clear();
                    ddl_scenario.DataSource = dt;
                    ddl_scenario.DataTextField = "Scenario";
                    ddl_scenario.DataValueField = "ItemTypesID";
                    ddl_scenario.DataBind();
                    ddl_scenario.Items.Insert(0, new ListItem("Add Free Text", ""));
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("This user rank has problem " + Session["Name"]);
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }

        private void LoadActivityComments(int TicketInformationID)
        {
            try
            {
                DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID)).Tables[0];
                Pnl_ActivityComments.Dispose();
                Pnl_ActivityComments.Controls.Clear();

                foreach (DataRow row in dt_TicketHistory.AsEnumerable())
                {

                    string comment = row["ActivityComments"].ToString();
                    string commentBy = row["CreatedBy"].ToString();
                    string datetime = row["CreationDate"].ToString();
                    string updatedBy = row["UpdatedBy"].ToString();

                    if (commentBy.Length < 1 && updatedBy.Length > 1) 
                    {
                        commentBy = row["UpdatedBy"].ToString();
                        datetime = row["UpdateDate"].ToString();
                    }

                    if (comment.Length > 0 && commentBy.Length > 0)
                    {
                        Label newline = new Label();
                        newline.Text = "<br/>";

                        HyperLink hyperLink = new HyperLink();
                        hyperLink.Text = commentBy;
                        hyperLink.NavigateUrl = "#";

                        Label heading = new Label();
                        heading.Text = " added a comment - " + datetime + "<br/>";

                        Label content = new Label();
                        content.Text = comment + "<br/>";

                        Pnl_ActivityComments.Controls.Add(newline);
                        Pnl_ActivityComments.Controls.Add(hyperLink);
                        Pnl_ActivityComments.Controls.Add(heading);
                        Pnl_ActivityComments.Controls.Add(content);
                    }

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadActivityComments(DataTable _dt_TicketHistory)
        {
            try
            {
                DataTable dt_TicketHistory = _dt_TicketHistory;
                Pnl_ActivityComments.Dispose();
                Pnl_ActivityComments.Controls.Clear();

                foreach (DataRow row in dt_TicketHistory.AsEnumerable())
                {

                    string comment = row["ActivityComments"].ToString();
                    string commentBy = row["CreatedBy"].ToString();
                    string datetime = row["CreationDate"].ToString();
                    string updatedBy = row["UpdatedBy"].ToString();

                    if (commentBy.Length < 1 && updatedBy.Length > 1)
                    {
                        commentBy = row["UpdatedBy"].ToString();
                        datetime = row["UpdateDate"].ToString();
                    }

                    if (comment.Length > 0 && commentBy.Length > 0)
                    {
                        Label newline = new Label();
                        newline.Text = "<br/>";

                        HyperLink hyperLink = new HyperLink();
                        hyperLink.Text = commentBy;
                        hyperLink.NavigateUrl = "#";

                        Label heading = new Label();
                        heading.Text = " added a comment - " + datetime + "<br/>";

                        Label content = new Label();
                        content.Text = comment + "<br/>";

                        Pnl_ActivityComments.Controls.Add(newline);
                        Pnl_ActivityComments.Controls.Add(hyperLink);
                        Pnl_ActivityComments.Controls.Add(heading);
                        Pnl_ActivityComments.Controls.Add(content);
                    }

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private void LoadComments(int TicketInformationID)
        {
            try
            {
                DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID)).Tables[0];

                CommentsContainer.Dispose();
                CommentsContainer.Controls.Clear();

                foreach (DataRow row in dt_TicketHistory.AsEnumerable())
                {

                    string comment = row["Comments"].ToString();
                    string commentBy = row["CreatedBy"].ToString();
                    string datetime = row["CreationDate"].ToString();
                    if (comment.Length > 0 && commentBy.Length > 0)
                    {
                        Label newline = new Label();
                        newline.Text = "<br/>";

                        HyperLink hyperLink = new HyperLink();
                        hyperLink.Text = commentBy;
                        hyperLink.NavigateUrl = "#";

                        Label heading = new Label();
                        heading.Text = " added a comment - " + datetime + "<br/>";

                        Label content = new Label();
                        content.Text = comment + "<br/>";

                        CommentsContainer.Controls.Add(newline);
                        CommentsContainer.Controls.Add(hyperLink);
                        CommentsContainer.Controls.Add(heading);
                        CommentsContainer.Controls.Add(content);
                    }

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        private void LoadComments(DataTable _dt_TicketHistory)
        {
            try
            {
                //DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID)).Tables[0];
                DataTable dt_TicketHistory = _dt_TicketHistory;
                CommentsContainer.Dispose();
                CommentsContainer.Controls.Clear();

                foreach (DataRow row in dt_TicketHistory.AsEnumerable())
                {

                    string comment = row["Comments"].ToString();
                    string commentBy = row["CreatedBy"].ToString();
                    string datetime = row["CreationDate"].ToString();
                    if (comment.Length > 0 && commentBy.Length > 0)
                    {
                        Label newline = new Label();
                        newline.Text = "<br/>";

                        HyperLink hyperLink = new HyperLink();
                        hyperLink.Text = commentBy;
                        hyperLink.NavigateUrl = "#";

                        Label heading = new Label();
                        heading.Text = " added a comment - " + datetime + "<br/>";

                        Label content = new Label();
                        content.Text = comment + "<br/>";

                        CommentsContainer.Controls.Add(newline);
                        CommentsContainer.Controls.Add(hyperLink);
                        CommentsContainer.Controls.Add(heading);
                        CommentsContainer.Controls.Add(content);
                    }

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }


        private void LoadResoltuionPanel(int TicketInformationID)
        {
            try
            {
                Entities.Resolution resolution = DAL.Operations.OpResolution.GetResolutionByTicketInformationID(TicketInformationID);

                if (resolution != null)
                {
                    int personid = 0;
                    string name = string.Empty;
                    if (int.TryParse(resolution.CreatedBy, out personid))
                    {
                        name = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(personid).First().FullName;
                    }
                    else
                    {
                        name = resolution.CreatedBy;
                    }

                    Label newline = new Label();
                    newline.Text = "<br/>";

                    HyperLink hyperLink = new HyperLink();
                    hyperLink.Text = name;
                    hyperLink.NavigateUrl = "#";

                    Label heading = new Label();
                    heading.Text = " created the resolution on - " + resolution.CreationDate + "<br/>";

                    Label content = new Label();
                    content.Text = "<b>Root Cause</b>: " + resolution.RootCause + "<br/><b>Steps Taken</b>: " + resolution.Steps;

                    Pnl_Resolution.Controls.Add(newline);
                    Pnl_Resolution.Controls.Add(hyperLink);
                    Pnl_Resolution.Controls.Add(heading);
                    Pnl_Resolution.Controls.Add(content);
                }
                else
                {
                    Label heading = new Label();
                    heading.Text = "<br/>The resolution for this ticket has not been posted.";

                    Pnl_Resolution.Controls.Add(heading);
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadAssignee()
        {
            try
            {
                DataTable DT_Assigne = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                ddl_assignee.DataSource = DT_Assigne.AsEnumerable().CopyToDataTable();
                ddl_assignee.DataTextField = "FullName";
                ddl_assignee.DataValueField = "PersonInformationID";
                ddl_assignee.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadJiraAssignee(string JiraNumber)
        {
            try
            {
                string result = Jira_CheckTicketExistGetResult(JiraNumber);
                JObject result2 = JObject.Parse(result);



                if (result2["errorMessages"] == null)
                {
                    if (result2["expand"].ToString() != null)
                    {
                        string applicationname = result2["fields"]["project"]["key"].ToString();
                        ddl_JiraAssignee.DataSource = DAL.Jira.JiraUsers.GetAssignableUserByApplicationKey(applicationname);
                        ddl_JiraAssignee.DataValueField = "Key";
                        ddl_JiraAssignee.DataTextField = "Name";
                        ddl_JiraAssignee.DataBind();
                        

                        btn_JiraAssignee.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadResolutionProperties(int TicketInformationID)
        {
            try
            {
                //int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                int ApplicationID = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID).ApplicationID;
                var List = DAL.Operations.OpApplicationProps.GetApplicationPropByApplicationId(ApplicationID);

                ddl_Component.DataSource = DAL.Helper.ListToDataset.ToDataSet(List.Where(x => x.Property == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ComponentTypeID"))).ToList()).Tables[0];
                ddl_Component.DataTextField = "Value";
                ddl_Component.DataValueField = "ApplicationPropID";
                ddl_Component.DataBind();

                ddl_Module.DataSource = DAL.Helper.ListToDataset.ToDataSet(List.Where(x => x.Property == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("ModuleTypeID"))).ToList()).Tables[0];
                ddl_Module.DataTextField = "Value";
                ddl_Module.DataValueField = "ApplicationPropID";
                ddl_Module.DataBind();

                ddl_Version.DataSource = DAL.Helper.ListToDataset.ToDataSet(List.Where(x => x.Property == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("VersionTypeID"))).ToList()).Tables[0];
                ddl_Version.DataTextField = "Value";
                ddl_Version.DataValueField = "ApplicationPropID";
                ddl_Version.DataBind();

                ddl_Category.DataSource = DAL.Helper.ListToDataset.ToDataSet(List.Where(x => x.Property == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CategoryTypeID"))).ToList()).Tables[0];
                ddl_Category.DataTextField = "Value";
                ddl_Category.DataValueField = "ApplicationPropID";
                ddl_Category.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadJiraDropDowns()
        {
            try
            {
                ddl_JiraApplication.DataSource = DAL.Jira.JiraProjects.GetAllApplication();
                ddl_JiraApplication.DataValueField = "Key";
                ddl_JiraApplication.DataTextField = "Name";
                ddl_JiraApplication.DataBind();

                //ddl_JiraTicketType.DataSource = DAL.Jira.JiraTicketType.GetAllTicketType();
                //ddl_JiraTicketType.DataValueField = "ID";
                //ddl_JiraTicketType.DataTextField = "Name";
                //ddl_JiraTicketType.DataBind();

                ddl_JiraPriority.DataSource = DAL.Jira.JiraPriority.GetAllPriority();
                ddl_JiraPriority.DataValueField = "ID";
                ddl_JiraPriority.DataTextField = "Name";
                ddl_JiraPriority.DataBind();

                ddl_JiraUsers.DataSource = DAL.Jira.JiraUsers.GetAssignableUserByApplicationKey(ddl_JiraApplication.SelectedValue);
                ddl_JiraUsers.DataValueField = "Key";
                ddl_JiraUsers.DataTextField = "Name";
                ddl_JiraUsers.DataBind();
                ddl_JiraUsers.Items.Insert(0, new ListItem("UnAssigned", "-1"));

                ddl_JiraTicketStatus.DataSource = DAL.Jira.JiraStatuses.GetStatuses();
                ddl_JiraTicketStatus.DataValueField = "ID";
                ddl_JiraTicketStatus.DataTextField = "Name";
                ddl_JiraTicketStatus.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadStatuses(bool isGuid)
        {
            try
            {
                DAL.Operations.Logger.Info("isGuid " + isGuid);
                if (!isGuid)
                {
                    DataTable dt_statuses = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpStatuses.GetAll().OrderBy(a => a.Description).Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatus")).ToList()).Tables[0];
                    PH_Statuses.Dispose();
                    PH_Statuses.Controls.Clear();

                    string REmoveStatusesforL2 = System.Configuration.ConfigurationManager.AppSettings.Get("REmoveStatusesforL2");
                    bool bool_REmoveStatusesforL2 = bool.Parse(REmoveStatusesforL2);


                    foreach (DataRow row in dt_statuses.Rows)
                    {
                        string Status_Id = row["StatusesID"].ToString();
                        string Status_Desc = row["Description"].ToString();

                        if (Session["Rank"].ToString() != "Admin")
                        {
                            if (bool_REmoveStatusesforL2)
                            {
                                if (Session["Role"].ToString() == "L2")
                                {
                                    if (Status_Desc.ToUpper().Contains("IN PROGRESS") ||
                                        Status_Desc.ToUpper().Contains("IN PROGRESS,L2 SUPPORT") ||
                                        Status_Desc.ToUpper().Contains("RESOLVED WITH KB") ||
                                        Status_Desc.ToUpper().Contains("RESOLVED")
                                        )
                                    {
                                        PopulateStatus(Status_Id, Status_Desc);
                                    }
                                }
                                else if (Session["Role"].ToString() == "L1")
                                {

                                    PopulateStatus(Status_Id, Status_Desc);
                                }
                            }
                        }
                        else
                        {
                            PopulateStatus(Status_Id, Status_Desc);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("This user rank has problem " + Session["Name"]);
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void PopulateStatus(string Status_Id,string Status_Desc)
        {
            try
            {
                Literal LI_Open = new Literal();
                LI_Open.Text = "<li>";
                PH_Statuses.Controls.Add(LI_Open);

                LinkButton LB_Status = new LinkButton();
                LB_Status.Text = Status_Desc + "<br/>";
                LB_Status.ID = Status_Id;
                LB_Status.CommandArgument = Status_Desc;
                LB_Status.CommandName = Status_Desc;
                LB_Status.Click += LB_Status_Click;
                PH_Statuses.Controls.Add(LB_Status);


                Literal Li_Close = new Literal();
                Li_Close.Text = "</li>";
                PH_Statuses.Controls.Add(Li_Close);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadButton(int TicketStatusID)
        {
            try
            {
                if (TicketStatusID == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusClosedID")))
                {
                    btn_closeTicketMain.Visible = false;
                }
                else
                {
                    btn_closeTicketMain.Visible = false;
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadJiraButton(string data)
        {
            try
            {
                if (data.Length > 0)
                {
                    link_JiraNumber.Text = data;
                    link_JiraNumber.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings.Get("JiraURL") + "browse/" + data;
                    //btn_CreateJira.Disabled = true;

                }
                else
                {
                    btn_CreateJira.Disabled = false;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
        }
        private string LoadFacilityName(string CallerFacility)
        {
            try
            {
                WebService.WSAutomation yo = new WebService.WSAutomation();
                List<string> list = yo.GetAllLicenses(CallerFacility);
                if (list != null)
                    if (list.Count > 0)
                        if (list[0].ToString() != null)
                            if (list[0].ToString().Length > 0)
                            {
                                CallerFacility = yo.GetAllLicenses(CallerFacility)[0].ToString();
                                string[] data = CallerFacility.Split('|');
                                CallerFacility = data[4] + " - " + data[2];
                            }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
            return CallerFacility;
        }
        private void LoadBusinessComments(Entities.TicketInformation ticketInformation)
        {
            try
            {


                txt_issueDesc.Text = ticketInformation.IssueDescription;
                txt_RActionTaken.Text = ticketInformation.ResolutionActions;
                txt_RSummary.Text = ticketInformation.ResolutionSummary;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadSynchJiraComments(int TicketInformationID)
        {
            try
            {
                //DAL.Jira.JiraSynch obj = new DAL.Jira.JiraSynch();
                //DAL.Operations.Logger.Info("Synching comments for ticket " + TicketInformationID);
                ////string jiraKey = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID).Where(x => x.JiraNumber != null && x.IncidentStatusID != 14).OrderBy(x => x.JiraNumber).Select(x => x.JiraNumber).First();
                //string jiraKey = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID).JiraNumber;
                ////string jiraKey = string.Empty;

                //if (jiraKey.Length > 0)
                //{
                //    DAL.Operations.Logger.Info("Synching comments for Jira  " + jiraKey);

                //    obj.Synch_Comments(jiraKey);
                //}
                ////DAL.Jira.JiraSynch
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region Custom Controls
        private void LB_Attachment_Click(object sender, EventArgs e)
        {
            try
            {
                var link = sender as LinkButton;
                string ilink = link.ID.Remove((link.ID.Count()) - 1, 1);
                var ticketAttachment = DAL.Operations.OpTicketAttachment.GetRecordbyID(int.Parse(ilink));

                string filename = ticketAttachment.filename;
                byte[] Byte_File = ticketAttachment.Attachment;
                string B64_filecontent = Encoding.UTF8.GetString(Byte_File);

                string AppDataDir = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data");
                string FileDir = AppDataDir + "\\" + filename;
                System.IO.File.WriteAllBytes(FileDir, Byte_File);
                download_string(FileDir);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void download_string(string path)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void download_string(string filename, string filecontent)
        {
            try
            {

                string file_ext = Path.GetExtension(filename);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();


                //if (file_ext.ToUpper() == ".TXT" || file_ext.ToUpper() == ".XML" ||)
                //{
                //    Response.AppendHeader("Content-Length", filecontent.Length.ToString());
                //    Response.ContentType = "text/plain";
                //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                //    Response.Write(filecontent);
                //}

                if (file_ext.ToUpper() == ".ZIP" || file_ext.ToUpper() == ".XLSX" || file_ext.ToUpper() == ".DOCX")
                {
                    byte[] file = Convert.FromBase64String(filecontent);

                    Response.ContentType = "application/x-compressed";
                    Response.Charset = string.Empty;
                    Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.BinaryWrite(file);
                    Response.OutputStream.Flush();
                    Response.OutputStream.Close();
                }
                else
                {
                    //Response.AppendHeader("Content-Length", filecontent.Length.ToString());
                    //Response.ContentType = "text/plain";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    //Response.Write(filecontent);
                    string path = System.Configuration.ConfigurationManager.AppSettings.Get("ReportsPath");
                    File.WriteAllBytes(path, Convert.FromBase64String(filecontent));

                    using (var client = new WebClient())
                    {
                        client.DownloadFile(path + filename, filename);
                    }

                }
            }
            catch (Exception) { }
            finally
            {
                try
                {
                    Response.End();
                }
                catch (Exception) { }
                finally
                {
                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    Thread.Sleep(1);
                }
            }
        }
        #endregion

        #region Add-Ons
        protected void btn_CommentTemplate1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddl_scenario.SelectedValue == "")
                {
                    txt_Comments.InnerText = "";
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showComment();", true);

                }
                else
                {
                    txt_Comments.InnerText = DAL.Operations.OpItemTypes.GetRecordbyID(int.Parse(ddl_scenario.SelectedValue)).Description;
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showComment();", true);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_AddCommetns_Click(object sender, EventArgs e)
        {
            int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);

            try
            {
                if (txt_Comments.InnerText.Length > 0)
                {
                    logUserIP("Comment added to "+TicketInformationID);

                    Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                    Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);

                    string Severity = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.SeverityID).Description;
                    string Priority = DAL.Operations.OpItemTypes.GetRecordbyID(ticketInformation.PriorityID).Description;
                    string ReportedBy = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketInformation.ReporterID).FullName;

                    txt_Comments.InnerText = Session["Name"].ToString() + " : " + Session["Role"].ToString() + " : " + txt_Comments.InnerText;
                    string ESCAPED_Comment = Jira_EscapeCharacters(txt_Comments.InnerText, true);

                    if (Jira_EscapeCharacters(ticketHistoryMain.Comments,true) != ESCAPED_Comment)
                    {

                        Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                        {
                            Comments = ESCAPED_Comment,
                            IncidentStatusID = ticketHistoryMain.IncidentStatusID,
                            IncidentSubStatusID = ticketHistoryMain.IncidentSubStatusID,
                            SeverityID = ticketHistoryMain.SeverityID,
                            PriorityID = ticketHistoryMain.PriorityID,
                            TicketInformationID = TicketInformationID,
                            JiraNumber = ticketHistoryMain.JiraNumber,
                            AssignedFromID = ticketHistoryMain.AssignedFromID,
                            AssignedTOID = ticketHistoryMain.AssignedTOID,
                            SupportTypeID = ticketHistoryMain.SupportTypeID,
                            Activity = "UPDATE TICKET - ADD COMMENT",
                            CreatedBy = Session["Name"].ToString(),
                            CreationDate = DateTime.Now

                        };



                        int result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
                        if (result > 0)
                        {

                            string ToAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistoryMain.AssignedTOID).Email;
                            string comments = Session["Name"].ToString() + " has commented on ticket: ";
                            CreateInternalNotification(TicketInformationID, comments, ToAddress, "Internal_Template_CS_02_INT_Ticket update");

                            //Create Comment in JIRA
                            string JiraTicketKey = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID).JiraNumber;
                            if (JiraTicketKey != null)
                            {
                                if (JiraTicketKey.Length > 0)
                                {
                                    if (Jira_CheckTicketExist(JiraTicketKey))
                                    {
                                        Jira_AddComments(ESCAPED_Comment, JiraTicketKey);
                                    }

                                    Entities.JiraTicketComments jiraTicketComments = new Entities.JiraTicketComments()
                                    {
                                        TicketInformationID = TicketInformationID,
                                        JiraTicketKey = JiraTicketKey,
                                        Comments = ESCAPED_Comment,

                                        CreatedBy = Session["Name"].ToString(),
                                        CreationDate = DateTime.Now
                                    };
                                    int result23 = DAL.Operations.OpJiraTicketComments.InsertRecord(jiraTicketComments);
                                }
                            }   



                            Loader(TicketInformationID);
                            //LoadPostBack(TicketInformationID);
                        }
                        else
                        {
                            lbl_message.Text = "ErrorCode: " + result;
                        }
                    }
                    else
                    {
                        lbl_message.Text = "Duplicate comment entry refrained";
                    }
                }
                else
                {
                    lbl_message.Text = "Please enter data in comments box";
                }
                txt_Comments.InnerText = null;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("This comment had a problem: " + txt_Comments.InnerText);
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_AssigneeSelect_Click(object sender, EventArgs e)
        {
            try
            {

                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                logUserIP("Internal assignee changed in " + TicketInformationID);

                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);

                string role = Session["Role"].ToString();
                string userrole = DAL.Operations.OpUserRoles.GetUserRoleByPersonId(int.Parse(ddl_assignee.SelectedValue)).Description;
                Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                {
                    AssignedTOID = int.Parse(ddl_assignee.SelectedValue.ToString()),
                    TicketInformationID = TicketInformationID,
                    SeverityID = ticketHistoryMain.SeverityID,
                    PriorityID = ticketHistoryMain.PriorityID,
                    IncidentStatusID = ticketHistoryMain.IncidentStatusID,
                    IncidentSubStatusID = ticketHistoryMain.IncidentSubStatusID,
                    SupportTypeID = ticketHistoryMain.SupportTypeID,
                    JiraNumber = lbl_JiraNumber.Text,
                    ActivityComments = "Assignee has been changed to " + ddl_assignee.SelectedItem.Text,
                    Activity = userrole != "" ? "ASSIGN TICKET TO " + userrole : "ASSIGN TICKET TO " + role,
                    CreatedBy = Session["Name"].ToString(),
                    CreationDate = DateTime.Now
                };


                int result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);


                if (result > 0)
                {
                    string _ToAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistoryMain.AssignedTOID).Email;
                    string _comments = Session["Name"].ToString() + " has assigned " + ddl_assignee.SelectedItem.Text + " on ticket ";
                    CreateInternalNotification(TicketInformationID, _comments, _ToAddress, "Internal_Template_CS_02_INT_Ticket update");
                    lbl_message.Text = "Record added successfully";
                    Loader(TicketInformationID);
                }
                else
                {
                    lbl_message.Text = "ErrorCode: " + result;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_JiraAssignee_Click(object sender, EventArgs e)
        {
            try
            {
                string JiraKey = lbl_JiraNumber.Text;
                string Name = ddl_JiraAssignee.SelectedItem.Text;
                string NameKey = ddl_JiraAssignee.SelectedValue;

              

                if (Jira_UpdateAssignee(NameKey, JiraKey))
                {

                    logUserIP("Jira assignee changed in Ticket  " + CheckQueryString());

                    Entities.TicketHistory history = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(CheckQueryString());

                    history.Comments = "";
                    history.ActivityComments = "Jira Assignee has been changed to " + Name;
                    history.CreatedBy = Session["Name"].ToString();
                    history.CreationDate = DateTime.Now;

                    if (DAL.Operations.OpTicketHistory.InsertRecord(history) > 0)
                    {
                        lbl_message.Text = "Jira Assignee updated";
                        LoadPostBack(CheckQueryString());
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }
        protected void btn_EditTicket_Click(object sender, EventArgs e)
        {
            try
            {
                logUserIP("Ticket being edited " + CheckQueryString());
                Response.Redirect("EditTicket.aspx?TicketInformationID=" + int.Parse(Request.QueryString["TicketInformationID"]), false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_CloseTicket_Click(object sender, EventArgs e)
        {
            try
            {
                

                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                logUserIP(TicketInformationID + " Ticket Closed ");
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                int ApplicationID = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID).ApplicationID;

                int CategoryId = 0, Compononent = 0, Version = 0, Module = 0;

                int.TryParse(ddl_Category.SelectedValue.ToString(), out CategoryId);
                int.TryParse(ddl_Component.SelectedValue.ToString(), out Compononent);
                int.TryParse(ddl_Version.SelectedValue.ToString(), out Version);
                int.TryParse(ddl_Module.SelectedValue.ToString(), out Module);

                Entities.Resolution resolution = new Entities.Resolution
                {
                    TicketInformationID = TicketInformationID,
                    CategoryID = CategoryId,

                    Steps = txt_StepsTaken.Text,
                    RootCause = txt_RootCause.Text,
                    CreatedBy = Session["PersonID"].ToString(),
                    CreationDate = DateTime.Now

                };

                Entities.Resolution resolutionbool = DAL.Operations.OpResolution.GetResolutionByTicketInformationID(TicketInformationID);




                //TO BE INSERTED MULTIPLE as this is 1 to Many
                Entities.TicketApplicationProp TAP_Component = new Entities.TicketApplicationProp
                {
                    ApplicationPropID = Compononent,
                    TicketID = TicketInformationID,

                    CreatedBy = Session["PersonID"].ToString(),
                    CreationDate = DateTime.Now
                };

                Entities.TicketApplicationProp TAP_Version = new Entities.TicketApplicationProp
                {
                    ApplicationPropID = Version,
                    TicketID = TicketInformationID,

                    CreatedBy = Session["PersonID"].ToString(),
                    CreationDate = DateTime.Now
                };


                Entities.TicketApplicationProp TAP_Module = new Entities.TicketApplicationProp
                {
                    ApplicationPropID = Module,
                    TicketID = TicketInformationID,

                    CreatedBy = Session["PersonID"].ToString(),
                    CreationDate = DateTime.Now
                };

                //This is closed because change status is being called on status click
                //ChangeStatus(int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusResolvedByKBID")));
                DAL.Operations.OpTicketApplicationProps.InsertRecord(TAP_Version);
                DAL.Operations.OpTicketApplicationProps.InsertRecord(TAP_Module);
                DAL.Operations.OpTicketApplicationProps.InsertRecord(TAP_Component);

                int result = 0;

                if (resolutionbool != null)
                {
                    resolutionbool.TicketInformationID = TicketInformationID;
                    resolutionbool.CategoryID = CategoryId;
                    resolutionbool.Steps = txt_StepsTaken.Text;
                    resolutionbool.RootCause = txt_RootCause.Text;
                    resolutionbool.UpdatedBy = Session["PersonID"].ToString();
                    resolutionbool.UpdateDate = DateTime.Now;
                    result = DAL.Operations.OpResolution.UpdateResolution(resolutionbool, resolutionbool.ResolutionID);

                }
                else
                {
                    result = DAL.Operations.OpResolution.InsertRecord(resolution);

                }


                if (result > 0)
                {
                    lbl_message.Text = "Record added successfully";
                }
                else
                {
                    lbl_message.Text = "ErrorCode: " + result;
                }

                Loader(TicketInformationID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LB_Status_Click(object sender, EventArgs e)
        {
            try
            {
                var status = sender as LinkButton;
                int Status_ID = int.Parse(status.ID);

                if (Status_ID == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusResolvedByKBID")))
                {
                    if (Session["Role"].ToString() != "L1")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "showCloseTicketModal();", true);
                    }
                    ChangeStatus(Status_ID);

                }
                else if(Status_ID == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusCustomerInfo")))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "none", "showAwaitingModal();", true);
                    //ChangeStatus(Status_ID);
                }
                else
                {
                    ChangeStatus(Status_ID);
                }


            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_AwaitInfor_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeStatus(int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusCustomerInfo")));
            }
            catch (Exception ex)
            {
                DAL.Helper.Log4Net.Error(ex);
            }
        }
        private void ChangeStatus(int Status_ID)
        {
            try
            {


                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                logUserIP(TicketInformationID + " Ticket Status changed ");

                Entities.TicketHistory ticketHistory_src = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                string ToAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistory_src.AssignedTOID).Email;


                //ticketHistory_src.Comments = "Ticket status has been changed to " + DAL.Operations.OpStatuses.GetStatusById(Status_ID).Description;
                ticketHistory_src.Comments = "";
                ticketHistory_src.ActivityComments = "Ticket status has been changed to " + DAL.Operations.OpStatuses.GetStatusById(Status_ID).Description;
                ticketHistory_src.Activity = "STATUS CHANGED";
                ticketHistory_src.CreatedBy = Session["Name"].ToString();
                ticketHistory_src.CreationDate = DateTime.Now;
                ticketHistory_src.IncidentStatusID = Status_ID;
                
                int result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory_src);
                if (result > 0)
                {
                    lbl_message.Text = "Ticket status updated";
                }
                else
                {
                    lbl_message.Text = "Something went wrong";
                }


                string Email_INT = string.Empty, Comment_INT = string.Empty, Email_CS = string.Empty, Comment_CS = string.Empty;
                string Prioriy = DAL.Operations.OpItemTypes.GetRecordbyID(ticketHistory_src.PriorityID).Description;

                switch (Status_ID)
                {
                    case 10:

                        Email_CS = "Customer_Template_CS_01_Ticket created";
                        Comment_CS = "Ticket status changed to New";

                        Email_INT = "Internal_Template_CS_01_INT_Ticket assigned to a group";
                        Comment_INT = "Ticket has been updated to New";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;
                    case 11:
                        Email_CS = "";
                        Comment_CS = "";

                        Email_INT = "Internal_Template_CS_02_INT_Ticket update";
                        Comment_INT = "Ticket status changed to In-Progress";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;
                    case 12:
                        Email_CS = "Customer_Template_CS_03_Ticket resolved";
                        Comment_CS = "Ticket has been resolved";

                        Email_INT = "Internal_Template_CS_03_INT_Ticket resolved";
                        Comment_INT = "Ticket status changed to Resolved with KB";

                        if (Session["Rank"].ToString() != "Admin")
                        {
                            if (Session["Role"].ToString() == "L2")
                            {
                                SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                            }
                            else
                            {
                                SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                            }
                        }
                        else
                        {
                            SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                            SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        }
                        break;
                    case 13:
                        Email_CS = "Customer_Template_CS_05_Ticket reopened";
                        Comment_CS = "Ticket has been Reopened";

                        Email_INT = "Internal_Template_CS_05_INT_Ticket reopened";
                        Comment_INT = "Ticket status changed to Reopened";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;
                    case 14:
                        Email_CS = "Customer_Template_CS_04_Ticket closed";
                        Comment_CS = "Ticket has been Closed";

                        Email_INT = "Internal_Template_CS_02_INT_Ticket update";
                        Comment_INT = "Ticket status changed to Closed";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;
                    case 37:
                        

                        Email_INT = "Internal_Template_CS_02_INT_Ticket update";
                        Comment_INT = "Ticket status changed to Awaiting customer information";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);

                        switch(Prioriy)
                        {

                            case "P0":
                                Email_CS = "Customer_Template_CS_02_Ticket update(request_for_information)";
                                Comment_CS = txt_AwaitInfor.Text;
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                                break;

                            case "P1":
                                Email_CS = "Customer_Template_CS_02_Request for information_P1";
                                Comment_CS = txt_AwaitInfor.Text;
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                                break;
                            case "P2":
                                Email_CS = "Customer_Template_CS_02_Request for information_P2";
                                Comment_CS = txt_AwaitInfor.Text;
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                                break;
                            case "P3":
                                Email_CS = "Customer_Template_CS_02_Request for information_P3";
                                Comment_CS = txt_AwaitInfor.Text;
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                                break;
                            case "P4":
                                Email_CS = "Customer_Template_CS_02_Request for information_P4";
                                Comment_CS = txt_AwaitInfor.Text;
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                                break;
                        }
                        



                        break;
                    case 38:
                        Email_CS = "";
                        Comment_CS = "";

                        Email_INT = "Internal_Template_CS_02_INT_Ticket update";
                        Comment_INT = "Ticket status changed to Awaiting Schedule";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;
                    case 39:
                        Email_CS = "Customer_Template_CS_03_Ticket resolved";
                        Comment_CS = "";

                        Email_INT = "Internal_Template_CS_03_INT_Ticket resolved";
                        Comment_INT = "Ticket status changed to Resolved without KB";

                        if (Session["Rank"].ToString() != "Admin")
                        {
                            if (Session["Role"].ToString() == "L2")
                            {
                                SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                            }
                            else
                            {
                                SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                                SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                            }
                        }
                        else
                        {
                            SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                            SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        }
                        break;
                    case 40:
                        Email_CS = "";
                        Comment_CS = "";

                        Email_INT = "Internal_Template_CS_02_INT_Ticket update";
                        Comment_INT = "Ticket status changed to In Progress by L2";

                        SendInternalNotification(TicketInformationID, Email_INT, Comment_INT, ToAddress);
                        SendExternalNotification(TicketInformationID, Email_CS, Comment_CS);
                        break;

                }
                Loader(TicketInformationID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_CreateJiraPage2_Click(object sender, EventArgs e)
        {
            try
            {
                ddl_JiraUsers.DataSource = DAL.Jira.JiraUsers.GetAssignableUserByApplicationKey(ddl_JiraApplication.SelectedValue);
                ddl_JiraUsers.DataTextField = "Name";
                ddl_JiraUsers.DataValueField = "Key";
                ddl_JiraUsers.DataBind();

                ddl_JiraTicketType.DataSource = DAL.Jira.JiraTicketType.GetAllTicketType(ddl_JiraApplication.SelectedValue);
                ddl_JiraTicketType.DataValueField = "ID";
                ddl_JiraTicketType.DataTextField = "Name";
                ddl_JiraTicketType.DataBind();

                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showModal();", true);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
        }
        protected void btn_CreateJiraSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string result = Jira_CreateTicket();
                logUserIP(CheckQueryString() + " Ticket a Jira ticket is being created ");


                if (result.Contains("key"))
                {
                    JObject jObject = JObject.Parse(result);
                    string JiraTicketID = jObject["key"].ToString();

                    JObject jObject2 = JObject.Parse(Jira_CheckTicketExistGetResult(JiraTicketID));
                    string JiraAssigneeKey = jObject2["fields"]["assignee"]["displayName"].ToString();
                    string JiraStatus = jObject2["fields"]["status"]["name"].ToString();

                    int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);

                    Jira_AddAttachments(JiraTicketID, TicketInformationID);

                    Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                    int subStatusId = DAL.Operations.OpStatuses.GetStatusbyNameandType("Sent to Development/L3", "Sub Incident Status").StatusesID;
                    Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                    {
                        //Comments = txt_ActionTaken.Text + " \n\n Ticket is assigned to L3 Team. Incident created in JIRA with JIRA Number: " + JiraTicketID,
                        Comments = " Ticket is assigned to L3 Team. Incident created in JIRA with JIRA Number: " + JiraTicketID,

                        IncidentStatusID = ticketHistoryMain.IncidentStatusID,
                        IncidentSubStatusID = subStatusId,
                        //IncidentSubStatusID = ticketHistoryMain.IncidentSubStatusID,
                        SeverityID = ticketHistoryMain.SeverityID,
                        PriorityID = ticketHistoryMain.PriorityID,
                        TicketInformationID = TicketInformationID,
                        JiraNumber = JiraTicketID,
                        SupportTypeID = ticketHistoryMain.SupportTypeID,

                        AssignedFromID = ticketHistoryMain.AssignedFromID,
                        AssignedTOID = ticketHistoryMain.AssignedTOID,
                        Activity = "ASSIGNED TO L3 - Jira Created",
                        CreatedBy = Session["Name"].ToString(),
                        CreationDate = DateTime.Now

                    };
                    int result2 = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);

                    Entities.TicketHistory ticketHistory_src = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                    string ToAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistory_src.AssignedTOID).Email;
                    SendInternalNotification(result2, "Internal_Template_CS_02_INT_Ticket update", "Jira created for the issue", ToAddress);

                    Entities.JiraTicket jiraTicket = new Entities.JiraTicket()
                    {
                        JiraTicketKey = JiraTicketID,
                        TicketInformationID = TicketInformationID,
                        Assignee = JiraAssigneeKey,
                        Status = JiraStatus,
                        CreatedBy = Session["Name"].ToString(),
                        CreationDate = DateTime.Now
                    };

                    int reul = DAL.Operations.OpJiraTicket.InsertRecord(jiraTicket);


                    if (result2 > 0)
                    {
                        var list = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TicketInformationID);

                        List<string> commentlist = list.Where(x => x.Comments != null).Select(x => x.Comments).ToList();
                        foreach (string comment in commentlist)
                        {
                            if (comment.Length > 0)
                            {
                                Jira_AddComments(comment, JiraTicketID);

                                Entities.JiraTicketComments jiraTicketComments = new Entities.JiraTicketComments()
                                {
                                    TicketInformationID = TicketInformationID,
                                    JiraTicketKey = JiraTicketID,
                                    Comments = comment,

                                    CreatedBy = Session["Name"].ToString(),
                                    CreationDate = DateTime.Now
                                };
                                int result23 = DAL.Operations.OpJiraTicketComments.InsertRecord(jiraTicketComments);
                            }
                        }

                        Loader(TicketInformationID);
                        lbl_message.Text = "Jira created successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + result;
                    }
                }
                else
                {
                    lbl_message.Text = result;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_SendtoInfra_Click(object sender, EventArgs e)
        {
            try
            {


                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                logUserIP(TicketInformationID + " Ticket being sent to INFRA Team");

                string InfraEmail = System.Configuration.ConfigurationManager.AppSettings.Get("InfraEmail");

                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                Entities.CallerInformation callerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(ticketInformation.CallerKeyID);

                //Add to assigne and cc group
                CreateInternalNotification(TicketInformationID, "Ticket has been sent to InfraStructure Team", InfraEmail, "Internal_Template_CS_02_INT_Ticket update");
                CreateTicketHistory("Ticket has been sent to InfraStructure Team");

                Loader(TicketInformationID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void CreateInternalNotification(int TicketInformationID, string _comments, string ToEmailAddress,string EmailCategory)
        {
            try
            {


                //string EmailCategory = "Internal_Template_CS_02_INT_Ticket update";
                string comments = _comments;

                string To_Address = ToEmailAddress;
                string CC_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email;
                CC_Address = CC_Address + ";" + System.Configuration.ConfigurationManager.AppSettings.Get("SupportEmail");

                Entities.Notification _notification1 = DAL.Operations.OpNotification.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void SendInternalNotification(int TicketInformationId,string Category,string Comments,string ToEmailAddress)
        {
            try
            {
                if (Category.Length > 1)
                {
                    Entities.Notification _notificationCS = WebService.SLAProcessing.GenerateNotification(TicketInformationId, false, Category, Comments, "EMAIL", false, false, ToEmailAddress);
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void SendExternalNotification(int TicketInformationId,string Category, string Comments)
        {
            try
            {
                if (Category.Length > 1)
                {
                    Entities.Notification _notificationCS = WebService.SLAProcessing.GenerateNotification(TicketInformationId, false, Category, Comments, "EMAIL", true, false);
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private int CreateTicketHistory(string comments)
        {
            int result = -1;
            string JiraTicketID = string.Empty;
            try
            {
                int subStatusId = DAL.Operations.OpStatuses.GetStatusbyNameandType("Sent to InfraStructure", "Sub Incident Status").StatusesID;
                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                {
                    Comments = comments,
                    //IncidentStatusID = ,
                    IncidentStatusID = ticketHistoryMain.IncidentStatusID,
                    //IncidentSubStatusID = ticketHistoryMain.IncidentSubStatusID,
                    IncidentSubStatusID = subStatusId,
                    SeverityID = ticketHistoryMain.SeverityID,
                    PriorityID = ticketHistoryMain.PriorityID,
                    TicketInformationID = ticketHistoryMain.TicketInformationID,
                    JiraNumber = ticketHistoryMain.JiraNumber,
                    AssignedFromID = ticketHistoryMain.AssignedFromID,
                    AssignedTOID = ticketHistoryMain.AssignedTOID,
                    Activity = "ASSIGNED TO INFRASTRUCTURE",
                    CreatedBy = Session["Name"].ToString(),
                    SupportTypeID = ticketHistoryMain.SupportTypeID,
                    // IncidentSubStatusID_FK = Session["Name"].ToString(),
                    CreationDate = DateTime.Now

                };

                result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        #endregion

        #region Reports
        protected void link_word_report_Click(object sender, EventArgs e)
        {
            //CreateWordDocument(false,false);
            HTMLtoWORD();


        }
        protected void link_pdf_Click(object sender, EventArgs e)
        {
            //CreateWordDocument(true,false);
            HTMLtoWORD();
        }
        protected void link_Business_Form_Click(object sender, EventArgs e)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showBusinessModal();", true);
        }
        protected void btn_BusinessForm_Click(object sender, EventArgs e)
        {
            try
            {
                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                logUserIP(TicketInformationID + " Ticket Business report being generated");


                //update ticket information with 3 values
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                ticketInformation.ResolutionSummary = txt_RSummary.Text;
                ticketInformation.IssueDescription = txt_issueDesc.Text;
                ticketInformation.ResolutionActions = txt_RActionTaken.Text;
                ticketInformation.UpdateDate = DateTime.Now;
                ticketInformation.UpdatedBy = Session["Name"].ToString();
                int TI_result = DAL.Operations.OpTicketInformation.UpdateRecord(ticketInformation, TicketInformationID);
                DAL.Operations.Logger.Info("Business reporte ticket information creation result: " + TI_result);

                //create ticket history with comment and activity
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                ticketHistory.Activity = "Business Update";
                ticketHistory.Comments = "Incident Report Generated by " + Session["Name"].ToString() + "\n";
                ticketHistory.Comments += " Resolution Summary: " + txt_RSummary.Text + "\n";
                ticketHistory.Comments += " Issue Description: " + txt_issueDesc.Text + "\n";
                ticketHistory.Comments += " Resolution Actions: " + txt_RActionTaken.Text + "\n";

                ticketHistory.CreationDate = DateTime.Now;
                ticketHistory.CreatedBy = Session["Name"].ToString();
                int TH_result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
                DAL.Operations.Logger.Info("Business reporte ticket history creation result: " + TH_result);

                //CreateWordDocument(true, true, txt_RSummary.Text, txt_issueDesc.Text, txt_RActionTaken.Text);
                //CreateWordDocument(true, true);
                HTMLtoWORD();



            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void HTMLtoWORD()
        {
            string templatecategory = string.Empty;
            string comments = string.Empty;
            string notificationtype = string.Empty;
            string filename = string.Empty;

            try
            {
                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                filename = ticketInformation.TicketNumber + "_" + DateTime.Now.ToString("ddMMyyyyHHmmsstt") + ".pdf";
                int ticketstatus = ticketHistory.IncidentStatusID;

                if (ticketstatus == 12 || ticketstatus == 14 || ticketstatus == 39)
                {
                    templatecategory = "Incident Report Template(resolved issue)";
                }
                else if (ticketstatus == 10 || ticketstatus == 11 || ticketstatus == 13 || ticketstatus == 37 || ticketstatus == 38 || ticketstatus == 40)
                {
                    templatecategory = "Incident Report Template(unresolved issue)";
                }

                Entities.Email email = DAL.Operations.OpEmail.GetEmailByCategory(templatecategory).First();
                string NotificationType = email.TemplateType;
                string HTML = WebService.SLAProcessing.ParseTemplate(TicketInformationID, true, templatecategory, comments, NotificationType);
                var result = PdfSharpConvert(HTML);

                string AppDataDir = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data");
                string FileDir = AppDataDir + "\\" + filename;
                System.IO.File.WriteAllBytes(FileDir, result);
                download_string(FileDir);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        public static byte[] PdfSharpConvert(string html)
        {
            byte[] res = null;

            try
            {
                res = OpenHtmlToPdf.Pdf.From(html).Content();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return res;
        }
        #endregion

        #region Jira Controls
        private string Jira_CreateTicket()
        {
            string result = string.Empty;
            string data = string.Empty;

            try
            {
                int TicketInformationID = int.Parse(Request.QueryString["TicketInformationID"]);
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);

                string SD_TicketKey = ticketInformation.TicketNumber;

                string ProjectKey = ddl_JiraApplication.SelectedValue;
                string IssueType = ddl_JiraTicketType.SelectedItem.Text;
                string Priority = ddl_JiraPriority.SelectedItem.Text;
                string Assignee = ddl_JiraUsers.SelectedValue;

                string status = ddl_JiraTicketStatus.SelectedItem.Text;
                string Summary = h3_Summary.InnerText;

                //string Desc = Regex.Replace(txt_Description.InnerText, @"[^0-9 a-zA-Z:,]+", "");
                string Desc = txt_Description.InnerText;
                string ActionTaken = txt_ActionTaken.Text;
                Desc = Desc + " \\n\\n ActionTaken: " + ActionTaken;


                if (Assignee != "" && Assignee.Length > 0)
                {
                }
                else
                {
                    Assignee = "-1";
                }

                Desc = Jira_EscapeCharacters(Desc,false);
                Summary = Jira_EscapeCharacters(Summary,true);

                data = "{\"fields\":{ " +
                                   "\"project\":{\"key\":\"" + ProjectKey + "\"}, " +
                                   "\"issuetype\":{\"name\":\"" + IssueType + "\"}, " +
                                   "\"priority\":{\"name\":\"" + Priority + "\"}, " +
                                   "\"assignee\":{\"name\":\"" + Assignee + "\"}, " +
                                   //"\"status\":{\"name\":\"" + status + "\"}, " +

                                   "\"labels\":[\"CCIS\",\"CCISTest\"], " +
                                   "\"description\":\"" + Desc + "\", " +
                                   "\"summary\":\"" + SD_TicketKey + " " + Summary + "\" " +
                                   "}}";



                result = DAL.Jira.Rest_API.API("issue", data, "POST");

                JObject yo = JObject.Parse(result);

                if(yo["errors"] != null)
                {

                }

                if(yo["errorMessages"] != null && yo["errors"] == null)
                {
                    Desc = Regex.Replace(txt_Description.InnerText +" \\n ActionTaken: "+ txt_ActionTaken.Text, @"[^0-9 a-zA-Z-:,]+", "");
                    data = "{\"fields\":{ " +
                                   "\"project\":{\"key\":\"" + ProjectKey + "\"}, " +
                                   "\"issuetype\":{\"name\":\"" + IssueType + "\"}, " +
                                   "\"priority\":{\"name\":\"" + Priority + "\"}, " +
                                   "\"assignee\":{\"name\":\"" + Assignee + "\"}, " +
                                   //"\"status\":{\"name\":\"" + status + "\"}, " +

                                   "\"labels\":[\"CCIS\",\"CCISTest\"], " +
                                   "\"description\":\"" + Desc + "\", " +
                                   "\"summary\":\"" + Summary + "\" " +
                                   "}}";



                    result = DAL.Jira.Rest_API.API("issue", data, "POST");

                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        private void Jira_AddComments(string Comment, string TicketKey)
        {
            try
            {
                //Comment = Jira_EscapeCharacters(Comment, true);
                string data = "{\"body\":\"" + Comment + "\"}";
                string result = DAL.Jira.Rest_API.API("issue/" + TicketKey + "/comment", data, "POST");

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private bool Jira_UpdateAssignee(string Assignee, string TicketKey)
        {
            bool result = false;
            try
            {
                string data = "{\"name\":\"" + Assignee + "\"}";
                string tresult = DAL.Jira.Rest_API.API("issue/" + TicketKey + "/assignee", data, "PUT");

                if (tresult != "")
                {
                    JObject result2 = JObject.Parse(tresult);
                    if (result2["errorMessages"] != null)
                    {
                        lbl_message.Text = tresult;
                    }
                }
                else if (tresult == "")
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;

        }
        private void Jira_AddAttachments(string TicketKey, int TicketinformationId)
        {
            try
            {
                //DAL.Jira.JiraSynch jiraSynch = new DAL.Jira.JiraSynch();
                //jiraSynch.UploadAttachments(TicketKey, TicketinformationId, HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"));
                DAL.Jira.JiraAttachments jiraSynch = new DAL.Jira.JiraAttachments();
                jiraSynch.UploadAttachments(TicketKey, TicketinformationId, HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"));
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void Jira_UpdateTicketStatus(string status, string TicketKey)
        {
            try
            {
                string data = "{\"status\":\"" + status + "\"}";
                string result = DAL.Jira.Rest_API.API("issue/" + TicketKey + "/assignee", data, "PUT");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void Jira_AddResolution(string data, string TicketKey)
        {
            try
            {

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private bool Jira_CheckTicketExist(string TicketKey)
        {
            bool exist = false;

            try
            {
                string result = DAL.Jira.Rest_API.API("issue/" + TicketKey, "", "GET");
                JObject result2 = JObject.Parse(result);
                if (result2["key"].ToString() == TicketKey)
                {
                    exist = true;
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return exist;
        }
        private string Jira_CheckTicketExistGetResult(string TicketKey)
        {
            string result = string.Empty;
            try
            {
                result = DAL.Jira.Rest_API.API("issue/" + TicketKey, "", "GET");
                JObject result2 = JObject.Parse(result);
                if (result2["key"].ToString() == TicketKey)
                {
                    result = result2.ToString();
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        private string Jira_EscapeCharacters(string Desc,bool toRemove)
        {
            try
            {

                if (!toRemove)
                {
                    Desc = Desc.Replace("\"", "\\\" "); //double quot
                    Desc = Desc.Replace("\r", "\\r "); // tab
                    Desc = Desc.Replace("\n", "\\n "); // newline
                    Desc = Desc.Replace("\b", "\\b "); // bold
                    Desc = Desc.Replace("\f", "\\f "); // form feed
                    Desc = Desc.Replace("\t", "\\t "); // Tab
                    Desc = Desc.Replace("\v", "\\v "); // vertical Tab
                    Desc = Desc.Replace("\'", "\\' "); // Apostrophie
                    Desc = Desc.Replace("\\", "\\"); // Backslash
                    Desc = Desc.Replace("•", " ");
                }
                else if (toRemove)
                {
                    Desc = Desc.Replace("\"", " "); //double quot
                    Desc = Desc.Replace("\r", " "); // tab
                    Desc = Desc.Replace("\n", " "); // newline
                    Desc = Desc.Replace("\b", " "); // bold
                    Desc = Desc.Replace("\f", " "); // form feed
                    Desc = Desc.Replace("\t", " "); // Tab
                    Desc = Desc.Replace("\v", " "); // vertical Tab
                    Desc = Desc.Replace("\'", " "); // Apostrophie
                    Desc = Desc.Replace("\\", " "); // Backslash
                    Desc = Desc.Replace("•", " ");
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("This comment was being escaped " + Desc);
                DAL.Operations.Logger.LogError(ex);
            }

            return Desc;
        }
        #endregion

        public void logUserIP(string activity)
        {
            try
            {
                string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string result = string.Empty;

                if (!string.IsNullOrEmpty(ipList))
                {
                    result = ipList.Split(',')[0];
                }
                else
                {
                    result = Request.ServerVariables["REMOTE_ADDR"];
                }

                DAL.Operations.Logger.Info("Session logged in."+ activity + ". PersonID " + Session["PersonId"] +
                   " Name " + Session["Name"] +
                   " Rank " + Session["Rank"] +
                   " Role " + Session["Role"] +
                   " LoginTime " + Session["LoginTime"] +
                   " IP " + result);

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

    }
}