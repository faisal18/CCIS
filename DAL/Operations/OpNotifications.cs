using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpNotification
    {

        public static Notification GetRecordbyID(int _NotificationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);
                    Notification RecordObj = DBContext.Notification.SingleOrDefault(x => x.NotificationID == _NotificationID);
                    //checkerRepository.Dispose();
                    DBContext.Dispose();
                    return RecordObj;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static Notification GetNotificationbyTicketInformationID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    Notification lstLocation = DBContext.Notification.Where(x => x.TicketInformationID == _TicketInformationID)
                        .OrderByDescending(x => x.NotificationID).FirstOrDefault();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static Notification GetLastInternalNotificationbyTicketInformationID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    Notification lstLocation = DBContext.Notification.Where(x => x.TicketInformationID == _TicketInformationID && x.isExternal == false)
                        .OrderByDescending(x => x.NotificationID).FirstOrDefault();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(Notification _Notification)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.Notification.Add(_Notification);
                    DBContext.SaveChanges();

                    int LastInsertedID = _Notification.NotificationID;
                    string LastInserted = _Notification.Comments;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }
        public static int InsertRecordAsync(Notification _Notification)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Notification);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }
        public static async Task<int> CreateRecordAsyncOp(Notification _Notification)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);

                    //checkerRepository.Add(_Notification);
                    DBContext.Notification.Add(_Notification);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _Notification.NotificationID;

                    //checkerRepository.Dispose();
                    // DBContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Notification> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    List<Notification> lstLocation = DBContext.Notification.OrderByDescending(a => a.TicketInformationID).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<Notification> GetTodayNotification()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DateTime Start = DateTime.Today.AddDays(-1);
                    DateTime End = DateTime.Today.AddDays(1);


                    List<Notification> lstLocation = DBContext.Notification
                        .Where(x => x.CreationDate > Start && x.CreationDate < End && x.isExternal == false)
                        .OrderByDescending(a => a.CreationDate).ToList();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<Notification> GetAllAsync()
        {
            try
            {


                Task<List<Notification>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static async Task<List<Notification>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    List<Notification> lstLocation = await DBContext.Notification.ToListAsync();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<string> GetAllString()
        {
            try
            {


                List<string> lstLocation = GetAll().Select(x => x.NotificationID + "-" + x.Comments).ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<Notification> GetNotificationbyComments(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    List<Notification> lstLocation = DBContext.Notification.Where(x => x.Comments == _Comments)
                        .OrderBy(x => x.StatusID).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<Notification> GetNotificationListbyTicketInformationID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



                    List<Notification> lstLocation = DBContext.Notification.Where(x => x.TicketInformationID == _TicketInformationID)
                        .OrderByDescending(x => x.NotificationID).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static bool DeletebyID(int _NotificationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);
                    Notification RecordObj = DBContext.Notification.SingleOrDefault(x => x.NotificationID == _NotificationID);
                    //checkerRepository.Dispose();
                    DBContext.Notification.Remove(RecordObj);
                    DBContext.SaveChanges();
                    // DBContext.Dispose();
                    return true; ;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return false;
            }
        }
        public static int UpdateRecord(Notification Obj, int __NotificationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    Notification CI = GetRecordbyID(__NotificationID);
                    CI.isExternal = Obj.isExternal;
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.CallerKeyID = Obj.CallerKeyID;
                    CI.Comments = Obj.Comments;
                    CI.NotificationTypeID = Obj.NotificationTypeID;
                    CI.RecipientID = Obj.RecipientID;
                    CI.SentByID = Obj.SentByID;
                    CI.StatusID = Obj.StatusID;
                    CI.TicketInformationID = Obj.TicketInformationID;

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;
                    return DBContext.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        //public static void SendNotificationToClient(int _TicketInformationID, string EmailTemplatecategory)
        //{

        //    try
        //    {
        //        using (var DBContext = new DataModel.DALDbContext())
        //        {

        //            bool SendEmailToClient = bool.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SendNotificationtoClients"));


        //            if (SendEmailToClient)
        //            {

        //            }
        //            else
        //            {

        //            }




        //            //DataModel.NotificationRepository checkerRepository = new DataModel.NotificationRepository(DBContext);



        //            //Notification lstLocation = DBContext.Notification.Where(x => x.TicketInformationID == _TicketInformationID)
        //            //    .OrderByDescending(x => x.NotificationID).FirstOrDefault();

        //            //checkerRepository.Dispose();
        //            //DBContext.Dispose();
        //            //      return lstLocation;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.LogError(ex);
        //        //  return null;
        //    }
        //}

        public static int SendNotification([Optional] int EmailTemplateID, [Optional] int SentByID, [Optional] int RecipientID, [Optional] int CallerKeyID,
            [Optional] int Notification_pending_statusid, [Optional] string To_Address, [Optional] string CC_Address, [Optional] string CreatedBy,
            [Optional] DateTime CreationDate)
        {
            int result = -1;
            try
            {
                Entities.Notification notification = new Entities.Notification
                {
                    //Comments = Session["Name"].ToString() + " has created ticket ",
                    //TicketInformationID = TicketInformationID,
                    SentByID = SentByID,
                    RecipientID = RecipientID,

                    ToAddress = To_Address,
                    CCAddress = CC_Address,

                    EmailTemplateID = EmailTemplateID,
                    CallerKeyID = CallerKeyID,

                    //isExternal = false,
                    //NotificationTypeID = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Notification Type").FirstOrDefault().ItemTypesID,
                    StatusID = Notification_pending_statusid,
                    //StatusID = DAL.Operations.OpStatuses.GetStatusesbyTypeName("Notifications").FirstOrDefault().StatusesID,

                    CreatedBy = CreatedBy,
                    CreationDate = CreationDate
                };

                result = DAL.Operations.OpNotification.InsertRecord(notification);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }


        public static string stringFindAndReplace(string CompleteString, string toFind, string toReplace)
        {

            string result = string.Empty;
            try
            {
                if(toReplace == null)
                {
                    toReplace = "";
                }

                if (CompleteString.ToUpper().Contains(toFind.ToUpper()))
                {
                    CompleteString = System.Text.RegularExpressions.Regex.Replace(CompleteString, toFind, toReplace, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                    //CompleteString = CompleteString.ToUpper().Replace(toFind, toReplace);
                    //CompleteString = System.Text.RegularExpressions.Regex.Replace(CompleteString, toFind, toReplace, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                }
                else
                {

                }

                result = CompleteString;
            }
            catch (Exception exc)
            {
                DAL.Operations.Logger.LogError(exc);
                //result =  CompleteString;
            }


            return result;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="TicketInformationID"></param>
        /// <param name="includeHistory"></param>
        /// <param name="_EmailTemplateCategory"></param>
        /// <param name="_Comments"></param>
        /// <param name="NotificationType"></param>
        /// <param name="ClientEmail"></param>
        /// <param name="NotifyAllClientMembers"></param>
        /// <param name="_toAddress"></param>
        /// <param name="_ccAddress"></param>
        /// <param name="GetObjectOnly"> if true it will not send any email and return object only</param>
        /// <returns></returns>
        public static Entities.Notification GenerateNotification(int TicketInformationID, bool includeHistory, string _EmailTemplateCategory, string _Comments, string NotificationType, bool ClientEmail, bool NotifyAllClientMembers, string _toAddress = "", string _ccAddress = "", bool GetObjectOnly = false)
        {
            try
            {
                Entities.Notification _NewNotification = new Entities.Notification();
                Entities.TicketLists _TIcketInformation = DAL.Operations.OpTicketLists.GetRecordbyTicketInformationID(TicketInformationID);
                Entities.Email _EmailTemplate = DAL.Operations.OpEmail.GetEmailObjectByCategoryandNotificationType(_EmailTemplateCategory, NotificationType);


                if (_TIcketInformation._TicketInformation.Count > 0 && _TIcketInformation._TicketHistory.Count > 0 && _EmailTemplate != null)
                {
                    string emailSubject = _EmailTemplate.EmailSubject;
                    string emailBody = _EmailTemplate.EmailBody;

                    /////////////////////////// EMAIL SUBJECT /////////////////////////////////
                    ///ticketnumber

                    string TicketNumber = _TIcketInformation._TicketInformation[0].TicketNumber;
                    emailSubject = stringFindAndReplace(emailSubject, "@@TICKETNUMBER", TicketNumber);

                    /////////////////////////// EMAIL BODY /////////////////////////////////
                    ///ticketnumber
                    ///
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETNUMBER", TicketNumber);



                    //Ticket Heading Comment Specially for the ESCALATION Email

                    emailBody = stringFindAndReplace(emailBody, "@@EMAILCOMMENT", _Comments.Trim());


                    /// Ticket Creation Date
                    DateTime _TicketDate = (DateTime)_TIcketInformation._TicketInformation[0].CreationDate;

                    string ticketCreationDate = _TicketDate.ToString("yyyy-MM-dd");
                    string ticketCreationTime = _TicketDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONDATE", ticketCreationDate);

                    /// TIcket creation  time
                    ///
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONTIME", ticketCreationTime);

                    /// Ticket reopen date 
                    DateTime _TicketReopenDate = (DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                    string ticketReopenDate = _TicketReopenDate.ToString("yyyy-MM-dd");
                    string ticketReopenTime = _TicketReopenDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENDATE", ticketReopenDate);

                    /// Ticket REopenTime
                    /// 
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENTIME", ticketReopenTime);

                    /// Ticket closure date
                    DateTime _TicketClosureDate = (DateTime)(DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                    string ticketClosureDate = _TicketClosureDate.ToString("yyyy-MM-dd");
                    string ticketClosureTime = _TicketClosureDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSUREDATE", ticketClosureDate);

                    /// Ticket CLosure Time
                    /// 
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSURETIME", ticketClosureTime);

                    /// Caller INformation 
                    Entities.CallerInformation _CallerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(_TIcketInformation._TicketInformation[0].CallerKeyID);

                    string callerLicense = _CallerInformation.CallerLicense;

                    List<string> _AllLicense = DAL.Operations.OpLookups.getAllLicenses();

                    string callerName = "";
                    var callerinfo = _AllLicense.Where(a => a.Contains(callerLicense)).FirstOrDefault();

                    if (callerinfo != null)
                    {
                        callerName = callerinfo;
                    }

                    emailBody = stringFindAndReplace(emailBody, "@@CALLERLICENSE", callerName);

                    /// Ticket TYpe

                    List<Entities.Statuses> _Statuses = DAL.Operations.OpStatuses.GetAll();

                    string ticketType = (_Statuses.Where(a => a.StatusesID == _TIcketInformation._TicketInformation[0].TicketType).FirstOrDefault()).Description;
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETTYPE", ticketType);

                    /// Ticket Status

                    Entities.TicketHistory _ticketHistory = _TIcketInformation._TicketHistory
                        .OrderByDescending(a => a.TicketHistoryID).ThenBy(x => x.CreationDate).FirstOrDefault();

                    string TicketStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentStatusID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETSTATUS", TicketStatus);

                    /// routing status
                    string routingStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentSubStatusID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@ROUTINGSTATUS", routingStatus);

                    /// Reported By

                    string Reportedby = _TIcketInformation._TicketInformation[0].CreatedBy;
                    emailBody = stringFindAndReplace(emailBody, "@@REPORTEDBY", Reportedby);

                    //// All Item Types
                    ///for Client/Internal Severity
                    ///client Severity
                    List<Entities.ItemTypes> _itemTypes = DAL.Operations.OpItemTypes.GetAll();

                    string ClientSeverity = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].SeverityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@CLIENTSEVERITY", ClientSeverity);

                    /// Internal Severity
                    string InternalSeverity = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.SeverityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@INTERNALSEVERITY", InternalSeverity);

                    /// Client Priority
                    string ClientPriority = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].PriorityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@CLIENTPRIORITY", ClientPriority);

                    /// Internal Priority
                    string InternalPriority = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.PriorityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@INTERNALPRIORITY", InternalPriority);

                    /// Application
                    /// 

                    string applicationName = DAL.Operations.OpApplications.GetRecordbyID(_TIcketInformation._TicketInformation[0].ApplicationID).Name;

                    emailBody = stringFindAndReplace(emailBody, "@@APPLICATION", applicationName);

                    /// resolution summary by business
                    string ResolutionSummary = _TIcketInformation._TicketInformation[0].ResolutionSummary;

                    //emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONDETAILS", ResolutionSummary);
                    emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONSUMMARY", ResolutionSummary);


                    /// update description by business
                    string IssueDescription = _TIcketInformation._TicketInformation[0].IssueDescription;

                    emailBody = stringFindAndReplace(emailBody, "@@UPDATEDESCRIPTION", IssueDescription);
                    /// Action Taken by business
                    string ActionTakeBusiness = _TIcketInformation._TicketInformation[0].ResolutionActions;

                    emailBody = stringFindAndReplace(emailBody, "@@ResolutionActionByBusiness", ActionTakeBusiness);

                    /// Complete Ticket Comments History.
                    /// 

                    if (includeHistory == true)
                    {
                        string commentshisotry = DAL.Operations.OpTicketHistory.GetAllCommentsbyTicketInformationID(TicketInformationID);

                        emailBody = stringFindAndReplace(emailBody, "@@COMMENTHISTORY", commentshisotry);

                    }

                    /// User SPECIFIC Comments attached with email.
                    /// 

                    emailBody = stringFindAndReplace(emailBody, "@@USERCOMMENT", _Comments);

                    ///  TICKET Comments .
                    /// 

                    string ticketcomments = _ticketHistory.Comments;

                    //emailBody = stringFindAndReplace(emailBody, "@@TICKETCOMMENT", ticketcomments);

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETCOMMENT", _Comments + " : " + ticketcomments);

                    string TicketDescription = _TIcketInformation._TicketInformation[0].Description;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETDESCRIPTION", TicketDescription);

                    string TIcketUPdatedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETUPDATEDBY", TIcketUPdatedBy);

                    string TIcketClosedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETCLOSEDBY", TIcketClosedBy);

                    // OLA--------------- 

                    string OLA = "";
                    Entities.TicketHistory _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L3 - Jira Created");

                    if (_OLATicketHistory != null)
                    {
                        //OLA = DAL.Helper.DateTimeHelper.getTimeElapsed(DateTime.Now, (DateTime)_OLATicketHistory.CreationDate);
                        TimeSpan _timespan1 = TimeSpan.FromMinutes(DAL.Helper.DateTimeHelper.CalculateHourse(_OLATicketHistory.CreationDate.ToString()));
                        OLA = String.Format("{0} days {1} hours {2} minutes ", _timespan1.Days, _timespan1.Hours, _timespan1.Minutes);

                    }
                    else
                    {
                        _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L2");
                        if (_OLATicketHistory != null)
                        {
                            //OLA = DAL.Helper.DateTimeHelper.getTimeElapsed(DateTime.Now , (DateTime)_OLATicketHistory.CreationDate);
                            TimeSpan _timespan1 = TimeSpan.FromMinutes(DAL.Helper.DateTimeHelper.CalculateHourse(_OLATicketHistory.CreationDate.ToString()));
                            OLA = String.Format("{0} days {1} hours {2} minutes ", _timespan1.Days, _timespan1.Hours, _timespan1.Minutes);
                        }
                    }

                    emailBody = stringFindAndReplace(emailBody, "@@OLA", OLA);

                    //SLA
                    //string SLA_PendingDays = (DateTime.Now - (DateTime)_TIcketInformation._TicketInformation[0].CreationDate).TotalDays.ToString();
                    //string SLA_PendingDays = DAL.Helper.DateTimeHelper.getTimeElapsed(DateTime.Now, (DateTime)_TIcketInformation._TicketInformation[0].CreationDate);

                    TimeSpan _timespan = TimeSpan.FromMinutes(DAL.Helper.DateTimeHelper.CalculateHourse(_TicketDate.ToString()));
                    string SLA_PendingDays = String.Format("{0} days {1} hours {2} minutes ", _timespan.Days, _timespan.Hours, _timespan.Minutes);

                    emailBody = stringFindAndReplace(emailBody, "@@SLA", SLA_PendingDays);

                    /// SLA crossed.

                    emailBody = stringFindAndReplace(emailBody, "@@DAYSOVEROLA", SLA_PendingDays);


                    string TIcketReopenedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETREOPENEDBY", TIcketReopenedBy);

                    //System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;



                    //emailBody = textInfo.ToTitleCase(emailBody.ToLower());
                    ////emailBody =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailBody.ToLower());

                    //emailSubject = textInfo.ToTitleCase(emailSubject.ToLower());

                    //emailSubject = emailSubject.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailSubject, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailSubject;
                    //emailBody = emailBody.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailBody, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailBody;
                    //emailSubject =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailSubject.ToLower());

                    string L1ConfigEmail = System.Configuration.ConfigurationManager.AppSettings.Get("SupportEmail");
                    _NewNotification = new Entities.Notification()
                    {
                        TicketInformationID = _TIcketInformation._TicketInformation[0].TicketInformationID,
                        CallerKeyID = _TIcketInformation._TicketInformation[0].CallerKeyID,
                        StatusID = _Statuses.Where(x => x.Description.ToUpper() == "PENDING" && x.Types.ToUpper() == "NOTIFICATIONS").FirstOrDefault().StatusesID,
                        EmailTemplateID = _EmailTemplate.TemplateID,
                        Subject = emailSubject,
                        Comments = _Comments,
                        EventCategory = _EmailTemplateCategory,
                        Body = emailBody,
                        CreationDate = DateTime.Now,
                        CreatedBy = _ticketHistory.CreatedBy,
                        SentByID = _ticketHistory.AssignedFromID,
                        RecipientID = _ticketHistory.AssignedTOID,
                        NotificationTypeID = _itemTypes.Where(a => a.Description.ToUpper() == NotificationType && a.Categories.ToUpper() == "NOTIFICATION TYPE").FirstOrDefault().ItemTypesID

                    };



                    if (ClientEmail && NotifyAllClientMembers)
                    {

                        _NewNotification.isExternal = true;

                        _NewNotification.CCAddress = L1ConfigEmail;
                        _NewNotification.ToAddress = DAL.Operations.OpCallerInfo.GetEmailStringbyLicenseID(
                          DAL.Operations.OpCallerInfo.GetRecordbyID
                          (
                              _TIcketInformation._TicketInformation[0].CallerKeyID
                          ).CallerLicense);

                    }
                    else
                     if (ClientEmail && !NotifyAllClientMembers)
                    {
                        _NewNotification.CCAddress = L1ConfigEmail;
                        _NewNotification.isExternal = true;
                        _NewNotification.ToAddress = DAL.Operations.OpCallerInfo.GetEmailStringbyCallerKeyID(
                        DAL.Operations.OpCallerInfo.GetRecordbyID
                        (
                            _TIcketInformation._TicketInformation[0].CallerKeyID
                        ).CallerKeyID);
                    }
                    
                    else
                     if (!ClientEmail && !NotifyAllClientMembers)
                    {
                        _NewNotification.isExternal = false;
                        _NewNotification.ToAddress = _toAddress;
                        _NewNotification.CCAddress = _ccAddress;
                    }

                    if (GetObjectOnly)
                    {
                    }
                    else
                    {
                      DAL.Operations.OpNotification.InsertRecord(_NewNotification);
                    }
                }
                else
                {

                    DAL.Operations.Logger.LogError("", "Ticket Information or Email Template missing !!!", "");
                }

                //  DAL.Helper.EmailHelper.SendTestEmail();
                return _NewNotification;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;

            }
        }

        public static string ParseTemplate(int TicketInformationID, bool includeHistory, string _EmailTemplateCategory, string _Comments, string NotificationType)
        {
            try
            {
                Entities.TicketLists _TIcketInformation = DAL.Operations.OpTicketLists.GetRecordbyTicketInformationID(TicketInformationID);
                Entities.Email _EmailTemplate = DAL.Operations.OpEmail.GetEmailObjectByCategoryandNotificationType(_EmailTemplateCategory, NotificationType);
                string emailBody = _EmailTemplate.EmailBody;
                string emailSubject = _EmailTemplate.EmailSubject;


                if (_TIcketInformation._TicketInformation.Count > 0 && _TIcketInformation._TicketHistory.Count > 0 && _EmailTemplate != null)
                {

                    /////////////////////////// EMAIL SUBJECT /////////////////////////////////
                    ///ticketnumber

                    string TicketNumber = _TIcketInformation._TicketInformation[0].TicketNumber;
                    emailSubject = stringFindAndReplace(emailSubject, "@@TICKETNUMBER", TicketNumber);

                    /////////////////////////// EMAIL BODY /////////////////////////////////
                    ///ticketnumber
                    ///
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETNUMBER", TicketNumber);

                    /// Ticket Creation Date
                    DateTime _TicketDate = (DateTime)_TIcketInformation._TicketInformation[0].CreationDate;

                    string ticketCreationDate = _TicketDate.ToString("yyyy-MM-dd");
                    string ticketCreationTime = _TicketDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONDATE", ticketCreationDate);

                    /// TIcket creation  time
                    ///
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCREATIONTIME", ticketCreationTime);

                    /// Ticket reopen date 
                    DateTime _TicketReopenDate = (DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                    string ticketReopenDate = _TicketReopenDate.ToString("yyyy-MM-dd");
                    string ticketReopenTime = _TicketReopenDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENDATE", ticketReopenDate);

                    /// Ticket REopenTime
                    /// 
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONREOPENTIME", ticketReopenTime);

                    /// Ticket closure date
                    DateTime _TicketClosureDate = (DateTime)(DateTime)DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(_TIcketInformation._TicketHistory).CreationDate;

                    string ticketClosureDate = _TicketClosureDate.ToString("yyyy-MM-dd");
                    string ticketClosureTime = _TicketClosureDate.ToString("hh:mm:ss");

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSUREDATE", ticketClosureDate);

                    /// Ticket CLosure Time
                    /// 
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETINFORMATIONCLOSURETIME", ticketClosureTime);

                    /// Caller INformation 
                    Entities.CallerInformation _CallerInformation = DAL.Operations.OpCallerInfo.GetRecordbyID(_TIcketInformation._TicketInformation[0].CallerKeyID);

                    string callerLicense = _CallerInformation.CallerLicense;

                    List<string> _AllLicense = DAL.Operations.OpLookups.getAllLicenses();

                    string callerName = "";
                    var callerinfo = _AllLicense.Where(a => a.Contains(callerLicense)).FirstOrDefault();

                    if (callerinfo != null)
                    {
                        callerName = callerinfo;
                    }

                    emailBody = stringFindAndReplace(emailBody, "@@CALLERLICENSE", callerName);

                    /// Ticket TYpe

                    List<Entities.Statuses> _Statuses = DAL.Operations.OpStatuses.GetAll();

                    string ticketType = (_Statuses.Where(a => a.StatusesID == _TIcketInformation._TicketInformation[0].TicketType).FirstOrDefault()).Description;
                    emailBody = stringFindAndReplace(emailBody, "@@TICKETTYPE", ticketType);

                    /// Ticket Status

                    Entities.TicketHistory _ticketHistory = _TIcketInformation._TicketHistory
                        .OrderByDescending(a => a.TicketHistoryID).ThenBy(x => x.CreationDate).FirstOrDefault();

                    string TicketStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentStatusID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETSTATUS", TicketStatus);

                    /// routing status
                    string routingStatus = (_Statuses.Where(a => a.StatusesID == _ticketHistory.IncidentSubStatusID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@ROUTINGSTATUS", routingStatus);

                    /// Reported By

                    string Reportedby = _TIcketInformation._TicketInformation[0].CreatedBy;
                    emailBody = stringFindAndReplace(emailBody, "@@REPORTEDBY", Reportedby);

                    //// All Item Types
                    ///for Client/Internal Severity
                    ///client Severity
                    List<Entities.ItemTypes> _itemTypes = DAL.Operations.OpItemTypes.GetAll();

                    string ClientSeverity = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].SeverityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@CLIENTSEVERITY", ClientSeverity);

                    /// Internal Severity
                    string InternalSeverity = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.SeverityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@INTERNALSEVERITY", InternalSeverity);

                    /// Client Priority
                    string ClientPriority = (_itemTypes.Where(a => a.ItemTypesID == _TIcketInformation._TicketInformation[0].PriorityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@CLIENTPRIORITY", ClientPriority);

                    /// Internal Priority
                    string InternalPriority = (_itemTypes.Where(a => a.ItemTypesID == _ticketHistory.PriorityID).FirstOrDefault()).Description;

                    emailBody = stringFindAndReplace(emailBody, "@@INTERNALPRIORITY", InternalPriority);

                    /// Application
                    /// 

                    string applicationName = DAL.Operations.OpApplications.GetRecordbyID(_TIcketInformation._TicketInformation[0].ApplicationID).Name;

                    emailBody = stringFindAndReplace(emailBody, "@@APPLICATION", applicationName);

                    /// resolution summary by business
                    string ResolutionSummary = _TIcketInformation._TicketInformation[0].ResolutionSummary;

                    emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONSUMMARY", ResolutionSummary);

                    /// update description by business
                    string IssueDescription = _TIcketInformation._TicketInformation[0].IssueDescription;

                    emailBody = stringFindAndReplace(emailBody, "@@UPDATEDESCRIPTION", IssueDescription);
                    /// Action Taken by business
                    string ActionTakeBusiness = _TIcketInformation._TicketInformation[0].ResolutionActions;

                    //emailBody = stringFindAndReplace(emailBody, "@@ResolutionActionByBusiness", ActionTakeBusiness);
                    emailBody = stringFindAndReplace(emailBody, "@@RESOLUTIONACTIONBYBUSINESS", ActionTakeBusiness);


                    /// Complete Ticket Comments History.
                    /// 

                    if (includeHistory == true)
                    {
                        string commentshisotry = DAL.Operations.OpTicketHistory.GetAllCommentsbyTicketInformationID(TicketInformationID);

                        emailBody = stringFindAndReplace(emailBody, "@@COMMENTHISTORY", commentshisotry);

                    }

                    /// User SPECIFIC Comments attached with email.
                    /// 

                    emailBody = stringFindAndReplace(emailBody, "@@USERCOMMENT", _Comments);

                    ///  TICKET Comments .
                    /// 

                    string ticketcomments = _ticketHistory.Comments;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETCOMMENT", ticketcomments);

                    string TicketDescription = _TIcketInformation._TicketInformation[0].Description;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETDESCRIPTION", TicketDescription);

                    string TIcketUPdatedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETUPDATEDBY", TIcketUPdatedBy);

                    string TIcketClosedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETCLOSEDBY", TIcketClosedBy);

                    // OLA--------------- 

                    string OLA = "";
                    Entities.TicketHistory _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L3 - Jira Created");

                    if (_OLATicketHistory != null)
                    {
                        OLA = (DateTime.Now - (DateTime)_OLATicketHistory.CreationDate).TotalDays.ToString();

                    }
                    else
                    {
                        _OLATicketHistory = DAL.Operations.OpTicketHistory.GetTicketHistorybyTicketIDandActivity(TicketInformationID, "Assigned to L2");
                        if (_OLATicketHistory != null)
                        {
                            OLA = (DateTime.Now - (DateTime)_OLATicketHistory.CreationDate).TotalDays.ToString();
                        }
                    }

                    emailBody = stringFindAndReplace(emailBody, "@@OLA", OLA);

                    //SLA
                    string SLA_PendingDays = (DateTime.Now - (DateTime)_TIcketInformation._TicketInformation[0].CreationDate).TotalDays.ToString();

                    emailBody = stringFindAndReplace(emailBody, "@@SLA", SLA_PendingDays);

                    /// SLA crossed.

                    emailBody = stringFindAndReplace(emailBody, "@@DAYSOVEROLA", SLA_PendingDays);


                    string TIcketReopenedBy = _ticketHistory.CreatedBy;

                    emailBody = stringFindAndReplace(emailBody, "@@TICKETREOPENEDBY", TIcketReopenedBy);

                    System.Globalization.TextInfo textInfo = new System.Globalization.CultureInfo("en-US", false).TextInfo;



                    //emailBody = textInfo.ToTitleCase(emailBody.ToLower());
                    //emailBody =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailBody.ToLower());

                    //emailSubject = textInfo.ToTitleCase(emailSubject.ToLower());
                    //emailSubject =  System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(emailSubject.ToLower());

                    emailSubject = emailSubject.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailSubject, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailSubject;
                    emailBody = emailBody.Contains(textInfo.ToTitleCase(TicketNumber)) ? stringFindAndReplace(emailBody, textInfo.ToTitleCase(TicketNumber), TicketNumber.ToUpper()) : emailBody;


                    //  DAL.Helper.EmailHelper.SendTestEmail();

                }
                return emailBody;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;

            }
        }



    }
}
