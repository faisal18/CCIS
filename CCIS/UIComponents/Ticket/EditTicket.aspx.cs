using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets
{
    public partial class EditTicket : System.Web.UI.Page
    {
    
        #region Loader
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
                    LoadControl();
                }
                else
                {
                    LoadAttachment(CheckQueryString());
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private int CheckQueryString()
        {
            int result = 0;
            try
            {
                if (Request.QueryString.HasKeys())
                {
                    result = int.Parse(Request.QueryString["TicketInformationID"]);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        
        private void LoadControl()
        {
            try
            {

                int TicketInformationID = CheckQueryString();
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                Entities.TicketHistory ticketHistoryLatest = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);

                txt_rich_description.InnerText = ticketInformation.Description;
                txt_hdn_ActionTaken.Text = ticketInformation.ActionTaken;
                txt_hidden_summary.Text = ticketInformation.Subject;
                txt_JiraNumbers.Text = ticketHistoryLatest.JiraNumber;
                txt_ContigencyPlan.Text = ticketInformation.ContingencyPlan;
                txt_DueDate.Text = ticketInformation.DueDate.ToString();

                LoadDropdowns(ticketInformation, ticketHistoryLatest, TicketInformationID);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadDropdowns(Entities.TicketInformation ticketInformation,Entities.TicketHistory ticketHistory,int TicketInformationID)
        {
            try
            {
                LoadProjects(ticketInformation);
                LoadSummary(ticketInformation);
                LoadActionTaken(ticketInformation);
                LoadPerson(ticketHistory);

                DataTable dt_SevPev = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                LoadSeverity(ticketInformation, ticketHistory, dt_SevPev);
                LoadPriority(ticketInformation, ticketHistory, dt_SevPev);

                DataTable dt_Statuses = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                LoadStatus(ticketHistory, dt_Statuses);
                LoadSubStaus(ticketHistory, dt_Statuses);

                LoadTicketType(ticketInformation, dt_Statuses);

                LoadAttachment(CheckQueryString());
                LoadSupportType(ticketHistory, dt_SevPev);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        private void LoadProjects(Entities.TicketInformation ticketInformation)
        {
            try
            {
                ddl_Project_List.DataSource = DAL.Operations.OpApplications.GetAll();
                ddl_Project_List.DataTextField = "Name";
                ddl_Project_List.DataValueField = "ApplicationsID";
                ddl_Project_List.DataBind();
                ddl_Project_List.Items.FindByValue(ticketInformation.ApplicationID.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadSummary(Entities.TicketInformation ticketInformation)
        {
            try
            {
                var list = DAL.Operations.OpTicketInformation.GetTicketInformationbyApplicationID(ticketInformation.ApplicationID).Where(x => x.Subject != null).OrderBy(x => x.CreationDate).Select(x => x.Subject).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                ddl_Summary.DataSource = DAL.Helper.ListToDataset.ToDataSet(list).Tables[0];
                ddl_Summary.DataTextField = "AllNames";
                ddl_Summary.DataValueField = "AllNames";
                ddl_Summary.DataBind();
                ddl_Summary.Items.FindByValue(ticketInformation.Subject.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadActionTaken(Entities.TicketInformation ticketInformation)
        {
            try
            {
                //var list = DAL.Operations.OpInquiryDetails.GetInquiryDetailsbyApplicationID(ticketInformation.ApplicationID).Where(x => x.ActionTaken != null && x.ActionTaken.Length > 1).OrderBy(x => x.CreationDate).Select(x => x.ActionTaken).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                var list = DAL.Operations.OpTicketInformation.GetTicketInformationbyApplicationID(ticketInformation.ApplicationID).Where(x => x.ActionTaken != null && x.ActionTaken.Length > 1).OrderBy(x => x.CreationDate).Select(x => x.ActionTaken).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();

                ddl_ActionTaken.DataSource = DAL.Helper.ListToDataset.ToDataSet(list).Tables[0];
                ddl_ActionTaken.DataTextField = "AllNames";
                ddl_ActionTaken.DataValueField = "AllNames";
                ddl_ActionTaken.DataBind();
                ddl_ActionTaken.Items.FindByValue(ticketInformation.ActionTaken.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadPerson(Entities.TicketHistory ticketHistory)
        {
            try
            {
                ddl_Assignee.DataSource = DAL.Operations.OpPersonInformation.GetAll();
                ddl_Assignee.DataTextField = "FullName";
                ddl_Assignee.DataValueField = "PersonInformationID";
                ddl_Assignee.DataBind();
                ddl_Assignee.Items.FindByValue(ticketHistory.AssignedTOID.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadSeverity(Entities.TicketInformation ticketInformation,Entities.TicketHistory ticketHistory,DataTable SeverityPriority)
        {
            try
            {
                ddl_Severity.DataSource = SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                ddl_Severity.DataTextField = "Description";
                ddl_Severity.DataValueField = "ItemTypesID";
                ddl_Severity.DataBind();
                ddl_Severity.Items.FindByValue(ticketHistory.SeverityID.ToString()).Selected = true;

                ddl_Client_Severity.DataSource = SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                ddl_Client_Severity.DataTextField = "Description";
                ddl_Client_Severity.DataValueField = "ItemTypesID";
                ddl_Client_Severity.DataBind();
                ddl_Client_Severity.Items.FindByValue(ticketInformation.SeverityID.ToString()).Selected = true;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

        }
        private void LoadPriority(Entities.TicketInformation ticketInformation, Entities.TicketHistory ticketHistory, DataTable SeverityPriority)
        {
            try
            {
                ddl_priority.DataSource = SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                ddl_priority.DataTextField = "Description";
                ddl_priority.DataValueField = "ItemTypesID";
                ddl_priority.DataBind();
                ddl_priority.Items.FindByValue(ticketHistory.PriorityID.ToString()).Selected = true;


                ddl_Client_priority.DataSource = SeverityPriority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                ddl_Client_priority.DataTextField = "Description";
                ddl_Client_priority.DataValueField = "ItemTypesID";
                ddl_Client_priority.DataBind();
                ddl_Client_priority.Items.FindByValue(ticketInformation.PriorityID.ToString()).Selected = true;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

        }
        private void LoadStatus(Entities.TicketHistory ticketHistory,DataTable Statuses)
        {
            try
            {
                ddl_Status.DataSource = Statuses.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Incident Status")).CopyToDataTable();
                ddl_Status.DataTextField = "Description";
                ddl_Status.DataValueField = "StatusesID";
                ddl_Status.DataBind();
                ddl_Status.Items.FindByValue(ticketHistory.IncidentStatusID.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadSubStaus(Entities.TicketHistory ticketHistory, DataTable Statuses)
        {
            try
            {
                ddl_SubStatus.DataSource = Statuses.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Sub Incident Status")).CopyToDataTable();
                ddl_SubStatus.DataTextField = "Description";
                ddl_SubStatus.DataValueField = "StatusesID";
                ddl_SubStatus.DataBind();
                ddl_SubStatus.Items.FindByValue(ticketHistory.IncidentSubStatusID.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadSupportType(Entities.TicketHistory ticketHistory, DataTable Items)
        {
            try
            {
                ddl_SupportType.DataSource = Items.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("SupportType")).CopyToDataTable();
                ddl_SupportType.DataTextField = "Description";
                ddl_SupportType.DataValueField = "ItemTypesID";
                ddl_SupportType.DataBind();
                ddl_SupportType.Items.FindByValue(ticketHistory.SupportTypeID.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadTicketType(Entities.TicketInformation ticketInformation, DataTable Statuses)
        {
            try
            {
                ddl_TicketType.DataSource = Statuses.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Ticket Type")).CopyToDataTable();
                ddl_TicketType.DataTextField = "Description";
                ddl_TicketType.DataValueField = "StatusesID";
                ddl_TicketType.DataBind();
                ddl_TicketType.Items.FindByValue(ticketInformation.TicketType.ToString()).Selected = true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadAttachment(int TicketInformationID)
        {
            try
            {
                DataTable dt_TicketAttachment = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketAttachment>(DAL.Operations.OpTicketAttachment.GetTicketAttachmentbyTicketID(TicketInformationID)).Tables[0];


                Place_Attachment.Dispose();
                foreach (DataRow row in dt_TicketAttachment.AsEnumerable())
                {
                    string filename = row["FileName"].ToString();
                    string AttachmentID = row["TicketAttachmentID"].ToString();

                    LinkButton LB_Attachment = new LinkButton();
                    LB_Attachment.Text = filename + "         ";
                    LB_Attachment.ID = AttachmentID;
                    LB_Attachment.CommandArgument = AttachmentID;
                    LB_Attachment.CommandName = AttachmentID;

                    LinkButton LB_Delete = new LinkButton();
                    LB_Delete.Text = "Remove File " + "<br/>";
                    LB_Delete.ID = AttachmentID + "-Delete";
                    LB_Delete.CommandArgument = AttachmentID;
                    LB_Delete.CommandName = "Delete File";

                    LB_Attachment.Click += LB_Attachment_Click;
                    LB_Delete.Click += LB_Delete_Click;
                    Place_Attachment.Controls.Add(LB_Attachment);
                    Place_Attachment.Controls.Add(LB_Delete);
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region Other Functionalities
        private void LB_Delete_Click(object sender, EventArgs e)
        {
            
            try
            {
                var link = sender as LinkButton;
                int FileId = int.Parse(link.CommandArgument);

                if (DAL.Operations.OpTicketAttachment.DeletebyID(FileId))
                {
                    lbl_message.Text = "File removed from ticket";
                    Place_Attachment.Dispose();
                    Place_Attachment.Controls.Clear();
                    LoadControl();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LB_Attachment_Click(object sender, EventArgs e)
        {
            try
            {
                //string filename = ticketAttachment.filename;
                //byte[] Byte_File = ticketAttachment.Attachment;
                //string B64_filecontent = Encoding.UTF8.GetString(Byte_File);

                //string AppDataDir = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data");
                //string FileDir = AppDataDir + "\\" + filename;
                //System.IO.File.WriteAllBytes(FileDir, Byte_File);
                //download_string(FileDir);

                var link = sender as LinkButton;
                var ticketAttachment = DAL.Operations.OpTicketAttachment.GetRecordbyID(int.Parse(link.ID));

                string filename = ticketAttachment.filename;
                string filecontent = Encoding.UTF8.GetString(ticketAttachment.Attachment);

                download_string(filename, filecontent);
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


                if (file_ext.ToUpper() == ".TXT" || file_ext.ToUpper() == ".XML")
                {
                    Response.AppendHeader("Content-Length", filecontent.Length.ToString());
                    Response.ContentType = "text/plain";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    Response.Write(filecontent);
                }
               
                else if (file_ext == ".zip" || file_ext == ".ZIP")
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
        private static byte[] ConvetStreamToByte(Stream input)
        {
            try
            {
                byte[] buffer = new byte[input.Length];
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
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        #endregion

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            try
            {
                logUserIP();

                string _TicketHistoryComment = "";
                int TicketInformationID = CheckQueryString();
                string txt_Summary = txt_hidden_summary.Text, description = txt_rich_description.InnerText, txt_rich_actiontake = txt_hdn_ActionTaken.Text;

                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID);
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);


                DateTime dueDate = new DateTime();
                if (txt_DueDate.Text.Length > 0) 
                {
                    dueDate = Convert.ToDateTime(txt_DueDate.Text);
                }
                else
                {
                    dueDate = DateTime.Now;
                }

                if (txt_rich_actiontake.Length > 0 && description.Length > 0 && txt_Summary.Length > 0)
                {
                    Entities.TicketInformation ent_ticketInformation = new Entities.TicketInformation
                    {
                        CallerKeyID = ticketInformation.CallerKeyID,
                        TicketNumber = ticketInformation.TicketNumber,

                        Subject = txt_Summary,
                        ApplicationID = int.Parse(ddl_Project_List.SelectedValue),
                        SeverityID = int.Parse(ddl_Client_Severity.SelectedValue),
                        PriorityID = int.Parse(ddl_Client_priority.SelectedValue),
                        TicketType = int.Parse(ddl_TicketType.SelectedValue),
                        DueDate = dueDate,
                        ContingencyPlan = txt_ContigencyPlan.Text,
                        ReporterID = ticketInformation.ReporterID,
                        
                        Description = description,
                        ActionTaken = txt_rich_actiontake,
                        UpdatedBy = Session["Name"].ToString(),
                        UpdateDate = DateTime.Now
                    };


                    Entities.TicketHistory ent_ticketHistory = new Entities.TicketHistory
                    {
                        TicketInformationID = TicketInformationID,
                        AssignedFromID = int.Parse(Session["PersonId"].ToString()),
                        JiraNumber = txt_JiraNumbers.Text,
                        IncidentStatusID = int.Parse(ddl_Status.SelectedValue),
                        IncidentSubStatusID = int.Parse(ddl_SubStatus.SelectedValue),
                        SeverityID = int.Parse(ddl_Severity.SelectedValue),
                        PriorityID = int.Parse(ddl_priority.SelectedValue),
                        AssignedTOID = int.Parse(ddl_Assignee.SelectedValue),
                        SupportTypeID = int.Parse(ddl_SupportType.SelectedValue),
                        
                        Activity = "UPDATE TICKET",
                
                        CreatedBy = Session["Name"].ToString(),
                        CreationDate = DateTime.Now
                    };
                  

                    if (file_Attachment.HasFile || file_Attachment.HasFiles)
                    {
                        foreach (HttpPostedFile file in file_Attachment.PostedFiles)
                        {
                            Entities.TicketAttachment ticketAttachment = new Entities.TicketAttachment
                            {
                                filename = file.FileName,
                                Attachment = ConvetStreamToByte(file.InputStream),
                                TicketHistoryID = ticketHistory.TicketHistoryID,
                                TicketInformationID = TicketInformationID,
                                UpdatedBy = Session["Name"].ToString(),
                                UpdateDate = DateTime.Now
                            };
                            int AttachmentID = DAL.Operations.OpTicketAttachment.InsertRecord(ticketAttachment);

                            if(txt_JiraNumbers.Text.Length>1)
                            {
                                string key = txt_JiraNumbers.Text;
                                if(Jira_CheckTicketExist(key))
                                {
                                    Jira_AddAttachments(key, TicketInformationID);
                                }
                            }


                        }
                    }

                    int problemtype = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeProblem"));
                    if (ticketInformation.TicketType == problemtype)
                    {
                        CreateProblem(TicketInformationID, ticketInformation.CallerKeyID);
                    }

                    _TicketHistoryComment += Environment.NewLine;  
                    _TicketHistoryComment +=   DAL.Helper.ObjectCompare.compareobj(DAL.Operations.OpTicketInformation.GetRecordbyID(TicketInformationID), ent_ticketInformation);
                    _TicketHistoryComment += Environment.NewLine;
                    _TicketHistoryComment += DAL.Helper.ObjectCompare.compareobj(DAL.Operations.OpTicketHistory.GetRecordbyID(ticketHistory.TicketHistoryID), ent_ticketHistory);
                    _TicketHistoryComment += Environment.NewLine;
                    ent_ticketHistory.ActivityComments = _TicketHistoryComment;


                    int result = DAL.Operations.OpTicketInformation.UpdateRecord(ent_ticketInformation, TicketInformationID);
                    int result2 = DAL.Operations.OpTicketHistory.InsertRecord(ent_ticketHistory);
                    string EmailCategory = "Internal_Template_CS_02_INT_Ticket update";
                    string comments = Session["Name"].ToString() + " has edited the ticket ";
                    string To_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(ddl_Assignee.SelectedValue)).Email;
                    string CC_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email;

                    Entities.Notification _notification1 = WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);
                    
                    if (result == 1 && result2 > 1)
                    {
                        Response.Redirect("ViewTicket.aspx?TicketInformationID=" + TicketInformationID, false);
                    }
                }
                else
                {
                    lbl_message.Text = "Please enter text in all text fields";
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void CreateProblem(int TicketInformationID, int CallerID)
        {
            try
            {
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                string role = Session["Role"].ToString();

                ticketHistory.Comments = txt_Justification.Text;
                ticketHistory.CreatedBy = Session["Name"].ToString();
                ticketHistory.CreationDate = DateTime.Now;

                DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);

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


                //EmailCategory = "Customer_Template_CS_01_Ticket created";
                //_notification1 = WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", true, false, To_Address, CC_Address);










                //Entities.Notification notification = new Entities.Notification
                //{
                //    Comments = Session["Name"].ToString() + " has created ticket ",
                //    TicketInformationID = TicketInformationID,
                //    SentByID = int.Parse(Session["PersonId"].ToString()),
                //    RecipientID = Assignee,

                //    ToAddress = To_email,
                //    CCAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email,

                //    EmailTemplateID = EmailTemplateID,
                //    CallerKeyID = CallerID,

                //    isExternal = false,

                //    NotificationTypeID = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Notification Type").FirstOrDefault().ItemTypesID,
                //    StatusID = Notification_pending_statusid,
                //    //StatusID = DAL.Operations.OpStatuses.GetStatusesbyTypeName("Notifications").FirstOrDefault().StatusesID,

                //    CreatedBy = Session["Name"].ToString(),
                //    CreationDate = DateTime.Now
                //};

                //int NotificationID = DAL.Operations.OpNotification.InsertRecord(notification);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }

        private void Jira_AddAttachments(string TicketKey, int TicketinformationId)
        {
            try
            {
                //DAL.Jira.JiraSynch jiraSynch = new DAL.Jira.JiraSynch();
                //jiraSynch.UploadAttachments(TicketKey, TicketinformationId, HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"));

                DAL.Jira.JiraAttachments jiraAttachments = new DAL.Jira.JiraAttachments();
                jiraAttachments.UploadAttachments(TicketKey, TicketinformationId, HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data"));

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

                DAL.Operations.Logger.Info("Session logged in.Edit Ticket. PersonID " + Session["PersonId"] +
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