using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpProviderType
    {

        public static int InsertRecord(ProviderType _ProviderType)
        {
            try
            {
                using (var ProviderTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(ProviderTypeIDContext);
                    //  _ProviderType = Helper.ProviderTypeHashCode.GenerateProviderTypeData(_ProviderType);

                    checkerRepository.Add(_ProviderType);
                    checkerRepository.Save();
                    int LastInsertedID = _ProviderType.ProviderTypeID;
                    string LastInserted = _ProviderType.Description;
                    checkerRepository.Dispose();
                    ProviderTypeIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateProviderTypeAsyncOp(ProviderType _ProviderType)
        {
            try
            {
                using (var ProviderTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(ProviderTypeIDContext);

                    checkerRepository.Add(_ProviderType);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _ProviderType.ProviderTypeID;

                    checkerRepository.Dispose();
                    ProviderTypeIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static int InsertRecordAsync(ProviderType _ProviderType)
        {
            try
            {

                Task<int> _result = CreateProviderTypeAsyncOp(_ProviderType);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }


        public static List<ProviderType> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(DSCLocationIDContext);
                    List<ProviderType> lstLocation = checkerRepository.GetAll().OrderBy(a => a.Description).ToList();
                    checkerRepository.Dispose();
                    DSCLocationIDContext.Dispose();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static ProviderType GetProviderTypebyID(int _ProviderTypeID)
        {
            try
            {
                using (var ProviderTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(ProviderTypeIDContext);
                    ProviderType ProviderTypeObj = checkerRepository.Get(_ProviderTypeID);
                    checkerRepository.Dispose();
                    ProviderTypeIDContext.Dispose();
                    return ProviderTypeObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }

        public static ProviderType GetRecordbyName(string _Name)
        {
            try
            {
                using (var ProviderTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(ProviderTypeIDContext);
                    ProviderType ProviderTypeObj = checkerRepository.Find(x => x.Description.Contains(_Name));

                    checkerRepository.Dispose();
                    ProviderTypeIDContext.Dispose();
                    return ProviderTypeObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }

        public static bool DeletebyID(int _ProviderTypeID)
        {
            try
            {
                using (var ProviderTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.ProviderTypeRepository checkerRepository = new DataModel.ProviderTypeRepository(ProviderTypeIDContext);
                    ProviderType ProviderTypeObj = checkerRepository.Get(_ProviderTypeID);

                    checkerRepository.Delete(ProviderTypeObj);
                    checkerRepository.Dispose();
                    ProviderTypeIDContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return false;
            }
        }

        public static int UpdateProviderType(ProviderType Obj, int __ProviderTypeID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    ProviderType CI = GetProviderTypebyID(__ProviderTypeID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;

                    CI.Description = Obj.Description;
                   

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();







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
