using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Provider : System.Web.UI.Page
    {

        public static DataTable dt = new DataTable();
        public static DataTable dt_providerType = new DataTable();

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
                    GV_Provider.DataSource = newDT;
                    GV_Provider.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_Provider.DataSource = newDT;
                    GV_Provider.DataBind();
                    GV_Provider.Rows[0].Cells.Clear();
                    GV_Provider.Rows[0].Cells.Add(new TableCell());
                    GV_Provider.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_Provider.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Provider.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Providers>(DAL.Operations.OpProviders.GetAll()).Tables[0];
                dt_providerType = DAL.Helper.ListToDataset.ToDataSet<Entities.ProviderType>(DAL.Operations.OpProviderType.GetAll()).Tables[0];
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
                New_DT.Columns.Add("ProviderID", typeof(int));
                New_DT.Columns.Add("ProviderUID", typeof(int));
                New_DT.Columns.Add("ProviderLicense", typeof(string));
                New_DT.Columns.Add("ProviderName", typeof(string));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("IsActive", typeof(Boolean));
                New_DT.Columns.Add("Emirate", typeof(string));
                New_DT.Columns.Add("Source", typeof(string));

                var LINQ_Providers = dt.AsEnumerable();
                var LINQ_ProviderType = dt_providerType.AsEnumerable();

                var result = from T1 in LINQ_Providers
                             join T2 in LINQ_ProviderType
                             on T1.Field<int>("ProviderTypeID") equals T2.Field<int>("ProviderTypeID")

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("ProviderID"),
                                 T1.Field<int>("ProviderUID"),
                                 T1.Field<string>("ProviderLicense"),
                                 T1.Field<string>("ProviderName"),
                                 T2.Field<string>("Description"),
                                 T1.Field<Boolean>("IsActive"),
                                 T1.Field<string>("Emirate"),
                                 T1.Field<string>("Source"),
                             }, false);
                New_DT = result.CopyToDataTable();
            }
            catch(Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }


            return New_DT;
        }

        protected void GV_Provider_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string ProviderLicense = (GV_Provider.FooterRow.FindControl("txt_ProviderLicenseFooter") as TextBox).Text.Trim();
                    string ProviderName = (GV_Provider.FooterRow.FindControl("txt_ProviderNameFooter") as TextBox).Text.Trim();

                    int ProviderTypeId = Convert.ToInt32((GV_Provider.FooterRow.FindControl("ddl_ProviderTypeFooter") as DropDownList).SelectedItem.Value.Trim());
                    string ProviderType = (GV_Provider.FooterRow.FindControl("ddl_ProviderTypeFooter") as DropDownList).SelectedItem.Text.Trim();
                    bool IsActive = Convert.ToBoolean((GV_Provider.FooterRow.FindControl("ddl_IsActiveFooter") as DropDownList).SelectedItem.Text.Trim());

                    string Emirate = (GV_Provider.FooterRow.FindControl("txt_EmirateFooter") as TextBox).Text.Trim();
                    string Source = (GV_Provider.FooterRow.FindControl("txt_SourceFooter") as TextBox).Text.Trim();


                    Entities.Providers Provs = new Entities.Providers
                    {
                        ProviderLicense = ProviderLicense,
                        ProviderName = ProviderName,
                        ProviderTypeID = ProviderTypeId,
                        IsActive = IsActive,
                        Emirate = Emirate,
                        Source = Source,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpProviders.InsertRecord(Provs);
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
        protected void GV_Provider_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int ProviderId = Convert.ToInt32((GV_Provider.Rows[e.RowIndex].FindControl("txt_ProviderId") as Label).Text.Trim());
                int ProviderUId = Convert.ToInt32((GV_Provider.Rows[e.RowIndex].FindControl("txt_ProviderUId") as Label).Text.Trim());

                string ProviderLicense = (GV_Provider.Rows[e.RowIndex].FindControl("txt_ProviderLicense") as TextBox).Text.Trim();
                string ProviderName = (GV_Provider.Rows[e.RowIndex].FindControl("txt_ProviderName") as TextBox).Text.Trim();

                int ProviderTypeId = Convert.ToInt32((GV_Provider.Rows[e.RowIndex].FindControl("ddl_ProviderType") as DropDownList).SelectedItem.Value.Trim());
                string ProviderType = (GV_Provider.Rows[e.RowIndex].FindControl("ddl_ProviderType") as DropDownList).SelectedItem.Text.Trim();
                bool IsActive = Convert.ToBoolean((GV_Provider.Rows[e.RowIndex].FindControl("ddl_IsActive") as DropDownList).SelectedItem.Text.Trim());

                string Emirate = (GV_Provider.Rows[e.RowIndex].FindControl("txt_Emirate") as TextBox).Text.Trim();
                string Source = (GV_Provider.Rows[e.RowIndex].FindControl("txt_Source") as TextBox).Text.Trim();

                GV_Provider.EditIndex = -1;

                
                Entities.Providers Provs = new Entities.Providers
                {
                    ProviderID = ProviderId,
                    ProviderUID = ProviderUId,
                    ProviderLicense = ProviderLicense,
                    ProviderName = ProviderName,
                    ProviderTypeID = ProviderTypeId,
                    IsActive = IsActive,
                    Emirate = Emirate,
                    Source = Source,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpProviders.UpdateRecord(Provs, ProviderId);
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
        protected void GV_Provider_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Provider.DataKeys[e.RowIndex].Value.ToString());
                if(DAL.Operations.OpProviders.DeletebyID(id))
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
        protected void GV_Provider_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_Provider.EditIndex == e.Row.RowIndex)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_ProviderType");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.ProviderType>(DAL.Operations.OpProviderType.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "ProviderTypeID";
                        ddlRole.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_ProviderTypeFooter");
                        ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.ProviderType>(DAL.Operations.OpProviderType.GetAll()).Tables[0];
                        ddlFRRole.DataTextField = "Description";
                        ddlFRRole.DataValueField = "ProviderTypeID";
                        ddlFRRole.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_Provider_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_Provider.EditIndex = e.NewEditIndex;
                GV_Provider.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Provider_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Provider.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Provider_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Provider.PageIndex = e.NewPageIndex;
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
            GV_Provider.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Provider.ShowFooter = false;
        }

    }
}