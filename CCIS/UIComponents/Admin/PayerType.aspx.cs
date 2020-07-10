using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Payer_Type : System.Web.UI.Page
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.PayerType>(DAL.Operations.OpPayerType.GetAll()).Tables[0];
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

                GetData();
                if (dt.Rows.Count > 0)
                {
                    GV_PayerType.DataSource = dt;
                    GV_PayerType.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_PayerType.DataSource = dt;
                    GV_PayerType.DataBind();
                    GV_PayerType.Rows[0].Cells.Clear();
                    GV_PayerType.Rows[0].Cells.Add(new TableCell());
                    GV_PayerType.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_PayerType.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_PayerType.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_PayerType_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string PayerDescription = (GV_PayerType.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    Entities.PayerType pt = new Entities.PayerType
                    {
                        Description = PayerDescription,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int i = DAL.Operations.OpPayerType.InsertRecord(pt);

                    if (i > 0)
                    {
                        lbl_message.Text = "Record added successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + i;
                    }
                    populate_grid();
                    Enable_Footer();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        protected void GV_PayerType_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            try
            {
                string PayerDescription = (GV_PayerType.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                int id = Convert.ToInt32((GV_PayerType.Rows[e.RowIndex].FindControl("txt_PayerId") as TextBox).Text.Trim());

                GV_PayerType.EditIndex = -1;

                Entities.PayerType pt = new Entities.PayerType
                {
                    Description = PayerDescription,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };
                int i = DAL.Operations.OpPayerType.UpdatePayerType(pt, id);
                if (i > 0)
                {
                    lbl_message.Text = "Record updated successfully";
                }
                else
                {
                    lbl_message.Text = "ErrorCode: " + i;
                }
                populate_grid();
                Enable_Footer();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }



        }
        protected void GV_PayerType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_PayerType.DataKeys[e.RowIndex].Value.ToString());
                if(DAL.Operations.OpPayerType.DeletebyID(id))
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
            GV_PayerType.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_PayerType.ShowFooter = false;
        }

        protected void GV_PayerType_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_PayerType.EditIndex = e.NewEditIndex;
                GV_PayerType.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }

        }
        protected void GV_PayerType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_PayerType.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        
    }
}