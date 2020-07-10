using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Statuses : System.Web.UI.Page
    {
        public static DataTable dt = new DataTable();

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
                if (dt.Rows.Count > 0)
                {
                    GV_Status.DataSource = dt;
                    GV_Status.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Status.DataSource = dt;
                    GV_Status.DataBind();
                    GV_Status.Rows[0].Cells.Clear();
                    GV_Status.Rows[0].Cells.Add(new TableCell());
                    GV_Status.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Status.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Status.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Statuses>(DAL.Operations.OpStatuses.GetAll()).Tables[0];
                dt = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("Types")).CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Status_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string Description = (GV_Status.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    string Types = (GV_Status.FooterRow.FindControl("txt_TypesFooter") as TextBox).Text.Trim();

                    Entities.Statuses statuses = new Entities.Statuses
                    {
                        Description = Description,
                        Types = Types,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpStatuses.InsertRecord(statuses);
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
        protected void GV_Status_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int StatusId = Convert.ToInt32((GV_Status.Rows[e.RowIndex].FindControl("lbl_StatusesID") as Label).Text.Trim());
                string Description = (GV_Status.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                string Types = (GV_Status.Rows[e.RowIndex].FindControl("txt_Types") as TextBox).Text.Trim();


                GV_Status.EditIndex = -1;


                Entities.Statuses statuses = new Entities.Statuses
                {
                    Description = Description,
                    Types = Types,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpStatuses.UpdateRecord(statuses, StatusId);
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
        protected void GV_Status_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Status.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpStatuses.DeletebyID(id))
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

        protected void GV_Status_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_Status.EditIndex = e.NewEditIndex;
                GV_Status.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Status_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Status.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Status_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Status.PageIndex = e.NewPageIndex;
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
            GV_Status.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Status.ShowFooter = false;
        }
    }
}