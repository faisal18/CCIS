using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class SystemAdminUser : System.Web.UI.Page
    {

        public static DataTable dt = new DataTable();
        public static DataTable dt_PersonInformation = new DataTable();

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
                    GV_SysAdminUser.DataSource = newDT;
                    GV_SysAdminUser.DataBind();
                }
                else
                {

                    newDT.Rows.Add(newDT.NewRow());
                    GV_SysAdminUser.DataSource = newDT;
                    GV_SysAdminUser.DataBind();
                    GV_SysAdminUser.Rows[0].Cells.Clear();
                    GV_SysAdminUser.Rows[0].Cells.Add(new TableCell());
                    GV_SysAdminUser.Rows[0].Cells[0].ColumnSpan = newDT.Columns.Count;
                    GV_SysAdminUser.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_SysAdminUser.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.SystemUser>(DAL.Operations.OpSystemUser.GetAll()).Tables[0];
                dt_PersonInformation = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
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
                New_DT.Columns.Add("SystemUserID", typeof(int));
                New_DT.Columns.Add("username", typeof(string));
                New_DT.Columns.Add("password", typeof(string));
                New_DT.Columns.Add("FullName", typeof(string));
                New_DT.Columns.Add("isAdmin", typeof(bool));
                New_DT.Columns.Add("isActive", typeof(bool));


                var LINQ_SystemUser = dt.AsEnumerable();
                var LINQ_Person = dt_PersonInformation.AsEnumerable();

                var result = from T1 in LINQ_SystemUser
                             join T2 in LINQ_Person
                             on T1.Field<int>("PersonID") equals T2.Field<int>("PersonInformationID")

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("SystemUserID"),
                                 T1.Field<string>("username"),
                                 T1.Field<string>("password"),
                                 T2.Field<string>("FullName"),
                                 T1.Field<bool>("isAdmin"),
                                 T1.Field<bool>("isActive")

                             }, false);
                New_DT = result.CopyToDataTable();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }


            return New_DT;
        }


        protected void GV_SysAdminUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string Username = (GV_SysAdminUser.FooterRow.FindControl("txt_usernameFooter") as TextBox).Text.Trim();
                    string Password = (GV_SysAdminUser.FooterRow.FindControl("txt_passwordFooter") as TextBox).Text.Trim();
                    bool isAdmin = bool.Parse((GV_SysAdminUser.FooterRow.FindControl("ddl_isAdminFooter") as DropDownList).SelectedItem.Value.Trim());
                    bool isActive = bool.Parse((GV_SysAdminUser.FooterRow.FindControl("ddl_isActiveFooter") as DropDownList).SelectedItem.Value.Trim());
                    int PersonNameID = Convert.ToInt32((GV_SysAdminUser.FooterRow.FindControl("ddl_FullNameFooter") as DropDownList).SelectedItem.Value.Trim());


                    Entities.SystemUser systemUser = new Entities.SystemUser
                    {
                        username = Username,
                        password = Password,
                        PersonID = PersonNameID,
                        isAdmin = isAdmin,
                        isActive = isActive,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpSystemUser.InsertRecord(systemUser);
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
        protected void GV_SysAdminUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int systemuserid = Convert.ToInt32((GV_SysAdminUser.Rows[e.RowIndex].FindControl("lbl_SystemUserID") as Label).Text.Trim());
                string username = (GV_SysAdminUser.Rows[e.RowIndex].FindControl("txt_username") as TextBox).Text.Trim();
                string password = (GV_SysAdminUser.Rows[e.RowIndex].FindControl("txt_password") as TextBox).Text.Trim();
                int PersonId = Convert.ToInt32((GV_SysAdminUser.Rows[e.RowIndex].FindControl("ddl_FullName") as DropDownList).SelectedItem.Value.Trim());
                bool isAdmin = bool.Parse((GV_SysAdminUser.Rows[e.RowIndex].FindControl("ddl_isAdmin") as DropDownList).SelectedItem.Value.Trim());
                bool isActive = bool.Parse((GV_SysAdminUser.Rows[e.RowIndex].FindControl("ddl_isActive") as DropDownList).SelectedItem.Value.Trim());



                GV_SysAdminUser.EditIndex = -1;


                Entities.SystemUser systemUser = new Entities.SystemUser
                {
                    username = username,
                    password = password,
                    PersonID = PersonId,
                    isAdmin = isAdmin,
                    isActive = isActive,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpSystemUser.UpdateRecord(systemUser, systemuserid);
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
        protected void GV_SysAdminUser_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_SysAdminUser.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpSystemUser.DeletebyID(id))
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
        protected void GV_SysAdminUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_SysAdminUser.EditIndex == e.Row.RowIndex)
                {

                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_FullName");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                        ddlRole.DataTextField = "FullName";
                        ddlRole.DataValueField = "PersonInformationID";
                        ddlRole.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_FullNameFooter");
                        ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                        ddlFRRole.DataTextField = "FullName";
                        ddlFRRole.DataValueField = "PersonInformationID";
                        ddlFRRole.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_SysAdminUser_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_SysAdminUser.EditIndex = e.NewEditIndex;
                GV_SysAdminUser.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SysAdminUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_SysAdminUser.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_SysAdminUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_SysAdminUser.PageIndex = e.NewPageIndex;
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
            GV_SysAdminUser.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_SysAdminUser.ShowFooter = false;
        }
    }
}