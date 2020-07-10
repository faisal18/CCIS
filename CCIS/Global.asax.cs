using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using System.IO;
using System.Reflection;

namespace CCIS
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            CacheObjects();

        }

        //Session logging
        void Application_AcquireRequestState(object sender, EventArgs e)
        {

            try
            {
                HttpContext context = HttpContext.Current;
                string Page = Path.GetFileName(context.Request.Url.AbsolutePath);
                bool isAccessingAdmin = Context.Request.Url.AbsolutePath.ToLower().Contains("admin");


                
                if (Page != "Login.aspx" && Page != "NotificationService.aspx" && Page != "SLATrigger.aspx" && Page != "TicketClosure.aspx" && Page != "Unauthourized.aspx" && Page != "jirasynchronization" && Page != "jirasynchronization.aspx" && isViewTicket() == false)
                {
                    if (context.Session != null)
                    {
                        if (context.Session.Keys.Count == 0)
                        {
                            FormsAuthentication.RedirectToLoginPage();
                        }
                        else if (context.Session.Keys.Count > 0) 
                        {
                            if (Session["Rank"].ToString() == "Admin")
                            {

                            }
                            else
                            {
                                if (isAccessingAdmin == true && Session["Rank"].ToString() != "Admin")
                                {
                                    Response.Redirect("~/UIComponents/User/Unauthourized.aspx", false);
                                }
                                else
                                {
                                    switch (Session["Role"].ToString().ToUpper())
                                    {
                                        case "L1":
                                            if (Page == "" || Page == "" || Page == "" || Page == "")
                                            {

                                            }
                                            break;
                                        case "L2":
                                            if (Page == "ListTickets.aspx" ||
                                                Page == "ViewTicket.aspx" ||
                                                Page == "Application.aspx" ||
                                                Page == "ApplicationDetails.aspx" ||
                                                Page == "KBRepository.aspx" ||
                                                Page == "KBRepositoryDetails.aspx" ||
                                                Page == "UserDetails.aspx" ||
                                                Page == "L2_TicketQueue.aspx" ||
                                                Page == "EditTicket.aspx" ||
                                                Page == "Dashboard.aspx")
                                            {
                                                //good to go
                                            }
                                            else
                                            {
                                                Response.Redirect("~/UIComponents/User/Unauthourized.aspx", false);
                                            }
                                            break;
                                        case "L3":
                                            if (Page == "ListTickets.aspx" ||
                                                Page == "ViewTicket.aspx" ||
                                                Page == "UserDetails.aspx" ||
                                                Page == "Dashboard.aspx")
                                            {
                                                //good to go
                                            }
                                            else
                                            {
                                                Response.Redirect("~/UIComponents/User/Unauthourized.aspx", false);
                                            }
                                            break;
                                        case "BUSINESS":
                                            if (Page == "ListTickets.aspx" ||
                                                Page == "ViewTicket.aspx" ||
                                                Page == "UserDetails.aspx" ||
                                                Page == "TicketsReport.aspx"||
                                                Page == "Dashboard.aspx")
                                            {
                                                //good to go
                                            }
                                            else
                                            {
                                                Response.Redirect("~/UIComponents/User/Unauthourized.aspx", false);
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }

                
            }
            catch(Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        void CacheObjects()
        {

            DataCaching.DataCaching DC = new DataCaching.DataCaching();

            try
            {
                if(System.Configuration.ConfigurationManager.AppSettings.Get("GlobalCaching").ToLower() == "true")
                {
                    if (System.Configuration.ConfigurationManager.AppSettings.Get("CallerCaching").ToLower() == "true")
                    {
                        var ds = DC.CallerInformation;
                    }
                    if (System.Configuration.ConfigurationManager.AppSettings.Get("PayerCaching").ToLower() == "true")
                    {
                        var dp = DC.PayersCashed;
                    }
                    if (System.Configuration.ConfigurationManager.AppSettings.Get("TicketCaching").ToLower() == "true")
                    {
                        var ddt = DC.TicketsCashed;
                    }
                    if (System.Configuration.ConfigurationManager.AppSettings.Get("LicenseCaching").ToLower() == "true")
                    {
                        var ALic = DC.GetAllLicenses;
                    }
                }
               
            }
            catch(Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }


        bool isViewTicket()
        {
            string guidkey = string.Empty;
            bool result = false;
            try
            {
                if (Request.QueryString.HasKeys())
                {
                    if (Request.QueryString["TicketGKey"] != null)
                    {
                        guidkey = Request.QueryString["TicketGKey"];
                        if (guidkey.Length > 1)
                        {
                            result = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
    }
}