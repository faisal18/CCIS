using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpItemTypes
    {

        public static int InsertRecord(ItemTypes _ItemTypes)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.ItemTypes.Add(_ItemTypes);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _ItemTypes.ItemTypesID;
                    string LastInserted = _ItemTypes.Description;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(ItemTypes _ItemTypes)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);

                    //checkerRepository.Add(_ItemTypes);
                    DBContext.ItemTypes.Add(_ItemTypes);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _ItemTypes.ItemTypesID;

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

     
        public static int InsertRecordAsync(ItemTypes _ItemTypes)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_ItemTypes);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<ItemTypes> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);



                    List<ItemTypes> lstLocation = DBContext.ItemTypes.OrderBy(b=>b.Categories).ThenBy(a => a.Description).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.ItemTypesID + "-" +x.Description ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<ItemTypes> GetItemTypesbyName(string _Name)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);



                    List<ItemTypes> lstLocation = DBContext.ItemTypes.Where (x => x.Description == _Name)
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

        public static List<ItemTypes> GetItemTypesbyCategoryName(string _CategoryName)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);



                    List<ItemTypes> lstLocation = DBContext.ItemTypes.Where (x => x.Categories == _CategoryName)
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
        public static int GetItemIDbyNameAndCategoryName(string _CategoryName, string _ItemTypeName)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);



                    int lstLocation = DBContext.ItemTypes.Where(x => x.Categories == _CategoryName && x.Description == _ItemTypeName)
                         .Select(x => x.ItemTypesID).FirstOrDefault();

                       // .OrderBy(x=>x.Description).ToList();

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



        public static List<ItemTypes> GetAllAsync()
        {
            try
            {


                Task<List<ItemTypes>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<ItemTypes>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);



                    List<ItemTypes> lstLocation = await DBContext.ItemTypes.ToListAsync();
                   
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




        public static ItemTypes GetRecordbyID(int _ItemTypesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);
                    ItemTypes RecordObj = DBContext.ItemTypes.SingleOrDefault(x => x.ItemTypesID == _ItemTypesID);
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


      


        public static bool DeletebyID(int _ItemTypesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);
                    ItemTypes RecordObj = DBContext.ItemTypes.SingleOrDefault(x => x.ItemTypesID == _ItemTypesID);
                    //checkerRepository.Dispose();
                    DBContext.ItemTypes.Remove(RecordObj);
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

     

        public static int UpdateRecord(ItemTypes Obj, int __ItemTypesID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ItemTypesRepository checkerRepository = new DataModel.ItemTypesRepository(DBContext);

                    ItemTypes CI = GetRecordbyID(__ItemTypesID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Description = Obj.Description;
                    CI.Categories = Obj.Categories;
                    CI.Role = CI.Role;
                    CI.Scenario = CI.Scenario;
                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __ItemTypesID);
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
