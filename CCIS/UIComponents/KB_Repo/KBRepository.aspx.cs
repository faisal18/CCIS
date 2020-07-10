using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.KB_Repo
{
    public partial class KBRepository : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session.Keys.Count > 0)
                {
                    string SessionName = Session["Name"].ToString();
                    pnl_ResolDetail.Visible = false;
                    
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if(!IsPostBack)
                {
                    LoadData();
                }
                else
                {
                    LoadData(ddl_Application.SelectedItem.Text);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }
        }

        private void LoadData()
        {
            try
            {

                if (!IsPostBack)
                {
                    ddl_Application.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                    ddl_Application.DataTextField = "Name";
                    ddl_Application.DataValueField = "ApplicationsID";
                    ddl_Application.DataBind();
                    ddl_Application.Items.FindByValue(8.ToString()).Selected = true;

                    PopulateData(ddl_Application.Items[7].Text);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private DataTable LoadData(string ApplicationName)
        {
            DataTable dataTable_New = new DataTable();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ApplicationDesc", typeof(string));
            dataTable.Columns.Add("Value", typeof(string));
            dataTable.Columns.Add("TicketNumber", typeof(string));
            dataTable.Columns.Add("Subject", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ActionTaken", typeof(string));
            dataTable.Columns.Add("TicketInformationID", typeof(int));

            try
            {
                DataTable dt_AppProps = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplicationProps.GetAll()).Tables[0];
                DataTable dt_Application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                DataTable dt_Resolution = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpResolution.GetAll()).Tables[0];
                DataTable dt_TicketInformation = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetAll()).Tables[0];




                var result = from T1 in dt_Resolution.AsEnumerable()
                             join T2 in dt_AppProps.AsEnumerable()
                             on T1.Field<int>("CategoryID") equals T2.Field<int>("ApplicationPropID")

                             join T3 in dt_TicketInformation.AsEnumerable()
                             on T1.Field<int>("TicketInformationID") equals T3.Field<int>("TicketInformationID")

                             join T4 in dt_Application.AsEnumerable()
                             on T3.Field<int>("ApplicationID") equals T4.Field<int>("ApplicationsID")

                             select dataTable.LoadDataRow(new object[]
                             {
                                 T4.Field<string>("Name"),
                                 T2.Field<string>("Value"),
                                 T3.Field<string>("TicketNumber"),
                                 T3.Field<string>("Subject"),
                                 T3.Field<string>("Description"),
                                 T3.Field<string>("ActionTaken"),
                                 T3.Field<int>("TicketInformationID"),
                                 


                             }, false);


                result =  result.Where(x => x.Field<string>("ApplicationDesc") == ApplicationName);

                if (result.Count() > 0) 
                {
                    dataTable_New = result.CopyToDataTable();
                }

                
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable_New;

        }
        private void PopulateData(string ApplicationNAme)
        {
            try
            {
                DataTable dt = LoadData(ApplicationNAme);

                if (dt.Rows.Count > 0)
                {
                    repeater_assignedtosession.DataSource = dt;
                    repeater_assignedtosession.DataBind();
                    lbl_message.Text = "";
                }
                else
                {
                    repeater_assignedtosession.DataSource = null;
                    repeater_assignedtosession.DataBind();
                    lbl_message.Text = "No data exist";
                }


            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
        }

        protected void ddl_Application_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateData(ddl_Application.SelectedItem.Text.Trim().ToString()); 
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void lb_Ticket_Command(object sender, CommandEventArgs e)
        {
            try
            {

                ScriptManager.RegisterStartupScript(this, GetType(), "Pop", "showModal();", true);
                var link = sender as LinkButton;
                int TicketInformationID = int.Parse(link.CommandName.ToString());


                Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(TicketInformationID).First();
                Entities.Resolution resolution = DAL.Operations.OpResolution.GetResolutionByTicketInformationID(TicketInformationID);

                lbl_Description.Text = ticketInformation.Description;
                lbl_ActionTaken.Text = ticketInformation.ActionTaken;

                lbl_CreatedBy.Text = ticketInformation.CreatedBy;
                lbl_ResolvedBy.Text = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(resolution.CreatedBy)).FullName;

                lbl_RootCause.Text = resolution.RootCause;
                lbl_StepsTaken.Text = resolution.Steps;

                pnl_ResolDetail.Visible = true;


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
                string search_query = txt_searchbox.Text;
                string category = ddl_search.SelectedValue;

                if (search_query.Length > 0)
                {
                    DataTable dt = LoadData(ddl_Application.SelectedItem.Text).AsEnumerable().Where(x => x.Field<string>(category) == search_query).CopyToDataTable();
                    repeater_assignedtosession.DataSource = dt;
                    repeater_assignedtosession.DataBind();
                }
                else
                {
                    PopulateData(ddl_Application.SelectedItem.Text);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
    }
}