using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpRoles
    {

        public static int InsertRecord(Roles _Roles)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.Roles.Add(_Roles);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _Roles.RolesID;
                    string LastInserted = _Roles.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Roles _Roles)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);

                    //checkerRepository.Add(_Roles);
                    DBContext.Roles.Add(_Roles);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _Roles.RolesID;

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

     
        public static int InsertRecordAsync(Roles _Roles)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Roles);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Roles> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);



                    List<Roles> lstLocation = DBContext.Roles.OrderBy(a=>a.Description).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.RolesID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<Roles> GetRolesbyName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);



                    List<Roles> lstLocation = DBContext.Roles.Where (x => x.Description == _Comments)
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

        public static List<Roles> GetAllAsync()
        {
            try
            {


                Task<List<Roles>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Roles>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);



                    List<Roles> lstLocation = await DBContext.Roles.ToListAsync();
                   
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


        public static Roles GetRecordbyID(int _RolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);
                    Roles RecordObj = DBContext.Roles.SingleOrDefault(x => x.RolesID == _RolesID);
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


        public static bool DeletebyID(int _RolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);
                    Roles RecordObj = DBContext.Roles.SingleOrDefault(x => x.RolesID == _RolesID);
                    //checkerRepository.Dispose();
                    DBContext.Roles.Remove(RecordObj);
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


        public static int UpdateRecord(Roles Obj, int __RolesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.RolesRepository checkerRepository = new DataModel.RolesRepository(DBContext);

                    Roles CI = GetRecordbyID(__RolesID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Description = Obj.Description;
                  
                  
                    


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                   return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __RolesID);
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
