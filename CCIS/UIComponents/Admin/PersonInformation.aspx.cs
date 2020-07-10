using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class PersonInformation : System.Web.UI.Page
    {

        public static DataTable dt = new DataTable();
        public static DataTable dt_Nationality = new DataTable();
        public static DataTable dt_AddressLocation = new DataTable();

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
                DataTable dt_NEW = MergeTable() ;
                if (dt_NEW.Rows.Count > 0)
                {
                    GV_PersonInformation.DataSource = dt_NEW;
                    GV_PersonInformation.DataBind();
                }
                else
                {
                    dt_NEW.Rows.Add(dt_NEW.NewRow());
                    GV_PersonInformation.DataSource = dt_NEW;
                    GV_PersonInformation.DataBind();
                    GV_PersonInformation.Rows[0].Cells.Clear();
                    GV_PersonInformation.Rows[0].Cells.Add(new TableCell());
                    GV_PersonInformation.Rows[0].Cells[0].ColumnSpan = dt_NEW.Columns.Count;
                    GV_PersonInformation.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_PersonInformation.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
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
                dt = DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetAll()).Tables[0];
                dt_AddressLocation = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
                dt_Nationality = DAL.Helper.ListToDataset.ToDataSet<Entities.Nationality>(DAL.Operations.OpNationality.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return dt;
        }
        public DataTable MergeTable()
        {
            DataTable New_DT = new DataTable();

            try
            {

                New_DT.Columns.Add("PersonInformationID", typeof(int));
                New_DT.Columns.Add("FullName", typeof(string));
                New_DT.Columns.Add("ContactNumber", typeof(string));
                New_DT.Columns.Add("Gender", typeof(string));
                New_DT.Columns.Add("Nationality", typeof(string));
                New_DT.Columns.Add("Email", typeof(string));
                New_DT.Columns.Add("ResidentialLocation", typeof(string));
                New_DT.Columns.Add("WorkLocation", typeof(string));


                var LINQ_Person = dt.AsEnumerable();
                var LINQ_Address = dt_AddressLocation.AsEnumerable();
                var LINQ_Nationality = dt_Nationality.AsEnumerable();

                var result = from T1 in LINQ_Person
                             join T2 in LINQ_Address
                             on T1.Field<int>("ResidentialLocation") equals T2.Field<int>("AddressLocationID")
                             join T3 in LINQ_Nationality
                             on T1.Field<int>("NationalityID") equals T3.Field<int>("NationalityID")

                             select New_DT.LoadDataRow(new object[]
                             {
                                 T1.Field<int>("PersonInformationID"),
                                 T1.Field<string>("FullName"),
                                 T1.Field<string>("ContactNumber"),
                                 T1.Field<string>("Gender"),
                                 T3.Field<string>("Description"),
                                 T1.Field<string>("Email"),
                                 T2.Field<string>("Description"),
                                 T2.Field<string>("Description"),
                             },
                             false);
                New_DT = result.CopyToDataTable();

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return New_DT;
        }

        protected void GV_PersonInformation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    string FullName = (GV_PersonInformation.FooterRow.FindControl("txt_FullNameFooter") as TextBox).Text.Trim();
                    string ContactNumber = (GV_PersonInformation.FooterRow.FindControl("txt_ContactNumberFooter") as TextBox).Text.Trim();
                    string Email = (GV_PersonInformation.FooterRow.FindControl("txt_EmailFooter") as TextBox).Text.Trim();

                    string Gender = (GV_PersonInformation.FooterRow.FindControl("ddl_GenderFooter") as DropDownList).SelectedItem.Text;

                    string Nationality = (GV_PersonInformation.FooterRow.FindControl("ddl_NationalityFooter") as DropDownList).SelectedItem.Text.Trim();
                    int NationalityId = Convert.ToInt32((GV_PersonInformation.FooterRow.FindControl("ddl_NationalityFooter") as DropDownList).SelectedItem.Value);

                    string ResidentialLocation = (GV_PersonInformation.FooterRow.FindControl("ddl_ResidentialLocationFooter") as DropDownList).SelectedItem.Text.Trim();
                    int ResidentialLocationId = Convert.ToInt32((GV_PersonInformation.FooterRow.FindControl("ddl_ResidentialLocationFooter") as DropDownList).SelectedItem.Value);

                    string Worklocation = (GV_PersonInformation.FooterRow.FindControl("ddl_WorkLocationFooter") as DropDownList).SelectedItem.Text.Trim();
                    int WorkLocationId = Convert.ToInt32((GV_PersonInformation.FooterRow.FindControl("ddl_WorkLocationFooter") as DropDownList).SelectedItem.Value);


                    //Entities.AddressLocations AL = new Entities.AddressLocations
                    //{
                    //    Description = ResidentialLocation
                    //};

                    //Entities.Nationality nationality = new Entities.Nationality
                    //{
                    //    Description = Nationality
                    //};


                    Entities.PersonInformation PI = new Entities.PersonInformation
                    {
                        FullName = FullName,
                        ContactNumber = ContactNumber,
                        Gender = Gender,
                        NationalityID = NationalityId,
                        //NationalityID_FK = nationality,
                        Email = Email,
                        ResidentialLocation = ResidentialLocationId,
                        //ResidentialLocationID_FK = AL,
                        WorkLocation = WorkLocationId,
                        //WorkLocationID_FK = AL
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now

                    };

                    int result = DAL.Operations.OpPersonInformation.InsertPersonInformation(PI);
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
        protected void GV_PersonInformation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int PersonInformationID = Convert.ToInt32((GV_PersonInformation.Rows[e.RowIndex].FindControl("txt_PersonInformationID") as Label).Text.Trim());
                string FullName = (GV_PersonInformation.Rows[e.RowIndex].FindControl("txt_FullName") as TextBox).Text.Trim();
                string ContactNumber = (GV_PersonInformation.Rows[e.RowIndex].FindControl("txt_ContactNumber") as TextBox).Text.Trim();
                string Email = (GV_PersonInformation.Rows[e.RowIndex].FindControl("txt_Email") as TextBox).Text.Trim();

                string Gender = (GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_Gender") as DropDownList).SelectedItem.Text.Trim();

                string Nationality = (GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_Nationality") as DropDownList).SelectedItem.Text.Trim();
                int NationalityId = Convert.ToInt32((GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_Nationality") as DropDownList).SelectedItem.Value);

                string ResidentialLocation = (GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_ResidentialLocation") as DropDownList).SelectedItem.Text.Trim();
                int ResidentialLocationId = Convert.ToInt32((GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_ResidentialLocation") as DropDownList).SelectedItem.Value);

                string Worklocation = (GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_WorkLocation") as DropDownList).SelectedItem.Text.Trim();
                int WorkLocationId = Convert.ToInt32((GV_PersonInformation.Rows[e.RowIndex].FindControl("ddl_WorkLocation") as DropDownList).SelectedItem.Value);
                GV_PersonInformation.EditIndex = -1;


                Entities.AddressLocations AL = new Entities.AddressLocations
                {
                    Description = ResidentialLocation
                };

                Entities.Nationality nationality = new Entities.Nationality
                {
                    Description = Nationality
                };


                Entities.PersonInformation PI = new Entities.PersonInformation
                {
                    FullName = FullName,
                    ContactNumber = ContactNumber,
                    Gender = Gender,
                    NationalityID = NationalityId,
                    NationalityID_FK = nationality,
                    Email = Email,
                    ResidentialLocation = ResidentialLocationId,
                    ResidentialLocationID_FK = AL,
                    WorkLocation = WorkLocationId,
                    WorkLocationID_FK = AL,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int result = DAL.Operations.OpPersonInformation.UpdatePersonInformation(PI, PersonInformationID);
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
        protected void GV_PersonInformation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_PersonInformation.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpPersonInformation.DeletebyID(id))
                {
                    lbl_message.Text = "Record deleted";
                }
                else
                {
                    lbl_message.Text = "Record deletion failed";
                }
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_PersonInformation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (GV_PersonInformation.EditIndex == e.Row.RowIndex)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddl_Nationality");
                        ddlRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Nationality>(DAL.Operations.OpNationality.GetAll()).Tables[0];
                        ddlRole.DataTextField = "Description";
                        ddlRole.DataValueField = "NationalityID";
                        ddlRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_ResidentialLocation");
                        ddlRole2.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
                        ddlRole2.DataTextField = "Description";
                        ddlRole2.DataValueField = "AddressLocationID";
                        ddlRole2.DataBind();

                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_WorkLocation");
                        ddlRole3.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
                        ddlRole3.DataTextField = "Description";
                        ddlRole3.DataValueField = "AddressLocationID";
                        ddlRole3.DataBind();
                    }
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        DropDownList ddlFRRole = (DropDownList)e.Row.FindControl("ddl_NationalityFooter");
                        ddlFRRole.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.Nationality>(DAL.Operations.OpNationality.GetAll()).Tables[0];
                        ddlFRRole.DataTextField = "Description";
                        ddlFRRole.DataValueField = "NationalityID";
                        ddlFRRole.DataBind();

                        DropDownList ddlRole2 = (DropDownList)e.Row.FindControl("ddl_ResidentialLocationFooter");
                        ddlRole2.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
                        ddlRole2.DataTextField = "Description";
                        ddlRole2.DataValueField = "AddressLocationID";
                        ddlRole2.DataBind();

                        DropDownList ddlRole3 = (DropDownList)e.Row.FindControl("ddl_WorkLocationFooter");
                        ddlRole3.DataSource = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
                        ddlRole3.DataTextField = "Description";
                        ddlRole3.DataValueField = "AddressLocationID";
                        ddlRole3.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_PersonInformation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_PersonInformation.EditIndex = e.NewEditIndex;
                GV_PersonInformation.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_PersonInformation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_PersonInformation.EditIndex = -1;
                Enable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_PersonInformation_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                GV_PersonInformation.PageIndex = e.NewPageIndex;
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
            GV_PersonInformation.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_PersonInformation.ShowFooter = false;
        }

    }
}