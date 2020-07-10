using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class UserApplication : System.Web.UI.Page
    {

        public static DataTable dt = new DataTable();
        public static DataTable dt_person = new DataTable();
        public static DataTable dt_group = new DataTable();
        public static DataTable dt_roles = new DataTable();
        public static DataTable dT_application = new DataTable();


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
                    GV_UserApplication.DataSource = newDT;
                    GV_UserApplication.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_UserApplication.DataSource = newDT;
                    GV_UserApplication.DataBind();
                    GV_UserApplication.Rows[0].Cells.Clear();
                    GV_UserApplication.Rows[0].Cells.Add(new TableCell());
                    GV_UserApplication.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_UserApplication.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_UserApplication.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.UserApplication>(DAL.Operations.OpUserApplication.GetAll()).Tables[0];
                dt_group = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                dt_roles = DAL.Helper.ListToDataset.ToDataSet<Entities.Roles>(DAL.Operations.OpRoles.GetAll()).Tables[0];
                dt_person = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                dT_application = DAL.Helper.ListToDataset.ToDataSet<Entities.Applications>(DAL.Operations.OpApplications.GetAll()).Tables[0];

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
                New_DT.Columns.Add("UserApplicationID", typeof(int));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("RoleDescription", typeof(string));
                New_DT.Columns.Add("FullName", typeof(string));
                New_DT.Columns.Add("ApplicationDescripion", typeof(string));
                New_DT.Columns.Add("GroupDescription", typeof(string));
               
                var LINQ_UserApplication = dt.AsEnumerable();
                var LINQ_Roles = dt_roles.AsEnumerable();
                var LINQ_Person = dt_person.AsEnumerable();
                var LINQ_Application = dT_application.AsEnumerable();
                var Linq_Group = dt_group.AsEnumerable();

                var result = from T1 in LINQ_UserApplication

                             join T2 in LINQ_Roles
                             on T1.Field<int>("RoleID") equals T2.Field<int>("RolesID")

                             join T3 in LINQ_Person
                             on T1.Field<int>("PersonID") equals T3.Field<int>("PersonInformationID")

                             join T4 in LINQ_Application
                             on T1.Field<int>("ApplicationID") equals T4.Field<int>("ApplicationsID")

                             join T5 in Linq_Group
                             on T1.Field<int>("GroupID") equals T5.Field<int>("GroupsID")


                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("UserApplicationID"),
                                 T1.Field<string>("Description"),
                                 T2.Field<string>("Description"),
                                 T3.Field<string>("FullName"),
                                 T4.Field<string>("Name"),
                                 T5.Field<string>("Description")

                             }, false);
                New_DT = result.CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }


            return New_DT;
        }

        protected void GV_UserApplication_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string description = "N/A";
                    //string description = (GV_UserApplication.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    int RoleId = Convert.ToInt32((GV_UserApplication.FooterRow.FindControl("ddl_RoleDescriptionFooter") as DropDownList).SelectedValue.Trim());

                    int GroupId = Convert.ToInt32((GV_UserApplication.FooterRow.FindControl("ddl_GroupDescriptionFooter") as DropDownList).SelectedValue.Trim());


                    int PersonId = Convert.ToInt32((GV_UserApplication.FooterRow.FindControl("ddl_FullNameFooter") as DropDownList).SelectedValue.Trim());
                    int AppId = Convert.ToInt32((GV_UserApplication.FooterRow.FindControl("ddl_ApplicationDescripionFooter") as DropDownList).SelectedValue.Trim());


                    Entities.UserApplication userApplication = new Entities.UserApplication
                    {
                        Description = description,
                        RoleID = RoleId,
                        GroupID = GroupId,
                        PersonID = PersonId,
                        ApplicationID = AppId,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpUserApplication.InsertRecord(userApplication);
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
        protected void GV_UserApplication_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_UserApplication.Rows[e.RowIndex].FindControl("lbl_UserApplicationID") as Label).Text.Trim());
                string description = "N/A";
                //string description = (GV_UserApplication.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                int RoleId = Convert.ToInt32((GV_UserApplication.Rows[e.RowIndex].FindControl("ddl_RoleDescription") as DropDownList).SelectedValue.Trim());

                int GroupId = Convert.ToInt32((GV_UserApplication.Rows[e.RowIndex].FindControl("ddl_GroupDescription") as DropDownList).SelectedValue.Trim());

                int PersonId = Convert.ToInt32((GV_UserApplication.Rows[e.RowIndex].FindControl("ddl_FullName") as DropDownList).SelectedValue.Trim());
                int AppId = Convert.ToInt32((GV_UserApplication.Rows[e.RowIndex].FindControl("ddl_ApplicationDescripion") as DropDownList).SelectedValue.Trim());


                GV_UserApplication.EditIndex = -1;


                Entities.UserApplication userApplication = new Entities.UserApplication
                {
                    Description = description,
                    GroupID = GroupId,
                    RoleID = RoleId,
                    PersonID = PersonId,
                    ApplicationID = AppId,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpUserApplication.UpdateRecord(userApplication, id);
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
        protected void GV_UserApplication_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_UserApplication.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpUserApplication.DeletebyID(id))
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
        protected void GV_UserApplication_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_UserApplication.EditIndex == e.Row.RowIndex)
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

                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_ApplicationDescripion");
                        ddlRole3.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Applications>(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlRole3.DataTextField = "Name";
                        ddlRole3.DataValueField = "ApplicationsID";
                        ddlRole3.DataBind();


                        DropDownList ddlRole4 = (DropDownList)e.Row.FindControl("ddl_GroupDescription");
                        ddlRole4.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        ddlRole4.DataTextField = "Description";
                        ddlRole4.DataValueField = "GroupsID";
                        ddlRole4.DataBind();

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

                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_ApplicationDescripionFooter");
                        ddlRole3.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Applications>(DAL.Operations.OpApplications.GetAll()).Tables[0];
                        ddlRole3.DataTextField = "Name";
                        ddlRole3.DataValueField = "ApplicationsID";
                        ddlRole3.DataBind();

                        DropDownList ddlRole4 = (DropDownList)e.Row.FindControl("ddl_GroupDescriptionFooter");
                        ddlRole4.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        ddlRole4.DataTextField = "Description";
                        ddlRole4.DataValueField = "GroupsID";
                        ddlRole4.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_UserApplication_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_UserApplication.EditIndex = e.NewEditIndex;
                GV_UserApplication.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserApplication_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_UserApplication.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            try
            {
                GV_UserApplication.PageIndex = e.NewPageIndex;
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
            GV_UserApplication.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_UserApplication.ShowFooter = false;
        }
    }
}