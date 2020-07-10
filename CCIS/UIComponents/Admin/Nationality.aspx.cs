using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Nationality : System.Web.UI.Page
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
                    GV_Nationality.DataSource = dt;
                    GV_Nationality.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Nationality.DataSource = dt;
                    GV_Nationality.DataBind();
                    GV_Nationality.Rows[0].Cells.Clear();
                    GV_Nationality.Rows[0].Cells.Add(new TableCell());
                    GV_Nationality.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Nationality.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Nationality.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Nationality>(DAL.Operations.OpNationality.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return dt;
        }

        protected void GV_Nationality_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string NationalityDesc = (GV_Nationality.FooterRow.FindControl("txt_NationalityDescFooter") as TextBox).Text.Trim();

                    Entities.Nationality nationality = new Entities.Nationality
                    {
                        Description = NationalityDesc,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int i = DAL.Operations.OpNationality.InsertRecord(nationality);

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
        protected void GV_Nationality_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_Nationality.Rows[e.RowIndex].FindControl("txt_NationalityId") as Label).Text.Trim());
                string NationalityDesc = (GV_Nationality.Rows[e.RowIndex].FindControl("txt_NationalityDesc") as TextBox).Text.Trim();
                GV_Nationality.EditIndex = -1;

                Entities.Nationality nationality = new Entities.Nationality
                {
                    Description = NationalityDesc,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int i = DAL.Operations.OpNationality.UpdateNationality(nationality, id);
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
        protected void GV_Nationality_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Nationality.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpNationality.DeletebyID(id))
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

        protected void GV_Nationality_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_Nationality.EditIndex = e.NewEditIndex;
                GV_Nationality.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Nationality_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Nationality.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }

        private void Enable_Footer()
        {
            GV_Nationality.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Nationality.ShowFooter = false;
        }


    }
}