using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Payers : System.Web.UI.Page
    {

        public static DataTable dt = new DataTable();
        public static DataTable dt_payerType = new DataTable();

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
                if (dt.Rows.Count > 0)
                {
                    GV_Payer.DataSource = newDT;
                    GV_Payer.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_Payer.Rows[0].Cells.Clear();
                    GV_Payer.Rows[0].Cells.Add(new TableCell());
                    GV_Payer.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_Payer.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Payer.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Payers>(DAL.Operations.OpPayers.GetAll()).Tables[0];
                dt_payerType = DAL.Helper.ListToDataset.ToDataSet<Entities.PayerType>(DAL.Operations.OpPayerType.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dt;
        }
        private DataTable MergeDataTables()
        {
            DataTable New_DT = new DataTable();

            try
            {

                New_DT.Columns.Add("PayerId", typeof(int));
                New_DT.Columns.Add("PayerCode", typeof(string));
                New_DT.Columns.Add("PayerName", typeof(string));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("IsActive", typeof(Boolean));
                New_DT.Columns.Add("Email", typeof(string));
                New_DT.Columns.Add("LoginKey", typeof(string));

                var LINQ_Payers = dt.AsEnumerable();
                var LINQ_PayerType = dt_payerType.AsEnumerable();

                var result = from T1 in LINQ_Payers
                             join T2 in LINQ_PayerType
                             on T1.Field<int>("PayerTypeID") equals T2.Field<int>("PayerTypeID")

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("PayerId"),
                                 T1.Field<string>("PayerCode"),
                                 T1.Field<string>("PayerName"),
                                 T2.Field<string>("Description"),
                                 T1.Field<Boolean>("IsActive"),
                                 T1.Field<string>("Email"),
                                 T1.Field<string>("LoginKey"),

                             }, false);
                New_DT = result.CopyToDataTable();

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return New_DT;
        }

        protected void GV_Payer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string PayerId = (GV_Payer.FooterRow.FindControl("txt_PayerIdFooter") as TextBox).Text.Trim();
                    string PayerName = (GV_Payer.FooterRow.FindControl("txt_PayerNameFooter") as TextBox).Text.Trim();
                    string PayerCode = (GV_Payer.FooterRow.FindControl("txt_PayerCodeFooter") as TextBox).Text.Trim();

                    string PayerTypeId = (GV_Payer.FooterRow.FindControl("ddl_PayerTypeFooter") as DropDownList).SelectedItem.Value.Trim();
                    string PayerType = (GV_Payer.FooterRow.FindControl("ddl_PayerTypeFooter") as DropDownList).SelectedItem.Text.Trim();
                    bool PayerisActive = Convert.ToBoolean((GV_Payer.FooterRow.FindControl("txt_PayerIsActiveFooter") as DropDownList).SelectedItem.Text.Trim());

                    string PayerEmail = (GV_Payer.FooterRow.FindControl("txt_EmailFooter") as TextBox).Text.Trim();
                    string PayerLoginKey = (GV_Payer.FooterRow.FindControl("txt_LoginKeyFooter") as TextBox).Text.Trim();


                    Entities.Payers payers = new Entities.Payers
                    {
                        PayerID = Convert.ToInt32(PayerId),
                        PayerCode = PayerCode,
                        PayerName = PayerName,
                        PayerTypeID = Convert.ToInt32(PayerTypeId),
                        IsActive = PayerisActive,
                        Email = PayerEmail,
                        LoginKey = PayerLoginKey,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int i = DAL.Operations.OpPayers.InsertRecord(payers);
                    if (i > 0)
                    {
                        lbl_message.Text = "Record added successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + i;
                    }
                    populate_grid();

                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Payer_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int PayerId = Convert.ToInt32((GV_Payer.Rows[e.RowIndex].FindControl("txt_PayerId") as TextBox).Text.Trim());
                string PayerName = (GV_Payer.Rows[e.RowIndex].FindControl("txt_PayerName") as TextBox).Text.Trim();
                string PayerCode = (GV_Payer.Rows[e.RowIndex].FindControl("txt_PayerCode") as TextBox).Text.Trim();

                string PayerTypeId = (GV_Payer.Rows[e.RowIndex].FindControl("ddl_PayerType") as DropDownList).SelectedItem.Value.Trim();
                string PayerType = (GV_Payer.Rows[e.RowIndex].FindControl("ddl_PayerType") as DropDownList).SelectedItem.Text.Trim();
                bool PayerisActive = Convert.ToBoolean((GV_Payer.Rows[e.RowIndex].FindControl("ddl_IsActive") as DropDownList).SelectedItem.Text.Trim());

                string PayerEmail = (GV_Payer.Rows[e.RowIndex].FindControl("txt_Email") as TextBox).Text.Trim();
                string PayerLoginKey = (GV_Payer.Rows[e.RowIndex].FindControl("txt_LoginKey") as TextBox).Text.Trim();

                GV_Payer.EditIndex = -1;

                Entities.Payers pt = new Entities.Payers
                {
                    PayerID = Convert.ToInt32(PayerId),
                    PayerCode = PayerCode,
                    PayerName = PayerName,
                    PayerTypeID = Convert.ToInt32(PayerTypeId),
                    IsActive = PayerisActive,
                    Email = PayerEmail,
                    LoginKey = PayerLoginKey,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };
                int i = DAL.Operations.OpPayers.UpdateRecord(pt, PayerId);
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
        protected void GV_Payer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Payer.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpPayers.DeletebyID(id))
                {
                    lbl_message.Text = "Record deleted successfully";
                }
                else
                {
                    lbl_message.Text = "Record deleltion failed";
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);

            }
        }
        protected void GV_Payer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_Payer.EditIndex == e.Row.RowIndex)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_PayerType");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PayerType>(DAL.Operations.OpPayerType.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "PayerTypeID";
                        ddlRole.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_PayerTypeFooter");
                        ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PayerType>(DAL.Operations.OpPayerType.GetAll()).Tables[0];
                        ddlFRRole.DataTextField = "Description";
                        ddlFRRole.DataValueField = "PayerTypeID";
                        ddlFRRole.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Payer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_Payer.EditIndex = e.NewEditIndex;
                GV_Payer.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Payer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Payer.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Payer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Payer.PageIndex = e.NewPageIndex;
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
            GV_Payer.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Payer.ShowFooter = false;
        }

    }
}