using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Admin
{
    public partial class Email : System.Web.UI.Page
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Email>(DAL.Operations.OpEmail.GetAll()).Tables[0];
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
                    GV_Email.DataSource = dt;
                    GV_Email.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Email.DataSource = dt;
                    GV_Email.DataBind();
                    GV_Email.Rows[0].Cells.Clear();
                    GV_Email.Rows[0].Cells.Add(new TableCell());
                    GV_Email.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Email.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Email.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Email_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string EmailSubject = (GV_Email.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    string EmailDesc = (GV_Email.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    string Category = (GV_Email.FooterRow.FindControl("txt_CategoryFooter") as TextBox).Text.Trim();
                    string NotificationType = (GV_Email.FooterRow.FindControl("txt_NotificationTypeFooter") as TextBox).Text.Trim();

                    Entities.Email pt = new Entities.Email
                    {
                        EmailBody = EmailDesc,
                        EmailSubject = EmailSubject,
                        TemplateType = NotificationType,
                        Category = Category,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int i = DAL.Operations.OpEmail.InsertRecord(pt);

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
        protected void GV_Email_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string Description = (GV_Email.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                string emailSubject = (GV_Email.Rows[e.RowIndex].FindControl("txt_EmailSubject") as TextBox).Text.Trim();
                string Category = (GV_Email.Rows[e.RowIndex].FindControl("txt_Category") as TextBox).Text.Trim();
                string NotificationType = (GV_Email.Rows[e.RowIndex].FindControl("txt_NotificationType") as TextBox).Text.Trim();
                int id = Convert.ToInt32((GV_Email.Rows[e.RowIndex].FindControl("txt_EmailID") as TextBox).Text.Trim());

                GV_Email.EditIndex = -1;

                Entities.Email pt = new Entities.Email
                {
                    TemplateID = id,
                    EmailBody = Description,
                    EmailSubject = emailSubject,
                    TemplateType = NotificationType,
                    Category = Category,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };
                int i = DAL.Operations.OpEmail.UpdateEmail(pt, id);
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
        protected void GV_Email_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Email.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpEmail.DeleteByID(id))
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
            GV_Email.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Email.ShowFooter = false;
        }

        protected void GV_Email_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_Email.EditIndex = e.NewEditIndex;
                GV_Email.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                throw;
            }
        }
        protected void GV_Email_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Email.EditIndex = -1;
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