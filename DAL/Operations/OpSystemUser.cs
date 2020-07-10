using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpSystemUser
    {

        public static int InsertRecord(SystemUser _SystemUser)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.SystemUser.Add(_SystemUser);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _SystemUser.SystemUserID;
                    string LastInserted = _SystemUser.username;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(SystemUser _SystemUser)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);

                    //checkerRepository.Add(_SystemUser);
                    DBContext.SystemUser.Add(_SystemUser);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _SystemUser.SystemUserID;

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

     
        public static int InsertRecordAsync(SystemUser _SystemUser)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_SystemUser);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<SystemUser> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);



                    List<SystemUser> lstLocation = DBContext.SystemUser.OrderBy(a => a.username).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.SystemUserID + "-" +x.username ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<SystemUser> GetSystemUserbyUserName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);



                    List<SystemUser> lstLocation = DBContext.SystemUser.Where (x => x.username == _Comments)
                        .OrderBy(x=>x.username).ToList();

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
         public static List<SystemUser> GetActiveSystemUserbyUserName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);



                    List<SystemUser> lstLocation = DBContext.SystemUser.Where (x => x.username == _Comments && x.isActive == true)
                        .OrderBy(x=>x.username).ToList();

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


        public static List<SystemUser> GetSystemUserbyPersonInformationID(int _PersonID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);



                    List<SystemUser> lstLocation = DBContext.SystemUser.Where(x => x.PersonID == _PersonID )
                        .OrderBy(x => x.username).ToList();

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

        public static List<SystemUser> GetAllAsync()
        {
            try
            {


                Task<List<SystemUser>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<SystemUser>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);



                    List<SystemUser> lstLocation = await DBContext.SystemUser.ToListAsync();
                   
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


        public static SystemUser GetRecordbyID(int _SystemUserID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);
                    SystemUser RecordObj = DBContext.SystemUser.SingleOrDefault(x => x.SystemUserID == _SystemUserID);
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


        public static bool DeletebyID(int _SystemUserID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);
                    SystemUser RecordObj = DBContext.SystemUser.SingleOrDefault(x => x.SystemUserID == _SystemUserID);
                    //checkerRepository.Dispose();
                    DBContext.SystemUser.Remove(RecordObj);
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


        public static int UpdateRecord(SystemUser Obj, int __SystemUserID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.SystemUserRepository checkerRepository = new DataModel.SystemUserRepository(DBContext);

                    SystemUser CI = GetRecordbyID(__SystemUserID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.isAdmin = Obj.isAdmin;
                    CI.password = Obj.password;
                    CI.username = Obj.username;
                    CI.PersonID = Obj.PersonID;
                   

                  
                  
                    


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                   return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __SystemUserID);
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
