using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Admin
{
    public partial class ApplicationProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string SessionName = Session["Name"].ToString();
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

        private void populate_grid()
        {
            try
            {
                DataTable dt_new = MergeData(GetDataApplicationProps(), GetDataApplications(),GetDataItemType());
                if (dt_new.Rows.Count > 0)
                {
                    GV_ApplicationProperty.DataSource = dt_new;
                    GV_ApplicationProperty.DataBind();
                }
                else
                {
                    dt_new.Rows.Add(dt_new.NewRow());
                    GV_ApplicationProperty.DataSource = dt_new;
                    GV_ApplicationProperty.DataBind();
                    GV_ApplicationProperty.Rows[0].Cells.Clear();
                    GV_ApplicationProperty.Rows[0].Cells.Add(new TableCell());
                    GV_ApplicationProperty.Rows[0].Cells[0].ColumnSpan = dt_new.Columns.Count;
                    GV_ApplicationProperty.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_ApplicationProperty.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private DataTable GetDataApplicationProps()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplicationProps.GetAll().OrderBy(x => x.Property).ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable GetDataApplications()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll().OrderBy(x => x.Name).ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable GetDataItemType()
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpItemTypes.GetAll().Where(x => x.Categories == System.Configuration.ConfigurationManager.AppSettings.Get("AppPropITypeCat")).OrderBy(x => x.Categories).ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable;
        }
        private DataTable MergeData(DataTable ApplicationProps,DataTable Application,DataTable ItemType)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable.Columns.Add("ApplicationPropID", typeof(int));
                dataTable.Columns.Add("ApplicationName", typeof(string));
                dataTable.Columns.Add("Property", typeof(string));
                dataTable.Columns.Add("Value", typeof(string));


                var result = from T1 in ApplicationProps.AsEnumerable()
                             join T2 in Application.AsEnumerable()
                             on T1.Field<int>("ApplicationsID") equals T2.Field<int>("ApplicationsID")

                             join T3 in ItemType.AsEnumerable()
                             on T1.Field<int>("Property") equals T3.Field<int>("ItemTypesID")

                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("ApplicationPropID"),
                                 T2.Field<string>("Name"),

                                 T3.Field<string>("Description"),
                                 T1.Field<string>("Value"),
                             }, false);
                dataTable = result.CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }
            return dataTable;
        }


        protected void GV_ApplicationProperty_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string ApplicationId = (GV_ApplicationProperty.FooterRow.FindControl("ddl_applicationFooter") as DropDownList).SelectedItem.Value.Trim();
                    string Property = (GV_ApplicationProperty.FooterRow.FindControl("ddl_PropertyFooter") as DropDownList).SelectedItem.Value.Trim();
                    string Value = (GV_ApplicationProperty.FooterRow.FindControl("txt_ValueFooter") as TextBox).Text.Trim();



                    Entities.ApplicationProp applicationProp = new Entities.ApplicationProp
                    {
                        ApplicationsID = int.Parse(ApplicationId),
                        Property = int.Parse(Property),
                        Value = Value,

                        CreatedBy = Session["PersonID"].ToString(),
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpApplicationProps.InsertRecord(applicationProp);
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
        protected void GV_ApplicationProperty_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int AppPropID = Convert.ToInt32((GV_ApplicationProperty.Rows[e.RowIndex].FindControl("lbl_ApplicationPropID") as Label).Text.Trim());
                string Value = (GV_ApplicationProperty.Rows[e.RowIndex].FindControl("txt_Value") as TextBox).Text.Trim();

                string ApplicationID = (GV_ApplicationProperty.Rows[e.RowIndex].FindControl("ddl_application") as DropDownList).SelectedItem.Value.Trim();
                string Property = (GV_ApplicationProperty.Rows[e.RowIndex].FindControl("ddl_Property") as DropDownList).SelectedItem.Value.Trim();

                GV_ApplicationProperty.EditIndex = -1;

                Entities.ApplicationProp pt = new Entities.ApplicationProp
                {
                    ApplicationsID = int.Parse(ApplicationID),
                    Property = int.Parse(Property),
                    Value = Value,

                    UpdatedBy = Session["PersonID"].ToString(),
                    UpdateDate = DateTime.Now
                };
                int i = DAL.Operations.OpApplicationProps.Update(pt, AppPropID);
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
        protected void GV_ApplicationProperty_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_ApplicationProperty.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpApplicationProps.DeleteByID(id))
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
        protected void GV_ApplicationProperty_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_ApplicationProperty.EditIndex == e.Row.RowIndex)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_application");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Name";
                        ddlRole.DataValueField = "ApplicationsID";
                        ddlRole.DataBind();
                        //ddlRole.Items.FindByValue(DAL.Operations.OpApplicationProps.GetApplicationPropById().AppID.ToString()).Selected = true;


                        DropDownList ddlProp = (DropDownList)e.Row.FindControl("ddl_Property");
                        ddlProp.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpItemTypes.GetAll().Where(x=>x.Categories == System.Configuration.ConfigurationManager.AppSettings.Get("AppPropITypeCat")).ToList()).Tables[0];
                        ddlProp.DataTextField = "Description";
                        ddlProp.DataValueField = "ItemTypesID";
                        ddlProp.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_applicationFooter");
                        ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlFRRole.DataTextField = "Name";
                        ddlFRRole.DataValueField = "ApplicationsID";
                        ddlFRRole.DataBind();

                        DropDownList ddlProp = (DropDownList)e.Row.FindControl("ddl_PropertyFooter");
                        ddlProp.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpItemTypes.GetAll().Where(x => x.Categories == System.Configuration.ConfigurationManager.AppSettings.Get("AppPropITypeCat")).ToList()).Tables[0];
                        ddlProp.DataTextField = "Description";
                        ddlProp.DataValueField = "ItemTypesID";
                        ddlProp.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }


        protected void GV_ApplicationProperty_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_ApplicationProperty.EditIndex = e.NewEditIndex;
                GV_ApplicationProperty.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        protected void GV_ApplicationProperty_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_ApplicationProperty.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_ApplicationProperty_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_ApplicationProperty.PageIndex = e.NewPageIndex;
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
            GV_ApplicationProperty.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_ApplicationProperty.ShowFooter = false;
        }
    }
}