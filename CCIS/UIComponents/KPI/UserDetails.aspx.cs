using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.KPI
{
    public partial class UserDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                    Loader();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void Loader()
        {
            int personid = int.Parse(Session["PersonID"].ToString());
            h3_title.InnerText = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(personid).FullName;
            Populate_AssignedOpenIssues(personid);
            OpenIssuesCount(personid);
            CloseIssuesCount(personid);
            InquiryCount(personid);
        }

        #region AssignedOpenIssues
        private DataTable GetData_AssignedOpenIssues(int personid)
        {


            //DataTable dataTable_New = new DataTable();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TicketNumber", typeof(string));
            dataTable.Columns.Add("TicketInformationID", typeof(int));

            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ActionTaken", typeof(string));

            dataTable.Columns.Add("Application", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("ApplicationID", typeof(int));



            try
            {
                DataTable dt_Applications = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                DataTable dt_TicketInformation = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];
                DataTable dt_TicketHistory = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetOpenIncidents().Where(x => x.AssignedTOID == personid).ToList()).Tables[0];
                DataTable dt_Statuses = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpStatuses.GetAll()).Tables[0];

                var result = from T1 in dt_TicketInformation.AsEnumerable()
                             join T2 in dt_TicketHistory.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T2.Field<int>("TicketInformationID")

                             join T3 in dt_Applications.AsEnumerable()
                             on T1.Field<int>("ApplicationID") equals T3.Field<int>("ApplicationsID")

                             join T4 in dt_Statuses.AsEnumerable()
                             on T2.Field<int>("IncidentStatusID") equals T4.Field<int>("StatusesID")

                             orderby T3.Field<string>("Name")

                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<string>("TicketNumber"),
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("Subject"),
                                 T1.Field<string>("Description"),
                                 T1.Field<string>("ActionTaken"),
                                 T3.Field<string>("Name"),
                                 T4.Field<string>("Description"),
                                 T3.Field<int>("ApplicationsID"),

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
        private void Populate_AssignedOpenIssues(int personid)
        {
            try
            {
                DataTable dt = GetData_AssignedOpenIssues(personid);
                if (dt.Rows.Count > 0)
                {
                    gv_AssignedOpenIssues.DataSource = dt;
                    gv_AssignedOpenIssues.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    gv_AssignedOpenIssues.DataSource = dt;
                    gv_AssignedOpenIssues.DataBind();
                    gv_AssignedOpenIssues.Rows[0].Cells.Clear();
                    gv_AssignedOpenIssues.Rows[0].Cells.Add(new TableCell());
                    gv_AssignedOpenIssues.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    gv_AssignedOpenIssues.Rows[0].Cells[0].Text = "No Data Found !!";
                    gv_AssignedOpenIssues.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void gv_AssignedOpenIssues_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int personid = int.Parse(Session["PersonID"].ToString());
            try
            {
                gv_AssignedOpenIssues.PageIndex = e.NewPageIndex;
                Populate_AssignedOpenIssues(personid);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void LB_Application_Command(object sender, CommandEventArgs e)
        {
            var link = sender as LinkButton;
            int DataID = int.Parse(link.CommandName.ToString());


            try
            {
                Response.Redirect("~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID=" + DataID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void lb_Ticket_Command(object sender, CommandEventArgs e)
        {
            var link = sender as LinkButton;
            int DataID = int.Parse(link.CommandName.ToString());
            try
            {
                Response.Redirect("~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID=" + DataID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region OpenIssuesCount
        private void OpenIssuesCount(int personID)
        {
            try
            {
                var dt_tickethistory = DAL.Operations.OpTicketHistory.GetOpenIncidentsByPersonID(personID);
                DataTable dt_TicketHistoryOpen = DAL.Helper.ListToDataset.ToDataSet(dt_tickethistory).Tables[0];
                DataTable dt_TicketInformation = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];

                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("ApplicationID", typeof(int));

                var result = from T1 in dt_TicketHistoryOpen.AsEnumerable()
                             join T2 in dt_TicketInformation.AsEnumerable()

                             on T1.Field<int>("TicketInformationID") equals T2.Field<int>("TicketInformationID")
                             select dt_new.LoadDataRow(new object[]
                             {
                                 T2.Field<int>("ApplicationID"),
                             }, false);

                if (result.Count() > 0)
                {
                    var reus = result.GroupBy(x => x.Field<int>("ApplicationID")).Select(x => new { x.Key, Count = x.Count() }).ToList();
                    DataTable dt_result = DAL.Helper.ListToDataset.ToDataSet(reus).Tables[0];
                    DataTable dt_Applications = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                    DataTable dT_merge = new DataTable();
                    dT_merge.Columns.Add("ApplicationID", typeof(int));
                    dT_merge.Columns.Add("Name", typeof(string));
                    dT_merge.Columns.Add("Count", typeof(int));

                    var resul2 = from T1 in dt_result.AsEnumerable()
                                 join T2 in dt_Applications.AsEnumerable()
                                 on T1.Field<int>("Key") equals T2.Field<int>("ApplicationsID")
                                 select dT_merge.LoadDataRow(new object[]
                                 {
                                     T2.Field<int>("ApplicationsID"),
                                     T2.Field<string>("Name"),
                                     T1.Field<int>("Count"),
                                 }, false);

                    if (resul2.Count() > 0)
                    {
                        gv_OIC.DataSource = resul2.OrderByDescending(x => x.Field<int>("Count")).CopyToDataTable();
                        gv_OIC.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void gv_OIC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int personid = int.Parse(Session["PersonID"].ToString());
            try
            {
                gv_OIC.PageIndex = e.NewPageIndex;
                OpenIssuesCount(personid);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void LB_ApplcationName_Command(object sender, CommandEventArgs e)
        {
            var link = sender as LinkButton;
            int DataID = int.Parse(link.CommandName.ToString());
            try
            {
                Response.Redirect("~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID=" + DataID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region CloseIssueCount
        private void CloseIssuesCount(int personID)
        {
            try
            {
                //DataTable dt_TicketHistoryClose = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketHistory.GetCloseIncidentsByPersonID(personID)).Tables[0];
                //DataTable dt_person = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpPersonInformation.GetAll().Where(x => x.PersonInformationID == personID).ToList()).Tables[0];

                var dt_tickethistory = DAL.Operations.OpTicketHistory.GetCloseIncidentsByPersonID(personID);
                DataTable dt_TicketHistoryClose = DAL.Helper.ListToDataset.ToDataSet(dt_tickethistory).Tables[0];
                DataTable dt_TicketInformation = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];

                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("ApplicationID", typeof(int));

                var result = from T1 in dt_TicketHistoryClose.AsEnumerable()
                             join T2 in dt_TicketInformation.AsEnumerable()

                             on T1.Field<int>("TicketInformationID") equals T2.Field<int>("TicketInformationID")
                             select dt_new.LoadDataRow(new object[]
                             {
                                 T2.Field<int>("ApplicationID"),
                             }, false);

                if (result.Count() > 0)
                {
                    var reus = result.GroupBy(x => x.Field<int>("ApplicationID")).Select(x => new { x.Key, Count = x.Count() }).ToList();
                    DataTable dt_result = DAL.Helper.ListToDataset.ToDataSet(reus).Tables[0];
                    DataTable dt_Applications = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                    DataTable dT_merge = new DataTable();
                    dT_merge.Columns.Add("ApplicationID", typeof(int));
                    dT_merge.Columns.Add("Name", typeof(string));
                    dT_merge.Columns.Add("Count", typeof(int));

                    var resul2 = from T1 in dt_result.AsEnumerable()
                                 join T2 in dt_Applications.AsEnumerable()
                                 on T1.Field<int>("Key") equals T2.Field<int>("ApplicationsID")
                                 select dT_merge.LoadDataRow(new object[]
                                 {
                                     T2.Field<int>("ApplicationsID"),
                                     T2.Field<string>("Name"),
                                     T1.Field<int>("Count"),
                                 }, false);

                    if (resul2.Count() > 0)
                    {
                        gv_CIC.DataSource = resul2.OrderByDescending(x => x.Field<int>("Count")).CopyToDataTable();
                        gv_CIC.DataBind();
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void gv_CIC_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int personid = int.Parse(Session["PersonID"].ToString());
            try
            {
                gv_CIC.PageIndex = e.NewPageIndex;
                CloseIssuesCount(personid);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion



        #region InquiryCount

        private void InquiryCount(int PersonID)
        {
            try
            {
                string personname = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(PersonID).FullName;
                var inquiries = DAL.Operations.OpInquiryDetails.GetAll().Where(x => x.CreatedBy == personname && x.NewInquiry == true).GroupBy(x => x.ApplicationID).Select(x => new { x.Key, Count = x.Count() }).ToList();
                DataTable dt_inquires = DAL.Helper.ListToDataset.ToDataSet(inquiries.OrderByDescending(x=>x.Count).ToList()).Tables[0];
                DataTable dt_applications = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                DataTable dt_new = new DataTable();
                dt_new.Columns.Add("ApplicationID",typeof(int));
                dt_new.Columns.Add("ApplicationDesc", typeof(string));
                dt_new.Columns.Add("Count", typeof(string));


                var result = from T1 in dt_inquires.AsEnumerable()
                             join T2 in dt_applications.AsEnumerable()

                             on T1.Field<int>("Key") equals T2.Field<int>("ApplicationsID")
                             select dt_new.LoadDataRow(new object[]
                             {
                                 T2.Field<int>("ApplicationsID"),
                                 T2.Field<string>("Name"),
                                 T1.Field<int>("Count"),


                             }, false);

                if (result.Count() > 0) 
                {
                    rptr_InqCount.DataSource = result.CopyToDataTable();
                    rptr_InqCount.DataBind();
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        #endregion

        #region UserAverageTATClosingIssue



        #endregion


    }
}