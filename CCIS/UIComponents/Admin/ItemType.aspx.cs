using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class ItemType : System.Web.UI.Page
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
                    GV_ItemTypes.DataSource = dt;
                    GV_ItemTypes.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_ItemTypes.DataSource = dt;
                    GV_ItemTypes.DataBind();
                    GV_ItemTypes.Rows[0].Cells.Clear();
                    GV_ItemTypes.Rows[0].Cells.Add(new TableCell());
                    GV_ItemTypes.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_ItemTypes.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_ItemTypes.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.ItemTypes>(DAL.Operations.OpItemTypes.GetAll()).Tables[0];
                dt = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("Categories")).CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_ItemTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string Description = (GV_ItemTypes.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    string Categories = (GV_ItemTypes.FooterRow.FindControl("txt_CategoriesFooter") as TextBox).Text.Trim();
                    string Role = (GV_ItemTypes.FooterRow.FindControl("txt_RoleFooter") as TextBox).Text.Trim();
                    string Scenario = (GV_ItemTypes.FooterRow.FindControl("txt_ScenarioFooter") as TextBox).Text.Trim();

                    Entities.ItemTypes itemTypes = new Entities.ItemTypes
                    {
                       Description = Description,
                       Categories = Categories,
                       Role = Role,
                       Scenario = Scenario,
                       CreatedBy = SessionName,
                       CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpItemTypes.InsertRecord(itemTypes);
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
        protected void GV_ItemTypes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_ItemTypes.Rows[e.RowIndex].FindControl("lbl_ItemTypesID") as Label).Text.Trim());
                string Description = (GV_ItemTypes.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                string Categories = (GV_ItemTypes.Rows[e.RowIndex].FindControl("txt_Categories") as TextBox).Text.Trim();
                string Role = (GV_ItemTypes.Rows[e.RowIndex].FindControl("txt_Role") as TextBox).Text.Trim();
                string Scenario = (GV_ItemTypes.Rows[e.RowIndex].FindControl("txt_Scenario") as TextBox).Text.Trim();

                GV_ItemTypes.EditIndex = -1;

                Entities.ItemTypes itemTypes = new Entities.ItemTypes
                {
                    Description = Description,
                    Categories = Categories,
                    Role = Role,
                    Scenario = Scenario,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpItemTypes.UpdateRecord(itemTypes,id);
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
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                
            }
        }
        protected void GV_ItemTypes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_ItemTypes.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpItemTypes.DeletebyID(id))
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

        protected void GV_ItemTypes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_ItemTypes.EditIndex = e.NewEditIndex;
                GV_ItemTypes.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_ItemTypes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_ItemTypes.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_ItemTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_ItemTypes.PageIndex = e.NewPageIndex;
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
            GV_ItemTypes.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_ItemTypes.ShowFooter = false;
        }
    }
}