using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpTicketHistory
    {
        /// <summary>
        /// Whatever added in the comments field will be returned.
        /// </summary>
        /// <param name="_TicketHistory"></param>
        /// <returns></returns>
        public static int InsertRecord(TicketHistory _TicketHistory)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.TicketHistory.Add(_TicketHistory);
                    DBContext.SaveChanges();

                    int LastInsertedID = _TicketHistory.TicketHistoryID;
                    string LastInserted = _TicketHistory.Comments;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }
        public static async Task<int> CreateRecordAsyncOp(TicketHistory _TicketHistory)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);

                    //checkerRepository.Add(_TicketHistory);
                    DBContext.TicketHistory.Add(_TicketHistory);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _TicketHistory.TicketHistoryID;

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
        public static int InsertRecordAsync(TicketHistory _TicketHistory)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_TicketHistory);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<TicketHistory> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.OrderBy(a => a.TicketInformationID).ThenBy(b => b.TicketHistoryID).ToList();

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

        public static List<TicketHistory> GetAll_Limited()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.OrderBy(a => a.TicketInformationID).Take(20).ToList();

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


                //List<string> lstLocation = GetAll().Select(x => x.TicketHistoryID + "-" +x.Comments ).ToList();
                List<string> lstLocation = GetAll().Select(x => x.TicketHistoryID + "-" + x.Comments + "-" + x.IncidentStatusesIDs_FK.Description).ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetTicketHistorybyPriority(int _PriorityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.PriorityID == _PriorityID)
                        .OrderBy(x => x.Comments).ToList();

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
        public static List<TicketHistory> GetTicketHistorybyStatus(int _StatusID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.IncidentStatusID == _StatusID)
                        .OrderByDescending(x => x.CreationDate).ToList();

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
        public static List<TicketHistory> GetTicketHistorybySubStatus(int _SubStatusID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.IncidentSubStatusID == _SubStatusID)
                        .OrderByDescending(x => x.CreationDate).ToList();

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
        public static List<TicketHistory> GetOpenIncidents()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> openStatus = new List<string>();
                    openStatus.Add("New");
                    openStatus.Add("In Progress");
                    openStatus.Add("ReOpened");
                    openStatus.Add("Awaiting info from customer");
                    openStatus.Add("Awaiting schedule");
                    openStatus.Add("In Progress, L2 Support");

                    List<int> statusIDs = OpStatuses.GetOpenStatusID(openStatus, "Incident Status");


                    List<int> _maxTicketHistoryID = DBContext.TicketHistory.GroupBy(a => a.TicketInformationID).Select(grp => grp.Max(a => a.TicketHistoryID)).ToList();
                    List<Entities.TicketHistory> getTicketHistorforMax = DBContext.TicketHistory.Where(b => _maxTicketHistoryID.Contains(b.TicketHistoryID)).ToList();
                    List<TicketHistory> checkTicketStatus = getTicketHistorforMax.Where(x => statusIDs.Contains(x.IncidentStatusID)).ToList();
                   
                    return checkTicketStatus;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetOpenIncidentsByTicketInformation(List<int> ticketInformation)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> openStatus = new List<string>();
                    openStatus.Add("New");
                    openStatus.Add("In Progress");
                    openStatus.Add("ReOpened");
                    openStatus.Add("Awaiting info from customer");
                    openStatus.Add("Awaiting schedule");
                    openStatus.Add("In Progress, L2 Support");

                    List<int> statusIDs = OpStatuses.GetOpenStatusID(openStatus, "Incident Status");

                    List<TicketHistory> ticketHistories = DBContext.TicketHistory.Where(x => ticketInformation.Contains(x.TicketInformationID)).ToList();

                    List<int> _maxTicketHistoryID = ticketHistories.GroupBy(a => a.TicketInformationID).Select(grp => grp.Max(a => a.TicketHistoryID)).ToList();
                    List<Entities.TicketHistory> getTicketHistorforMax = DBContext.TicketHistory.Where(b => _maxTicketHistoryID.Contains(b.TicketHistoryID)).ToList();
                    List<TicketHistory> checkTicketStatus = getTicketHistorforMax.Where(x => statusIDs.Contains(x.IncidentStatusID)).ToList();

                    //List<int> _maxTicketHistoryID = DBContext.TicketHistory.GroupBy(a => a.TicketInformationID).Select(grp => grp.Max(a => a.TicketHistoryID)).ToList();
                    //List<Entities.TicketHistory> getTicketHistorforMax = DBContext.TicketHistory.Where(b => _maxTicketHistoryID.Contains(b.TicketHistoryID)).ToList();
                    //List<TicketHistory> checkTicketStatus = getTicketHistorforMax.Where(x => statusIDs.Contains(x.IncidentStatusID)).ToList();

                    return checkTicketStatus;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static List<TicketHistory> GetOpenIncidentsByApplicationID(int _ApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);

                    List<string> openStatus = new List<string>();
                    openStatus.Add("New");
                    openStatus.Add("ReOpened");
                    openStatus.Add("In Progress");
                    openStatus.Add("Awaiting info from customer");
                    openStatus.Add("Awaiting schedule");
                    openStatus.Add("In Progress, L2 Support");

                    List<int> statusIDs = OpStatuses.GetOpenStatusID(openStatus, "Incident Status");
                    List<int> ticketInformation = OpTicketInformation.GetTicketInformationbyApplicationID(_ApplicationID).Select(x => x.TicketInformationID).ToList();


                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => statusIDs.Contains(x.IncidentStatusID) && ticketInformation.Contains(x.TicketInformationID))
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)

                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();


                    List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetOpenIncidentsByPersonID(int PersonID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> openStatus = new List<string>();
                    openStatus.Add("New");
                    openStatus.Add("ReOpened");
                    openStatus.Add("In Progress");
                    openStatus.Add("Awaiting info from customer");
                    openStatus.Add("Awaiting schedule");
                    openStatus.Add("In Progress, L2 Support");


                    List<int> statusIDs = OpStatuses.GetOpenStatusID(openStatus, "Incident Status");


                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => statusIDs.Contains(x.IncidentStatusID) && x.AssignedTOID == PersonID)
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)

                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();


                    List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();

                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketHistory> GetCloseIncidents()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> CloseStatus = new List<string>();
                    //CloseStatus.Add("Closed");
                    CloseStatus.Add("Resolved without KB");
                    CloseStatus.Add("Resolved with KB");

                    List<int> statusIDs = OpStatuses.GetOpenStatusID(CloseStatus, "Incident Status");

                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => statusIDs.Contains(x.IncidentStatusID))
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)
                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();


                    List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();

                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetResolvedIncidents()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> CloseStatus = new List<string>();
                    CloseStatus.Add("Resolved without KB");
                    CloseStatus.Add("Resolved with KB");

                    List<int> statusIDs = OpStatuses.GetOpenStatusID(CloseStatus, "Incident Status");

                    //List<TicketHistory> lstLocation = DBContext.TicketHistory
                    // .Where(x => statusIDs.Contains(x.IncidentStatusID))
                    // .OrderByDescending(x => x.TicketHistoryID)
                    // .ToList();

                    //var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)
                    //    .Select(grp => grp.Max(a => a.TicketHistoryID))
                    //    .ToList();

                    //List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();
                    //return lstLocation3;

                    List<int> _maxTicketHistoryID = DBContext.TicketHistory.GroupBy(a => a.TicketInformationID).
                            Select(grp => grp.Max(a => a.TicketHistoryID))
                            .ToList();
                    List<Entities.TicketHistory> getTicketHistorforMax = DBContext.TicketHistory.Where(b => _maxTicketHistoryID.Contains(b.TicketHistoryID)).ToList();
                    List<TicketHistory> checkTicketStatus = getTicketHistorforMax.Where(x => statusIDs.Contains(x.IncidentStatusID)).ToList();

                    return checkTicketStatus;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetCloseIncidentsByApplicationID(int _ApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);

                    List<string> CloseStatus = new List<string>();
                    CloseStatus.Add("Closed");
                    CloseStatus.Add("Resolved without KB");
                    CloseStatus.Add("Resolved with KB");


                    List<int> statusIDs = OpStatuses.GetOpenStatusID(CloseStatus, "Incident Status");
                    List<int> ticketInformation = OpTicketInformation.GetTicketInformationbyApplicationID(_ApplicationID).Select(x => x.TicketInformationID).ToList();


                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => statusIDs.Contains(x.IncidentStatusID) && ticketInformation.Contains(x.TicketInformationID))
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)

                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();


                    List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketHistory> GetCloseIncidentsByPersonID(int PersonID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<string> CloseStatus = new List<string>();
                    CloseStatus.Add("Closed");
                    CloseStatus.Add("Resolved without KB");
                    CloseStatus.Add("Resolved with KB");


                    List<int> statusIDs = OpStatuses.GetOpenStatusID(CloseStatus, "Incident Status");


                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => statusIDs.Contains(x.IncidentStatusID) && x.AssignedTOID == PersonID)
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)

                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();


                    List<TicketHistory> lstLocation3 = lstLocation.Where(x => lstLocation2.Contains(x.TicketHistoryID)).ToList();

                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketHistory> GetGroupedByTicket(int _ApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    List<int> ticketInformation = OpTicketInformation.GetTicketInformationbyApplicationID(_ApplicationID)
                        .Select(x => x.TicketInformationID)
                        .ToList();

                    List<TicketHistory> lstLocation = DBContext.TicketHistory
                     .Where(x => ticketInformation.Contains(x.TicketInformationID))
                     .OrderByDescending(x => x.TicketHistoryID)
                     .ToList();

                    var lstLocation2 = lstLocation.GroupBy(x => x.TicketInformationID)
                        .Select(grp => grp.Max(a => a.TicketHistoryID))
                        .ToList();

                    List<TicketHistory> lstLocation3 = lstLocation
                        .Where(x => lstLocation2.Contains(x.TicketHistoryID))
                        .ToList();

                    return lstLocation3;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketHistory> GetTicketHistorybyStatuses(int _StatusID, int _SubStatusID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.IncidentSubStatusID == _SubStatusID && x.IncidentStatusID == _StatusID)
                        .OrderByDescending(x => x.CreationDate).ToList();

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

        public static TicketHistory GetLatestTicketHistoryByTicketID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var max = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketInformationID).Select(x => x.TicketHistoryID).Max();
                    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketInformationID && x.TicketHistoryID == max).OrderByDescending(x => x.CreationDate).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static TicketHistory GetLatestTicketHistoryByTicketID_Old(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketInformationID).Select(x => x.TicketHistoryID).Max();
                    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketInformationID && x.TicketHistoryID == max).OrderByDescending(x => x.CreationDate).FirstOrDefault();
                    return result;

                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }



        public static TicketHistory GetTicketHistorybyTicketIDandActivity(int _TicketinformationID, string _Activity)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    // var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketinformationID).Select(x => x.TicketHistoryID).Max();
                    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketinformationID && x.Activity == _Activity).OrderByDescending(x => x.CreationDate).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static TicketHistory GetLatestTicketHistoryByTicketID(List<Entities.TicketHistory> _TicketHistoryList)
        {
            try
            {
                Entities.TicketHistory _TicketHistoryMax = new TicketHistory();

                _TicketHistoryList.OrderByDescending(x => x.TicketHistoryID).FirstOrDefault();

                return _TicketHistoryMax;


                //using (var DBContext = new DataModel.DALDbContext())
                //{
                //    var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketID).Select(x => x.TicketHistoryID).Max();
                //    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketID && x.TicketHistoryID == max).OrderByDescending(x => x.CreationDate).FirstOrDefault();

                //}
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static TicketHistory GetLatestTicketHistoryByTicketID(List<Entities.TicketHistory> _TicketHistoryList, int _TicketInformationID)
        {
            try
            {
                List<Entities.TicketHistory> ticketHistories = _TicketHistoryList.Where(a => a.TicketInformationID == _TicketInformationID).ToList();
                Entities.TicketHistory _TicketHistoryMax = new TicketHistory();

                _TicketHistoryMax = ticketHistories.OrderByDescending(x => x.TicketHistoryID).FirstOrDefault();

                return _TicketHistoryMax;


                //using (var DBContext = new DataModel.DALDbContext())
                //{
                //    var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketID).Select(x => x.TicketHistoryID).Max();
                //    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketID && x.TicketHistoryID == max).OrderByDescending(x => x.CreationDate).FirstOrDefault();

                //}
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }



        public static string GetAllCommentsbyTicketInformationID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //  var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketInformationID).Select(x => x.Comments).;
                    var AllComments = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketInformationID)
                        .OrderByDescending(x => x.CreationDate)
                        .ThenBy(a => a.TicketHistoryID)
                        .Select(f => f.CreationDate + " || " + f.CreatedBy + " || " + f.Activity + " || " + f.Comments).ToList();

                    string result = "";
                    foreach (var item in AllComments)
                    {
                        result += Environment.NewLine + item + Environment.NewLine;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketHistory> GetTicketHistorybySLA(int _StatusID, int _SubStatusID, int _prioritID, int _SeverityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where
                        (x => x.IncidentSubStatusID == _SubStatusID && x.IncidentStatusID == _StatusID
                        && x.PriorityID == _prioritID && x.SeverityID == _SeverityID
                        )
                        .OrderByDescending(x => x.CreationDate).ToList();

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
        public static List<TicketHistory> GetTicketHistorybySeverity(int _SeverityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.SeverityID == _SeverityID)
                        .OrderBy(x => x.Comments).ToList();

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
        public static List<TicketHistory> GetTicketHistorybyTicketID(int _TicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketID)
                        .OrderBy(x => x.TicketInformationID).ToList();

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
        public static List<TicketHistory> GetTicketHistorybyTicketHistoryID(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = DBContext.TicketHistory.Where(x => x.TicketHistoryID == _TicketHistoryID)
                        .OrderBy(x => x.TicketInformationID).ToList();

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
        public static List<TicketHistory> GetAllAsync()
        {
            try
            {


                Task<List<TicketHistory>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static async Task<List<TicketHistory>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);



                    List<TicketHistory> lstLocation = await DBContext.TicketHistory.ToListAsync();

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
        public static TicketHistory GetRecordbyID(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);
                    TicketHistory RecordObj = DBContext.TicketHistory.SingleOrDefault(x => x.TicketHistoryID == _TicketHistoryID);
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
        public static int GetAssigneeforTicket(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);
                    int RecordObj = DBContext.TicketHistory
                        .Where(x => x.TicketHistoryID == _TicketHistoryID)
                        .Select(x => x.AssignedTOID).Single();
                    //checkerRepository.Dispose();
                    DBContext.Dispose();
                    return RecordObj;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }
        public static string GetTicketGroupAssignment(List<Entities.TicketHistory> _TicketHistoryList, int _TicketInformationID)
        {
            try
            {
                List<Entities.TicketHistory> ticketHistories = _TicketHistoryList.Where(a => a.Activity.ToUpper().Contains("ASSIGNED TO")).ToList();
                Entities.TicketHistory _TicketHistoryMax = new TicketHistory();

                if (ticketHistories.Count > 0)
                {
                    _TicketHistoryMax = ticketHistories.OrderByDescending(x => x.TicketHistoryID).FirstOrDefault();

                    return _TicketHistoryMax.Activity;
                }
                else
                {
                    return null;
                }


                //using (var DBContext = new DataModel.DALDbContext())
                //{
                //    var max = DBContext.TicketHistory.ToList().Where(x => x.TicketInformationID == _TicketID).Select(x => x.TicketHistoryID).Max();
                //    var result = DBContext.TicketHistory.Where(x => x.TicketInformationID == _TicketID && x.TicketHistoryID == max).OrderByDescending(x => x.CreationDate).FirstOrDefault();

                //}
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }



        public static bool DeletebyID(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);
                    TicketHistory RecordObj = DBContext.TicketHistory.SingleOrDefault(x => x.TicketHistoryID == _TicketHistoryID);
                    //checkerRepository.Dispose();
                    DBContext.TicketHistory.Remove(RecordObj);
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
        public static int UpdateRecord(TicketHistory Obj, int __TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketHistoryRepository checkerRepository = new DataModel.TicketHistoryRepository(DBContext);

                    TicketHistory CI = GetRecordbyID(__TicketHistoryID);
                    CI.JiraNumber = Obj.JiraNumber;
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.AssignedFromID = Obj.AssignedFromID;
                    CI.AssignedTOID = Obj.AssignedTOID;
                    CI.Comments = Obj.Comments;
                    CI.PriorityID = Obj.PriorityID;
                    CI.SeverityID = Obj.SeverityID;
                    CI.IncidentStatusID = Obj.IncidentStatusID;
                    CI.IncidentSubStatusID = Obj.IncidentSubStatusID;
                    CI.TicketInformationID = Obj.TicketInformationID;
                    CI.Activity = Obj.Activity;
                    CI.SupportTypeID = Obj.SupportTypeID;






                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __TicketHistoryID);
                    ////checkerRepository.Dispose();
                    //DBContext.Dispose();
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }


        

    }
}
