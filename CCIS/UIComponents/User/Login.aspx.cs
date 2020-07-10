using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.User
{
    public partial class Login : System.Web.UI.Page
    {
        private static DataTable dt_SysAdminUser = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void submit_Click(object sender, EventArgs e)
        {

            try
            {
                string txt_username = UserName.Value.ToString();
                string txt_password = Password.Value.ToString();

                if (LoadData(txt_username))
                {
                    string password = dt_SysAdminUser.Rows[0]["password"].ToString();
                    if (password == txt_password)
                    {

                        string Rank = string.Empty, Role = string.Empty;
                        int PersonId = int.Parse(dt_SysAdminUser.Rows[0]["PersonID"].ToString());
                        string Name = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(PersonId).FullName;

                        if (dt_SysAdminUser.Rows[0]["isAdmin"].ToString().ToUpper() == "TRUE")
                        {
                            Rank = "Admin";
                            //Role = "Admin";
                        }
                        else
                        {
                            Rank = "Staff";
                        }

                        string roleID = DAL.Operations.OpUserRoles.GetUserRoleByPersonId(PersonId).RoleID.ToString();
                        if (roleID.Length > 0)
                        {
                            string roleName = DAL.Operations.OpRoles.GetRecordbyID(int.Parse(roleID)).Description;

                            if (roleName.Contains("L1"))
                            {
                                Role = "L1";
                            }
                            else if (roleName.Contains("L2"))
                            {
                                Role = "L2";
                            }
                            else if (roleName.Contains("L3"))
                            {
                                Role = "L3";
                            }
                            else if (roleName.ToUpper().Contains("BUSINESS"))
                            {
                                Role = "BUSINESS";
                            }
                            else
                            {
                                //Role = "Admin";
                            }
                        }

                        lbl_message.Text = "Login Success";

                        Session["PersonId"] = PersonId;
                        Session["Name"] = Name;
                        Session["Rank"] = Rank;
                        Session["Role"] = Role;
                        Session["LoginTime"] = DateTime.Now;
                        logUserIP();


                        FormsAuthentication.SetAuthCookie(Session["Name"].ToString(), true);
                        if (Request.UrlReferrer.ToString() == HttpContext.Current.Request.Url.AbsoluteUri)
                        {
                            //For Reporting only
                            //Response.Redirect("~/UIComponents/Notification/TicketsReport.aspx", false);
                            if (Rank == "Admin")
                            {
                                Response.Redirect("~/UIComponents/admin/admincontrol.aspx", false);
                            }
                            else
                            {
                                if (Role == "L1")
                                {
                                    Response.Redirect("~/UIComponents/CallerInformation.aspx", false);
                                }
                                else if (Role == "L2")
                                {
                                    Response.Redirect("~/UIComponents/Ticket/L2/L2_TicketQueue.aspx", false);
                                }
                                else if (Role == "L3")
                                {
                                    Response.Redirect("~/UIComponents/Ticket/L2/L2_TicketQueue.aspx", false);
                                }
                                else if (Role == "BUSINESS")
                                {
                                    Response.Redirect("~/UIComponents/Ticket/ListTickets.aspx", false);
                                }
                                else
                                {
                                    Response.Redirect("~/UIComponents/User/Unauthourized.aspx", false);
                                }
                            }
                        }
                        else
                        {
                            Response.Redirect(Request.UrlReferrer.ToString());
                        }
                    }
                    else
                    {
                        lbl_message.Text = "Password incorrect";
                    }
                }
                else
                {
                    lbl_message.Text = "Username incorrect";
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private bool LoadData(string txt_username)
        {
            bool result = false;

            try
            {
                dt_SysAdminUser = DAL.Helper.ListToDataset.ToDataSet<Entities.SystemUser>(DAL.Operations.OpSystemUser.GetActiveSystemUserbyUserName(txt_username)).Tables[0];
                if (dt_SysAdminUser.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        public void logUserIP()
        {
            try
            {
                string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string result = string.Empty;

                if (!string.IsNullOrEmpty(ipList))
                {
                    result = ipList.Split(',')[0];
                }
                else
                {
                    result = Request.ServerVariables["REMOTE_ADDR"];
                }

                DAL.Operations.Logger.Info("Session logged in.Create Ticket. PersonID " + Session["PersonId"] +
                    " Name " + Session["Name"] +
                    " Rank " + Session["Rank"] +
                    " Role " + Session["Role"] +
                    " LoginTime " + Session["LoginTime"] +
                    " IP " + result);

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }


    }
}