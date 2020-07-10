using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpStatuses
    {

        public static int InsertRecord(Statuses _Statuses)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.Statuses.Add(_Statuses);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _Statuses.StatusesID;
                    string LastInserted = _Statuses.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Statuses _Statuses)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);

                    //checkerRepository.Add(_Statuses);
                    DBContext.Statuses.Add(_Statuses);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _Statuses.StatusesID;

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

     
        public static int InsertRecordAsync(Statuses _Statuses)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Statuses);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Statuses> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    List<Statuses> lstLocation = DBContext.Statuses.OrderBy(a => a.Types).ThenBy(b=>b.Description).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.StatusesID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<int> GetOpenStatusID(List<string> _Comments, string StatusType)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    List<int> lstLocation = DBContext.Statuses
                       
                        .Where(x =>   x.Types == StatusType && _Comments.Contains(x.Description) )
                        .Select(x => x.StatusesID)
                        .ToList()  ;

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

        public static int GetStatusID(string _StatusName, string _StatusType)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    int lstLocation = DBContext.Statuses

                        .Where(x => x.Types.ToUpper() == _StatusType.ToUpper() && x.Description.ToUpper() == _StatusName.ToUpper())
                        .Select(x => x.StatusesID).FirstOrDefault();
                       // .ToList()  ;

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }


        public static List<Statuses> GetStatusesbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    List<Statuses> lstLocation = DBContext.Statuses.Where (x => x.Description == _Comments)
                        .OrderBy(x=>x.Description).ToList();

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

        public static List<Statuses> GetStatusesbyTypeName(string _TypeName)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    List<Statuses> lstLocation = DBContext.Statuses.Where(x => x.Types == _TypeName)
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
        public static Statuses GetStatusbyNameandType(string _Comments, string _Type)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                   Statuses lstLocation = DBContext.Statuses.Where (x => x.Description == _Comments && x.Types == _Type)
                        .FirstOrDefault();

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
  public static int GetStatusIDbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                   int lstLocation = DBContext.Statuses.Where (x => x.Description == _Comments)
                        .SingleOrDefault().StatusesID;

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static Statuses GetStatusById(int id)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    return DBContext.Statuses.Where(x => x.StatusesID == id).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

     



        public static List<Statuses> GetAllAsync()
        {
            try
            {


                Task<List<Statuses>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Statuses>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);



                    List<Statuses> lstLocation = await DBContext.Statuses.ToListAsync();
                   
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


        public static Statuses GetRecordbyID(int _StatusesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);
                    Statuses RecordObj = DBContext.Statuses.SingleOrDefault(x => x.StatusesID == _StatusesID);
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


        public static bool DeletebyID(int _StatusesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);
                    Statuses RecordObj = DBContext.Statuses.SingleOrDefault(x => x.StatusesID == _StatusesID);
                    //checkerRepository.Dispose();
                    DBContext.Statuses.Remove(RecordObj);
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


        public static int UpdateRecord(Statuses Obj, int __StatusesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.StatusesRepository checkerRepository = new DataModel.StatusesRepository(DBContext);

                    Statuses CI = GetRecordbyID(__StatusesID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Description = Obj.Description;
                    CI.Types = Obj.Types;

                  
                  
                    


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __StatusesID);
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
