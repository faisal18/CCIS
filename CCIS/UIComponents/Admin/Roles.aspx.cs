using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Roles : System.Web.UI.Page
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
                    GV_Roles.DataSource = dt;
                    GV_Roles.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Roles.DataSource = dt;
                    GV_Roles.DataBind();
                    GV_Roles.Rows[0].Cells.Clear();
                    GV_Roles.Rows[0].Cells.Add(new TableCell());
                    GV_Roles.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Roles.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Roles.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Roles>(DAL.Operations.OpRoles.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Roles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    //int ProviderId = Convert.ToInt32((GV_Roles.FooterRow.FindControl("lbl_RolesID") as Label).Text.Trim());
                    string Description = (GV_Roles.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();

                    GV_Roles.EditIndex = -1;


                    Entities.Roles roles = new Entities.Roles
                    {
                        Description = Description,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpRoles.InsertRecord(roles);
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
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Roles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int ProviderId = Convert.ToInt32((GV_Roles.Rows[e.RowIndex].FindControl("lbl_RolesID") as Label).Text.Trim());
                string Description = (GV_Roles.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();

                GV_Roles.EditIndex = -1;


                Entities.Roles roles = new Entities.Roles
                {
                    Description = Description,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpRoles.UpdateRecord(roles, ProviderId);
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
        protected void GV_Roles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Roles.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpRoles.DeletebyID(id))
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

        protected void GV_Roles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_Roles.EditIndex = e.NewEditIndex;
                GV_Roles.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Roles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Roles.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Roles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Roles.PageIndex = e.NewPageIndex;
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
            GV_Roles.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Roles.ShowFooter = false;
        }
    }
}