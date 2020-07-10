using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpSLADeclarations
    {

        public static int InsertRecord(SLADeclarations _SLADeclarations)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.SLADeclarations.Add(_SLADeclarations);
                    DBContext.SaveChanges();

                    int LastInsertedID = _SLADeclarations.SLADeclarationsID;
                    string LastInserted = _SLADeclarations.Description;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(SLADeclarations _SLADeclarations)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);

                    //checkerRepository.Add(_SLADeclarations);
                    DBContext.SLADeclarations.Add(_SLADeclarations);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _SLADeclarations.SLADeclarationsID;

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


        public static int InsertRecordAsync(SLADeclarations _SLADeclarations)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_SLADeclarations);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<SLADeclarations> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);



                    List<SLADeclarations> lstLocation = DBContext.SLADeclarations.OrderBy(a => a.ApplicationID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.SLADeclarationsID + "-" + x.Description).ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<SLADeclarations> GetSLADeclarationsbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);



                    List<SLADeclarations> lstLocation = DBContext.SLADeclarations.Where(x => x.Description == _Comments)
                        .OrderBy(x => x.Description).ToList();

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
        public static List<SLADeclarations> GetActiveSLAs()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);



                    List<SLADeclarations> lstLocation = DBContext.SLADeclarations.Where(x => x.isActive == true)
                        .OrderBy(x => x.Description).ToList();

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

        public static List<SLADeclarations> GetAllAsync()
        {
            try
            {


                Task<List<SLADeclarations>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<SLADeclarations>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);



                    List<SLADeclarations> lstLocation = await DBContext.SLADeclarations.ToListAsync();

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


        public static SLADeclarations GetRecordbyID(int _SLADeclarationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);
                    SLADeclarations RecordObj = DBContext.SLADeclarations.SingleOrDefault(x => x.SLADeclarationsID == _SLADeclarationsID);
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


        public static bool DeletebyID(int _SLADeclarationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);
                    SLADeclarations RecordObj = DBContext.SLADeclarations.SingleOrDefault(x => x.SLADeclarationsID == _SLADeclarationsID);
                    //checkerRepository.Dispose();
                    DBContext.SLADeclarations.Remove(RecordObj);
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


        public static int UpdateRecord(SLADeclarations Obj, int __SLADeclarationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SLADeclarationsRepository checkerRepository = new DataModel.SLADeclarationsRepository(DBContext);

                    SLADeclarations CI = GetRecordbyID(__SLADeclarationsID);
                    CI.ApplicationID = Obj.ApplicationID;
                    CI.SLASequenceID = Obj.SLASequenceID;
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Description = Obj.Description;
                    CI.ActionRequiredID = Obj.ActionRequiredID;
                    CI.PriorityID = Obj.PriorityID;
                    CI.SeverityID = Obj.SeverityID;
                    CI.StatusID = Obj.StatusID;
                    CI.SubStatusID = Obj.SubStatusID;
                    CI.TimeinMinutes = Obj.TimeinMinutes;






                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __SLADeclarationsID);
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
