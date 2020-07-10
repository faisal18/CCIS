using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpTicketInformation
    {
        /// <summary>
        /// Whatever added in the Subject field will be returned.
        /// </summary>
        /// <param name="_TicketInformation"></param>
        /// <returns></returns>
        public static int InsertRecord(TicketInformation _TicketInformation)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.TicketInformation.Add(_TicketInformation);
                    DBContext.SaveChanges();

                    int LastInsertedID = _TicketInformation.TicketInformationID;
                    string LastInserted = _TicketInformation.Subject;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(TicketInformation _TicketInformation)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);

                    //checkerRepository.Add(_TicketInformation);
                    DBContext.TicketInformation.Add(_TicketInformation);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _TicketInformation.TicketInformationID;

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


        public static int InsertRecordAsync(TicketInformation _TicketInformation)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_TicketInformation);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<TicketInformation> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.OrderByDescending(a => a.CreationDate).ThenByDescending(b => b.TicketInformationID).ToList();

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
        public static List<TicketInformation> GetTicketInformationByCreationDate(DateTime date)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x=>x.CreationDate >= date).OrderByDescending(a => a.CreationDate).ThenByDescending(b => b.TicketInformationID).ToList();

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

        public static List<TicketInformation> GetAll_Limited()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.OrderByDescending(a => a.CreationDate).Take(20).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.TicketInformationID + "-" + x.Subject + "-" + x.Description).ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<TicketInformation> GetTicketInformationbyPriority(int _PriorityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.PriorityID == _PriorityID)
                        .OrderBy(x => x.TicketNumber).ToList();

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

        public static List<TicketInformation> GetTicketInformationbyApplicationID(int _ApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.ApplicationID == _ApplicationID)
                        .OrderBy(x => x.TicketNumber).ToList();

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


        public static List<TicketInformation> GetTicketInformationbySeverity(int _SeverityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.SeverityID == _SeverityID)
                        .OrderBy(x => x.TicketNumber).ToList();

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

        public static List<TicketInformation> GetTicketInformationbyTicketID(int _TicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.TicketInformationID == _TicketID)
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

        public static List<TicketInformation> GetTicketInformationbyTicketNumber(string _TicketNumber)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.TicketNumber == _TicketNumber)
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

        public static string GetTicketCount()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    var ALlJoinData = DBContext.TicketInformation.Join(DBContext.Applications,
                        TI => TI.ApplicationID, a => a.ApplicationsID,

                        (TI, a) => new { AppDetails = a, TicketsDetails = TI }).ToList();

                    // .Select(a => new  {  count = a.Count(), a.FirstOrDefault().ApplicationID }).ToList();


                    var lstLocation = ALlJoinData.GroupBy(x => x.AppDetails.ApplicationsID)
                        .Select(a => new { count = a.Count(), a.FirstOrDefault().AppDetails.Name }).ToList();

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return "";
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }



        public static List<TicketInformation> GetTicketInformationbyTicketInformationID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = DBContext.TicketInformation.Where(x => x.TicketInformationID == _TicketInformationID)
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

        public static List<TicketInformation> GetTicketInformationByCallerID(int _CallerID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    List<TicketInformation> result = DBContext.TicketInformation.Where(x => x.CallerKeyID == _CallerID).OrderByDescending(x => x.CreatedBy).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketInformation> GetAllAsync()
        {
            try
            {


                Task<List<TicketInformation>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<TicketInformation>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);



                    List<TicketInformation> lstLocation = await DBContext.TicketInformation.ToListAsync();

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


        public static TicketInformation GetRecordbyID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);
                    TicketInformation RecordObj = DBContext.TicketInformation.SingleOrDefault(x => x.TicketInformationID == _TicketInformationID);
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

        public static Guid GetTicketGUID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);
                    TicketInformation RecordObj = DBContext.TicketInformation.SingleOrDefault(x => x.TicketInformationID == _TicketInformationID);
                    //checkerRepository.Dispose();
                    DBContext.Dispose();
                    return RecordObj.TicketGUIDKey;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return Guid.Empty;
            }
        }



        public static bool DeletebyID(int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);
                    TicketInformation RecordObj = DBContext.TicketInformation.SingleOrDefault(x => x.TicketInformationID == _TicketInformationID);
                    //checkerRepository.Dispose();
                    DBContext.TicketInformation.Remove(RecordObj);
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


        public static int UpdateRecord(TicketInformation Obj, int __TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketInformationRepository checkerRepository = new DataModel.TicketInformationRepository(DBContext);

                    TicketInformation CI = GetRecordbyID(__TicketInformationID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.ApplicationID = Obj.ApplicationID;
                    CI.CallerKeyID = Obj.CallerKeyID;
                    CI.Description = Obj.Description;
                    CI.ActionTaken = Obj.ActionTaken;
                    CI.PriorityID = Obj.PriorityID;
                    CI.SeverityID = Obj.SeverityID;
                    CI.TicketType = Obj.TicketType;
                    CI.TicketNumber = Obj.TicketNumber;
                    CI.ReporterID = Obj.ReporterID;
                    CI.Subject = Obj.Subject;
                    CI.Comments = Obj.Comments;
                    CI.ContingencyPlan = Obj.ContingencyPlan;
                    CI.DueDate = Obj.DueDate;
                    CI.IssueDescription = Obj.IssueDescription;
                    CI.ResolutionSummary = Obj.ResolutionSummary;
                    CI.ResolutionActions = Obj.ResolutionActions;


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __TicketInformationID);
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
