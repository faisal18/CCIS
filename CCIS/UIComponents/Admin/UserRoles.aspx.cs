using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class UserRoles : System.Web.UI.Page
    {
        public static DataTable dt = new DataTable();
        public static DataTable dt_roles = new DataTable();
        public static DataTable dt_person = new DataTable();


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
                    GV_UserRoles.DataSource = newDT;
                    GV_UserRoles.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_UserRoles.DataSource = newDT;
                    GV_UserRoles.DataBind();
                    GV_UserRoles.Rows[0].Cells.Clear();
                    GV_UserRoles.Rows[0].Cells.Add(new TableCell());
                    GV_UserRoles.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_UserRoles.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_UserRoles.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.UserRoles>(DAL.Operations.OpUserRoles.GetAll()).Tables[0];
                dt_person = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                dt_roles = DAL.Helper.ListToDataset.ToDataSet<Entities.Roles>(DAL.Operations.OpRoles.GetAll()).Tables[0];

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
                New_DT.Columns.Add("UserRoleID", typeof(int));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("RoleDescription", typeof(string));
                New_DT.Columns.Add("FullName", typeof(string));
                

                var LINQ_UserRoles = dt.AsEnumerable();
                var LINQ_Persons = dt_person.AsEnumerable();
                var LINQ_Roles = dt_roles.AsEnumerable();

                var result = from T1 in LINQ_UserRoles
                             join T2 in LINQ_Persons
                             on T1.Field<int>("PersonID") equals T2.Field<int>("PersonInformationID")
                             join T3 in LINQ_Roles
                             on T1.Field<int>("RoleID") equals T3.Field<int>("RolesID")


                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("UserRoleID"),
                                 T1.Field<string>("Description"),
                                 T3.Field<string>("Description"),
                                 T2.Field<string>("FullName"),

                             }, false);
                New_DT = result.CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }


            return New_DT;
        }

        protected void GV_UserRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string UserRoleDescription = "N/A";
                    //string UserRoleDescription = (GV_UserRoles.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    int RoleID = Convert.ToInt32((GV_UserRoles.FooterRow.FindControl("ddl_RoleDescriptionFooter") as DropDownList).SelectedItem.Value.Trim());
                    int PersonID = Convert.ToInt32((GV_UserRoles.FooterRow.FindControl("ddl_FullNameFooter") as DropDownList).SelectedItem.Value.Trim());


                    Entities.UserRoles userRoles = new Entities.UserRoles
                    {
                        Description = UserRoleDescription,
                        RoleID = RoleID,
                        PersonID = PersonID,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpUserRoles.InsertRecord(userRoles);
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
        protected void GV_UserRoles_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int UserRoleID = Convert.ToInt32((GV_UserRoles.Rows[e.RowIndex].FindControl("lbl_UserRoleID") as Label).Text.Trim());
                string Description = "N/A";
                //string Description = (GV_UserRoles.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                int RoleID = Convert.ToInt32((GV_UserRoles.Rows[e.RowIndex].FindControl("ddl_RoleDescription") as DropDownList).SelectedItem.Value.Trim());
                int PersonID = Convert.ToInt32((GV_UserRoles.Rows[e.RowIndex].FindControl("ddl_FullName") as DropDownList).SelectedItem.Value.Trim());

                GV_UserRoles.EditIndex = -1;


                Entities.UserRoles userRoles = new Entities.UserRoles
                {
                    Description = Description,
                    RoleID = RoleID,
                    PersonID = PersonID,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpUserRoles.UpdateRecord(userRoles, UserRoleID);
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
        protected void GV_UserRoles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_UserRoles.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpUserRoles.DeletebyID(id))
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
        protected void GV_UserRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_UserRoles.EditIndex == e.Row.RowIndex)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_RoleDescription");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Roles>(DAL.Operations.OpRoles.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "RolesID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_FullName");
                        ddlRole2.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                        ddlRole2.DataTextField = "FullName";
                        ddlRole2.DataValueField = "PersonInformationID";
                        ddlRole2.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_RoleDescriptionFooter");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Roles>(DAL.Operations.OpRoles.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "RolesID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_FullNameFooter");
                        ddlRole2.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                        ddlRole2.DataTextField = "FullName";
                        ddlRole2.DataValueField = "PersonInformationID";
                        ddlRole2.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_UserRoles_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_UserRoles.EditIndex = e.NewEditIndex;
                GV_UserRoles.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserRoles_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_UserRoles.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserRoles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_UserRoles.PageIndex = e.NewPageIndex;
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
            GV_UserRoles.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_UserRoles.ShowFooter = false;
        }
    }
}