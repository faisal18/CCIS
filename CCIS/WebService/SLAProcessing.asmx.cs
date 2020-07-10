using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Reflection;


namespace CCIS.WebService
{
    /// <summary>
    /// Summary description for SLAProcessing
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SLAProcessing : System.Web.Services.WebService
    {

        [WebMethod]
        public string HeartBeat()
        {
            DAL.Operations.Logger.Info("Heartbeat");
            return DateTime.Now.ToLongTimeString();
        }

        //[WebMethod]
        public List<Entities.TicketHistory> GetIncidentsbyStatus(int StatusID, int SubStatusID)
        {
            try
            {
                return DAL.Operations.OpTicketHistory.GetTicketHistorybyStatuses(StatusID, SubStatusID);


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        public List<Entities.TicketHistory> GetOpenIncidents()
        {
            try
            {
                List<Entities.TicketInformation> list_tickets =  DAL.Operations.OpTicketInformation.GetTicketInformationByCreationDate(Convert.ToDateTime("2020-02-09"));
                return DAL.Operations.OpTicketHistory.GetOpenIncidentsByTicketInformation(list_tickets.Select(x => x.TicketInformationID).ToList());
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        //[WebMethod]
        public List<Entities.TicketHistory> GetIncidentsbySLA(int StatusID, int SubStatusID, int priorityID, int severityID)
        {
            try
            {
                return DAL.Operations.OpTicketHistory.GetTicketHistorybySLA(StatusID, SubStatusID, priorityID, severityID);


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }



        [WebMethod]
        public string ProcessTickets()
        {
            try
            {
                string SLAProcessingEnabled = System.Configuration.ConfigurationManager.AppSettings.Get("SLAProcessingEnabled");
                bool SLAProcessingEnabledBool = bool.Parse(SLAProcessingEnabled);
                if (SLAProcessingEnabledBool)
                {
                    string SLAorOLA = System.Configuration.ConfigurationManager.AppSettings.Get("SLAorOLA");
                    string IncludeNonWorkingHours = System.Configuration.ConfigurationManager.AppSettings.Get("IncludeNonWorkingHours");
                    bool includeNonWorkingHoursBool = bool.Parse(IncludeNonWorkingHours);

                    List<Entities.TicketHistory> OpenTickets = new List<Entities.TicketHistory>();
                    OpenTickets = GetOpenIncidents();

                    List<Entities.SLADeclarations> SLAs = new List<Entities.SLADeclarations>();
                    SLAs = DAL.Operations.OpSLADeclarations.GetActiveSLAs();

                    List<Entities.SLAExecutionLog> SLAExecx = new List<Entities.SLAExecutionLog>();
                    SLAExecx = DAL.Operations.OpSLAExecutionLog.GetSLAExecutionLogbyTicketList(OpenTickets);

                    DAL.Operations.Logger.Info("SLA PROCESSING STARTED for Ticket of Open Count :  " + OpenTickets.Count);

                    foreach (var ti in OpenTickets)
                    {

                        Entities.TicketLists _ticketLists = DAL.Operations.OpTicketLists.GetRecordbyTicketHistoryID(ti.TicketHistoryID);
                        int ExecLogCount = SLAExecx.Where(x => x.TicketInformationID == ti.TicketInformationID).Count();

                        DateTime currentTime = DateTime.Now;
                        DateTime ticketCreationTime = DateTime.Now;
                        if (SLAorOLA == "SLA")
                        {
                            ticketCreationTime = (DateTime)_ticketLists._TicketInformation.FirstOrDefault(x => x.TicketInformationID == ti.TicketInformationID).CreationDate;
                        }
                        else
                        {
                            ticketCreationTime = (DateTime)ti.CreationDate;
                        }


                        //double TimeDifference = 60;
                        //15,30,40,60,70,90,1440
                        double TimeDifference = DAL.Helper.DateTimeHelper.CalculateHourse(ticketCreationTime.ToString());



                        Entities.SLADeclarations eligibleSLA = new Entities.SLADeclarations();
                        bool use_Faisal = true;

                        if (!use_Faisal)
                        {
                            eligibleSLA = SLAs.Where(
                                x => x.PriorityID == ti.PriorityID && x.SeverityID == ti.SeverityID && x.StatusID == ti.IncidentStatusID
                                && x.SubStatusID == ti.IncidentSubStatusID && x.TimeinMinutes <= TimeDifference && x.isActive == true
                                && x.ApplicationID == _ticketLists._TicketInformation.Select(t => t.ApplicationID).Distinct().FirstOrDefault())
                                .OrderByDescending(x => x.TimeinMinutes)
                                .FirstOrDefault();
                        }
                        else if(use_Faisal)
                        {
                             eligibleSLA = SLAs.Where(
                                x => x.PriorityID == ti.PriorityID &&
                                //x.SeverityID == ti.SeverityID && 
                                //x.StatusID == ti.IncidentStatusID && 
                                //x.SubStatusID == ti.IncidentSubStatusID && 
                                x.TimeinMinutes <= TimeDifference && 
                                x.isActive == true 
                                //&& x.ApplicationID == _ticketLists._TicketInformation.Select(t => t.ApplicationID).Distinct().FirstOrDefault()
                                
                                )
                                .OrderByDescending(x => x.TimeinMinutes)
                                .FirstOrDefault();
                        }


                        if (eligibleSLA != null && ExecLogCount == 0)
                        {
                            DAL.Operations.OpActionsExecution.TakeAction(_ticketLists, eligibleSLA);
                        }

                        else if (eligibleSLA != null && ExecLogCount > 0)
                        {
                            DAL.Operations.Logger.Info(" SLA: " + eligibleSLA.Description + "with Previous Execution Count: " + ExecLogCount);
                            if (SLAExecx.Where(a => a.TicketInformationID == ti.TicketInformationID && a.SLAID == eligibleSLA.SLADeclarationsID).Count() > 0)
                            {
                                //   DAL.Operations.Logger.Info(" Same SLA Executed already ");
                            }
                            else
                            {
                                DAL.Operations.OpActionsExecution.TakeAction(_ticketLists, eligibleSLA);
                            }
                        }
                        else
                        if (eligibleSLA == null && ExecLogCount == 0)
                        {
                            //   DAL.Operations.Logger.Info("SLA NOT Found for TIcket:  " + ti.TicketInformationID  );
                        }
                        _ticketLists = null;
                    }


                    DAL.Operations.Logger.Info("SLA PROCESSING FINISED for Ticket of Open Count :  " + OpenTickets.Count);

                    SLAProcessingEnabled = "  Tickets Processed :" + OpenTickets.Count.ToString();
                    SLAExecx = null;
                    OpenTickets = null;
                    SLAs = null;
                }
                return SLAProcessingEnabled;


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;

            }

        }

        [WebMethod]
        public string SendTestEmail()
        {
            try
            {


                DAL.Helper.EmailHelper.SendTestEmail();
                return "*** Success ***";
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return "!!! Failed !!!";

            }
        }
        public static object GetPropValue(object src, string propName)
        {
            if (src.GetType().GetProperty(propName) != null)
            {
                var x = src.GetType().GetProperty(propName).GetValue(src, null);
                return x;
            }
            return null;
        }


        // [WebMethod]
        public static Entities.Notification GenerateNotification(int TicketInformationID, bool includeHistory, string _EmailTemplateCategory, string _Comments, string NotificationType, bool ClientEmail, bool NotifyAllClientMembers, string _toAddress = "", string _ccAddress = "")
        {
            try
            {

                Entities.Notification _NewNotification = DAL.Operations.OpNotification.GenerateNotification(TicketInformationID, includeHistory, _EmailTemplateCategory, _Comments, NotificationType, ClientEmail, NotifyAllClientMembers, _toAddress, _ccAddress, false);
                return _NewNotification;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        public static string stringFindAndReplace(string CompleteString, string toFind, string toReplace)
        {

            try
            {
                if (CompleteString.ToUpper().Contains(toFind.ToUpper()))
                {
                    CompleteString = CompleteString.ToUpper().Replace(toFind, toReplace);
                }
                else
                {

                }

                return CompleteString;
            }
            catch (Exception exc)
            {
                DAL.Operations.Logger.LogError(exc);
                return CompleteString;
            }


        }
        public static string ParseTemplate(int TicketInformationID, bool includeHistory, string _EmailTemplateCategory, string _Comments, string NotificationType)
        {
            try
            {

                string emailBody = DAL.Operations.OpNotification.ParseTemplate(TicketInformationID, includeHistory, _EmailTemplateCategory, _Comments, NotificationType);


                //Entities.TicketLists _TIcketInformation = DAL.Operations.OpTicketLists.GetRecordbyTicketInformationID(TicketInformationID);
                //Entities.Email _EmailTemplate = DAL.Operations.OpEmail.GetEmailObjectByCategoryandNotificationType(_EmailTemplateCategory, NotificationType);
                //string emailBody = _EmailTemplate.EmailBody;
                //string emailSubject = _EmailTemplate.EmailSubject;


                //if (_TIcketInformation._TicketInformation.Count > 0 && _TIcketInformation._TicketHistory.Count > 0 && _EmailTemplate != null)
                //{

                //    /////////////////////////// EMAIL SUBJECT /////////////////////////////////
                //    ///ticketnumber

                //    string TicketNumber = _TIcketInformation._TicketInformation[0].TicketNumber;
                //    emailSubject = stringFindAndReplace(emailSubject, "@@TICKETNUMBER", TicketNumber);

                //    /////////////////////////// EMAIL BODY /////////////////////////////////
                //    ///ticketnumber
                //    ///
                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETNUMBER", TicketNumber);

                //    /// Ticket Creation Date
                //    DateTime _TicketDate = (DateTime)_TIcketInformation._TicketInformation[0].CreationDate;

                //    string ticketCreationDate = _TicketDate.ToString("yyyy-MM-dd");
                //    string ticketCreationTime = _TicketDate.ToString("hh:mm:ss");

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONDATE", ticketCreationDate);

                //    /// TIcket creation  time
                //    ///
                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONTIME", ticketCreationTime);

                //    /// Ticket reopen date 
                //    DateTime _TicketReopenDate = (DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                //    string ticketReopenDate = _TicketReopenDate.ToString("yyyy-MM-dd");
                //    string ticketReopenTime = _TicketReopenDate.ToString("hh:mm:ss");

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENDATE", ticketReopenDate);

                //    /// Ticket REopenTime
                //    /// 
                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENTIME", ticketReopenTime);

                //    /// Ticket closure date
                //    DateTime _TicketClosureDate = (DateTime)(DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                //    string ticketClosureDate = _TicketClosureDate.ToString("yyyy-MM-dd");
                //    string ticketClosureTime = _TicketClosureDate.ToString("hh:mm:ss");

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSUREDATE", ticketClosureDate);

                //    /// Ticket CLosure Time
                //    /// 
                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSURETIME", ticketClosureTime);

                //    /// Caller INformation 
                //    Entities.CallerInformation _CallerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(_TIcketInformation._TicketInformation[0].CallerKeyID);

                //    string callerLicense = _CallerInformation.CallerLicense;

                //    List<string> _AllLicense = DAL.Operations.OpLookups.getAllLicenses();

                //    string callerName = "";
                //    var callerinfo = _AllLicense.Where(a => a.Contains(callerLicense)).FirstOrDefault();

                //    if (callerinfo != null)
                //    {
                //        callerName = callerinfo;
                //    }

                //    emailBody = stringFindAndReplace(emailBody, "@@CALLERLICENSE", callerName);

                //    /// Ticket TYpe

                //    List<Entities.Statuses> _Statuses = DAL.Operations.OpStatuses.GetAll();

                //    string ticketType = (_Statuses.Where(a => a.StatusesID == _TIcketInformation._TicketInformation[0].TicketType).FirstOrDefault()).Description;
                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETTYPE", ticketType);

                //    /// Ticket Status

                //    Entities.TicketHistory _ticketHistory = _TIcketInformation._TicketHistory
                //        .OrderByDescending(a => a.TicketHistoryID).ThenBy(x => x.CreationDate).FirstOrDefault();

                //    string TicketStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentStatusID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETSTATUS", TicketStatus);

                //    /// routing status
                //    string routingStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentSubStatusID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@ROUTINGSTATUS", routingStatus);

                //    /// Reported By

                //    string Reportedby = _TIcketInformation._TicketInformation[0].CreatedBy;
                //    emailBody = stringFindAndReplace(emailBody, "@@REPORTEDBY", Reportedby);

                //    //// All Item Types
                //    ///for Client/Internal Severity
                //    ///client Severity
                //    List<Entities.ItemTypes> _itemTypes = DAL.Operations.OpItemTypes.GetAll();

                //    string ClientSeverity = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].SeverityID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@CLIENTSEVERITY", ClientSeverity);

                //    /// Internal Severity
                //    string InternalSeverity = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.SeverityID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@INTERNALSEVERITY", InternalSeverity);

                //    /// Client Priority
                //    string ClientPriority = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].PriorityID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@CLIENTPRIORITY", ClientPriority);

                //    /// Internal Priority
                //    string InternalPriority = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.PriorityID).FirstOrDefault()).Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@INTERNALPRIORITY", InternalPriority);

                //    /// Application
                //    /// 

                //    string applicationName = DAL.Operations.OpApplications.GetRecordbyID(_TIcketInformation._TicketInformation[0].ApplicationID).Name;

                //    emailBody = stringFindAndReplace(emailBody, "@@APPLICATION", applicationName);

                //    /// resolution summary by business
                //    string ResolutionSummary = _TIcketInformation._TicketInformation[0].ResolutionSummary;

                //    emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONDETAILS", ResolutionSummary);

                //    /// update description by business
                //    string IssueDescription = _TIcketInformation._TicketInformation[0].IssueDescription;

                //    emailBody = stringFindAndReplace(emailBody, "@@UPDATEDESCRIPTION", IssueDescription);
                //    /// Action Taken by business
                //    string ActionTakeBusiness = _TIcketInformation._TicketInformation[0].ResolutionActions;

                //    //emailBody = stringFindAndReplace(emailBody, "@@ResolutionActionByBusiness", ActionTakeBusiness);
                //    emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONACTIONBYBUSINESS", ActionTakeBusiness);


                //    /// Complete Ticket Comments History.
                //    /// 

                //    if (includeHistory == true)
                //    {
                //        string commentshisotry = DAL.Operations.OpTicketHistory.GetAllCommentsbyTicketInformationID(TicketInformationID);

                //        emailBody = stringFindAndReplace(emailBody, "@@COMMENTHISTORY", commentshisotry);

                //    }

                //    /// User SPECIFIC Comments attached with email.
                //    /// 

                //    emailBody = stringFindAndReplace(emailBody, "@@USERCOMMENT", _Comments);

                //    ///  TICKET Comments .
                //    /// 

                //    string ticketcomments = _ticketHistory.Comments;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETCOMMENT", ticketcomments);

                //    string TicketDescription = _TIcketInformation._TicketInformation[0].Description;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETDESCRIPTION", TicketDescription);

                //    string TIcketUPdatedBy = _ticketHistory.CreatedBy;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETUPDATEDBY", TIcketUPdatedBy);

                //    string TIcketClosedBy = _ticketHistory.CreatedBy;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETCLOSEDBY", TIcketClosedBy);

                //    // OLA--------------- 

                //    string OLA = "";
                //    Entities.TicketHistory _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L3 - Jira Created");

                //    if (_OLATicketHistory != null)
                //    {
                //        OLA = (DateTime.Now - (DateTime)_OLATicketHistory.CreationDate).TotalDays.ToString();

                //    }
                //    else
                //    {
                //        _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L2");
                //        if (_OLATicketHistory != null)
                //        {
                //            OLA = (DateTime.Now - (DateTime)_OLATicketHistory.CreationDate).TotalDays.ToString();
                //        }
                //    }

                //    emailBody = stringFindAndReplace(emailBody, "@@OLA", OLA);

                //    //SLA
                //    string SLA_PendingDays = (DateTime.Now - (DateTime)_TIcketInformation._TicketInformation[0].CreationDate).TotalDays.ToString();

                //    emailBody = stringFindAndReplace(emailBody, "@@SLA", SLA_PendingDays);

                //    /// SLA crossed.

                //    emailBody = stringFindAndReplace(emailBody, "@@DAYSOVEROLA", SLA_PendingDays);


                //    string TIcketReopenedBy = _ticketHistory.CreatedBy;

                //    emailBody = stringFindAndReplace(emailBody, "@@TICKETREOPENEDBY", TIcketReopenedBy);

                //    System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;



                //    emailBody = textInfo.ToTitleCase(emailBody.ToLower());
                //    //emailBody =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailBody.ToLower());

                //    emailSubject = textInfo.ToTitleCase(emailSubject.ToLower());
                //    //emailSubject =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailSubject.ToLower());

                //    emailSubject = emailSubject.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailSubject, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailSubject;
                //    emailBody = emailBody.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailBody, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailBody;


                //    //  DAL.Helper.EmailHelper.SendTestEmail();

                //}
                return emailBody;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;

            }
        }


        //[WebMethod]
        public List<Entities.SLADeclarations> GetSLARules()
        {
            try
            {
                return DAL.Operations.OpSLADeclarations.GetAll();


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }


        [WebMethod]
        public List<string> GetSLARulesNames()
        {
            try
            {
                return DAL.Operations.OpSLADeclarations.GetAllString();


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }


        //public static double CalculateHourse(string date)
        //{
        //    double count = 0;

        //    try
        //    {
        //        date = Convert.ToDateTime(date).ToString("dd/MM/yyyy HH:mm");
        //        DateTime start = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None);
        //        DateTime end = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None);

        //        List<DateTime> holidays = new List<DateTime>();

        //        holidays.Add(new DateTime(2020, 05, 24));
        //        holidays.Add(new DateTime(2020, 05, 25));
        //        holidays.Add(new DateTime(2020, 05, 26));
        //        holidays.Add(new DateTime(2020, 07, 30));
        //        holidays.Add(new DateTime(2020, 07, 31));
        //        holidays.Add(new DateTime(2020, 08, 01));
        //        holidays.Add(new DateTime(2020, 08, 02));
        //        holidays.Add(new DateTime(2020, 08, 23));
        //        holidays.Add(new DateTime(2020, 10, 29));
        //        holidays.Add(new DateTime(2020, 12, 01));
        //        holidays.Add(new DateTime(2020, 12, 02));
        //        holidays.Add(new DateTime(2020, 12, 03));



        //        for (var i = start; i < end; i = i.AddHours(1))
        //        {
        //            if (i.DayOfWeek != DayOfWeek.Friday && i.DayOfWeek != DayOfWeek.Saturday)
        //            {
        //                if (!holidays.Any(x => x.Day == i.Day && x.Month == i.Month && x.Year == i.Year))
        //                {
        //                    if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours <= 17)
        //                    {
        //                        count++;
        //                    }
        //                }
        //            }
        //        }

        //        TimeSpan value = TimeSpan.FromHours(count);
        //        count = value.TotalMinutes;

        //    }
        //    catch (Exception ex)
        //    {
        //        DAL.Helper.Log4Net.Error(ex);
        //    }
        //    return count;
        //}




    }
}
