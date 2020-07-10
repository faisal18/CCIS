using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace CCIS.UIComponents.Ticket.L2
{
    public partial class L2_TicketQueue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session.Keys.Count > 0)
            {
            }
            else
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (!IsPostBack)
            {
                PopulateGrid();
                PopulateGridGuys();
                PopulateIncident();
            }
        }

        private void PopulateGrid()
        {
            try
            {
                
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(false), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());

                if (dt_TicketInformation.Rows.Count > 0)
                {
                    GV_ListTickets.DataSource = dt_TicketInformation;
                    GV_ListTickets.DataBind();
                }
                else
                {
                    dt_TicketInformation.Rows.Add(dt_TicketInformation.NewRow());
                    GV_ListTickets.DataSource = dt_TicketInformation;
                    GV_ListTickets.DataBind();
                    GV_ListTickets.Rows[0].Cells.Clear();
                    GV_ListTickets.Rows[0].Cells.Add(new TableCell());
                    GV_ListTickets.Rows[0].Cells[0].ColumnSpan = dt_TicketInformation.Columns.Count;
                    GV_ListTickets.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_ListTickets.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void PopulateGridGuys()
        {
            try
            {

                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(true), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());

                if (dt_TicketInformation.Rows.Count > 0)
                {
                    GV_TechGuys.DataSource = dt_TicketInformation;
                    GV_TechGuys.DataBind();
                }
                else
                {
                    dt_TicketInformation.Rows.Add(dt_TicketInformation.NewRow());
                    GV_TechGuys.DataSource = dt_TicketInformation;
                    GV_TechGuys.DataBind();
                    GV_TechGuys.Rows[0].Cells.Clear();
                    GV_TechGuys.Rows[0].Cells.Add(new TableCell());
                    GV_TechGuys.Rows[0].Cells[0].ColumnSpan = dt_TicketInformation.Columns.Count;
                    GV_TechGuys.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_TechGuys.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void PopulateIncident()
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus(),LoadTicketType(), true);

                if (dt_TicketInformation.Rows.Count > 0)
                {
                    GV_NonIncident.DataSource = dt_TicketInformation;
                    GV_NonIncident.DataBind();
                }
                else
                {
                    dt_TicketInformation.Rows.Add(dt_TicketInformation.NewRow());
                    GV_NonIncident.DataSource = dt_TicketInformation;
                    GV_NonIncident.DataBind();
                    GV_NonIncident.Rows[0].Cells.Clear();
                    GV_NonIncident.Rows[0].Cells.Add(new TableCell());
                    GV_NonIncident.Rows[0].Cells[0].ColumnSpan = dt_TicketInformation.Columns.Count;
                    GV_NonIncident.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_NonIncident.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }


        private DataTable LoadTicketInformation()
        {
            DataTable dataTable = new DataTable();
            try
            {
                //int L2QueueUserAssignee = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("L2QueueUserAssignee"));
                //Entities.TicketLists _TicketList = DAL.Operations.OpTicketLists.GetOpenTicketsbyAssignee(L2QueueUserAssignee);

                dataTable = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadTicketHistory()
        {
            DataTable dataTable = new DataTable();
            List<int> TechGuys = new List<int>();

            try
            {

                var techRoles = DAL.Operations.OpRoles.GetAll().Where(x => x.Description.Contains("L2")).Select(x => x.RolesID).ToList();
                var techPersons = DAL.Operations.OpUserRoles.GetAll().Where(x => techRoles.Contains(x.RoleID)).Select(x => x.PersonID).ToList();

                TechGuys = techPersons;
                //TechGuys.Add(33);
                //TechGuys.Add(8);
                //TechGuys.Add(9);
                //TechGuys.Add(29);
                //TechGuys.Add(23);
                //TechGuys.Add(78);
                TechGuys.Add(7);

                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketHistory>(DAL.Operations.OpTicketHistory.GetOpenIncidents().Where(x => TechGuys.Contains(x.AssignedTOID)).ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadTicketHistory(bool isguys)
        {
            DataTable dataTable = new DataTable();
            List<int> TechGuys = new List<int>();
            try
            {

                var techRoles = DAL.Operations.OpRoles.GetAll().Where(x => x.Description.Contains("L2")).Select(x => x.RolesID).ToList();
                var techPersons = DAL.Operations.OpUserRoles.GetAll().Where(x => techRoles.Contains(x.RoleID)).Select(x => x.PersonID).ToList();

                if (isguys)
                {
                    TechGuys = techPersons;

                    //TechGuys.Add(33);
                    //TechGuys.Add(8);
                    //TechGuys.Add(9);
                    //TechGuys.Add(29);
                    //TechGuys.Add(23);
                    //TechGuys.Add(78);
                }
                else
                {
                    TechGuys.Add(7);
                }

                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketHistory>(DAL.Operations.OpTicketHistory.GetOpenIncidents().Where(x => TechGuys.Contains(x.AssignedTOID)).ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadPersonInformation()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadTicketStatus()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetStatusesbyTypeName("Incident Status")).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadCallerInformation()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.CallerInformation>(DAL.Operations.OpCallerInfo.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadTicketType()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpStatuses.GetStatusesbyTypeName("Ticket Type").ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return dt;
        }
        private DataTable MergeData(DataTable dt_TicketInformation, DataTable dt_TicketHistory, DataTable dt_personinformation, DataTable dt_caller, DataTable dt_TicketStatus,DataTable dt_TicketType, bool nonincident)
        {
            DataTable New_DT = new DataTable();

            try
            {
                int IncidentType = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeIncident"));

                New_DT.Columns.Add("TicketInformationID", typeof(int));
                New_DT.Columns.Add("TicketNumber", typeof(string));

                New_DT.Columns.Add("CallerID", typeof(int));
                New_DT.Columns.Add("CallerName", typeof(string));
                //New_DT.Columns.Add("StatusID", typeof(int));
                //New_DT.Columns.Add("StatusName", typeof(string));

                New_DT.Columns.Add("Subject", typeof(string));
                New_DT.Columns.Add("ReportedBy", typeof(string));
                New_DT.Columns.Add("Assignee", typeof(string));
                New_DT.Columns.Add("CreationDate", typeof(DateTime));
                New_DT.Columns.Add("Status", typeof(string));
                New_DT.Columns.Add("Type", typeof(string));



                List<int> List_MAX_TicketHistoryId = new List<int>();
                var MaxQuery = dt_TicketHistory.AsEnumerable()
                                .GroupBy(x => x.Field<int>("TicketInformationID"))
                                .Select(g => g.OrderByDescending(x => x.Field<int>("tickethistoryid")).First())
                                .OrderByDescending(x => x.Field<int>("tickethistoryid"));
                List_MAX_TicketHistoryId = MaxQuery.Select(x => x.Field<int>("tickethistoryid")).ToList();



                var result = from T1 in dt_TicketInformation.AsEnumerable()
                             join T2 in dt_TicketHistory.AsEnumerable()
                             on T1.Field<int>("ticketinformationid") equals T2.Field<int>("ticketinformationid")

                             join T3 in dt_caller.AsEnumerable()
                             on T1.Field<int>("CallerKeyID") equals T3.Field<int>("CallerInformationID")

                             join T4 in dt_personinformation.AsEnumerable()
                             on T1.Field<int>("ReporterID") equals T4.Field<int>("PersonInformationID")

                             join T5 in dt_personinformation.AsEnumerable()
                             on T2.Field<int>("AssignedTOID") equals T5.Field<int>("PersonInformationID")

                             join T6 in dt_TicketStatus.AsEnumerable()
                             on T2.Field<int>("IncidentStatusID") equals T6.Field<int>("StatusesID")

                             join T7 in dt_TicketType.AsEnumerable()
                             on T1.Field<int>("TicketType") equals T7.Field<int>("StatusesID")

                             where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid"))
                             && T6.Field<string>("Description").ToLower() != "closed"
                             && T1.Field<int>("TicketType") != IncidentType)

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("TicketNumber"),
                                 T3.Field<int>("CallerInformationID"),
                                 T3.Field<string>("name"),
                                 T1.Field<string>("Subject"),
                                 T4.Field<string>("FullName"),
                                 T5.Field<string>("FullName"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T6.Field<string>("Description"),
                                 T7.Field<string>("Description"),

                             }, false);

                if (result.AsEnumerable().Count() > 0)
                {
                    New_DT = result.AsEnumerable().Distinct(DataRowComparer.Default).OrderByDescending(x => x.Field<DateTime>("CreationDate")).CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return New_DT;
        }
        private DataTable MergeData(DataTable dt_TicketInformation, DataTable dt_TicketHistory, DataTable dt_personinformation, DataTable dt_caller, DataTable dt_TicketStatus)
        {
            DataTable New_DT = new DataTable();
            
            try
            {
                int IncidentType = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeIncident"));

                New_DT.Columns.Add("TicketInformationID", typeof(int));
                New_DT.Columns.Add("TicketNumber", typeof(string));

                New_DT.Columns.Add("CallerID", typeof(int));
                New_DT.Columns.Add("CallerName", typeof(string));
                //New_DT.Columns.Add("StatusID", typeof(int));
                //New_DT.Columns.Add("StatusName", typeof(string));

                New_DT.Columns.Add("Subject", typeof(string));
                New_DT.Columns.Add("ReportedBy", typeof(string));
                New_DT.Columns.Add("Assignee", typeof(string));
                New_DT.Columns.Add("CreationDate", typeof(DateTime));
                New_DT.Columns.Add("Status", typeof(string));


                List<int> List_MAX_TicketHistoryId = new List<int>();
                var MaxQuery = dt_TicketHistory.AsEnumerable()
                                .GroupBy(x => x.Field<int>("TicketInformationID"))
                                .Select(g => g.OrderByDescending(x => x.Field<int>("tickethistoryid")).First())
                                .OrderByDescending(x => x.Field<int>("tickethistoryid"));
                List_MAX_TicketHistoryId = MaxQuery.Select(x => x.Field<int>("tickethistoryid")).ToList();


               
                var result = from T1 in dt_TicketInformation.AsEnumerable()
                             join T2 in dt_TicketHistory.AsEnumerable()
                             on T1.Field<int>("ticketinformationid") equals T2.Field<int>("ticketinformationid")

                             join T3 in dt_caller.AsEnumerable()
                             on T1.Field<int>("CallerKeyID") equals T3.Field<int>("CallerInformationID")

                             join T4 in dt_personinformation.AsEnumerable()
                             on T1.Field<int>("ReporterID") equals T4.Field<int>("PersonInformationID")

                             join T5 in dt_personinformation.AsEnumerable()
                             on T2.Field<int>("AssignedTOID") equals T5.Field<int>("PersonInformationID")

                             join T6 in dt_TicketStatus.AsEnumerable()
                             on T2.Field<int>("IncidentStatusID") equals T6.Field<int>("StatusesID")

                                     where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid"))
                                     && T6.Field<string>("Description").ToLower() != "closed"
                                     && T1.Field<int>("TicketType") == IncidentType)

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("TicketNumber"),
                                 T3.Field<int>("CallerInformationID"),
                                 T3.Field<string>("name"),
                                 T1.Field<string>("Subject"),
                                 T4.Field<string>("FullName"),
                                 T5.Field<string>("FullName"),
                                 T1.Field<DateTime>("CreationDate"),
                                  T6.Field<string>("Description"),

                             }, false);

                if (result.AsEnumerable().Count() > 0)
                { 
                    New_DT = result.AsEnumerable().Distinct(DataRowComparer.Default).OrderByDescending(x => x.Field<DateTime>("CreationDate")).CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return New_DT;
        }

        protected void Link_Button_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int TicketInformaitonID = int.Parse(e.CommandArgument.ToString());
                if (e.CommandName == "Summary")
                {
                    Response.Redirect("../ViewTicket.aspx?TicketInformationID=" + TicketInformaitonID, false);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void link_AssignToMe_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int TicketInformaitonID = int.Parse(e.CommandArgument.ToString());
                if (e.CommandName == "AssignToMe")
                {
                    if (CreateTicketHistory(TicketInformaitonID))
                    {
                        if (CreateNotification(TicketInformaitonID))
                        {
                            PopulateGrid();
                            PopulateGridGuys();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private bool CreateTicketHistory(int TicketInformationID)
        {
            string role = Session["Role"].ToString();
            var result = false;
            try
            {
                Entities.TicketHistory ticketHistory = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                ticketHistory.AssignedTOID = int.Parse(Session["PersonID"].ToString());
                ticketHistory.CreatedBy = Session["Name"].ToString();
                ticketHistory.CreationDate = DateTime.Now;
                ticketHistory.Comments = "";
                ticketHistory.ActivityComments = Session["Name"].ToString() + " has assigned the ticket to themselves";
                ticketHistory.Activity = "ASSIGNED TO " + role; ;
                var res = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistory);
                if (res > 0)
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
        private bool CreateNotification(int TicketInformationID)
        {
            var result = false;
            int ClientEmailTemplateID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("EmailToClient_TemplateID"));
            int Notification_pending_statusid = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Pending")).AsEnumerable().Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());
            string name = Session["Name"].ToString();
            string toaddress = string.Empty;


            try
            {




                string EmailCategory = "Internal_Template_CS_02_INT_Ticket update";
                string comments = "Ticket has been to assigned to " + name;

                string To_Address = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(int.Parse(Session["PersonID"].ToString())).First().Email;

                string CC_Address = System.Configuration.ConfigurationManager.AppSettings.Get("SupportEmail");


                Entities.Notification _notification1 = WebService.SLAProcessing.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);


                

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }


        protected void GV_ListTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_ListTickets.PageIndex = e.NewPageIndex;
                PopulateGrid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_TechGuys_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_TechGuys.PageIndex = e.NewPageIndex;
                PopulateGridGuys();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_NonIncident_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_NonIncident.PageIndex = e.NewPageIndex;
                PopulateIncident();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }


        protected void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(false), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());

                if (txt_searchbox.Text.Length > 1)
                {
                    string category = ddl_search.SelectedValue;
                    string search_query = txt_searchbox.Text;

                    var result = dt_TicketInformation.AsEnumerable().Where(x => x.Field<string>(category) == search_query).ToList();
                    if (result.Count > 0)
                    {
                        GV_ListTickets.DataSource = result.CopyToDataTable();
                        GV_ListTickets.DataBind();
                    }

                    //TechGuys.Attributes.Clear();
                    //TechGuys.Attributes.Remove("class");
                    //TechGuys.Attributes.Add("class", "tab-pane fade in ");

                    //TechSupport.Attributes.Clear();
                    //TechSupport.Attributes.Remove("class");
                    //TechSupport.Attributes.Add("class", "tab-pane fade in active");
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }
        protected void btn_Search2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(true), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());

                if (txt_SearchBox2.Text.Length > 1)
                {
                    string category = ddl_search2.SelectedValue;
                    string search_query = txt_SearchBox2.Text;

                    var result = dt_TicketInformation.AsEnumerable().Where(x => x.Field<string>(category) == search_query).ToList();
                    if (result.Count > 0)
                    {
                        GV_TechGuys.DataSource = result.CopyToDataTable();
                        GV_TechGuys.DataBind();
                    }


                    //TechGuys.Attributes.Clear();
                    //TechGuys.Attributes.Remove("class");
                    //TechGuys.Attributes.Add("class", "tab-pane fade in active");

                    //TechSupport.Attributes.Clear();
                    //TechSupport.Attributes.Remove("class");
                    //TechSupport.Attributes.Add("class", "tab-pane fade in");
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus(), LoadTicketType(), true);

                if (txt_SearchBox2.Text.Length > 1)
                {
                    string category = ddl_nonincident.SelectedValue;
                    string search_query = TextBox1.Text;

                    var result = dt_TicketInformation.AsEnumerable().Where(x => x.Field<string>(category) == search_query).ToList();
                    if (result.Count > 0)
                    {
                        GV_NonIncident.DataSource = result.CopyToDataTable();
                        GV_NonIncident.DataBind();
                    }


                    //TechGuys.Attributes.Clear();
                    //TechGuys.Attributes.Remove("class");
                    //TechGuys.Attributes.Add("class", "tab-pane fade in active");

                    //TechSupport.Attributes.Clear();
                    //TechSupport.Attributes.Remove("class");
                    //TechSupport.Attributes.Add("class", "tab-pane fade in");
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }

    }
}