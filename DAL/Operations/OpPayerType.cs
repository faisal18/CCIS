using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpPayerType
    {

        public static int InsertRecord(PayerType _PayerType)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(PayerTypeIDContext);
                    //  _PayerType = Helper.PayerTypeHashCode.GeneratePayerTypeData(_PayerType);

                    checkerRepository.Add(_PayerType);
                    checkerRepository.Save();
                    int LastInsertedID = _PayerType.PayerTypeID;
                    string LastInserted = _PayerType.Description;
                    checkerRepository.Dispose();
                    PayerTypeIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreatePayerTypeAsyncOp(PayerType _PayerType)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(PayerTypeIDContext);

                    checkerRepository.Add(_PayerType);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _PayerType.PayerTypeID;

                    checkerRepository.Dispose();
                    PayerTypeIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static int InsertRecordAsync(PayerType _PayerType)
        {
            try
            {

                Task<int> _result = CreatePayerTypeAsyncOp(_PayerType);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }


        public static List<PayerType> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(DSCLocationIDContext);
                    List<PayerType> lstLocation = checkerRepository.GetAll().OrderBy(a => a.Description).ToList();
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


        public static PayerType GetPayerTypebyID(int _PayerTypeID)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(PayerTypeIDContext);
                    PayerType PayerTypeObj = checkerRepository.Get(_PayerTypeID);
                    checkerRepository.Dispose();
                    PayerTypeIDContext.Dispose();
                    return PayerTypeObj;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static PayerType GetRecordbyName(string _Name)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(PayerTypeIDContext);
                    PayerType PayerTypeObj = checkerRepository.Find(x => x.Description.Contains(_Name));

                    checkerRepository.Dispose();
                    PayerTypeIDContext.Dispose();
                    return PayerTypeObj;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static bool DeletebyID(int _PayerTypeID)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayerTypeRepository checkerRepository = new DataModel.PayerTypeRepository(PayerTypeIDContext);
                    PayerType PayerTypeObj = checkerRepository.Get(_PayerTypeID);

                    checkerRepository.Delete(PayerTypeObj);
                    checkerRepository.Dispose();
                    PayerTypeIDContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return false;
            }
        }

        public static int UpdatePayerType(PayerType PayerType, int __PayerTypeID)
        {
            try
            {
                using (var PayerTypeIDContext = new DataModel.DALDbContext())
                {
                    Entities.PayerType pt = new PayerType();
                    pt = GetPayerTypebyID(__PayerTypeID);
                    pt.UpdateDate = DateTime.Now;
                    pt.UpdatedBy = PayerType.UpdatedBy;
                    pt.Description = PayerType.Description;
                    PayerTypeIDContext.Entry(pt).State = System.Data.Entity.EntityState.Modified;

                    int result = PayerTypeIDContext.SaveChanges();
                    return result;
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
