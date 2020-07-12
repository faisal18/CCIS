using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Notification
{
    public partial class TicketsReport : System.Web.UI.Page
    {
        #region Loaders
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session.Keys.Count > 0)
                {
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }
                if (!IsPostBack)
                {
                    LoadDropdDown();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message; DAL.Operations.Logger.LogError(ex);
            }
        }
        private void LoadDropdDown()
        {
            try
            {
                ddl_Application.DataSource = DAL.Operations.OpApplications.GetAll();
                ddl_Application.DataTextField = "Name";
                ddl_Application.DataValueField = "ApplicationsID";
                ddl_Application.DataBind();
                ddl_Application.Items.Insert(0, new ListItem("All", "0"));

                ddl_TicketType.DataSource = DAL.Operations.OpStatuses.GetStatusesbyTypeName("Ticket Type");
                ddl_TicketType.DataTextField = "Description";
                ddl_TicketType.DataValueField = "StatusesID";
                ddl_TicketType.DataBind();
                ddl_TicketType.Items.Insert(0, new ListItem("All", "0"));
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime.TryParse(txt_FromDate.Text, out DateTime From);
                DateTime.TryParse(txt_ToDate.Text, out DateTime To);

                logUserIP();


                //ThreadStart childthreat = new ThreadStart(GenerateReport(From,To));
                Response.Write("Child Thread Started <br/>");
                HttpContext ctx = HttpContext.Current;

                //HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data");

                Thread child = new Thread(() => { HttpContext.Current = ctx; GenerateReport(From, To); });


                lbl_message.Text = "Email will be sent to you shortly";
                Thread.Sleep(200);
                lbl_message.Text += "<br/>Report generation started";

                child.Start();

                //child.Abort();


                //GenerateReport(From, To);
            }
           
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

        private void GenerateReport(DateTime From, DateTime To)
        {
            string workingticket = string.Empty;

            try
            {

                #region Handlers
                DAL.Operations.Logger.Info("Generating Ticket Report from " + From + " To " + To + " for user " + Session["Name"].ToString());
                string EmailAddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(int.Parse(Session["PersonId"].ToString())).Email;

                StringBuilder sb = new StringBuilder();
                sb.Append("Ticket Number|Ticket Type|SupportType|Open Date|Ticket Status|Resolved Date|Closed Date|Caller Facility|Caller Facility Name|C. Severity|C. Priority|Severity|Priority|Application Name|Ticket Summary|Closed Within OLA(Y/N)|Closed Within SLA(Y/N)|Days Over OLA|Days Over SLA|Routing Status|Date Ticket Assigned to L2|Date Ticket Assigned to L3|Date TIcket Assigned to Infrastructure|Reported By|Assigned To\n");

                int Incident = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("TicketTypeIncident").ToString());


                List<int> TicketInformationIDs = new List<int>();

                //All Applications and All Types
                if (ddl_Application.SelectedValue == "0" && ddl_TicketType.SelectedValue == "0")
                {
                    TicketInformationIDs = DAL.Operations.OpTicketInformation.GetAll()
                .Where(x => x.CreationDate >= From && x.CreationDate <= To)
                .Select(x => x.TicketInformationID).Distinct().ToList();
                }
                else if (ddl_Application.SelectedValue != "0" && ddl_TicketType.SelectedValue != "0")
                {
                    TicketInformationIDs = DAL.Operations.OpTicketInformation.GetAll()
                .Where(x => x.CreationDate >= From && x.CreationDate <= To)
                .Where(x => x.ApplicationID == int.Parse(ddl_Application.SelectedValue))
                .Where(x => x.TicketType == int.Parse(ddl_TicketType.SelectedValue))
                .Select(x => x.TicketInformationID).Distinct().ToList();
                }
                //All Application and Selected Type
                else if (ddl_Application.SelectedValue == "0" && ddl_TicketType.SelectedValue != "0")
                {
                    TicketInformationIDs = DAL.Operations.OpTicketInformation.GetAll()
                .Where(x => x.CreationDate >= From && x.CreationDate <= To)
                //.Where(x => x.ApplicationID == int.Parse(ddl_Application.SelectedValue))
                .Where(x => x.TicketType == int.Parse(ddl_TicketType.SelectedValue))
                .Select(x => x.TicketInformationID).Distinct().ToList();
                }
                //Selected Application and All Type

                else if (ddl_Application.SelectedValue != "0" && ddl_TicketType.SelectedValue == "0")
                {
                    TicketInformationIDs = DAL.Operations.OpTicketInformation.GetAll()
                .Where(x => x.CreationDate >= From && x.CreationDate <= To)
                .Where(x => x.ApplicationID == int.Parse(ddl_Application.SelectedValue))
                //.Where(x=>x.TicketType == int.Parse(ddl_TicketType.SelectedValue))
                .Select(x => x.TicketInformationID).Distinct().ToList();
                }



                List<string> ClosedStatus = new List<string>();
                ClosedStatus.Add("Closed");
                ClosedStatus.Add("Resolved with KB");
                ClosedStatus.Add("Resolved without KB");
                List<int> statusIDs = DAL.Operations.OpStatuses.GetOpenStatusID(ClosedStatus, "Incident Status");

                List<string> Facilities = DAL.Operations.OpLookups.getAllLicenses();

                var Statuses = DAL.Operations.OpStatuses.GetAll();
                var Applications = DAL.Operations.OpApplications.GetAll();
                var ItemTypes = DAL.Operations.OpItemTypes.GetAll();
                int count = 0;
                int total = TicketInformationIDs.Count; 
                #endregion


                foreach (int TI_ID in TicketInformationIDs)
                {

                    //if (TI_ID != 24761)
                    //{
                    //    continue;
                    //}



                    try
                    {
                        #region Declaration
                        workingticket = TI_ID.ToString();
                        string DaysOverOLA = string.Empty, DaysOverSLA = string.Empty, ClosedWithinOLA = "No", ClosedWithinSLA = "No";
                        //double SLAMinutes, SLATime, OLAMinutes, OLATime;
                        count = count + 1;
                        DAL.Operations.Logger.Info("Ticket report remaining status: " + count + " out of " + total + " tickets processed");


                        Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetRecordbyID(TI_ID);
                        Entities.TicketHistory ticketHistory_Latest = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TI_ID);
                        var ticketHistory_All = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TI_ID); 
                        #endregion

                        #region Basic Values
                        string TicketNumber = ticketInformation.TicketNumber;
                        string TicketType = Statuses.Where(x => x.StatusesID == ticketInformation.TicketType).Select(x => x.Description).FirstOrDefault();
                        string OpenDate = ticketInformation.CreationDate.ToString();
                        string TicketStatus = Statuses.Where(x => x.StatusesID == ticketHistory_Latest.IncidentStatusID).Select(x => x.Description).FirstOrDefault();
                        string RoutingStatus = Statuses.Where(x => x.StatusesID == ticketHistory_Latest.IncidentSubStatusID).Select(x => x.Description).FirstOrDefault();
                        string Priority = ItemTypes.Where(x => x.ItemTypesID == ticketHistory_Latest.PriorityID).Select(x => x.Description).FirstOrDefault();
                        string ClientSeverity = ItemTypes.Where(x => x.ItemTypesID == ticketInformation.SeverityID).Select(x => x.Description).FirstOrDefault();
                        string ClientPriority = ItemTypes.Where(x => x.ItemTypesID == ticketInformation.PriorityID).Select(x => x.Description).FirstOrDefault();
                        string SupportType = ItemTypes.Where(x => x.ItemTypesID == ticketHistory_Latest.SupportTypeID).Select(x => x.Description).FirstOrDefault();


                        string Severity = ItemTypes.Where(x => x.ItemTypesID == ticketHistory_Latest.SeverityID).Select(x => x.Description).FirstOrDefault();
                        string ApplicationName = Applications.Where(x => x.ApplicationsID == ticketInformation.ApplicationID).Select(x => x.Name).FirstOrDefault();
                        string TicketSummary = ticketInformation.Subject;
                        string assignedGroup = DAL.Operations.OpTicketHistory.GetTicketGroupAssignment(DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketID(TI_ID), TI_ID);
                        string CallerFacility = DAL.Operations.OpCallerInfo.GetRecordbyID(ticketInformation.CallerKeyID).CallerLicense;
                        string ReportedBy = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistory_All.First().AssignedFromID).FullName;
                        string ReportedTo = DAL.Operations.OpPersonInformation.GetPersonInformationbyID(ticketHistory_Latest.AssignedTOID).FullName;
                        #endregion

                        #region Complex Variables
                        string CallerFacilityName = string.Empty;
                        string ResolvedDate = string.Empty;
                        string TempResolve = string.Empty;
                        string ClosedDate = string.Empty;

                        Regex pattern = new Regex(".*" + CallerFacility + ".*");
                        var yo = Facilities.Where<string>(item => pattern.IsMatch(item)).ToList();
                        if (yo.Count > 0)
                        {
                            string[] fac = yo[0].ToString().Split('|');
                            CallerFacilityName = fac[4].Trim();
                        }
                        #endregion

                        #region Date Assigned to L2/L3/Infra
                        var Date_AssigntoL2 = ticketHistory_All.Where(x => x.Activity == "ASSIGNED TO L2").Select(x => x.CreationDate).DefaultIfEmpty().First();
                        string dt_Date_AssigntoL2 = string.Empty;
                        if (Date_AssigntoL2 != null)
                        {
                            dt_Date_AssigntoL2 = Date_AssigntoL2.ToString();
                        }

                        var Date_AssigntoL3 = ticketHistory_All.Where(x => x.Activity == "ASSIGNED TO L3 - Jira Created").Select(x => x.CreationDate).DefaultIfEmpty().First();
                        string dt_Date_AssigntoL3 = string.Empty;
                        if (Date_AssigntoL3 != null)
                        {
                            dt_Date_AssigntoL3 = Date_AssigntoL3.ToString();
                        }

                        var dt_Date_AssigntoInfrav = ticketHistory_All.Where(x => x.Activity == "TICKET SENT TO INFRASTRUCTURE").Select(x => x.CreationDate).DefaultIfEmpty().First();
                        string Date_AssigntoInfra = string.Empty;
                        if (dt_Date_AssigntoInfrav != null)
                        {
                            Date_AssigntoInfra = dt_Date_AssigntoInfrav.ToString();
                        }
                        #endregion

                        #region SLA calculation

                        ResolvedDate = DAL.Helper.DateTimeHelper.caclulateSLA_Date(ticketHistory_All, "resolve");
                        ClosedDate = DAL.Helper.DateTimeHelper.caclulateSLA_Date(ticketHistory_All, "close");


                        //SLAMinutes = DAL.Helper.DateTimeHelper.calculateSLA(ticketHistory_All);
                        //SLATime = DAL.Helper.DateTimeHelper.SLACalculations(ApplicationName, SLAMinutes, Priority, assignedGroup, "SLA");

                        DAL.Helper.Result_SLAOLA obj = DAL.Helper.DateTimeHelper.Controller_SLAOLA(ticketHistory_All, ApplicationName, Priority, assignedGroup);




                        if (obj.SLATime <= 0)
                        {
                            if (is_closed(TicketStatus))
                            {
                                ClosedWithinSLA = "No";
                            }

                            //DaysOverSLA = DAL.Helper.DateTimeHelper.getTimeElapsedstringfromMinutes(SLATime);
                            DaysOverSLA = obj.DaysOverSLA;
                        }
                        else
                        {
                            if (is_closed(TicketStatus))
                            {
                                ClosedWithinSLA = "Yes";
                            }
                        }

                        #endregion

                        #region OLA CALCULATION

                        //OLAMinutes = SLAMinutes;
                        //OLATime = DAL.Helper.DateTimeHelper.SLACalculations(ApplicationName, OLAMinutes, Priority, assignedGroup, "OLA");

                        if (obj.OLATime <= 0)
                        {
                            if (is_closed(TicketStatus))
                            {
                                ClosedWithinOLA = "No";
                            }

                            //DaysOverOLA = DAL.Helper.DateTimeHelper.getTimeElapsedstringfromMinutes(OLATime);
                            DaysOverOLA = obj.DaysOverOLA;
                        }
                        else
                        {
                            if (is_closed(TicketStatus))
                            {
                                ClosedWithinOLA = "Yes";
                            }
                        }
                        #endregion

                        #region output
                        string data = TicketNumber + "|" +
                                         TicketType + "|" +
                                         SupportType + "|" +
                                         OpenDate + "|" +
                                         TicketStatus + "|" +
                                         ResolvedDate + "|" +
                                         ClosedDate + "|" +
                                         CallerFacility + "|" +
                                         CallerFacilityName + "|" +
                                         ClientSeverity + "|" +
                                         ClientPriority + "|" +
                                         Severity + "|" +
                                         Priority + "|" +
                                         ApplicationName + "|" +
                                         TicketSummary + "|" +
                                         ClosedWithinOLA + "|" +
                                         ClosedWithinSLA + "|" +
                                         DaysOverOLA + "|" +
                                         DaysOverSLA + "|" +
                                         RoutingStatus + "|" +
                                         Date_AssigntoL2 + "|" +
                                         Date_AssigntoL3 + "|" +
                                         Date_AssigntoInfra + "|" +
                                         ReportedBy + "|" +
                                         ReportedTo +
                                         "\n";

                        sb.Append(data); 
                        #endregion
                    }
                    catch (ThreadAbortException ex)
                    {
                        DAL.Operations.Logger.LogError(ex);
                        DAL.Operations.Logger.Info("ThreadAbortException innerloop Generating Report.Check TicketId " + workingticket);
                        sb.Append("ThreadAbortException innerloop Generating Report.Ask Faisal to check TicketId " + workingticket);
                    }
                    catch (Exception ex)
                    {
                        DAL.Operations.Logger.LogError(ex);
                        DAL.Operations.Logger.Info("Exception innerloop Generating Report.Check TicketId " + workingticket);
                        sb.Append("Exception innerloop Generating Report.Ask Faisal to check TicketId " + workingticket);
                    }


                }

                string filename = "TicketReports_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
                string path = System.Configuration.ConfigurationManager.AppSettings.Get("ReportsPath");

                string FileDir = path + "\\" + filename;
                System.IO.File.WriteAllText(FileDir, sb.ToString());

                string ExcelPath = ConvertToExcel(FileDir, FileDir);
                SendEmail(ExcelPath, EmailAddress);
            }
            catch (ThreadAbortException ex)
            {
                DAL.Operations.Logger.LogError(ex);
                DAL.Operations.Logger.Info("ThreadAbortException Generating Report.Check TicketId " + workingticket);
                DAL.Operations.Logger.Info("Report Failed with Errors");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                DAL.Operations.Logger.Info("Exception Generating Report.Check TicketId " + workingticket);
                DAL.Operations.Logger.Info("Report Failed with Errors");
            }
            DAL.Operations.Logger.Info("Ticket Report Generated for User " + Session["Name"].ToString());
        }

        #region Helper functions
        private static bool is_closed(string data)
        {
            bool is_clsoed = false;
            try
            {
                if(data == "Closed" || data == "Resolved without KB" || data == "Resolved with KB")
                {
                    is_clsoed = true;
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return is_clsoed;
        }
        private void download_string(string path)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    //Response.End();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void SendEmail(string path, string toaddress)
        {
            try
            {
                DAL.Operations.Logger.Info("Sending email for ticket report to " + toaddress);
                bool result = DAL.Helper.EmailHelper.SendEmailwithAttachment("[IQServiceDesk] Ticket Report " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), "Dear Concern, PFA ticket report", toaddress, path);
                if (result)
                {
                    DAL.Operations.Logger.Info("Email sent for ticket report to " + toaddress);
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private string ConvertToExcel(string CSVpath, string EXCELPath)
        {
            try
            {
                string Filename = System.IO.Path.GetFileNameWithoutExtension(CSVpath);
                string DirectoryName = System.IO.Path.GetDirectoryName(CSVpath);
                EXCELPath = DirectoryName + "\\" + Filename + ".xlsx";

                string worksheetsName = "Report";
                bool firstRowIsHeader = false;

                var format = new OfficeOpenXml.ExcelTextFormat();
                format.Delimiter = '|';
                format.EOL = "\n";

                using (OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(EXCELPath)))
                {
                    string dateformat = "m/d/yy h:mm";

                    OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(new System.IO.FileInfo(CSVpath), format, OfficeOpenXml.Table.TableStyles.Medium2, firstRowIsHeader);

                    worksheet.Column(4).Style.Numberformat.Format = dateformat;
                    worksheet.Column(6).Style.Numberformat.Format = dateformat;
                    worksheet.Column(7).Style.Numberformat.Format = dateformat;
                    worksheet.Column(21).Style.Numberformat.Format = dateformat;
                    worksheet.Column(22).Style.Numberformat.Format = dateformat;
                    worksheet.Column(23).Style.Numberformat.Format = dateformat;

                    package.Save();
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return EXCELPath;
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

                 DAL.Operations.Logger.Info("Session logged in.Ticket Report being generated. PersonID " + Session["PersonId"] +
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
        #endregion
    }
}