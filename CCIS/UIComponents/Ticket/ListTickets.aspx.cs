using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets
{
    public partial class ListTickets : System.Web.UI.Page
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
            }
        }

        private void PopulateGrid()
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(), LoadPersonInformation(), LoadCallerInformation(),LoadTicketStatus());
                dt_TicketInformation = dt_TicketInformation.AsEnumerable().Where(x => x.Field<string>("Status").ToLower() != "closed").CopyToDataTable();

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

        private DataTable LoadTicketInformation()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketInformation>(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];
            }
            catch(Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable LoadTicketHistory()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketHistory>(DAL.Operations.OpTicketHistory.GetAll()).Tables[0];
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
                dataTable =  DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
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
                dataTable =  DAL.Helper.ListToDataset.ToDataSet<Entities.CallerInformation>(DAL.Operations.OpCallerInfo.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable MergeData(DataTable dt_TicketInformation,DataTable dt_TicketHistory,DataTable dt_personinformation,DataTable dt_caller, DataTable dt_TicketStatus)
        {
            DataTable New_DT = new DataTable();
            int IncidentType = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeIncident"));

            try
            {
                New_DT.Columns.Add("TicketInformationID", typeof(int));
                New_DT.Columns.Add("TicketNumber", typeof(string));

                New_DT.Columns.Add("CallerID", typeof(int));
                New_DT.Columns.Add("CallerName", typeof(string));
                New_DT.Columns.Add("CallerLicense", typeof(string));

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
                List_MAX_TicketHistoryId = MaxQuery.Select(x=>x.Field<int>("tickethistoryid")).ToList();

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
                             
                             join T7 in dt_TicketStatus.AsEnumerable()
                             on T1.Field<int>("TicketType") equals T7.Field<int>("StatusesID") 

                             where (List_MAX_TicketHistoryId.Contains(T2.Field<int>("tickethistoryid"))
                             //&& T1.Field<int>("TicketType") == IncidentType)
                             )
                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("TicketInformationID"),
                                 T1.Field<string>("TicketNumber"),
                                 T3.Field<int>("CallerInformationID"),
                                 T3.Field<string>("name"),
                                 T3.Field<string>("CallerLicense"),
                                 T1.Field<string>("Subject"),
                                 T4.Field<string>("FullName"),
                                 T5.Field<string>("FullName"),
                                 T1.Field<DateTime>("CreationDate"),
                                 T6.Field<string>("Description"),
                                 T7.Field<string>("Description"),


                             }, false);


                New_DT = result.AsEnumerable().OrderByDescending(x => x.Field<DateTime>("CreationDate")).CopyToDataTable();

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
                    Response.Redirect("ViewTicket.aspx?TicketInformationID=" + TicketInformaitonID, false);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_ListTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_ListTickets.PageIndex = e.NewPageIndex;
                if(txt_searchbox.Text.Length>0)
                {
                    BindGrid();
                }
                else
                {
                    PopulateGrid();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
        protected void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_TicketInformation = new DataTable();
                dt_TicketInformation = MergeData(LoadTicketInformation(), LoadTicketHistory(), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());

                if (txt_searchbox.Text.Length > 1)
                {
                    string category = ddl_search.SelectedValue;
                    string search_query = txt_searchbox.Text;

                    var result = dt_TicketInformation.AsEnumerable().Where(x => x.Field<string>(category) == search_query || x.Field<string>(category).Contains(search_query)).ToList();
                    if (result.Count > 0)
                    {
                        GV_ListTickets.DataSource = result.CopyToDataTable();
                        GV_ListTickets.DataBind();
                    }
                }
                if (txt_searchbox.Text.Length > 1) 
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }

        private DataTable GetSearchResults(string category,string search_query)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = MergeData(LoadTicketInformation(), LoadTicketHistory(), LoadPersonInformation(), LoadCallerInformation(), LoadTicketStatus());
                var result  = dt.AsEnumerable().Where(x => x.Field<string>(category).ToLower().Contains(search_query.ToLower())).ToList();
                if (result.Count() > 0) 
                {
                    dt = result.CopyToDataTable();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return dt;
        }
        private void BindGrid()
        {
            try
            {
                DataTable dataTable = GetSearchResults(ddl_search.SelectedValue, txt_searchbox.Text);
                GV_ListTickets.DataSource = dataTable;
                GV_ListTickets.DataBind();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}