using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class SLADeclaration : System.Web.UI.Page
    {
        public static DataTable dt = new DataTable();
        public static DataTable dt_itemtypes = new DataTable();
        public static DataTable dt_Statuses = new DataTable();
        public static DataTable dt_application = new DataTable();

        public static string SessionName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                SessionName = Session["Name"].ToString();
                if (!IsPostBack)
                {
                    populate_grid();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        public void populate_grid()
        {
            try
            {

                GetData();
                DataTable newDT = MergeDataTables();
                if (newDT.Rows.Count > 0)
                {
                    GV_SLADeclaration.DataSource = newDT;
                    GV_SLADeclaration.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_SLADeclaration.DataSource = newDT;
                    GV_SLADeclaration.DataBind();
                    GV_SLADeclaration.Rows[0].Cells.Clear();
                    GV_SLADeclaration.Rows[0].Cells.Add(new TableCell());
                    GV_SLADeclaration.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_SLADeclaration.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_SLADeclaration.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        public void GetData()
        {
            try
            {
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.SLADeclarations>(DAL.Operations.OpSLADeclarations.GetAll()).Tables[0];
                dt_itemtypes = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                dt_Statuses = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                dt_application = DAL.Helper.ListToDataset.ToDataSet<Entities.Applications>(DAL.Operations.OpApplications.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable MergeDataTables()
        {
            DataTable New_DT = new DataTable();

            try
            {
                New_DT.Columns.Add("SLADeclarationsID", typeof(int));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("isActive", typeof(Boolean));
                New_DT.Columns.Add("SLASequenceID", typeof(int));
                New_DT.Columns.Add("TimeinMinutes", typeof(int));
                New_DT.Columns.Add("StatusDescription", typeof(string));
                New_DT.Columns.Add("SubStatusDescription", typeof(string));
                New_DT.Columns.Add("ActionRequiredDescription", typeof(string));
                New_DT.Columns.Add("PriorityDescription", typeof(string));
                New_DT.Columns.Add("SeverityDescription", typeof(string));
                New_DT.Columns.Add("ApplicationDescription", typeof(string));

                New_DT.Columns.Add("NotificationTypeDescription", typeof(string));
                New_DT.Columns.Add("TicketTypeDescription", typeof(string));



                var LINQ_SLA = dt.AsEnumerable();
                var LINQ_ItemType = dt_itemtypes.AsEnumerable();
                var LINQ_Statuses = dt_Statuses.AsEnumerable();
                var LINQ_Application = dt_application.AsEnumerable();

                var result = from T1 in LINQ_SLA

                             join T2 in LINQ_ItemType
                             on T1.Field<int>("SeverityID") equals T2.Field<int>("ItemTypesID")
                             join T3 in LINQ_ItemType
                             on T1.Field<int>("PriorityID") equals T3.Field<int>("ItemTypesID")

                             join T6 in LINQ_ItemType
                             on T1.Field<int>("ActionRequiredID") equals T6.Field<int>("ItemTypesID")

                             join T7 in LINQ_Application
                             on T1.Field<int>("ApplicationID") equals T7.Field<int>("ApplicationsID")

                             join T4 in LINQ_Statuses
                             on T1.Field<int>("StatusID") equals T4.Field<int>("StatusesID")
                             join T5 in LINQ_Statuses
                             on T1.Field<int>("SubStatusID") equals T5.Field<int>("StatusesID")
                             
                             join T8 in LINQ_ItemType
                             on T1.Field<int>("NotificationType") equals T8.Field<int>("ItemTypesID")

                             join T9 in LINQ_Statuses
                            on T1.Field<int>("TicketTypeID") equals T9.Field<int>("StatusesID")




                             orderby T1.Field<int>("SLADeclarationsID")
                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("SLADeclarationsID"),
                                 T1.Field<string>("Description"),
                                 T1.Field<Boolean>("isActive"),
                                 T1.Field<int>("SLASequenceID"),
                                 T1.Field<int>("TimeinMinutes"),
                                 T4.Field<string>("Description"),
                                 T5.Field<string>("Description"),
                                 T6.Field<string>("Description"),
                                 T3.Field<string>("Description"),
                                 T2.Field<string>("Description"),
                                 T7.Field<string>("Name"),

                                 T8.Field<string>("Description"),
                                 T9.Field<string>("Description"),


                             }, false);
                New_DT = result.CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }


            return New_DT;
        }

        protected void GV_SLADeclaration_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string description = (GV_SLADeclaration.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    int IsActive = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_IsActiveFooter") as DropDownList).SelectedItem.Value.Trim());
                    int SequenceID = int.Parse((GV_SLADeclaration.FooterRow.FindControl("txt_SLASequenceIDFooter") as TextBox).Text.Trim());

                    int TimeinMinutes = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("txt_TimeinMinutesFooter") as TextBox).Text.Trim());

                    int StatusId = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_StatusFooter") as DropDownList).SelectedItem.Value.Trim());
                    int SubStatusId = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_SubStatusFooter") as DropDownList).SelectedItem.Value.Trim());
                    int ActionRequired = int.Parse((GV_SLADeclaration.FooterRow.FindControl("ddl_ActionRequiredFooter") as DropDownList).SelectedItem.Value.Trim());
                    int PriorityId = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_PriorityFooter") as DropDownList).SelectedItem.Value.Trim());
                    int SeverityId = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_SeverityFooter") as DropDownList).SelectedItem.Value.Trim());

                    int ApplicationId = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_ApplicationFooter") as DropDownList).SelectedItem.Value.Trim());

                    int NotificationType = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_NotificationFooter") as DropDownList).SelectedItem.Value.Trim());
                    int TicketType = Convert.ToInt32((GV_SLADeclaration.FooterRow.FindControl("ddl_TicketTypeFooter") as DropDownList).SelectedItem.Value.Trim());




                    Entities.SLADeclarations sLADeclarations = new Entities.SLADeclarations
                    {
                        ApplicationID = ApplicationId,
                        Description = description,
                        SLASequenceID = SequenceID,
                        isActive = Convert.ToBoolean(IsActive),
                        TimeinMinutes = TimeinMinutes,
                        StatusID = StatusId,
                        SubStatusID = SubStatusId,
                        ActionRequiredID = ActionRequired,
                        PriorityID = PriorityId,
                        SeverityID = SeverityId,
                        NotificationType = NotificationType,
                        TicketTypeID = TicketType,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpSLADeclarations.InsertRecord(sLADeclarations);
                    if (result > 0)
                    {
                        lbl_message.Text = "Record added successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + result;
                    }

                    Enable_Footer();
                    populate_grid();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SLADeclaration_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int ID = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("lbl_SLADeclarationsID") as Label).Text.Trim());
                string description = (GV_SLADeclaration.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();

                int SequenceID = int.Parse((GV_SLADeclaration.Rows[e.RowIndex].FindControl("txt_SLASequenceID") as TextBox).Text.Trim());
                int IsActive = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_IsActive") as DropDownList).SelectedItem.Value.Trim());
                int TimeinMinutes = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("txt_TimeinMinutes") as TextBox).Text.Trim());

                int StatusId = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_Status") as DropDownList).SelectedItem.Value.Trim());
                int SubStatusId = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_SubStatus") as DropDownList).SelectedItem.Value.Trim());


                int ActionRequired = int.Parse((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_ActionRequired") as DropDownList).SelectedItem.Value.Trim());
                int PriorityId = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_Priority") as DropDownList).SelectedItem.Value.Trim());
                int SeverityId = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_Severity") as DropDownList).SelectedItem.Value.Trim());
                int AppllicationId = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_Application") as DropDownList).SelectedItem.Value.Trim());


                int NotificationID = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_Notification") as DropDownList).SelectedItem.Value.Trim());
                int TicketTypeID = Convert.ToInt32((GV_SLADeclaration.Rows[e.RowIndex].FindControl("ddl_TicketType") as DropDownList).SelectedItem.Value.Trim());




                GV_SLADeclaration.EditIndex = -1;

                Entities.SLADeclarations sLADeclarations = new Entities.SLADeclarations
                {
                    ApplicationID = AppllicationId,
                    Description = description,
                    StatusID = StatusId,
                    SLASequenceID = SequenceID,
                    SubStatusID = SubStatusId,
                    isActive = Convert.ToBoolean(IsActive),
                    TimeinMinutes = TimeinMinutes,
                    ActionRequiredID = ActionRequired,
                    PriorityID = PriorityId,
                    SeverityID = SeverityId,
                    NotificationType = NotificationID,
                    TicketTypeID = TicketTypeID,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpSLADeclarations.UpdateRecord(sLADeclarations, ID);
                if (result > 0)
                {
                    lbl_message.Text = "Record updated successfully";
                }
                else
                {
                    lbl_message.Text = "ErrorCode: " + result;
                }


                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SLADeclaration_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_SLADeclaration.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpSLADeclarations.DeletebyID(id))
                {
                    lbl_message.Text = "Record deleted successfully";
                }
                else
                {
                    lbl_message.Text = "Record deleltion failed";
                }
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SLADeclaration_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_SLADeclaration.EditIndex == e.Row.RowIndex)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_Priority");
                        DataTable DT_Priority = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole.DataSource = DT_Priority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "ItemTypesID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_Severity");
                        DataTable DT_Severity = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole2.DataSource = DT_Severity.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                        ddlRole2.DataTextField = "Description";
                        ddlRole2.DataValueField = "ItemTypesID";
                        ddlRole2.DataBind();

                        
                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_Status");
                        DataTable DT_Status = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole3.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Incident Status")).CopyToDataTable();
                        ddlRole3.DataTextField = "Description";
                        ddlRole3.DataValueField = "StatusesID";
                        ddlRole3.DataBind();

                        DropDownList ddlRole4 = (DropDownList)e.Row.FindControl("ddl_SubStatus");
                        DataTable DT_SubStatus = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole4.DataSource = DT_SubStatus.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Sub Incident Status")).CopyToDataTable();
                        ddlRole4.DataTextField = "Description";
                        ddlRole4.DataValueField = "StatusesID";
                        ddlRole4.DataBind();

                        DropDownList ddlRole5 = (DropDownList)e.Row.FindControl("ddl_ActionRequired");
                        DataTable DT_Action = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole5.DataSource = DT_Action.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Action Required")).CopyToDataTable();
                        ddlRole5.DataTextField = "Description";
                        ddlRole5.DataValueField = "ItemTypesID";
                        ddlRole5.DataBind();


                        DropDownList ddlRole6 = (DropDownList)e.Row.FindControl("ddl_Application");
                        DataTable dt_Application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlRole6.DataSource = dt_Application.AsEnumerable().CopyToDataTable();
                        ddlRole6.DataTextField = "Name";
                        ddlRole6.DataValueField = "ApplicationsID";
                        ddlRole6.DataBind();


                        DropDownList ddlRole7 = (DropDownList)e.Row.FindControl("ddl_Notification");
                        DataTable DT_Notification = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole7.DataSource = DT_Notification.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Notification Type")).CopyToDataTable();
                        ddlRole7.DataTextField = "Description";
                        ddlRole7.DataValueField = "ItemTypesID";
                        ddlRole7.DataBind();

                        DropDownList ddlRole8 = (DropDownList)e.Row.FindControl("ddl_TicketType");
                        DataTable DT_TicketType = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole8.DataSource = DT_TicketType.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Ticket Type")).CopyToDataTable();
                        ddlRole8.DataTextField = "Description";
                        ddlRole8.DataValueField = "StatusesID";
                        ddlRole8.DataBind();

                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_PriorityFooter");
                        DataTable DT_Priority = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole.DataSource = DT_Priority.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Priority")).CopyToDataTable();
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "ItemTypesID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_SeverityFooter");
                        DataTable DT_Severity = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole2.DataSource = DT_Severity.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Severity")).CopyToDataTable();
                        ddlRole2.DataTextField = "Description";
                        ddlRole2.DataValueField = "ItemTypesID";
                        ddlRole2.DataBind();

                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_StatusFooter");
                        DataTable DT_Status = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole3.DataSource = DT_Status.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Incident Status")).CopyToDataTable();
                        ddlRole3.DataTextField = "Description";
                        ddlRole3.DataValueField = "StatusesID";
                        ddlRole3.DataBind();

                        DropDownList ddlRole4 = (DropDownList)e.Row.FindControl("ddl_SubStatusFooter");
                        DataTable DT_SubStatus = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole4.DataSource = DT_SubStatus.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Sub Incident Status")).CopyToDataTable();
                        ddlRole4.DataTextField = "Description";
                        ddlRole4.DataValueField = "StatusesID";
                        ddlRole4.DataBind();

                        DropDownList ddlRole5 = (DropDownList)e.Row.FindControl("ddl_ActionRequiredFooter");
                        DataTable DT_Action = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole5.DataSource = DT_Action.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Action Required")).CopyToDataTable();
                        ddlRole5.DataTextField = "Description";
                        ddlRole5.DataValueField = "ItemTypesID";
                        ddlRole5.DataBind();

                        DropDownList ddlRole6 = (DropDownList)e.Row.FindControl("ddl_ApplicationFooter");
                        DataTable Dt_Application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlRole6.DataSource = Dt_Application.AsEnumerable().CopyToDataTable();
                        ddlRole6.DataTextField = "Name";
                        ddlRole6.DataValueField = "ApplicationsID";
                        ddlRole6.DataBind();


                        DropDownList ddlRole7 = (DropDownList)e.Row.FindControl("ddl_NotificationFooter");
                        DataTable DT_Notification = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                        ddlRole7.DataSource = DT_Notification.AsEnumerable().Where(r => r.Field<string>("Categories").Equals("Notification Type")).CopyToDataTable();
                        ddlRole7.DataTextField = "Description";
                        ddlRole7.DataValueField = "ItemTypesID";
                        ddlRole7.DataBind();

                        DropDownList ddlRole8 = (DropDownList)e.Row.FindControl("ddl_TicketTypeFooter");
                        DataTable DT_TicketType = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                        ddlRole8.DataSource = DT_TicketType.AsEnumerable().Where(r => r.Field<string>("Types").Equals("Ticket Type")).CopyToDataTable();
                        ddlRole8.DataTextField = "Description";
                        ddlRole8.DataValueField = "StatusesID";
                        ddlRole8.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_SLADeclaration_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_SLADeclaration.EditIndex = e.NewEditIndex;
                GV_SLADeclaration.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SLADeclaration_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_SLADeclaration.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SLADeclaration_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_SLADeclaration.PageIndex = e.NewPageIndex;
                populate_grid();
                Enable_Footer();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private void Enable_Footer()
        {
            GV_SLADeclaration.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_SLADeclaration.ShowFooter = false;
        }


        private void CheckSLARecords()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}