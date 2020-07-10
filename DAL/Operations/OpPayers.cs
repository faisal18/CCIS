using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpPayers
    {

        public static int InsertRecord(Payers _Payers)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
                    //  _Payers = Helper.MemberHashCode.GenerateMemberData(_Payers);

                    checkerRepository.Add(_Payers);
                    checkerRepository.Save();
                    int LastInsertedID = _Payers.PayerID;
                    string LastInserted = _Payers.PayerCode;
                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateMemberAsyncOp(Payers _Payers)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);

                    checkerRepository.Add(_Payers);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _Payers.PayerID;

                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }


        public static int InsertRecordAsync(Payers _Payers)
        {
            try
            {

                Task<int> _result = CreateMemberAsyncOp(_Payers);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Payers> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(DSCLocationIDContext);
                    List<Payers> lstLocation = checkerRepository.GetAll().OrderBy(a => a.PayerCode).ToList();
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
        
        public static List<string> GetAllString()
        {
            try
            {


                List<string> lstLocation = GetAll().Where(x=>x.PayerCode != null && x.PayerName != null).Select(x => " Payer || " +    x.PayerCode.Trim() + " || " + x.PayerName.Trim())
                    .Distinct().ToList();
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


                List<string> lstLocation = GetAll().Select(x =>x.PayerCode).Distinct().ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<Payers> GetAllAsync()
        {
            try
            {


                Task<List<Payers>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<Payers>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<Payers> lstLocation = await DBContext.payerLookups.ToListAsync();

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



        public static Payers GetPayerbyID(int _PayersID)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
                    Payers memberObj = checkerRepository.Get(_PayersID);
                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return memberObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }


        public static Payers GetRecordbyPayerCode(string _PayerCode)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
                    Payers memberObj = checkerRepository.Find(x => x.PayerCode == _PayerCode);

                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return memberObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }


        //public static bool Authenticate(string _PayerCode, string _username , string _password)
        //{
        //    try
        //    {
        //        using (var MemberIDContext = new DataModel.DALDbContext())
        //        {
        //            DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
        //            Payers memberObj = checkerRepository.Find(x => x.PayerCode  == _PayerCode && x.UserName == _username && x.Password == _password );
        //            if (memberObj != null)
        //            {

                        
        //                return true;

        //            }

        //            checkerRepository.Dispose();
        //            MemberIDContext.Dispose();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //         Logger.LogError(ex);
        //        return null;
        //    }
        //}

        //public static bool AuthenticateAsync(string _PayerCode, string _username, string _password)
        //{
        //    try
        //    {
        //        Task<bool> _result = Authenticate_AsyncCall(_PayerCode, _username, _password);
        //        return _result.Result;
            
        //    }
        //    catch (Exception ex)
        //    {

        //         Logger.LogError(ex);
        //        return null;
        //    }
        //}


        //public static async Task<bool>  Authenticate_AsyncCall(string _PayerCode, string _username, string _password)
        //{
        //    try
        //    {
        //        using (var MemberIDContext = new DAL.DataModel.DALDbContext())
        //        {
        //           DAL.DataModel.PayersRepository checkerRepository = new DAL.DataModel.PayersRepository(MemberIDContext);
        //            Payers memberObj = await checkerRepository.FindAsync(x => x.PayerCode == _PayerCode && x.UserName == _username && x.Password == _password);
        //            if (memberObj != null)
        //            {

                     
        //                return true;

        //            }

        //            checkerRepository.Dispose();
        //            MemberIDContext.Dispose();
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //         Logger.LogError(ex);
        //        return null;
        //    }
        //}

        public static bool DeletebyID(int _PayersID)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
                    Payers memberObj = checkerRepository.Get(_PayersID);

                    checkerRepository.Delete(memberObj);
                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return false;
            }
        }

        public static bool DeletebyPayerCode(string _PayerCode)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.PayersRepository checkerRepository = new DataModel.PayersRepository(MemberIDContext);
                    Payers memberObj = GetRecordbyPayerCode(_PayerCode);

                    checkerRepository.Delete(memberObj);
                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return false;
            }
        }


        public static int UpdateRecord(Payers Obj, int __PayersID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);

                    Payers CI = GetPayerbyID(__PayersID);
                    CI.UpdateDate = DateTime.Now;
                    CI.IsActive = Obj.IsActive;
                    CI.PayerName = Obj.PayerName;
                    CI.LicenseEndDate = Obj.LicenseEndDate;
                    CI.LicenseStartDate = Obj.LicenseStartDate;
                    CI.Email = Obj.Email;
                    CI.LoginKey = Obj.LoginKey;
                    CI.PayerCode = Obj.PayerCode;
                    CI.PayerTypeID = Obj.PayerTypeID;
                  




                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;
                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __CallerInformationID);
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
