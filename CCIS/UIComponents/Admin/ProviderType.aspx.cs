using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class ProviderType : System.Web.UI.Page
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

        public DataTable GetData()
        {
            try
            {
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.ProviderType>(DAL.Operations.OpProviderType.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dt;
        }
        public void populate_grid()
        {
            try
            {
                dt = GetData();
                if (dt.Rows.Count > 0)
                {
                    GV_ProviderType.DataSource = dt;
                    GV_ProviderType.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_ProviderType.DataSource = dt;
                    GV_ProviderType.DataBind();
                    GV_ProviderType.Rows[0].Cells.Clear();
                    GV_ProviderType.Rows[0].Cells.Add(new TableCell());
                    GV_ProviderType.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_ProviderType.Rows[0].Cells[0].Text = "No Records Found";
                    GV_ProviderType.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_ProviderType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string ProviderName = (GV_ProviderType.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();

                    Entities.ProviderType pt = new Entities.ProviderType
                    {
                        Description = ProviderName,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };
                    int i = DAL.Operations.OpProviderType.InsertRecord(pt);

                    if (i > 0)
                    {
                        lbl_message.Text = "Record added successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + i;
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
        protected void GV_ProviderType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_ProviderType.Rows[e.RowIndex].FindControl("txt_ProviderTypeID") as TextBox).Text.Trim());
                string ProviderName = (GV_ProviderType.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                GV_ProviderType.EditIndex = -1;
                Entities.ProviderType pt = new Entities.ProviderType
                {
                    Description = ProviderName,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };
                int i = DAL.Operations.OpProviderType.UpdateProviderType(pt, id);
                if (i > 0) 
                {
                    lbl_message.Text = "Record updated successfully";
                }
                else
                {
                    lbl_message.Text = "ErrorCode: " + i;
                }

                Enable_Footer();
                populate_grid();

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_ProviderType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_ProviderType.DataKeys[e.RowIndex].Value.ToString());
                if(DAL.Operations.OpProviderType.DeletebyID(id))
                {
                    lbl_message.Text = "Record deleted successfully";
                }
                else
                {
                    lbl_message.Text = "Record deleltion failed";
                }

                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private void Enable_Footer()
        {
            GV_ProviderType.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_ProviderType.ShowFooter = false;
        }

        protected void GV_ProviderType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_ProviderType.EditIndex = e.NewEditIndex;
                GV_ProviderType.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_ProviderType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_ProviderType.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
    }
}