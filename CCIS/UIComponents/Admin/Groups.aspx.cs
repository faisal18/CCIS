using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Groups : System.Web.UI.Page
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
                    GV_Groups.DataSource = dt;
                    GV_Groups.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Groups.DataSource = dt;
                    GV_Groups.DataBind();
                    GV_Groups.Rows[0].Cells.Clear();
                    GV_Groups.Rows[0].Cells.Add(new TableCell());
                    GV_Groups.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Groups.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Groups.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Groups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string ProviderDescription = (GV_Groups.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    string GroupEmail = (GV_Groups.FooterRow.FindControl("txt_GroupEmailFooter") as TextBox).Text.Trim();

                    Entities.Groups groups = new Entities.Groups
                    {
                        Description = ProviderDescription,
                        GroupEmail = GroupEmail,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpGroups.InsertRecord(groups);
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
        protected void GV_Groups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_Groups.Rows[e.RowIndex].FindControl("lbl_GroupsID") as Label).Text.Trim());
                string ProviderDescription = (GV_Groups.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                string GroupEmail = (GV_Groups.Rows[e.RowIndex].FindControl("txt_GroupEmail") as TextBox).Text.Trim();


                Entities.Groups groups = new Entities.Groups
                {
                    Description = ProviderDescription,
                    GroupEmail = GroupEmail,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                GV_Groups.EditIndex = -1;

                int result = DAL.Operations.OpGroups.UpdateRecord(groups, id);
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
        protected void GV_Groups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Groups.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpGroups.DeletebyID(id))
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

        protected void GV_Groups_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_Groups.EditIndex = e.NewEditIndex;
                GV_Groups.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Groups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Groups.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Groups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Groups.PageIndex = e.NewPageIndex;
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
            GV_Groups.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Groups.ShowFooter = false;
        }
    }
}