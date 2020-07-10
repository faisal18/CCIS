using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpUserGroups
    {

        public static int InsertRecord(UserGroups _UserGroups)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.UserGroups.Add(_UserGroups);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _UserGroups.UserGroupsID;
                    string LastInserted = _UserGroups.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(UserGroups _UserGroups)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);

                    //checkerRepository.Add(_UserGroups);
                    DBContext.UserGroups.Add(_UserGroups);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _UserGroups.UserGroupsID;

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

     
        public static int InsertRecordAsync(UserGroups _UserGroups)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_UserGroups);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<UserGroups> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);



                    List<UserGroups> lstLocation = DBContext.UserGroups.OrderBy(a => a.PersonID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.UserGroupsID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<UserGroups> GetUserGroupsbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);



                    List<UserGroups> lstLocation = DBContext.UserGroups.Where (x => x.Description == _Comments)
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

        /// <summary>
        /// Get User groups User ID
        /// </summary>
        /// <param name="_PersonInformationUserID"></param>
        /// <returns></returns>
        public static List<UserGroups> GetUserGroupbyUserID(int _PersonInformationUserID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);



                    List<UserGroups> lstLocation = DBContext.UserGroups.Where (x => x.PersonID == _PersonInformationUserID)
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

        public static List<UserGroups> GetAllAsync()
        {
            try
            {


                Task<List<UserGroups>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<UserGroups>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);



                    List<UserGroups> lstLocation = await DBContext.UserGroups.ToListAsync();
                   
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


        public static UserGroups GetRecordbyID(int _UserGroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);
                    UserGroups RecordObj = DBContext.UserGroups.SingleOrDefault(x => x.UserGroupsID == _UserGroupsID);
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
      



        public static bool DeletebyID(int _UserGroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);
                    UserGroups RecordObj = DBContext.UserGroups.SingleOrDefault(x => x.UserGroupsID == _UserGroupsID);
                    //checkerRepository.Dispose();
                    DBContext.UserGroups.Remove(RecordObj);
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


        public static int UpdateRecord(UserGroups Obj, int __UserGroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserGroupsRepository checkerRepository = new DataModel.UserGroupsRepository(DBContext);

                    UserGroups CI = GetRecordbyID(__UserGroupsID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.GroupID = Obj.GroupID;
                    CI.Description = Obj.Description;
                    CI.PersonID = Obj.PersonID;
                

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                   return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __UserGroupsID);
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
