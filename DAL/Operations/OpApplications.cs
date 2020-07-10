using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpApplications
    {

        public static int InsertRecord(Applications _Applications)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.Applications.Add(_Applications);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _Applications.ApplicationsID;
                    string LastInserted = _Applications.Name;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Applications _Applications)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);

                    //checkerRepository.Add(_Applications);
                    DBContext.Applications.Add(_Applications);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _Applications.ApplicationsID;

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

      
        public static int InsertRecordAsync(Applications _Applications)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Applications);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Applications> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);



                    List<Applications> lstLocation = DBContext.Applications.OrderBy(a => a.Name).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.ApplicationsID  + "-" + x.Name).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<Applications> GetApplicationsbyName(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);



                    List<Applications> lstLocation = DBContext.Applications.Where (x => x.Name == _licenseID)
                        .OrderBy(x=>x.Name).ToList();

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

        public static List<Applications> GetAllAsync()
        {
            try
            {


                Task<List<Applications>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Applications>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);



                    List<Applications> lstLocation = await DBContext.Applications.ToListAsync();
                   
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




        public static Applications GetRecordbyID(int _ApplicationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);
                    Applications RecordObj = DBContext.Applications.SingleOrDefault(x => x.ApplicationsID == _ApplicationsID);
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


     
        public static bool DeletebyID(int _ApplicationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);
                    Applications RecordObj = DBContext.Applications.SingleOrDefault(x => x.ApplicationsID == _ApplicationsID);
                    //checkerRepository.Dispose();
                    DBContext.Applications.Remove(RecordObj);
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

     

        public static int UpdateRecord(Applications Obj, int __ApplicationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ApplicationsRepository checkerRepository = new DataModel.ApplicationsRepository(DBContext);

                    Applications CI = GetRecordbyID(__ApplicationsID);
                    CI.UpdateDate = DateTime.Now;
                    CI.Contact_Number = Obj.Contact_Number;
                    CI.Contact_Person = Obj.Contact_Person;
                    CI.Name = Obj.Name ;
                    CI.Owner = Obj.Owner;
                    CI.Owner_Email = Obj.Owner_Email;
                    CI.URL = Obj.URL;
                    CI.UpdatedBy = Obj.UpdatedBy;

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;


                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __ApplicationsID);
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
