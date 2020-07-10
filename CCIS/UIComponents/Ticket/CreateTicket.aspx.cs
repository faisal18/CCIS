using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets
{
    public partial class Tickets : System.Web.UI.Page
    {



        #region Loaders
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session.Keys.Count > 0)
                {
                    //SessionName = Session["Name"].ToString();
                    //SessionPersonId = int.Parse(Session["PersonId"].ToString());
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if (!IsPostBack)
                {

                    LoadData(CheckQueryString());
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private int CheckQueryString()
        {
            int id = 0;
            try
            {
                if (Request.QueryString.HasKeys())
                {
                    id = int.Parse(Request.QueryString["InquiryID"]);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return id;
        }

        private void LoadData(int inquiryID)
        {

            try
            {
                //DropdDownList Severity/Priority and Severity from ItemTypes
                DataTable DT_SeverityPriority = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                ddl_Severity.DataSource = DT_SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                ddl_Severity.DataTextField = "Description";
                ddl_Severity.DataValueField = "ItemTypesID";
                ddl_Severity.DataBind();

                ddl_priority.DataSource = DT_SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                ddl_priority.DataTextField = "Description";
                ddl_priority.DataValueField = "ItemTypesID";
                ddl_priority.DataBind();


                //  DataTable DT_SeverityPriority = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                ddl_Internal_Severity.DataSource = DT_SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                ddl_Internal_Severity.DataTextField = "Description";
                ddl_Internal_Severity.DataValueField = "ItemTypesID";
                ddl_Internal_Severity.DataBind();

                ddl_Internal_priority.DataSource = DT_SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                ddl_Internal_priority.DataTextField = "Description";
                ddl_Internal_priority.DataValueField = "ItemTypesID";
                ddl_Internal_priority.DataBind();

                ddl_SupportType.DataSource = DT_SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("SupportType")).CopyToDataTable();
                ddl_SupportType.DataTextField = "Description";
                ddl_SupportType.DataValueField = "ItemTypesID";
                ddl_SupportType.DataBind();

                //DropdDownList Assignee from PersonInformation
                ddl_Assignee.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                ddl_Assignee.DataTextField = "FullName";
                ddl_Assignee.DataValueField = "PersonInformationID";
                ddl_Assignee.DataBind();
                ddl_Assignee.SelectedIndex = ddl_Assignee.Items.IndexOf(ddl_Assignee.Items.FindByValue(System.Configuration.ConfigurationManager.AppSettings.Get("L2QueueUserAssignee")));

                //DropDownlist Status / SubStatuses / TicketType from Status Entity
                DataTable DT_Status = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];

                ddl_Status.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Incident Status")).CopyToDataTable();
                ddl_Status.DataTextField = "Description";
                ddl_Status.DataValueField = "StatusesID";
                ddl_Status.DataBind();

                if (ddl_Status.Items.FindByText("New") != null)
                {
                    ddl_Status.SelectedIndex = ddl_Status.Items.IndexOf(ddl_Status.Items.FindByText("New"));

                }

                ddl_SubStatus.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Sub Incident Status")).CopyToDataTable();
                ddl_SubStatus.DataTextField = "Description";
                ddl_SubStatus.DataValueField = "StatusesID";
                ddl_SubStatus.DataBind();

                if (ddl_SubStatus.Items.FindByText("Sent to TechSupport/L2") != null)
                {
                    ddl_SubStatus.SelectedIndex = ddl_SubStatus.Items.IndexOf(ddl_SubStatus.Items.FindByText("Sent to TechSupport/L2"));

                }

                ddl_tickettype.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Ticket Type")).CopyToDataTable();
                ddl_tickettype.DataTextField = "Description";
                ddl_tickettype.DataValueField = "StatusesID";
                ddl_tickettype.DataBind();

                if (ddl_tickettype.Items.FindByText("Incident") != null)
                {
                    ddl_tickettype.SelectedIndex = ddl_tickettype.Items.IndexOf(ddl_tickettype.Items.FindByText("Incident"));
                }


                ddl_LinkRelation.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Ticket Relation")).CopyToDataTable();
                ddl_LinkRelation.DataTextField = "Description";
                ddl_LinkRelation.DataValueField = "StatusesID";
                ddl_LinkRelation.DataBind();


                //TextBox Description from InquriyDetails
                Entities.InquiryDetails inquiryDetails = DAL.Operations.OpInquiryDetails.GetRecordbyID(inquiryID);
                txt_rich_description.InnerText = inquiryDetails.Description;

                //Label ApplicationName from InquiryID joined with Application
                lbl_Project.Text = DAL.Operations.OpApplications.GetRecordbyID(inquiryDetails.ApplicationID).Name;

                //TextBox ActionTaken from InquiryDetails
                txt_rich_actiontake.InnerText = inquiryDetails.ActionTaken;


                //Dropdown list for link ticket

                var result = DAL.Operations.OpTicketInformation.GetAll().Where(x => x.ApplicationID == inquiryDetails.ApplicationID).ToList();
                var result2 = from x in result.AsEnumerable()
                              select new
                              {
                                  x.TicketInformationID,
                                  x.TicketNumber,
                                  x.Subject,
                                  DisplayField = String.Format("{0} - {1}", x.TicketNumber, x.Subject)
                              };

                ddl_LinkTicket.DataSource = result2;
                ddl_LinkTicket.DataTextField = "DisplayField";
                ddl_LinkTicket.DataValueField = "TicketInformationID";
                ddl_LinkTicket.DataBind();

                //Dropdownlist for Summary(show previously created tickets summary ordered by recent and distinct)

                var summaries = DAL.Operations.OpTicketInformation.GetTicketInformationbyApplicationID(inquiryDetails.ApplicationID).Where(x => x.Subject != null).OrderBy(x => x.CreationDate).Select(x => x.Subject).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                ddl_Summary.DataSource = summaries;
                ddl_Summary.DataBind();


            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private static byte[] ConvetStreamToByte(Stream input)
        {
            byte[] buffer = new byte[input.Length];
            //byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        #endregion

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                logUserIP();

                var summary = txt_hidden_summary.Text;

                if (summary.Length > 0 && txt_rich_description.InnerText.Length > 0 && txt_rich_actiontake.InnerText.Length > 0)
                {
                    Entities.InquiryDetails inquiry = DAL.Operations.OpInquiryDetails.GetRecordbyID(CheckQueryString());
                    int CallerID = inquiry.CallerKeyID;

                    int TicketInformationID = CreateTicket();
                    if (TicketInformationID > 0)
                    {
                        
                        int TicketHistoryID = CreateTicketHistory(TicketInformationID);

                        CreateTicketRelation(TicketInformationID);
                        CreateAttachments(TicketInformationID, TicketHistoryID);
                        CreateNotification(TicketInformationID, CallerID);
                        CreateClientNotification(TicketInformationID, CallerID);
                        CreateProblem(TicketInformationID, CallerID);
                        CreateGuidComment(TicketInformationID);

                        Response.Redirect("ViewTicket.aspx?TicketInformationID=" + TicketInformationID, false);
                    }

                    txt_rich_description.InnerText = "";
                    txt_rich_actiontake.InnerText = "";

                }
                else
                {
                    lbl_message.Text = "Missing data in mandatory fields";
                }

            }
            catch (Exception ex)
            {
                txt_rich_description.InnerText = "";
                txt_rich_actiontake.InnerText = "";
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }

        private int CreateTicket()
        {
            int result = -1;
            try
            {
                Entities.InquiryDetails inquiry = DAL.Operations.OpInquiryDetails.GetRecordbyID(CheckQueryString());
                int CallerID = inquiry.CallerKeyID;
                int InquiryDetailsID = inquiry.InquiryDetailsID;
                string Application = lbl_Project.Text;
                string TicketNumber = Application.Substring(0, 3).ToUpper() + "-" + InquiryDetailsID.ToString("0000");
                int applicationid = inquiry.ApplicationID;
                string Summary = txt_hidden_summary.Text;
                string Description = txt_rich_description.InnerText;
                string ActionTaken = txt_rich_actiontake.InnerText;
                int Priority = int.Parse(ddl_priority.SelectedValue.ToString());
                int Severity = int.Parse(ddl_Severity.SelectedValue.ToString());
                string callersource = inquiry.CallerSource.ToString();
                string justification = txt_Justification.Text;


                DateTime DueDate = DateTime.Now;

                if (txt_DueDate.Text.Length > 0)
                {
                    DateTime yo = new DateTime();
                    if (DateTime.TryParse(txt_DueDate.Text, out yo))
                    {
                        DueDate = Convert.ToDateTime(txt_DueDate.Text);
                    }
                }
                string CPlan = txt_ContigencyPlan.Text;
                int TicketType = int.Parse(ddl_tickettype.SelectedItem.Value);


                //string AssigneeName = ddl_Assignee.SelectedItem.Text;
                //string Caller_Email = DAL.Operations.OpCallerInfo.GetRecordbyID(CallerID).Email;


                var ifexist = DAL.Operations.OpTicketInformation.GetAll().Where(x => x.TicketNumber == TicketNumber);

                if (ifexist.Count() == 0)
                {

                    Entities.TicketInformation ticketInformation = new Entities.TicketInformation
                    {
                        CallerKeyID = CallerID,
                        TicketNumber = TicketNumber,
                        ApplicationID = applicationid,
                        Subject = Summary,
                        Description = Description,
                        ActionTaken = ActionTaken,

                        PriorityID = Priority,
                        SeverityID = Severity,

                        DueDate = DueDate,
                        ContingencyPlan = CPlan,

                        ProblemJustification = justification,

                        TicketType = TicketType,
                        TicketGUIDKey = Guid.NewGuid(),
                        ReporterID = int.Parse(Session["PersonId"].ToString()),
                        CallerSource = callersource,
                        CreatedBy = Session["Name"].ToString(),
                        CreationDate = DateTime.Now
                    };

                    result = DAL.Operations.OpTicketInformation.InsertRecord(ticketInformation);
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        private int CreateTicketHistory(int TicketInformationID)
        {
            int Assignee = int.Parse(ddl_Assignee.SelectedValue.ToString());
            int Priority = int.Parse(ddl_Internal_priority.SelectedValue.ToString());
            int Severity = int.Parse(ddl_Internal_Severity.SelectedValue.ToString());
            int SupportType = int.Parse(ddl_SupportType.SelectedValue.ToString());
            int result = -1;
            string activity = "Create Ticket-UserAssigned";
            try
            {



                if (Assignee == int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("L2QueueUserAssignee")))
                {
                    activity = "ASSIGNED TO L2";
                }

                Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                {
                    TicketInformationID = TicketInformationID,
                    AssignedFromID = int.Parse(Session["PersonId"].ToString()),
                    AssignedTOID = Assignee,
                    //Comments = "New ticket created",
                    IncidentStatusID = int.Parse(ddl_Status.SelectedValue),
                    IncidentSubStatusID = int.Parse(ddl_SubStatus.SelectedValue),
                    //JiraNumber = txt_JiraNumbers.Text,
                    //IncidentSubStatusID = int.Parse(ddl_SubStatus.SelectedValue),
                    Activity = activity,
                    PriorityID = Priority,
                    SeverityID = Severity,
                    SupportTypeID = SupportType,

                    CreatedBy = Session["Name"].ToString(),
                    CreationDate = DateTime.Now
                };

                result = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }

        private int CreateGuidComment(int TicketInformationID)
        {
            int result2 = -1;
            try
            {
                string guid = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID).TicketGUIDKey.ToString();
                Entities.TicketHistory ticketHistoryMain2 = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                string url = Request.Url.GetLeftPart(UriPartial.Path);
                url = url.Replace("CreateTicket", "ViewTicket");
                url = url + "?TicketGKey=" + guid;
                ticketHistoryMain2.Comments = "Visit this URL to view the ticket on IQServiceDesk <br> " + url;
                ticketHistoryMain2.CreatedBy = Session["Name"].ToString();
                ticketHistoryMain2.CreationDate = DateTime.Now;
                result2 = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistoryMain2);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result2;
        }

        private void CreateTicketRelation(int TicketInformationID)
        {

            try
            {
                int TicketRelation = int.Parse(ddl_LinkRelation.SelectedValue);
                int TicketRelationToID = int.Parse(ddl_LinkTicket.SelectedValue);

                if (TicketRelation != 0 && TicketRelationToID != 0)
                {
                    Entities.TicketRelation ticketRelation = new Entities.TicketRelation
                    {
                        TR_TI_ID = TicketInformationID,
                        TR_RelationTypeID = TicketRelation,
                        TR_TI_ToID = TicketRelationToID,

                        CreatedBy = Session["Name"].ToString(),
                        CreationDate = DateTime.Now
                    };
                    DAL.Helper.Log4Net.Info(DAL.Operations.OpTicketRelation.InsertRecord(ticketRelation));
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void CreateAttachments(int TicketInformationID, int TicketHistoryID)
        {
            try
            {
                if (file_Attachment.HasFile || file_Attachment.HasFiles)
                {
                    foreach (HttpPostedFile file in file_Attachment.PostedFiles)
                    {
                        Entities.TicketAttachment ticketAttachment = new Entities.TicketAttachment
                        {
                            filename = file.FileName,
                            Attachment = ConvetStreamToByte(file.InputStream),
                            TicketHistoryID = TicketHistoryID,

                            TicketInformationID = TicketInformationID,

                            CreatedBy = Session["Name"].ToString(),
                            CreationDate = DateTime.Now
                        };

                        int AttachmentID = DAL.Operations.OpTicketAttachment.InsertRecord(ticketAttachment);
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        private void CreateNotification(int TicketInformationID, int CallerID, [Optional] string To_email)
        {
            try
            {
                int Notification_pending_statusid = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Pending")).AsEnumerable().Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());
                int EmailTemplateID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Email_TemplateID"));
                int Assignee = int.Parse(ddl_Assignee.SelectedValue.ToString());
                if (To_email != null)
                {
                    if (To_email.Length > 0)
                    {

                    }
                }
                else
                {
                    To_email = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(Assignee).Email;

                }

                string EmailCategory = "Internal_Template_CS_01_INT_Ticket assigned to a group";
                string comments = Session["Name"].ToString() + " has created ticket ";

                string To_Address = To_email;
                string CC_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email;

                Entities.Notification _notification1 = WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
        private void CreateClientNotification(int TicketInformationID, int CallerID)
        {
            try
            {
                string comments = Session["Name"].ToString() + " has created ticket ";
                string EmailCategory = EmailCategory = "Customer_Template_CS_01_Ticket created";
                Entities.Notification _notification1 = WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", true, false);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }

        private void CreateProblem(int TicketInformationID, int CallerID)
        {
            try
            {
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                int problemtype = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeProblem"));
                if (ticketInformation.TicketType == problemtype)
                {
                    string role = Session["Role"].ToString();


                    if (role.ToUpper() == "L1")
                    {
                        int ManagerRoleID = DAL.Operations.OpRoles.GetAll().Where(x => x.Description == "L1 Manager").ToList().First().RolesID;
                        int ManagerID = DAL.Operations.OpUserRoles.GetAll().Where(x => x.RoleID == ManagerRoleID).ToList().Select(x => x.PersonID).First();
                        string ManagerEmail = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(ManagerID).First().Email;

                        CreateNotification(TicketInformationID, CallerID, ManagerEmail);
                    }
                    else if (role.ToUpper() == "L2")
                    {
                        int ManagerRoleID = DAL.Operations.OpRoles.GetAll().Where(x => x.Description == "L2 Manager").ToList().First().RolesID;
                        int ManagerID = DAL.Operations.OpUserRoles.GetAll().Where(x => x.RoleID == ManagerRoleID).ToList().Select(x => x.PersonID).First();
                        string ManagerEmail = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(ManagerID).First().Email;

                        CreateNotification(TicketInformationID, CallerID, ManagerEmail);
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void btn_modal_problem_Click(object sender, EventArgs e)
        {
            //System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showProblemModal();", true);
        }

        public void logUserIP()
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

                DAL.Operations.Logger.Info("Session logged in.Create Ticket. PersonID " + Session["PersonId"] +
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