using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpSLAExecutionLog
    {

        public static int InsertRecord(SLAExecutionLog _SLAExecutionLog)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.SLAExecutionLogs.Add(_SLAExecutionLog);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _SLAExecutionLog.SLAExecutionLogID;
                    string LastInserted = _SLAExecutionLog.ActionComments;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(SLAExecutionLog _SLAExecutionLog)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);

                    //checkerRepository.Add(_SLAExecutionLog);
                    DBContext.SLAExecutionLogs.Add(_SLAExecutionLog);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _SLAExecutionLog.SLAExecutionLogID;

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

     
        public static int InsertRecordAsync(SLAExecutionLog _SLAExecutionLog)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_SLAExecutionLog);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<SLAExecutionLog> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = DBContext.SLAExecutionLogs.OrderByDescending(a => a.TicketInformationID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.SLAExecutionLogID + "-" +x.SLAID + "-" + x.ActionComments ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<SLAExecutionLog> GetSLAExecutionLogbySLAID(int _SLAID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = DBContext.SLAExecutionLogs.Where (x => x.SLAID == _SLAID)
                        .OrderBy(x=>x.SLAID).ToList();

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

        public static List<SLAExecutionLog> GetSLAExecutionLogbyTicketID(int _TicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = DBContext.SLAExecutionLogs.Where(x => x.TicketInformationID == _TicketID)
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

        public static List<SLAExecutionLog> GetSLAExecutionLogbyTicketnHistoryID(int _TicketID, int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = DBContext.SLAExecutionLogs.Where(x => x.TicketInformationID == _TicketID && x.TicketHistoryID == _TicketHistoryID)
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
        public static List<SLAExecutionLog> GetSLAExecutionLogbyTicketnSLAID(int _TicketID, int _SLAID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = DBContext.SLAExecutionLogs.Where(x => x.TicketInformationID == _TicketID && x.SLAID == _SLAID)
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


        public static List<SLAExecutionLog> GetSLAExecutionLogbyTicketList(List<TicketHistory> _TicketList)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);

                    List<int> ticketIDs = _TicketList.Select(x => x.TicketInformationID).ToList();


                    List<SLAExecutionLog> lstSLAExecLog = DBContext.SLAExecutionLogs
                        .Where( x => ticketIDs.Contains(x.TicketInformationID))
                        .OrderByDescending(x => x.SLACheckingTime).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstSLAExecLog;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<SLAExecutionLog> GetAllAsync()
        {
            try
            {


                Task<List<SLAExecutionLog>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<SLAExecutionLog>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);



                    List<SLAExecutionLog> lstLocation = await DBContext.SLAExecutionLogs.ToListAsync();
                   
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


        public static SLAExecutionLog GetRecordbyID(int _SLAExecutionLogID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);
                    SLAExecutionLog RecordObj = DBContext.SLAExecutionLogs.SingleOrDefault(x => x.SLAExecutionLogID == _SLAExecutionLogID);
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


        public static bool DeletebyID(int _SLAExecutionLogID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);
                    SLAExecutionLog RecordObj = DBContext.SLAExecutionLogs.SingleOrDefault(x => x.SLAExecutionLogID == _SLAExecutionLogID);
                    //checkerRepository.Dispose();
                    DBContext.SLAExecutionLogs.Remove(RecordObj);
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


        public static int UpdateRecord(SLAExecutionLog Obj, int __SLAExecutionLogID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLAExecutionLogRepository checkerRepository = new DataModel.SLAExecutionLogRepository(DBContext);

                    SLAExecutionLog CI = GetRecordbyID(__SLAExecutionLogID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.ActionComments = Obj.ActionComments;
                    CI.ActionRequiredID = Obj.ActionRequiredID;

                    CI.Comments = Obj.Comments;
                    CI.IsBreached = Obj.IsBreached;
                    CI.SLAID = Obj.SLAID;
                    CI.TicketInformationID = Obj.TicketInformationID;
                    CI.Triggered = Obj.Triggered;
                  
                  
                    


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __SLAExecutionLogID);
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
