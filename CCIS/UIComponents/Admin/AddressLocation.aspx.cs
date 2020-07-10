using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.Admin
{
    public partial class AddressLocation : System.Web.UI.Page
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
            }catch(Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        public DataTable GetData()
        {
            try
            {
               dt = DAL.Helper.ListToDataset.ToDataSet<Entities.AddressLocations>(DAL.Operations.OpAddressLocations.GetAll()).Tables[0];
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
            return dt;
        }
        public void populate_grid()
        {
            try
            {
                GetData();
                if (dt.Rows.Count > 0)
                {
                    GV_AddressLocation.DataSource = dt;
                    GV_AddressLocation.DataBind();
                }
                else
                {
                    dt.Rows.Add(dt.NewRow());
                    GV_AddressLocation.DataSource = dt;
                    GV_AddressLocation.DataBind();
                    GV_AddressLocation.Rows[0].Cells.Clear();
                    GV_AddressLocation.Rows[0].Cells.Add(new TableCell());
                    GV_AddressLocation.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                    GV_AddressLocation.Rows[0].Cells[0].Text = "No Data Found !!";
                    GV_AddressLocation.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void GV_AddressLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddNew"))
                {
                    //int id = Convert.ToInt32((GV_AddressLocation.FooterRow.FindControl("txt_AddressLocationIdFooter") as TextBox).Text.Trim());
                    string AddressDesc = (GV_AddressLocation.FooterRow.FindControl("txt_AddressDescFooter") as TextBox).Text.Trim();
                    string AddressDescAR = (GV_AddressLocation.FooterRow.FindControl("txt_AddressARDescFooter") as TextBox).Text.Trim();
                    //string ResidenceLocation = (GV_AddressLocation.FooterRow.FindControl("txt_ResidenceLocationFooter") as TextBox).Text.Trim();
                    //string WorkLocation = (GV_AddressLocation.FooterRow.FindControl("txt_WorkLocationFooter") as TextBox).Text.Trim();


                    Entities.AddressLocations addressLocations = new Entities.AddressLocations
                    {
                        Description = AddressDesc,
                        DescriptionAR = AddressDescAR,
                        CreatedBy = SessionName,
                        CreationDate = DateTime.Now
                    };

                    int i = DAL.Operations.OpAddressLocations.InsertRecord(addressLocations);
                    if (i > 0)
                    {
                        lbl_message.Text = "Record added successfully";
                    }
                    else
                    {
                        lbl_message.Text = "ErrorCode: " + i;
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
        protected void GV_AddressLocation_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32((GV_AddressLocation.Rows[e.RowIndex].FindControl("txt_AddressLocationId") as Label).Text.Trim());
                string AddressDesc = (GV_AddressLocation.Rows[e.RowIndex].FindControl("txt_AddressDesc") as TextBox).Text.Trim();
                string AddressDescAR = (GV_AddressLocation.Rows[e.RowIndex].FindControl("txt_AddressARDesc") as TextBox).Text.Trim();
                //string ResidenceLocation = (GV_AddressLocation.Rows[e.RowIndex].FindControl("txt_ResidenceLocation") as TextBox).Text.Trim();
                //string WorkLocation = (GV_AddressLocation.Rows[e.RowIndex].FindControl("txt_WorkLocation") as TextBox).Text.Trim();
                GV_AddressLocation.EditIndex = -1;

                Entities.AddressLocations addressLocations = new Entities.AddressLocations
                {
                    Description = AddressDesc,
                    DescriptionAR = AddressDescAR,
                    UpdatedBy = SessionName,
                    UpdateDate = DateTime.Now
                };

                int i = DAL.Operations.OpAddressLocations.UpdateLocation(addressLocations, id);
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
        protected void GV_AddressLocation_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(GV_AddressLocation.DataKeys[e.RowIndex].Value.ToString());
                if(DAL.Operations.OpAddressLocations.DeletebyID(id))
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

        protected void GV_AddressLocation_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                GV_AddressLocation.EditIndex = e.NewEditIndex;
                GV_AddressLocation.FooterRow.Visible = false;
                Disable_Footer();
                populate_grid();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        protected void GV_AddressLocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                GV_AddressLocation.EditIndex = -1;
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
            GV_AddressLocation.ShowFooter = true;
        }
        private void Disable_Footer()
        {
            GV_AddressLocation.ShowFooter = false;
        }
     
    }
}