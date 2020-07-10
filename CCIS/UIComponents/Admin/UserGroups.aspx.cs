using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class UserGroups : System.Web.UI.Page
    {
        public static DataTable dt = new DataTable();
        public static DataTable dt_group = new DataTable();
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
                    GV_UserGroups.DataSource = newDT;
                    GV_UserGroups.DataBind();
                }
                else
                {
                    newDT.Rows.Add(newDT.NewRow());
                    GV_UserGroups.DataSource = newDT;
                    GV_UserGroups.DataBind();
                    GV_UserGroups.Rows[0].Cells.Clear();
                    GV_UserGroups.Rows[0].Cells.Add(new TableCell());
                    GV_UserGroups.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_UserGroups.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_UserGroups.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.UserGroups>(DAL.Operations.OpUserGroups.GetAll()).Tables[0];
                dt_group = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                dt_person = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
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
                New_DT.Columns.Add("UserGroupsID", typeof(int));
                New_DT.Columns.Add("Description", typeof(string));
                New_DT.Columns.Add("GroupDescription", typeof(string));
                New_DT.Columns.Add("FullName", typeof(string));


                var LINQ_UserRoles = dt.AsEnumerable();
                var LINQ_Persons = dt_person.AsEnumerable();
                var LINQ_Groups = dt_group.AsEnumerable();

                var result = from T1 in LINQ_UserRoles
                             join T2 in LINQ_Persons
                             on T1.Field<int>("PersonID") equals T2.Field<int>("PersonInformationID")
                             join T3 in LINQ_Groups
                             on T1.Field<int>("GroupID") equals T3.Field<int>("GroupsID")


                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("UserGroupsID"),
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

        protected void GV_UserGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string Description = "N/A";
                    //string Description = (GV_UserGroups.FooterRow.FindControl("txt_DescriptionFooter") as TextBox).Text.Trim();
                    int GroupID = Convert.ToInt32((GV_UserGroups.FooterRow.FindControl("ddl_GroupDescriptionFooter") as DropDownList).SelectedItem.Value.Trim());
                    int PersonID = Convert.ToInt32((GV_UserGroups.FooterRow.FindControl("ddl_FullNameFooter") as DropDownList).SelectedItem.Value.Trim());


                    Entities.UserGroups userGroups = new Entities.UserGroups
                    {
                        Description = Description,
                        GroupID = GroupID,
                        PersonID = PersonID,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpUserGroups.InsertRecord(userGroups);
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
        protected void GV_UserGroups_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int UserGroupID = Convert.ToInt32((GV_UserGroups.Rows[e.RowIndex].FindControl("lbl_UserGroupsID") as Label).Text.Trim());
                string Description = "N/A";
                //string Description = (GV_UserGroups.Rows[e.RowIndex].FindControl("txt_Description") as TextBox).Text.Trim();
                int GroupID = Convert.ToInt32((GV_UserGroups.Rows[e.RowIndex].FindControl("ddl_GroupDescription") as DropDownList).SelectedItem.Value.Trim());
                int PersonID = Convert.ToInt32((GV_UserGroups.Rows[e.RowIndex].FindControl("ddl_FullName") as DropDownList).SelectedItem.Value.Trim());



                GV_UserGroups.EditIndex = -1;


                Entities.UserGroups userGroups = new Entities.UserGroups
                {
                    Description = Description,
                    GroupID = GroupID,
                    PersonID = PersonID,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpUserGroups.UpdateRecord(userGroups, UserGroupID);
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
        protected void GV_UserGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_UserGroups.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpUserGroups.DeletebyID(id))
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
        protected void GV_UserGroups_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            try
            {
                if (GV_UserGroups.EditIndex == e.Row.RowIndex)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_GroupDescription");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "GroupsID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_FullName");
                        ddlRole2.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                        ddlRole2.DataTextField = "FullName";
                        ddlRole2.DataValueField = "PersonInformationID";
                        ddlRole2.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_GroupDescriptionFooter");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "GroupsID";
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

        protected void GV_UserGroups_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_UserGroups.EditIndex = e.NewEditIndex;
                GV_UserGroups.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserGroups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_UserGroups.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_UserGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_UserGroups.PageIndex = e.NewPageIndex;
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
            GV_UserGroups.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_UserGroups.ShowFooter = false;
        }
    }
}