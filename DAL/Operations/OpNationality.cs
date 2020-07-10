using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpNationality
    {

        public static int InsertRecord(Nationality _Nationality)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(MemberIDContext);
                    //  _Nationality = Helper.MemberHashCode.GenerateMemberData(_Nationality);

                    checkerRepository.Add(_Nationality);
                    checkerRepository.Save();
                    int LastInsertedID = _Nationality.NationalityID;
                    string LastInserted = _Nationality.Description;
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

        public static async Task<int> CreateMemberAsyncOp(Nationality _Nationality)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(MemberIDContext);

                    checkerRepository.Add(_Nationality);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _Nationality.NationalityID;

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

        public static int InsertRecordAsync(Nationality _Nationality)
        {
            try
            {

                Task<int> _result = CreateMemberAsyncOp(_Nationality);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<Nationality> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(DSCLocationIDContext);
                    List<Nationality> lstLocation = checkerRepository.GetAll().OrderBy(a => a.Description).ToList();
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


        public static Nationality GetNationalitybyID(int _NationalityID)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(MemberIDContext);
                    Nationality memberObj = checkerRepository.Get(_NationalityID);
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

        public static Nationality GetRecordbyCountry(string _CountryName)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(MemberIDContext);
                    Nationality memberObj = checkerRepository.Find(x => x.Description.Contains(_CountryName));

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

        public static bool DeletebyID(int _NationalityID)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.NationalityRepository checkerRepository = new DataModel.NationalityRepository(MemberIDContext);
                    Nationality memberObj = checkerRepository.Get(_NationalityID);

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

        public static int UpdateNationality(Nationality Obj, int __NationalityID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    Nationality CI = GetNationalitybyID(__NationalityID);
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
