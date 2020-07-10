using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpUserRoles
    {

        public static int InsertRecord(UserRoles _UserRoles)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.UserRoles.Add(_UserRoles);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _UserRoles.UserRoleID;
                    string LastInserted = _UserRoles.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(UserRoles _UserRoles)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);

                    //checkerRepository.Add(_UserRoles);
                    DBContext.UserRoles.Add(_UserRoles);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _UserRoles.UserRoleID;

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

     
        public static int InsertRecordAsync(UserRoles _UserRoles)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_UserRoles);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<UserRoles> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);



                    List<UserRoles> lstLocation = DBContext.UserRoles.OrderBy(a => a.PersonID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.UserRoleID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<UserRoles> GetUserRolesbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);



                    List<UserRoles> lstLocation = DBContext.UserRoles.Where (x => x.Description == _Comments)
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
        public static UserRoles GetUserRoleByPersonName(string _Name)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    int personId = int.Parse(DAL.Helper.ListToDataset.ToDataSet<Entities.PersonInformation>(DAL.Operations.OpPersonInformation.GetPersonInformationbyName(_Name)).Tables[0].Rows[0]["Role"].ToString());
                    var entity = DBContext.UserRoles.FirstOrDefault(x => x.PersonID.Equals(personId));

                    return entity;
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static UserRoles GetUserRoleByPersonId(int _Id)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    UserRoles entity = DBContext.UserRoles.SingleOrDefault(x => x.PersonID.Equals(_Id));

                    return entity;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }


        public static List<UserRoles> GetAllAsync()
        {
            try
            {


                Task<List<UserRoles>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<UserRoles>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);



                    List<UserRoles> lstLocation = await DBContext.UserRoles.ToListAsync();
                   
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


        public static UserRoles GetRecordbyID(int _UserRolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);
                    UserRoles RecordObj = DBContext.UserRoles.SingleOrDefault(x => x.UserRoleID == _UserRolesID);
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


        public static bool DeletebyID(int _UserRolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);
                    UserRoles RecordObj = DBContext.UserRoles.SingleOrDefault(x => x.UserRoleID == _UserRolesID);
                    //checkerRepository.Dispose();
                    DBContext.UserRoles.Remove(RecordObj);
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


        public static int UpdateRecord(UserRoles Obj, int __UserRolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.UserRolesRepository checkerRepository = new DataModel.UserRolesRepository(DBContext);

                    UserRoles CI = GetRecordbyID(__UserRolesID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.RoleID = Obj.RoleID;
                    CI.Description = Obj.Description;
                    CI.PersonID = Obj.PersonID;
                

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __UserRolesID);
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
