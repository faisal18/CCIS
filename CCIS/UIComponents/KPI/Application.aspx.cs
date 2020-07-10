using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.KPI
{
    public partial class Application : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session.Keys.Count > 0)
                {
                    Session["Name"].ToString();
                    int.Parse(Session["PersonId"].ToString());
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if(!IsPostBack)
                {
                    LoadData();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;DAL.Operations.Logger.LogError(ex);
            }
        }

        private void LoadData()
        {
            try
            {
                rptr_Applications.DataSource = GetData();
                rptr_Applications.DataBind();

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }

        private DataTable GetData()
        {
            DataTable dataTable_New = new DataTable();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("ApplicationsID", typeof(int));
            dataTable.Columns.Add("Owner", typeof(string));
            dataTable.Columns.Add("URL", typeof(string));

            try
            {
                DataTable dt_application = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpApplications.GetAll()).Tables[0];

                var result = from T1 in dt_application.AsEnumerable()
                            
                             select dataTable.LoadDataRow(new object[]
                             {
                                 T1.Field<string>("Name"),
                                 T1.Field<int>("ApplicationsID"),
                                 T1.Field<string>("Owner"),
                                 T1.Field<string>("URL"),
                             }, false);


                if (result.Count() > 0)
                {
                    dataTable_New = result.OrderByDescending(x=>x.Field<string>("Name")).CopyToDataTable();
                }

            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return dataTable_New;

        }


        protected void LB_Project_Command(object sender, CommandEventArgs e)
        {
            var link = sender as LinkButton;
            int ApplicationID = int.Parse(link.CommandName.ToString());


            try
            {
                Response.Redirect("~/UIComponents/KPI/ApplicationDetails.aspx?ApplicationID=" + ApplicationID, false);
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }



        }

        protected void lbl_OwnerName_Command(object sender, CommandEventArgs e)
        {
            var link = sender as LinkButton;
            string Owner = link.CommandName.ToString();
        }
    }

 
}