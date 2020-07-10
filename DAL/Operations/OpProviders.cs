using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpProviders
    {

        public static int InsertRecord(Providers _Providers)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.Providers.Add(_Providers);
                    DBContext.SaveChanges();

                    int LastInsertedID = _Providers.ProviderID;
                    string LastInserted = _Providers.ProviderLicense;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Providers _Providers)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);

                    //checkerRepository.Add(_Providers);
                    DBContext.Providers.Add(_Providers);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _Providers.ProviderID;

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


        public static int InsertRecordAsync(Providers _Providers)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Providers);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Providers> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);



                    List<Providers> lstLocation = DBContext.Providers.OrderBy(a => a.ProviderLicense).ThenBy(b=>b.ProviderName).ToList();

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


                List<string> lstLocation = GetAll().Where(x=>x.ProviderLicense != null && x.ProviderName != null).OrderBy(x=>x.ProviderLicense).Select(x => "Provider || " + x.ProviderLicense.Trim() + " || " + x.ProviderName.Trim()).Distinct().ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<string> GetAllStringNoSplit()
        {
            try
            {


                List<string> lstLocation = GetAll().Select(x =>x.ProviderLicense).Distinct().ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<Providers> GetProvidersbyLicenseID(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);



                    List<Providers> lstLocation = DBContext.Providers.Where(x => x.ProviderLicense == _licenseID)
                        .OrderBy(x => x.ProviderLicense).ToList();

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

        public static List<Providers> GetAllAsync()
        {
            try
            {


                Task<List<Providers>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Providers>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);



                    List<Providers> lstLocation = await DBContext.Providers.ToListAsync();

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




        public static Providers GetRecordbyID(int _ProvidersID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);
                    Providers RecordObj = DBContext.Providers.SingleOrDefault(x => x.ProviderID == _ProvidersID);
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



        public static bool DeletebyID(int _ProvidersID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);
                    Providers RecordObj = DBContext.Providers.SingleOrDefault(x => x.ProviderID == _ProvidersID);
                    //checkerRepository.Dispose();
                    DBContext.Providers.Remove(RecordObj);
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



        public static int UpdateRecord(Providers Obj, int __ProvidersID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.ProvidersRepository checkerRepository = new DataModel.ProvidersRepository(DBContext);

                    Providers CI = GetRecordbyID(__ProvidersID);
                    CI.UpdateDate = DateTime.Now;
                    CI.Emirate = Obj.Emirate;
                    CI.IsActive = Obj.IsActive;
                    CI.ProviderLicense = Obj.ProviderLicense;
                    CI.ProviderName = Obj.ProviderName;
                    CI.ProviderTypeID = Obj.ProviderTypeID;
                    CI.ProviderTypeID = Obj.ProviderTypeID;
                    CI.ProviderUID = Obj.ProviderUID;
                    CI.Source = Obj.Source;
                    CI.UpdatedBy = Obj.UpdatedBy;

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;


                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __ProvidersID);
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
