using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class Application : System.Web.UI.Page
    {
        public static DataTable dt = new DataTable();
        public static DataTable dt_group = new DataTable();

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
                DataTable dt_new = dt;
                //DataTable dt_new = MergeDataTables();
                if (dt_new.Rows.Count > 0)
                {
                    GV_Application.DataSource = dt_new;
                    GV_Application.DataBind();
                }
                else
                {
                    dt_new.Rows.Add(dt_new.NewRow());
                    GV_Application.DataSource = dt_new;
                    GV_Application.DataBind();
                    GV_Application.Rows[0].Cells.Clear();
                    GV_Application.Rows[0].Cells.Add(new TableCell());
                    GV_Application.Rows[0].Cells[0].ColumnSpan = dt_new.Columns.Count;
                    GV_Application.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_Application.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.Applications>(DAL.Operations.OpApplications.GetAll().OrderBy(x=>x.Name).ToList()).Tables[0];
                //dt_group = DAL.Helper.ListToDataset.ToDataSet<Entities.Groups>(DAL.Operations.OpGroups.GetAll()).Tables[0];
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

                New_DT.Columns.Add("ApplicationsID", typeof(int));
                New_DT.Columns.Add("Name", typeof(string));
                //New_DT.Columns.Add("Owner", typeof(string));
                //New_DT.Columns.Add("Owner_Email", typeof(string));
                New_DT.Columns.Add("Contact_Number", typeof(string));
                New_DT.Columns.Add("Contact_Person", typeof(string));
                New_DT.Columns.Add("URL", typeof(string));
                //New_DT.Columns.Add("GroupEmailDesc", typeof(string));


                var Linq_Application = dt.AsEnumerable();
                var Linq_Groups = dt_group.AsEnumerable();

                var result = from T1 in Linq_Application
                             join T2 in Linq_Groups
                             on T1.Field<int>("ApplicationGroupID") equals T2.Field<int>("GroupsID")
                             orderby T1.Field<string>("Name")

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("ApplicationsID"),
                                 T1.Field<string>("Name"),
                                 //T1.Field<string>("Owner"),
                                 //T1.Field<string>("Owner_Email"),
                                 T1.Field<string>("Contact_Number"),
                                 T1.Field<string>("Contact_Person"),
                                 T1.Field<string>("URL"),
                                 //T2.Field<string>("GroupEmail"),


                             }, false);
                New_DT = result.CopyToDataTable();

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return New_DT;
        }

        protected void GV_Application_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    //string ApplicationsID = (GV_Application.FooterRow.FindControl("txt_ApplicationsIDFooter") as TextBox).Text.Trim();
                    string Name = (GV_Application.FooterRow.FindControl("txt_NameFooter") as TextBox).Text.Trim();
                    string Owner = "N/A";
                    //string Owner = (GV_Application.FooterRow.FindControl("txt_OwnerFooter") as TextBox).Text.Trim();
                    string Owner_Email = "N/A";
                    //string Owner_Email = (GV_Application.FooterRow.FindControl("txt_Owner_EmailFooter") as TextBox).Text.Trim();
                    string Contact_Number = (GV_Application.FooterRow.FindControl("txt_Contact_NumberFooter") as TextBox).Text.Trim();
                    string Contact_Person = (GV_Application.FooterRow.FindControl("txt_Contact_PersonFooter") as TextBox).Text.Trim();
                    string URL = (GV_Application.FooterRow.FindControl("txt_URLFooter") as TextBox).Text.Trim();
                    int GroupId = 0;
                    //int GroupId = int.Parse((GV_Application.FooterRow.FindControl("ddl_GroupEmailFooter") as DropDownList).SelectedValue);


                    Entities.Applications applications = new Entities.Applications
                    {
                        Name = Name,
                        Owner = Owner,
                        Owner_Email = Owner_Email,
                        Contact_Number = Contact_Number,
                        Contact_Person = Contact_Person,
                        URL = URL,
                        ApplicationGroupID = GroupId,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int result = DAL.Operations.OpApplications.InsertRecord(applications);
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
        protected void GV_Application_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                int ApplicationsID = Convert.ToInt32((GV_Application.Rows[e.RowIndex].FindControl("lbl_AplicationID") as Label).Text.Trim());
                string Name = (GV_Application.Rows[e.RowIndex].FindControl("txt_Name") as TextBox).Text.Trim();
                string Owner = "N/A";
                //string Owner = (GV_Application.Rows[e.RowIndex].FindControl("txt_Owner") as TextBox).Text.Trim();
                string Owner_Email = "N/A";
                //string Owner_Email = (GV_Application.Rows[e.RowIndex].FindControl("txt_Owner_Email") as TextBox).Text.Trim();
                string Contact_Number = (GV_Application.Rows[e.RowIndex].FindControl("txt_Contact_Number") as TextBox).Text.Trim();
                string Contact_Person = (GV_Application.Rows[e.RowIndex].FindControl("txt_Contact_Person") as TextBox).Text.Trim();
                string URL = (GV_Application.Rows[e.RowIndex].FindControl("txt_URL") as TextBox).Text.Trim();
                int GroupID = 0;
                //int GroupID = int.Parse((GV_Application.Rows[e.RowIndex].FindControl("ddl_GroupEmail") as DropDownList).SelectedValue);

                GV_Application.EditIndex = -1;

                Entities.Applications applications = new Entities.Applications
                {
                    Name = Name,
                    Owner = Owner,
                    Owner_Email = Owner_Email,
                    Contact_Number = Contact_Number,
                    Contact_Person = Contact_Person,
                    URL = URL,
                    ApplicationGroupID = GroupID,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpApplications.UpdateRecord(applications, ApplicationsID);
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
        protected void GV_Application_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_Application.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpApplications.DeletebyID(id))
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

        protected void GV_Application_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {

                GV_Application.EditIndex = e.NewEditIndex;
                GV_Application.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Application_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_Application.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_Application_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_Application.PageIndex = e.NewPageIndex;
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
            GV_Application.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_Application.ShowFooter = false;
        }

        protected void GV_Application_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_Application.EditIndex == e.Row.RowIndex)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        //DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_GroupEmail");
                        //ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        //ddlRole.DataTextField = "GroupEmail";
                        //ddlRole.DataValueField = "GroupsID";
                        //ddlRole.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        //DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_GroupEmailFooter");
                        //ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpGroups.GetAll()).Tables[0];
                        //ddlFRRole.DataTextField = "GroupEmail";
                        //ddlFRRole.DataValueField = "GroupsID";
                        //ddlFRRole.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}