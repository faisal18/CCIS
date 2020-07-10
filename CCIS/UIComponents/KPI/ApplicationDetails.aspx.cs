using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.KPI
{
    public partial class ApplicationDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session.Keys.Count > 0)
                {
                    Session["Name"].ToString();
                    int.Parse(Session["PersonId"].ToString());
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if (!IsPostBack)
                {
                    int ApplicationID = CheckQueryString();
                    Loader(ApplicationID);
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            CheckQueryString();
        }
        private int CheckQueryString()
        {
            int ApplicationID = 0;
            try
            {
                if (Request.QueryString.HasKeys())
                {
                    ApplicationID = int.Parse(Request.QueryString["ApplicationID"]);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return ApplicationID;
        }
        private void Loader(int ApplicationID)
        {
            try
            {
                LoadIssuesDue(ApplicationID);
                LoadRptrIssuesUpdated(ApplicationID);
                TopUsers(ApplicationID);
                InquiryCount(ApplicationID);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }
        }

        #region Issues Updated
        private void LoadRptrIssuesUpdated(int ApplicationID)
        {
            try
            {
                DataTable dt = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetCloseIncidentsByApplicationID(ApplicationID)).Tables[0];
                DataTable dt_Persons = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                DataTable dt_Ticket = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetTicketInformationbyApplicationID(ApplicationID)).Tables[0];
                rptr_issuesUpdated.DataSource = MergerIssuesUpdated(dt, dt_Persons, dt_Ticket);
                rptr_issuesUpdated.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable MergerIssuesUpdated(DataTable dt, DataTable dt_persons, DataTable dt_Ticket)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("AssignedTo", typeof(string));
            dataTable.Columns.Add("TicketNumber", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("AssignedDate", typeof(DateTime));
            dataTable.Columns.Add("TicketInformationID", typeof(int));

            try
            {
                var result = from T1 in dt.AsEnumerable()
                             join T2 in dt_persons.AsEnumerable()
                             on T1.Field<int>("AssignedTOID") equals T2.Field<int>("PersonInformationID")

                             join T3 in dt_Ticket.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T3.Field<int>("TicketInformationID")

                             select dataTable.LoadDataRow(new object[]
                                         {
                                 T2.Field<string>("FullName"),
                                 T3.Field<string>("TicketNumber"),
                                 T3.Field<string>("Description"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T1.Field<int>("TicketInformationID")
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
        protected void lbl_TicketNumber_IssuesUpdaeted_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var link = sender as LinkButton;
                int TicketInformationID = int.Parse(link.CommandName.ToString());

                Response.Redirect("~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID=" + TicketInformationID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        } 
        #endregion


        #region Issues Due
        private void LoadIssuesDue(int ApplicationID)
        {
            try
            {
                DataTable dt = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetOpenIncidentsByApplicationID(ApplicationID).Take(5).ToList()).Tables[0];
                DataTable dt_Persons = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                DataTable dt_Ticket = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetTicketInformationbyApplicationID(ApplicationID)).Tables[0];

                rptr_issuesDue.DataSource = MergeIssuesDueData(dt, dt_Persons, dt_Ticket);
                rptr_issuesDue.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable MergeIssuesDueData(DataTable dt, DataTable dt_persons, DataTable dt_Ticket)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("AssignedTo", typeof(string));
            dataTable.Columns.Add("TicketNumber", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("AssignedDate", typeof(DateTime));
            dataTable.Columns.Add("TicketInformationID", typeof(int));

            try
            {
                var result = from T1 in dt.AsEnumerable()
                             join T2 in dt_persons.AsEnumerable()
                             on T1.Field<int>("AssignedTOID") equals T2.Field<int>("PersonInformationID")

                             join T3 in dt_Ticket.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T3.Field<int>("TicketInformationID")

                             select dataTable.LoadDataRow(new object[]
                                         {
                                 T2.Field<string>("FullName"),
                                 T3.Field<string>("TicketNumber"),
                                 T3.Field<string>("Description"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T1.Field<int>("TicketInformationID")
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
        protected void lbl_TicketNumber_IssuesDue_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var link = sender as LinkButton;
                int TicketInformationID = int.Parse(link.CommandName.ToString());

                Response.Redirect("~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID=" + TicketInformationID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        [System.Web.Services.WebMethod]
        public static Bar_Priority[] GetPriority(int ApplicationID)
        {
            List<Bar_Priority> list_priority = new List<Bar_Priority>();
            try
            {

                var result = DAL.Operations.OpTicketHistory.GetGroupedByTicket(ApplicationID);
                var result2 = result.GroupBy(x => x.PriorityID).Select(x => new { x.Key, Count = x.Count() }).ToList();
                DataTable dt = DAL.Helper.ListToDataset.ToDataSet(result2).Tables[0];
                Bar_Priority obj_bar;

                if (dt.Rows.Count > 0) 
                {
                    //obj_bar = new Bar_Unresolved_Priority();
                    //obj_bar.CheckingTime = DateTime.Now.ToString();
                    //obj_bar.SuperUrgent = (int)dt.Rows[0]["Count"];
                    //obj_bar.Urgent = (int)dt.Rows[1]["Count"];
                    //obj_bar.High = (int)dt.Rows[2]["Count"];
                    //obj_bar.Medium = (int)dt.Rows[3]["Count"];
                    //obj_bar.Low = (int)dt.Rows[4]["Count"];
                    //list_unresolved.Add(obj_bar);
                    for (int i = 0; i < dt.Rows.Count; i++) 
                    {
                        obj_bar = new Bar_Priority();
                        obj_bar.CheckingTime = DateTime.Now.ToString();

                        if(result2[i].Key == 18)
                        {
                            obj_bar.SuperUrgent = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 19)
                        {
                            obj_bar.Urgent = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 20)
                        {
                            obj_bar.High = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 21)
                        {
                            obj_bar.Medium = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 22)
                        {
                            obj_bar.Low = (int)dt.Rows[i]["Count"];
                        }
                        list_priority.Add(obj_bar);
                    }
                }

                obj_bar = new Bar_Priority();
                obj_bar.SuperUrgent = list_priority.Sum(x => x.SuperUrgent);
                obj_bar.Urgent = list_priority.Sum(x => x.Urgent);
                obj_bar.High = list_priority.Sum(x => x.High);
                obj_bar.Medium = list_priority.Sum(x => x.Low);
                obj_bar.Low = list_priority.Sum(x => x.Low);
                list_priority = null;

                List<Bar_Priority> list_priorites2 = new List<Bar_Priority>();
                list_priorites2.Add(obj_bar);
                Bar_Priority[] bar = list_priorites2.ToArray();
                return bar;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public class Bar_Priority
        {
            public string CheckingTime { get; set; }
            public int SuperUrgent { get; set; }
            public int Urgent { get; set; }
            public int High { get; set; }
            public int Medium { get; set; }
            public int Low { get; set; }
        }


        [System.Web.Services.WebMethod]
        public static Bar_Statuses[] GetStatus(int ApplicationID)
        {
            List<Bar_Statuses> List_Status = new List<Bar_Statuses>();
            try
            {

                var result = DAL.Operations.OpTicketHistory.GetGroupedByTicket(ApplicationID);
                var result2 = result.GroupBy(x => x.IncidentStatusID).Select(x => new { x.Key, Count = x.Count() }).ToList();
                DataTable dt = DAL.Helper.ListToDataset.ToDataSet(result2).Tables[0];
                Bar_Statuses obj_bar;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_bar = new Bar_Statuses();
                        obj_bar.CheckingTime = DateTime.Now.ToString();

                        if (result2[i].Key == 10)
                        {
                            obj_bar.New = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 11)
                        {
                            obj_bar.InProgress = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 12)
                        {
                            obj_bar.Resolved = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 13)
                        {
                            obj_bar.ReOpened = (int)dt.Rows[i]["Count"];
                        }
                        if (result2[i].Key == 14)
                        {
                            obj_bar.Close = (int)dt.Rows[i]["Count"];
                        }
                        List_Status.Add(obj_bar);
                    }

                    
                }

                obj_bar = new Bar_Statuses();
                obj_bar.New = List_Status.Sum(x => x.New);
                obj_bar.InProgress = List_Status.Sum(x => x.InProgress);
                obj_bar.Resolved = List_Status.Sum(x => x.Resolved);
                obj_bar.ReOpened = List_Status.Sum(x => x.ReOpened);
                obj_bar.Close = List_Status.Sum(x => x.Close);

                List_Status = null;

                List<Bar_Statuses> List_Status2 = new List<Bar_Statuses>();
                List_Status2.Add(obj_bar);

                Bar_Statuses[] bar = List_Status2.ToArray();
                return bar;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public class Bar_Statuses
        {
            public string CheckingTime { get; set; }
            public int New { get; set; }
            public int InProgress { get; set; }
            public int Resolved { get; set; }
            public int ReOpened { get; set; }
            public int Close { get; set; }
        }


        private void TopUsers(int ApplicationID)
        {
            try
            {
                var result = DAL.Operations.OpTicketHistory.GetCloseIncidentsByApplicationID(ApplicationID);
                DataTable dt_tickets = DAL.Helper.ListToDataset.ToDataSet(result.GroupBy(x => x.AssignedTOID).Select(x => new { x.Key, Count = x.Count() }).Take(5).ToList()).Tables[0];
                DataTable dt_person = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];

                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("AgentName", typeof(string));
                dt_new.Columns.Add("AgentID", typeof(int));
                dt_new.Columns.Add("Count", typeof(int));


                var result2 = from T1 in dt_tickets.AsEnumerable()
                             join T2 in dt_person.AsEnumerable()
                             on T1.Field<int>("Key") equals T2.Field<int>("PersonInformationID")
                             orderby T1.Field<int>("Count") descending

                              select dt_new.LoadDataRow(new object[]
                                         {
                                             T2.Field<string>("FullName"),
                                             T2.Field<int>("PersonInformationID"),
                                             T1.Field<int>("Count"),
                                         }, false);

                if (result2.Count() > 0) 
                {
                    rptr_Agent.DataSource = result2.CopyToDataTable();
                    rptr_Agent.DataBind();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void lbl_Agent_Command(object sender, CommandEventArgs e)
        {

        }


        private void InquiryCount(int ApplicationID)
        {
            try
            {
                var inquiry = DAL.Operations.OpInquiryDetails.GetInquiryDetailsbyApplicationID(ApplicationID).GroupBy(x => x.CreatedBy).Select(x => new { x.Key, Count = x.Count() }).OrderByDescending(x=>x.Count).ToList();
                DataTable dt_inquiry = DAL.Helper.ListToDataset.ToDataSet(inquiry).Tables[0];
                DataTable dt_application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];

                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("AgentID", typeof(int));
                dt_new.Columns.Add("AgentName", typeof(string));
                dt_new.Columns.Add("Count", typeof(int));

                //FullName

                var result = from T1 in dt_inquiry.AsEnumerable()
                             join T2 in dt_application.AsEnumerable()
                             on T1.Field<string>("Key") equals T2.Field<string>("FullName")

                             select dt_new.LoadDataRow(new object[]
                             {
                                 T2.Field<int>("PersonInformationID"),
                                 T2.Field<string>("FullName"),
                                 T1.Field<int>("Count"),

                             }, false);

                if (result.Count() > 0) 
                {
                    dt_new = result.CopyToDataTable();
                    rptr_InqCount.DataSource = dt_new;
                    rptr_InqCount.DataBind();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}