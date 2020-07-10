using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets
{
    public partial class CallerInformation : System.Web.UI.Page
    {
        private static string facilitylicense = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultCallerLicense");
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session.Keys.Count > 0)
                {
                    Session["Name"].ToString();
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if (!IsPostBack)
                {
                    txt_CallerLicense.Attributes.Add("onblur", "fromServer()");
                    Populate_Grid(facilitylicense,"","","");
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        #region Loaders
        private DataTable GetData(string license,string CallerName, string emailadd, string phoneNumber)
        {
            DataTable dt = new DataTable();
            try
            {

                ///	1	1	1	1	1
                ///	2	1	1	1	
                ///	3	1	1		1
                ///	4	1	1		
                ///	5	1		1	1
                ///	6	1		1	
                ///	7	1			1
                ///	8	1			
                ///	9		1	1	1
                ///	10		1	1	
                ///	11		1		1
                ///	12		1		
                ///	13			1	1
                ///	14			1	
                ///	15				1
                ///	16				

                
                //1
                if (license.Length > 1 && CallerName.Length > 1 && emailadd.Length > 1 && phoneNumber.Length > 1)
                {

                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Name.ToLower() == CallerName.ToLower() && x.Email.ToLower() == emailadd.ToLower() && x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //2
                else
                if (license.Length > 1 && CallerName.Length > 1 && emailadd.Length > 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Name.ToLower() == CallerName.ToLower() && x.Email.ToLower() == emailadd.ToLower() );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //3
                else
                if (license.Length > 1 && CallerName.Length > 1 && emailadd.Length < 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Name.ToLower() == CallerName.ToLower() &&  x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //4
                else
                if (license.Length > 1 && CallerName.Length > 1 && emailadd.Length < 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Name.ToLower() == CallerName.ToLower() );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //5
                else
                if (license.Length > 1 && CallerName.Length > 1 && emailadd.Length > 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Name.ToLower() == CallerName.ToLower() && x.Email.ToLower() == emailadd.ToLower() );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //6
                else
                if (license.Length > 1 && CallerName.Length < 1 && emailadd.Length > 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.Email.ToLower() == emailadd.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //7
                else
                if (license.Length > 1 && CallerName.Length < 1 && emailadd.Length < 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll()
                        .OrderByDescending(x => x.CreationDate)
                        .Where(x => x.CallerLicense.ToLower() == license.ToLower() && x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //8
                else
                if (license.Length > 1 && CallerName.Length < 1 && emailadd.Length < 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x => x.CallerLicense.ToLower() == license.ToLower() );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //9
                else
                if (license.Length < 1 && CallerName.Length > 1 && emailadd.Length > 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll()
                        .OrderByDescending(x => x.CreationDate)
                        .Where(x => x.Name.ToLower() == CallerName.ToLower() && x.Email.ToLower() == emailadd.ToLower() && x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //10
                else
                if (license.Length < 1 && CallerName.Length > 1 && emailadd.Length > 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x => 
                        x.Name.ToLower() == CallerName.ToLower() && x.Email.ToLower() == emailadd.ToLower() );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //11
                else
                if (license.Length < 1 && CallerName.Length > 1 && emailadd.Length < 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll()
                        .OrderByDescending(x => x.CreationDate)
                        .Where(x => 
                        x.Name.ToLower() == CallerName.ToLower()  && x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //12
                else
                if (license.Length < 1 && CallerName.Length > 1 && emailadd.Length < 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x => 
                        x.Name.ToLower() == CallerName.ToLower()  );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //13
                else
                if (license.Length < 1 && CallerName.Length < 1 && emailadd.Length > 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x => 
                        x.Email.ToLower() == emailadd.ToLower() && x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //14
                else
                if (license.Length < 1 && CallerName.Length > 1 && emailadd.Length < 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x =>  x.Name.ToLower() == CallerName.ToLower()  );
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                //15
                else
                if (license.Length < 1 && CallerName.Length < 1 && emailadd.Length < 1 && phoneNumber.Length > 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate)
                        .Where(x => 
                        x.PhoneNumber.ToLower() == phoneNumber.ToLower());
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];

                }
                else
                if (license.Length < 1 && CallerName.Length < 1 && emailadd.Length > 1 && phoneNumber.Length < 1)
                {
                    var result = DAL.Operations.OpCallerInfo.GetAll();
                    result = result.Where(x => String.Equals(x.Email,emailadd,StringComparison.CurrentCultureIgnoreCase)).ToList();
                    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];
                }
                //16
                else
                if (license.Length < 1 && CallerName.Length < 1 && emailadd.Length < 1 && phoneNumber.Length < 1)
                {

                }





                //-----------------------------------

                //if (CallerName.Length > 1)
                //{
                //    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x=>x.CreationDate).Where(x => x.CallerLicense == license.ToLower() && x.Name.ToLower() == CallerName.ToLower());
                //    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];
                //}
                //else
                //{
                //    var result = DAL.Operations.OpCallerInfo.GetAll().OrderByDescending(x => x.CreationDate).Where(x => x.CallerLicense == license.ToLower());
                //    dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.CallerInformation>().ToList()).Tables[0];
                //}
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dt;
        }
        public void Populate_Grid(string license,string name, string emailadd, string phonenumber)
        {

            try
            {

                DataTable dt = GetData(license,name, emailadd, phonenumber);
                if (dt.Rows.Count > 0)
                {
                    DBGrid.DataSource = dt;
                    DBGrid.DataBind();

                    Populate_SessionGrid(license);
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    DBGrid.DataSource = dt;
                    DBGrid.DataBind();
                    DBGrid.Rows[0].Cells.Clear();
                    DBGrid.Rows[0].Cells.Add(new TableCell());
                    DBGrid.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    DBGrid.Rows[0].Cells[0].Text = "No Data Found !!";
                    DBGrid.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        private DataTable GetSessionData(string license)
        {
            DataTable dt = new DataTable();
            
            try
            {
                string sessionName = Session["Name"].ToString();
                int sessionPersonId = int.Parse(Session["PersonId"].ToString());
                int IncidentType = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeIncident"));

                var result = DAL.Operations.OpTicketInformation.GetAll().Where(x => x.ReporterID == sessionPersonId && x.TicketType == IncidentType).OrderByDescending(x => x.CreationDate).Take(5);
                dt = DAL.Helper.ListToDataset.ToDataSet(result.Cast<Entities.TicketInformation>().ToList()).Tables[0];
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.Info("Looking up for license " + license);
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dt;
        }
        private void Populate_SessionGrid(string license)
        {
            try
            {
                DataTable dt = GetSessionData(license);
                if (dt.Rows.Count > 0)
                {
                    repeater_assignedtosession.DataSource = dt;
                    repeater_assignedtosession.DataBind();
                }
            }
            catch(Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region LoadCallersByMethods
        [WebMethod]
        public static string Transformlicense(string license)
            {
            string facilitylicense2 = string.Empty;

            try
            {
                if (license.Length > 0)
                {
                    if (license.Contains('|'))
                    {
                        string[] splitter = license.Split('|');
                        string type = splitter[0].Trim();
                        facilitylicense2 = splitter[2].Trim();
                        string name = splitter[4].Trim();
                    }
                    else
                    {
                        facilitylicense2 = license;
                    }
                }

                return facilitylicense2.ToUpper();

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return ex.Message;
            }
        }
        protected void txt_hiddenfield_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string facility = txt_hiddenfield.Text;
                Populate_Grid(facility,"","","");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);

            }
        }
        protected void txt_Name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string CallerName = txt_Name.Text;
                string Facility = txt_hiddenfield.Text;
                Populate_Grid(Facility,CallerName,"","");

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        #region GridControls
        protected void DBGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                DBGrid.EditIndex = e.NewEditIndex;
                DBGrid.FooterRow.Visible = false;
                Disable_Footer();
                Populate_Grid(txt_CallerLicense.Text, txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);

                //Populate_Grid(txt_CallerLicense.Text);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void DBGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                DBGrid.EditIndex = -1;
                Enable_Footer();
                Populate_Grid(txt_CallerLicense.Text, txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);

                //Populate_Grid(txt_CallerLicense.Text);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void DBGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32((DBGrid.Rows[e.RowIndex].FindControl("txt_CallerInformationID") as TextBox).Text.Trim());
            string CallerKeyID = (DBGrid.Rows[e.RowIndex].FindControl("txt_CallerKeyID") as TextBox).Text.Trim();
            string Name = (DBGrid.Rows[e.RowIndex].FindControl("txt_Name") as TextBox).Text.Trim();
            string CallerLicene = (DBGrid.Rows[e.RowIndex].FindControl("txt_CallerLicense") as TextBox).Text.Trim();
            string PhoneNumber = (DBGrid.Rows[e.RowIndex].FindControl("txt_PhoneNumber") as TextBox).Text.Trim();
            bool isowner = (DBGrid.Rows[e.RowIndex].FindControl("txt_isOwner") as CheckBox).Checked;
            bool iscontactperson = (DBGrid.Rows[e.RowIndex].FindControl("txt_isContactPerson") as CheckBox).Checked;
            string email = (DBGrid.Rows[e.RowIndex].FindControl("txt_Email") as TextBox).Text.Trim();
            string department = (DBGrid.Rows[e.RowIndex].FindControl("txt_Department") as TextBox).Text.Trim(); 
            string location = (DBGrid.Rows[e.RowIndex].FindControl("txt_Location") as TextBox).Text.Trim(); 
            string machinename = (DBGrid.Rows[e.RowIndex].FindControl("txt_MachineName") as TextBox).Text.Trim(); 
            string OS = (DBGrid.Rows[e.RowIndex].FindControl("txt_OperatingSystem") as TextBox).Text.Trim();

            Entities.CallerInformation callerInformation = new Entities.CallerInformation
            {
                CallerKeyID = CallerKeyID,
                CallerLicense = CallerLicene,
                Email = email,
                isContactPerson = iscontactperson,
                isOwner = isowner,
                Name = Name,
                PhoneNumber = PhoneNumber,
                Department = department,
                Location = location,
                MachineName = machinename,
                OperatingSystem = OS,
                UpdatedBy = Session["Name"].ToString(),
                UpdateDate = DateTime.Now
            };

            int result = DAL.Operations.OpCallerInfo.UpdateRecord(callerInformation, id);
            DBGrid.EditIndex = -1;
            if (result > 0)
            {
                lbl_message.Text = "Caller Record updated";
            }
            else

            {
                lbl_message.Text = "Please contact Admin";
            }

            Enable_Footer();
            //Populate_Grid(txt_CallerLicense.Text);
            Populate_Grid(txt_CallerLicense.Text, txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);


        }
        protected void DBGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(DBGrid.DataKeys[e.RowIndex].Value.ToString());
                if (DAL.Operations.OpAddressLocations.DeletebyID(id))
                {
                    lbl_message.Text = "Record deleted successfully";
                }
                else
                {
                    lbl_message.Text = "Record deleltion failed";
                }
                Populate_Grid(txt_CallerLicense.Text, txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);

                //Populate_Grid(txt_CallerLicense.Text);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);

            }
        }
        protected void DBGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DBGrid.PageIndex = e.NewPageIndex;
            Enable_Footer();
            Populate_Grid(txt_CallerLicense.Text,txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);
        }
        private void Enable_Footer()
        {
            DBGrid.ShowFooter = false;
        }
        private void Disable_Footer()
        {
            DBGrid.ShowFooter = false;
        }
        #endregion

        #region Link/Save
        protected void link_Callerkey_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int row_number = Convert.ToInt32(e.CommandArgument) % DBGrid.PageSize;
                GridViewRow row = DBGrid.Rows[row_number];
                int CallerKeyID = int.Parse((row.Controls[0].Controls[3] as HiddenField).Value);
                if (e.CommandName == "CallerKey")
                {
                    Response.Redirect("InquiryDetails.aspx?CallerKeyID=" + CallerKeyID, false);
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
                //throw;
            }
        }
        protected void lbl_Name_SessionGrid_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string ticketnumber = e.CommandName;
                Response.Redirect("Ticket//ViewTicket.aspx?TicketInformationID=" + ticketnumber, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

        }
        protected void btnSaveCaller_Click(object sender, EventArgs e)
        {

            bool FoundLicense = false;
            try
            {

                if (txt_CallerLicense.Text.Length > 0 && txt_PhoneNumber.Text.Length > 0)
                {
                    string license = string.Empty;
                    if (txt_CallerLicense.Text.Contains('|'))
                    {
                        string[] splitter = txt_CallerLicense.Text.Split('|');
                        if (splitter.Length > 1)
                        {
                            string type = splitter[0].Trim();
                            string name = splitter[4].Trim();
                            license = splitter[2].Trim().ToUpper();
                        }
                    }
                    else
                    {
                        license = txt_CallerLicense.Text.ToUpper();
                    }

                    //FoundLicense = DAL.Operations.OpLookups.getAllLicensesNoSplit().Contains(license);
                    FoundLicense = true;
                    //bypassing the LMU synch protocol

                    if (FoundLicense)
                    {

                        bool Callerexist = DAL.Operations.OpCallerInfo.GetCallerInformationbyLicenseID(license).Where(x => x.PhoneNumber == txt_PhoneNumber.Text && x.Name.ToLower() == txt_Name.Text.ToLower()).Any();

                        if (!Callerexist)
                        {
                            Entities.CallerInformation callerInformation = new Entities.CallerInformation
                            {

                                CallerLicense = license,
                                Name = txt_Name.Text,
                                Email = txt_Email.Text.ToLower(),
                                PhoneNumber = txt_PhoneNumber.Text,
                                isContactPerson = chkBoxContactPerson.Checked,
                                isOwner = CheckBoxOwner.Checked,
                                Department = txt_Department.Text.ToLower(),
                                Location = txt_Location.Text.ToLower(),
                                MachineName = txt_MachineName.Text.ToLower(),
                                OperatingSystem = txt_OperatingSystem.Text.ToLower(),

                                CreatedBy = Session["Name"].ToString(),
                                CreationDate = DateTime.Now,

                            };

                            int LastInsertedID = DAL.Operations.OpCallerInfo.InsertRecord(callerInformation);
                            DAL.Operations.OpCallerInfo.generateCallerID(license, LastInsertedID);
                            //lbl_message.Text = "Caller " + txt_Name.Text + " with number " + txt_PhoneNumber.Text + " added successfully to this license";
                            Populate_Grid(license, txt_Name.Text, txt_Email.Text, txt_PhoneNumber.Text);

                            txt_Name.Text = string.Empty;
                            txt_PhoneNumber.Text = string.Empty;
                            txt_Email.Text = string.Empty;
                        }
                        else
                        {
                            lbl_message.Text = "Caller " + txt_Name.Text + " with number " + txt_PhoneNumber.Text + " already exist for this license";
                        }
                    }


                    else
                    {
                        lbl_message.Text = "License : " + license + " does not exist";
                    }
                }
                else
                {
                    lbl_message.Text = "Please enter Name/Phone Number of the caller";
                }


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        protected void btn_Search_Click(object sender, EventArgs e)
        {

            try
            {
                string CallerName = txt_Name.Text;
                string Facility = Transformlicense( txt_CallerLicense.Text);
                string phonenumber = txt_PhoneNumber.Text;
                string email = txt_Email.Text;
                Populate_Grid(Facility, CallerName, email , phonenumber);
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }


           
        }
    }


}