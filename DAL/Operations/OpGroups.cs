using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpGroups
    {

        public static int InsertRecord(Groups _Groups)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.Groups.Add(_Groups);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _Groups.GroupsID;
                    string LastInserted = _Groups.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Groups _Groups)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);

                    //checkerRepository.Add(_Groups);
                    DBContext.Groups.Add(_Groups);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _Groups.GroupsID;

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

     
        public static int InsertRecordAsync(Groups _Groups)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Groups);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Groups> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);



                    List<Groups> lstLocation = DBContext.Groups.OrderBy(a => a.Description).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.GroupsID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<Groups> GetGroupsbyName(string _Name)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);



                    List<Groups> lstLocation = DBContext.Groups.Where (x => x.Description == _Name)
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

        public static string GetEmailbyGroupID(int _GroupID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);


                    Groups groupRec = new Groups();
                    groupRec = DBContext.Groups.Where(x => x.GroupsID == _GroupID).FirstOrDefault();

                    string groupEmail = groupRec.GroupEmail;

                        
                        //DBContext.Groups.Where(x => x.GroupsID == _GroupID)
                        // .Select(a => a.GroupEmail).ToString();
                        

                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return groupEmail;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static List<Groups> GetAllAsync()
        {
            try
            {


                Task<List<Groups>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Groups>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);



                    List<Groups> lstLocation = await DBContext.Groups.ToListAsync();
                   
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




        public static Groups GetRecordbyID(int _GroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);
                    Groups RecordObj = DBContext.Groups.SingleOrDefault(x => x.GroupsID == _GroupsID);
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


      


        public static bool DeletebyID(int _GroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);
                    Groups RecordObj = DBContext.Groups.SingleOrDefault(x => x.GroupsID == _GroupsID);
                    //checkerRepository.Dispose();
                    DBContext.Groups.Remove(RecordObj);
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

     

        public static int UpdateRecord(Groups Obj, int __GroupsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.GroupsRepository checkerRepository = new DataModel.GroupsRepository(DBContext);

                    Groups CI = GetRecordbyID(__GroupsID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Description = Obj.Description;
                    CI.GroupEmail = Obj.GroupEmail;
                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __GroupsID);
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
