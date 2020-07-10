using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpUserApplication
    {

        public static int InsertRecord(UserApplication _UserApplication)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.UserApplication.Add(_UserApplication);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _UserApplication.UserApplicationID;
                    string LastInserted = _UserApplication.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(UserApplication _UserApplication)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);

                    //checkerRepository.Add(_UserApplication);
                    DBContext.UserApplication.Add(_UserApplication);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _UserApplication.UserApplicationID;

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

     
        public static int InsertRecordAsync(UserApplication _UserApplication)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_UserApplication);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<UserApplication> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.OrderBy(a => a.ApplicationID).ThenBy(b=>b.PersonID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.UserApplicationID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<UserApplication> GetUserApplicationbyDescription(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where (x => x.Description == _Comments)
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
        public static List<UserApplication> GetUserApplicationbyPersonID(int _PersonID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where (x => x.PersonID == _PersonID)
                        .OrderBy(x=>x.PersonID).ToList();

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

        public static List<UserApplication> GetUserApplicationbyRole(int _RoleID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.RoleID == _RoleID)
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

        public static List<UserApplication> GetUserApplicationbyApplication(int _ApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.ApplicationID == _ApplicationID)
                        .OrderBy(x => x.ApplicationID).ToList();

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

        public static List<UserApplication> GetUserApplicationbyGroup(int _GroupID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.GroupID == _GroupID)
                        .OrderBy(x => x.ApplicationID).ToList();

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

        public static List<UserApplication> GetUserApplicationbyAll(int _ApplicationID,  int _ROleID, int _PersonID, int _GroupID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.ApplicationID == _ApplicationID && x.RoleID == _ROleID && x.PersonID == _PersonID && x.GroupID ==_GroupID)
                        .OrderBy(x => x.ApplicationID).ToList();

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
        public static List<UserApplication> GetUserApplicationbyRolenApplication(int _ApplicationID,  int _ROleID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.ApplicationID == _ApplicationID && x.RoleID == _ROleID )
                        .OrderBy(x => x.ApplicationID).ToList();

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
        public static List<UserApplication> GetUserApplicationbyPersonnApplication(int _ApplicationID, int _PersonID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = DBContext.UserApplication.Where(x => x.ApplicationID == _ApplicationID  && x.PersonID == _PersonID )
                        .OrderBy(x => x.ApplicationID).ToList();

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


        public static List<UserApplication> GetAllAsync()
        {
            try
            {


                Task<List<UserApplication>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<UserApplication>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);



                    List<UserApplication> lstLocation = await DBContext.UserApplication.ToListAsync();
                   
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


        public static UserApplication GetRecordbyID(int _UserApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);
                    UserApplication RecordObj = DBContext.UserApplication.SingleOrDefault(x => x.UserApplicationID == _UserApplicationID);
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


        public static bool DeletebyID(int _UserApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);
                    UserApplication RecordObj = DBContext.UserApplication.SingleOrDefault(x => x.UserApplicationID == _UserApplicationID);
                    //checkerRepository.Dispose();
                    DBContext.UserApplication.Remove(RecordObj);
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


        public static int UpdateRecord(UserApplication Obj, int __UserApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserApplicationRepository checkerRepository = new DataModel.UserApplicationRepository(DBContext);

                    UserApplication CI = GetRecordbyID(__UserApplicationID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.RoleID = Obj.RoleID;
                    CI.ApplicationID = Obj.ApplicationID;
                    CI.Description = Obj.Description;
                    CI.PersonID = Obj.PersonID;
                    CI.GroupID = Obj.GroupID;
                

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __UserApplicationID);
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
