using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets
{
    public partial class RecieveCall : System.Web.UI.Page
    {

        public static int CallerKeyID = 0;
        #region Loaders
        protected void Page_Load(object sender, EventArgs e)
        {
            //int CallerKeyID = 0;

            try
            {
                if (Session.Keys.Count > 0)
                {
                    string SessionName = Session["Name"].ToString();
                    int SessionPersonID = int.Parse(Session["PersonId"].ToString());
                    //SessionPersonId = int.Parse(Session["PersonId"].ToString());
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if (!IsPostBack)
                {
                    if (Request.QueryString.Count > 0 && Request.QueryString.AllKeys.Contains("CallerKeyID"))
                    {
                        CallerKeyID = int.Parse(Request.QueryString["CallerKeyID"]);

                    }
                    else
                    {
                        CallerKeyID = 3;
                    }
                    LoadData(CallerKeyID);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadData(int CallerKeyID)
        {
            try
            {
                //Label Caller from CallerInfo
                if (CallerKeyID != 0)
                {

                    Entities.CallerInformation CallerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(CallerKeyID);
                    string CallerLicense = CallerInformation.CallerLicense;
                    lbl_CallerId.Text = CallerInformation.CallerKeyID + " - " + CallerInformation.Name;


                }

                //Dropdownlist from Applications
                DataTable dt_application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll().OrderBy(x => x.Name).ToList()).Tables[0];
                ddl_Application.DataSource = dt_application;
                ddl_Application.DataTextField = "Name";
                ddl_Application.DataValueField = "ApplicationsID";
                ddl_Application.DataBind();

                DataTable DT_ddl_InquirySource = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                ddl_InquirySource.DataSource = DT_ddl_InquirySource.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("InquirySource")).CopyToDataTable();
                ddl_InquirySource.DataTextField = "Description";
                ddl_InquirySource.DataValueField = "ItemTypesID";
                ddl_InquirySource.DataBind();

                var actions = DAL.Operations.OpInquiryDetails.GetInquiryDetailsbyApplicationID(int.Parse(ddl_Application.SelectedItem.Value)).Where(x => x.ActionTaken != null && x.ActionTaken.Length > 1).OrderBy(x => x.CreationDate).Select(x => x.ActionTaken).Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                ddl_ActionTaken.DataSource = actions;
                ddl_ActionTaken.DataBind();



                //populate caller history grid
                PopulateGrid(CallerKeyID);

                //populate close ticket history grid
                ClosedPopulateTicketGrid(CallerKeyID);

                //populate inquriy history grid
                PopulateInquiryGrid(CallerKeyID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        //Closed Ticket HistoryGrid
        private void ClosedPopulateTicketGrid(int CallerKeyID)
        {
            try
            {
                DataTable newDT = GetData(CallerKeyID, true);
                if (newDT.Rows.Count > 0)
                {
                    GV_CloseTicketHistory.DataSource = newDT;
                    GV_CloseTicketHistory.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_CloseTicketHistory.DataSource = newDT;
                    GV_CloseTicketHistory.DataBind();
                    GV_CloseTicketHistory.Rows[0].Cells.Clear();
                    GV_CloseTicketHistory.Rows[0].Cells.Add(new TableCell());
                    GV_CloseTicketHistory.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_CloseTicketHistory.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_CloseTicketHistory.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable ClosedMergeDataTables(DataTable dt_ticketinformation, DataTable dt_history, DataTable dt_Status, DataTable dt_person, DataTable dt_Application)
        {
            DataTable dataTable = new DataTable();
            try
            {

                dataTable.Columns.Add("TicketInformationID", typeof(int));
                dataTable.Columns.Add("TicketNumber", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("AssignedTo", typeof(string));
                dataTable.Columns.Add("Subject", typeof(string));
                dataTable.Columns.Add("ActionTaken", typeof(string));
                dataTable.Columns.Add("CreatedDate", typeof(DateTime));
                dataTable.Columns.Add("Application", typeof(string));



                List<int> List_MAX_TicketHistoryId = new List<int>();
                var MaxQuery = dt_history.AsEnumerable()
                                .GroupBy(x => x.Field<int>("TicketInformationID"))
                                .Select(g => g.OrderByDescending(x => x.Field<int>("tickethistoryid")).First())
                                .OrderByDescending(x => x.Field<int>("tickethistoryid"));
                List_MAX_TicketHistoryId = MaxQuery.Select(x => x.Field<int>("tickethistoryid")).ToList();

                var result = from T1 in dt_ticketinformation.AsEnumerable()

                             join T2 in dt_history.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T2.Field<int>("TicketInformationID")

                             join T3 in dt_Status.AsEnumerable()
                             on T2.Field<int>("IncidentStatusID") equals T3.Field<int>("StatusesID")

                             join T4 in dt_person.AsEnumerable()
                             on T2.Field<int>("AssignedTOID") equals T4.Field<int>("PersonInformationID")

                             join T5 in dt_Application.AsEnumerable()
                             on T1.Field<int>("ApplicationID") equals T5.Field<int>("ApplicationsID")

                             //where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid")))
                             where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid"))
                             && T3.Field<string>("Description").ToLower() == "closed"
                             && T1.Field<string>("Subject") != string.Empty
                             && T1.Field<string>("Subject") != "")




                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("TicketNumber"),
                                 T3.Field<string>("Description"),
                                 T4.Field<string>("FullName"),
                                 T1.Field<string>("Subject"),
                                 T1.Field<string>("ActionTaken"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T5.Field<string>("Name"),

                             }, false);



                if (result.Count() > 0)
                {
                    dataTable = result.CopyToDataTable();
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }

        //Open Ticket History Grid
        private void PopulateGrid(int CallerKeyID)
        {
            try
            {
                DataTable newDT = GetData(CallerKeyID,false);
                if (newDT.Rows.Count > 0)
                {
                    GV_CallerHistory.DataSource = newDT;
                    GV_CallerHistory.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_CallerHistory.DataSource = newDT;
                    GV_CallerHistory.DataBind();
                    GV_CallerHistory.Rows[0].Cells.Clear();
                    GV_CallerHistory.Rows[0].Cells.Add(new TableCell());
                    GV_CallerHistory.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_CallerHistory.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_CallerHistory.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable GetData(int CallerKeyID,bool closed)
        {
            DataTable dataTable_new = new DataTable();
            try
            {
                DataTable dt_ticketinformation = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetTicketInformationByCallerID(CallerKeyID)).Tables[0];
                DataTable dt_history = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetAll()).Tables[0];
                DataTable dt_Status = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                DataTable dt_person = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                DataTable dt_Application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll().OrderBy(x => x.Name).ToList()).Tables[0];

                if (!closed)
                {
                    dataTable_new = MergeDataTables(dt_ticketinformation, dt_history, dt_Status, dt_person, dt_Application);
                }
                else if(closed)
                {
                    dataTable_new = ClosedMergeDataTables(dt_ticketinformation, dt_history, dt_Status, dt_person, dt_Application);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return dataTable_new;
        }
        private DataTable MergeDataTables(DataTable dt_ticketinformation, DataTable dt_history, DataTable dt_Status, DataTable dt_person, DataTable dt_Application)
        {
            DataTable dataTable = new DataTable();
            try
            {

                dataTable.Columns.Add("TicketInformationID", typeof(int));
                dataTable.Columns.Add("TicketNumber", typeof(string));
                dataTable.Columns.Add("Status", typeof(string));
                dataTable.Columns.Add("AssignedTo", typeof(string));
                dataTable.Columns.Add("Subject", typeof(string));
                dataTable.Columns.Add("ActionTaken", typeof(string));
                dataTable.Columns.Add("CreatedDate", typeof(DateTime));
                dataTable.Columns.Add("Application", typeof(string));



                List<int> List_MAX_TicketHistoryId = new List<int>();
                var MaxQuery = dt_history.AsEnumerable()
                                .GroupBy(x => x.Field<int>("TicketInformationID"))
                                .Select(g => g.OrderByDescending(x => x.Field<int>("tickethistoryid")).First())
                                .OrderByDescending(x => x.Field<int>("tickethistoryid"));
                List_MAX_TicketHistoryId = MaxQuery.Select(x => x.Field<int>("tickethistoryid")).ToList();

                var result = from T1 in dt_ticketinformation.AsEnumerable()

                             join T2 in dt_history.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T2.Field<int>("TicketInformationID")

                             join T3 in dt_Status.AsEnumerable()
                             on T2.Field<int>("IncidentStatusID") equals T3.Field<int>("StatusesID")

                             join T4 in dt_person.AsEnumerable()
                             on T2.Field<int>("AssignedTOID") equals T4.Field<int>("PersonInformationID")

                             join T5 in dt_Application.AsEnumerable()
                             on T1.Field<int>("ApplicationID") equals T5.Field<int>("ApplicationsID")
                             
                             //where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid")))
                             where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid"))
                             && T3.Field<string>("Description").ToLower() != "closed"
                             && T1.Field<string>("Subject") != string.Empty
                             && T1.Field<string>("Subject") != "")




                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("TicketNumber"),
                                 T3.Field<string>("Description"),
                                 T4.Field<string>("FullName"),
                                 T1.Field<string>("Subject"),
                                 T1.Field<string>("ActionTaken"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T5.Field<string>("Name"),

                             }, false);



                if (result.Count() > 0)
                {
                    dataTable = result.CopyToDataTable();
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }

        //Inquiry History Grid
        private void PopulateInquiryGrid(int CallerKeyID)
        {
            try
            {

                DataTable newDT = GetInquiryData(CallerKeyID);

                if (newDT.Rows.Count > 0)
                {
                    GV_CallerInquiryHistory.DataSource = newDT;
                    GV_CallerInquiryHistory.DataBind();
                }

                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_CallerInquiryHistory.DataSource = newDT;
                    GV_CallerInquiryHistory.DataBind();
                    GV_CallerInquiryHistory.Rows[0].Cells.Clear();
                    GV_CallerInquiryHistory.Rows[0].Cells.Add(new TableCell());
                    GV_CallerInquiryHistory.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_CallerInquiryHistory.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_CallerInquiryHistory.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable GetInquiryData(int CallerKeyID)
        {
            DataTable dataTable_new = new DataTable();
            try
            {
                DataTable dt_Application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                DataTable dt_inquiry = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpInquiryDetails.GetInquiryDetailsbyCallerKeyIDNew(CallerKeyID).Where(x => x.NewInquiry == true).ToList()).Tables[0];

                dataTable_new = InquiryMergeTables(dt_Application, dt_inquiry);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
                DAL.Operations.Logger.LogError(ex);
            }

            return dataTable_new;
        }
        private DataTable InquiryMergeTables(DataTable dt_Application, DataTable dt_inquiry)
        {
            DataTable dataTable = new DataTable();
            try
            {
                DataTable dt_itemtype = new DataTable();
                dt_itemtype = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                dataTable.Columns.Add("CallerKeyID", typeof(int));
                dataTable.Columns.Add("ActionTaken", typeof(string));
                dataTable.Columns.Add("CallerSource", typeof(string));

                dataTable.Columns.Add("CreatedBy", typeof(string));
                dataTable.Columns.Add("CreatedDate", typeof(DateTime));
                dataTable.Columns.Add("Application", typeof(string));

                var result = from T1 in dt_inquiry.AsEnumerable()
                             join T2 in dt_Application.AsEnumerable()
                             on T1.Field<int>("ApplicationID") equals T2.Field<int>("ApplicationsID")
                             join T3 in dt_itemtype.AsEnumerable()
                            on T1.Field<int>("CallerSource") equals T3.Field<int>("ItemTypesID")
                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("CallerKeyID"),
                                 T1.Field<string>("ActionTaken"),
                                 T3.Field<string>("Description"),
                                 T1.Field<string>("CreatedBy"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T2.Field<string>("Name"),

                             }, false);

                if (result.Count() > 0)
                {
                    dataTable = result.AsEnumerable().OrderByDescending(x => x.Field<DateTime>("CreatedDate")).CopyToDataTable();
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
            return dataTable;

        }
        #endregion

        #region Controls
        protected void btn_submit_Click(object sender, EventArgs e)
        {

            bool inquiry = false;
            bool issue = false;
            bool followup = false;
            logUserIP();

            try
            {
                int CallerKeyID = int.Parse(Request.QueryString["CallerKeyID"]);
                string SessionName = Session["Name"].ToString();

                switch (rdl_InquiryType.SelectedItem.ToString())
                {
                    case "Inquiry":
                        inquiry = true;
                        break;
                    case "Issue":
                        issue = true;
                        break;

                }


                var actiontaken = txt_hidden_actiontaken.Text;

                if (actiontaken.Length > 0 && txt_rich_description.InnerText.Length > 0)
                {

                    Entities.InquiryDetails inquiryDetails = new Entities.InquiryDetails
                    {
                        CallerKeyID = CallerKeyID,
                        Description = txt_rich_description.InnerText,
                        ActionTaken = txt_hidden_actiontaken.Text,

                        NewInquiry = inquiry,
                        NewIssue = issue,
                        FollowUp = followup,

                        ApplicationID = int.Parse(ddl_Application.SelectedValue),
                        CallerSource = int.Parse(ddl_InquirySource.SelectedValue),
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now

                    };

                    int result = DAL.Operations.OpInquiryDetails.InsertRecord(inquiryDetails);
                    if (result > 0)
                    {
                        if (issue)
                        {
                            Response.Redirect("Ticket\\CreateTicket.aspx?InquiryID=" + result + "&CallerKeyID=" + CallerKeyID, false);
                        }

                        if (inquiry)
                        {
                            txt_rich_description.InnerText = "Issue Details: \n\n\n\n\n\n\nImpact Analysis: \n1. What is the scope of user impact? Is it growing or limited/stagnant?\n\n2. How many departments are affected?\n\n3. Are both critical internal users (E.g. -  Senior Management, VPs, etc.) and external users? \n\n4. How many are affected?\n\n5. Does this incident cause major risks to the organization?\n\n6. Are both critical internal users (E.g. -  Senior Management, VPs, etc.) and external users? How many are affected?\n\n7. Is there a financial impact?\n\n8. Does this incident cause a major financial loss or damage to reputation?\n\n\nURGENCY:\n1. Is the urgency because of the IMPACT (above-mentioned reasons), or because of other reasons? (This will be a comments section)";
                            ddl_ActionTaken.SelectedItem.Text = null;

                            Entities.InquiryDetails inquiryDetails2 = DAL.Operations.OpInquiryDetails.GetRecordbyID(result);
                            int inquiryDetailsID = inquiryDetails2.InquiryDetailsID;

                            int TicketInformationID = CreateTicketInformation(inquiryDetailsID, inquiryDetails2.Description, inquiryDetails2.ActionTaken);
                            if (TicketInformationID > 0)
                            {
                                CreateTicketHistory(TicketInformationID);
                                CreateNotificaiton(TicketInformationID);
                            }

                            LoadData(CallerKeyID);
                        }
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + result;
                    }
                }
                else
                {
                    lbl_message.Text = "Please enter data in the fields";
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private int CreateTicketInformation(int InquiryDetailsID, string txtdesc, string txtaction)
        {
            int result = -1;
            try
            {
                Entities.TicketInformation ticketInformation = new Entities.TicketInformation
                {
                    CallerKeyID = int.Parse(Request.QueryString["CallerKeyID"]),
                    TicketNumber = ddl_Application.SelectedItem.Text.Substring(0, 3).ToUpper() + "-" + InquiryDetailsID.ToString("0000"),
                    ApplicationID = int.Parse(ddl_Application.SelectedValue),
                    //Subject = Summary,
                    Description = txtdesc,
                    ActionTaken = txtaction,

                    TicketType = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeInquiry")),
                    PriorityID = 22,
                    SeverityID = 15,

                    TicketGUIDKey = Guid.NewGuid(),
                    ReporterID = int.Parse(Session["PersonId"].ToString()),

                    CreatedBy = Session["Name"].ToString(),
                    CreationDate = DateTime.Now
                };

                result = DAL.Operations.OpTicketInformation.InsertRecord(ticketInformation);

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        private void CreateTicketHistory(int TicketInformationID)
        {
            try
            {
                Entities.TicketHistory ticketHistory = new Entities.TicketHistory
                {
                    TicketInformationID = TicketInformationID,
                    AssignedFromID = int.Parse(Session["PersonId"].ToString()),
                    AssignedTOID = int.Parse(Session["PersonId"].ToString()),
                    Comments = "Inquiry",
                    IncidentStatusID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusClosedID")),
                    IncidentSubStatusID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusL1ID")),
                    SupportTypeID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SupportTypeID")),

                    PriorityID = 22,
                    SeverityID = 15,
                    Activity = "CREATE TICKET FOR INQUIRY",
                    CreatedBy = Session["Name"].ToString(),
                    CreationDate = DateTime.Now
                };

                int TicketHistoryID = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void CreateNotificaiton(int TicketInformationID, string  emailCOmments="", string _emailCategory = "")
        {
            try
            {
                //int Notification_pending_statusid = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Pending")).AsEnumerable().Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());

                string EmailCategory = "Internal_Template_CS_01_INT_Ticket assigned to a group";
                string comments = Session["Name"].ToString() + " has created Inquiry ";

                string To_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email;
                string CC_Address = System.Configuration.ConfigurationManager.AppSettings.Get("SupportEmail");

                Entities.Notification _notification1 = CCIS.WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false,To_Address, CC_Address);
                // CLIENT EMAIL OBJECT
                //_notification1 = CCIS.WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", true, false,To_Address, CC_Address);

                //  Entities.Email email =  DAL.Operations.OpEmail.GetEmailObjectByCategory(EmailCategory);

             //   int EmailTemplateID = email.TemplateID;
                //int SentByID = int.Parse(Session["PersonId"].ToString());
                //int RecipientID = int.Parse(Session["PersonId"].ToString());
                //int CallerKeyID = int.Parse(Request.QueryString["CallerKeyID"]);

               //string CreatedBy = Session["Name"].ToString();

                //DateTime CreationDate = DateTime.Now;

                //DAL.Operations.OpNotification.SendNotification(EmailTemplateID, SentByID, RecipientID, CallerKeyID, Notification_pending_statusid,
                //    To_Address, CC_Address, CreatedBy, CreationDate);

               
                    //Comments = Session["Name"].ToString() + " has created ticket ",
                    //TicketInformationID = TicketInformationID,
                //    _notification1.SentByID = SentByID;
                //_notification1.RecipientID = RecipientID;

                //_notification1.ToAddress = To_Address;
                //_notification1.CCAddress = CC_Address;

                //_notification1.EmailTemplateID = EmailTemplateID;
                //_notification1.CallerKeyID = CallerKeyID;

                ////isExternal = false,
                ////NotificationTypeID = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Notification Type").FirstOrDefault().ItemTypesID,
                //_notification1.StatusID = Notification_pending_statusid;
                ////StatusID = DAL.Operations.OpStatuses.GetStatusesbyTypeName("Notifications").FirstOrDefault().StatusesID,

                //_notification1.CreatedBy = CreatedBy;
                //_notification1.CreationDate = CreationDate;
               

                //int NotificationID = DAL.Operations.OpNotification.InsertRecord(_notification1);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void link_TicketNumber_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int CallerKeyID = int.Parse(Request.QueryString["CallerKeyID"]);
                string SessionName = Session["Name"].ToString();
                int SessionPersonID = int.Parse(Session["PersonId"].ToString());

                int row_number = Convert.ToInt32(e.CommandArgument) % GV_CallerHistory.PageSize;
                GridViewRow row = GV_CallerHistory.Rows[row_number];
                int TicketInformaitonID = int.Parse((row.Controls[0].Controls[3] as HiddenField).Value);
                if (e.CommandName == "TicketNumber")
                {
                    Response.Redirect("Ticket//ViewTicket.aspx?TicketInformationID=" + TicketInformaitonID, false);
                }
                else if (e.CommandName == "FollowUp")
                {
                    //Create an Inquiry of followUp
                    Entities.InquiryDetails inquiryDetails = new Entities.InquiryDetails
                    {
                        CallerKeyID = CallerKeyID,
                        Description = txt_rich_description.InnerText,
                        ActionTaken = txt_hidden_actiontaken.Text,

                        NewInquiry = false,
                        NewIssue = false,
                        FollowUp = true,

                        ApplicationID = int.Parse(ddl_Application.SelectedValue),
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now

                    };

                    int result = DAL.Operations.OpInquiryDetails.InsertRecord(inquiryDetails);


                    //Create a ticket history
                    Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformaitonID);
                    Entities.TicketHistory ticketHistory2 = new Entities.TicketHistory
                    {
                        TicketInformationID = ticketHistory.TicketInformationID,
                        AssignedFromID = ticketHistory.AssignedFromID,
                        AssignedTOID = ticketHistory.AssignedTOID,
                        Comments = "A Follow-Up has been made for this ticket",
                        IncidentStatusID = ticketHistory.IncidentStatusID,
                        //IncidentSubStatusID = int.Parse(ddl_SubStatus.SelectedValue),
                        Activity = "UPDATE TICKET - FOLLOW UP",
                        PriorityID = ticketHistory.PriorityID,
                        SeverityID = ticketHistory.SeverityID,

                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    ticketHistory.Comments = "A Follow-Up has been made for this ticket";
                    ticketHistory.Activity = "UPDATE TICKET - FOLLOW UP";
                    ticketHistory.CreatedBy = SessionName;
                    ticketHistory.CreationDate = DateTime.Now;

                    int TicketHistoryID = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);


                    //Create a notification



                    //string EmailCategory = "Internal_Template_CS_02_INT_Ticket update";
                    string EmailCategory = "Internal_Template_CS_04_INT_Ticket pending";
                    string comments = SessionName + " has requested for a Follow-Up on ticket ";

                    string To_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistory.AssignedTOID).Email;
                    string CC_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(SessionPersonID).Email;

                    Entities.Notification _notification1 = CCIS.WebService.SLAProcessing.GenerateNotification(ticketHistory.TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);

                    // CLIENT EMAIL OBJECT
                     //_notification1 = CCIS.WebService.SLAProcessing.GenerateNotification(ticketHistory.TicketInformationID, false, EmailCategory, comments, "EMAIL", true, false, To_Address, CC_Address);








                    //int Notification_pending_statusid = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Pending")).AsEnumerable().Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());
                    //int EmailTemplateID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("Email_FollowUp_TemplateID"));

                    //Entities.Notification notification = new Entities.Notification
                    //{
                    //    Comments = SessionName + " has requested for a Follow-Up on ticket ",
                    //    TicketInformationID = ticketHistory.TicketInformationID,

                    //    SentByID = SessionPersonID,
                    //    ToAddress = ,

                    //    RecipientID = ticketHistory.AssignedTOID,
                    //    CCAddress = ,

                    //    EmailTemplateID = EmailTemplateID,
                    //    CallerKeyID = CallerKeyID,

                    //    NotificationTypeID = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Notification Type").FirstOrDefault().ItemTypesID,
                    //    StatusID = Notification_pending_statusid,

                    //    CreatedBy = SessionName,
                    //    CreationDate = DateTime.Now
                    //};

                    //int NotificationID = DAL.Operations.OpNotification.InsertRecord(notification);

                    ddl_ActionTaken.SelectedItem.Text = null;
                    txt_rich_description.InnerText = "Issue Details: \n\n\n\n\n\n\nImpact Analysis: \n1. What is the scope of user impact? Is it growing or limited/stagnant?\n\n2. How many departments are affected?\n\n3. Are both critical internal users (E.g. -  Senior Management, VPs, etc.) and external users? \n\n4. How many are affected?\n\n5. Does this incident cause major risks to the organization?\n\n6. Are both critical internal users (E.g. -  Senior Management, VPs, etc.) and external users? How many are affected?\n\n7. Is there a financial impact?\n\n8. Does this incident cause a major financial loss or damage to reputation?\n\n\nURGENCY:\n1. Is the urgency because of the IMPACT (above-mentioned reasons), or because of other reasons? (This will be a comments section)";
                    //lbl_message.Text = "A Follow-Up request has been created with ID :" + result;

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        protected void GV_CallerInquiryHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_CallerInquiryHistory.PageIndex = e.NewPageIndex;
                PopulateInquiryGrid(CallerKeyID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_CallerHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_CallerHistory.PageIndex = e.NewPageIndex;
                PopulateGrid(CallerKeyID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_CloseTicketHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_CloseTicketHistory.PageIndex = e.NewPageIndex;
                ClosedPopulateTicketGrid(CallerKeyID);
                //PopulateGrid(CallerKeyID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
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

                DAL.Operations.Logger.Info("Session logged in.Inquiry Create. PersonID " + Session["PersonId"] +
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