using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            CheckSession();
        }

        private void CheckSession()
        {

            try
            {
                if (Session["Name"] != null)
                {
                    if (Session["Name"].ToString().Length > 0) 
                    {
                        lbl_username.Text = Session["Name"].ToString();
                        pnl_user.Visible = true;
                        pnl_logout.Visible = true;
                        pnl_topbar.Visible = true;
                    }
                }
                else
                {
                    pnl_user.Visible = false;
                    pnl_logout.Visible = false;
                    pnl_topbar.Visible = false;

                }

                if (Session["Role"] != null)
                {
                    if (Session["Rank"].ToString() != "Admin")
                    {
                        
                        if (Session["Role"].ToString().Length > 0 && Session["Role"].ToString().ToUpper() == "L1")
                        {
                            pnl_admin.Visible = false;
                            pnl_l1.Visible = true;
                            pnl_l2.Visible = false;
                            pnl_Business.Visible = false;
                            pnl_l3.Visible = false;
                        }
                        else if (Session["Role"].ToString().Length > 0 && Session["Role"].ToString().ToUpper() == "L2")
                        {
                            pnl_admin.Visible = false;
                            pnl_l1.Visible = false;
                            pnl_l2.Visible = true;
                            pnl_l3.Visible = false;
                            pnl_Business.Visible = false;
                        }
                        else if (Session["Role"].ToString().Length > 0 && Session["Role"].ToString().ToUpper() == "L3")
                        {
                            pnl_admin.Visible = false;
                            pnl_l1.Visible = false;
                            pnl_l2.Visible = false;
                            pnl_l3.Visible = true;
                            pnl_Business.Visible = false;
                        }
                        else if (Session["Role"].ToString().Length > 0 && Session["Role"].ToString().ToUpper() == "BUSINESS")
                        {
                            pnl_admin.Visible = false;
                            pnl_l1.Visible = false;
                            pnl_l2.Visible = false;
                            pnl_Business.Visible = true;
                            pnl_l3.Visible = false;
                        }
                        else
                        {
                            pnl_admin.Visible = false;
                            pnl_l1.Visible = false;
                            pnl_l2.Visible = false;
                            pnl_Business.Visible = false;
                        }
                    }
                    else
                    {
                        pnl_admin.Visible = true;
                        pnl_l1.Visible = false;
                        pnl_l2.Visible = false;
                        pnl_Business.Visible = false;
                        pnl_l3.Visible = false;
                    }
                }
                else
                {
                    pnl_user.Visible = false;
                    pnl_logout.Visible = false;
                    pnl_topbar.Visible = false;
                }

            }
            catch(Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        protected void link_logout_Click(object sender, EventArgs e)
        {

            try
            {
                Session.Abandon();
                FormsAuthentication.RedirectToLoginPage();
            }
            catch(Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

        }

        protected void link_user_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["Name"] != null)
                {
                    Response.Redirect("~/UIComponents/KPI/UserDetails.aspx", false);
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}