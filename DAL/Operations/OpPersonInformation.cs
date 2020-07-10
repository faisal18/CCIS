using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpPersonInformation
    {
        public static int InsertPersonInformation(PersonInformation _MRPersonInformation)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(MRPersonInformationIDContext);

                    checkerRepository.Add(_MRPersonInformation);
                    checkerRepository.Save();
                    int LastInsertedID = _MRPersonInformation.PersonInformationID;

                    checkerRepository.Dispose();
                    MRPersonInformationIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreatePersonInformationAsyncOp(PersonInformation _MRPersonInformation)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(MRPersonInformationIDContext);
                    //_MRPersonInformation = Helper.MRPersonInformationHashCode.GenerateMRPersonInformationData(_MRPersonInformation);

                    checkerRepository.Add(_MRPersonInformation);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _MRPersonInformation.PersonInformationID;

                    checkerRepository.Dispose();
                    MRPersonInformationIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return -1;
            }
        }

        public static int InsertPersonInformationAsync(PersonInformation _MRPersonInformation)
        {
            try
            {

                Task<int> _result = CreatePersonInformationAsyncOp(_MRPersonInformation);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return -1;
            }
        }

        public static PersonInformation GetPersonInformationbyID(int _MRPersonInformationID)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(MRPersonInformationIDContext);
                    PersonInformation MRPersonInformationObj = checkerRepository.Get(_MRPersonInformationID);
                    checkerRepository.Dispose();
                    MRPersonInformationIDContext.Dispose();
                    return MRPersonInformationObj;
                }
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return null;
            }
        }
        public static string GetEmailbyUserId(int _MRPersonInformationID)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(MRPersonInformationIDContext);
                    string MRPersonInformationObj = checkerRepository.Get(_MRPersonInformationID).Email;
                    checkerRepository.Dispose();
                    MRPersonInformationIDContext.Dispose();
                    return MRPersonInformationObj;
                }
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return null;
            }
        }

        public static List<PersonInformation> GetPersonInformationbyName(string _MRPersonInformationName)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    var entity = MRPersonInformationIDContext.PersonInformation.Where(e => e.FullName.Equals(_MRPersonInformationName)).ToList();
                    return entity;
                }
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return null;
            }
        }
        public static List<PersonInformation> GetPersonInformationbyPersonID(int _MRPersonInformationID)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    var entity = MRPersonInformationIDContext.PersonInformation.Where(e => e.PersonInformationID.Equals(_MRPersonInformationID)).ToList();
                    return entity;
                }
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return null;
            }
        }

        public static bool DeletebyID(int _PersonInformationID)
        {
            try
            {
                using (var MRPersonInformationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(MRPersonInformationIDContext);
                    PersonInformation MRPersonInformationObj = checkerRepository.Get(_PersonInformationID);

                    checkerRepository.Delete(MRPersonInformationObj);
                    checkerRepository.Dispose();
                    MRPersonInformationIDContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);
                return false;
            }
        }

        public static int UpdatePersonInformation(PersonInformation Obj, int __PersonInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    PersonInformation CI = GetPersonInformationbyID(__PersonInformationID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;

                    CI.ContactNumber = Obj.ContactNumber;
                    CI.Email = Obj.Email;
                    CI.FullName = Obj.FullName;
                    CI.Gender = Obj.Gender;
                    CI.NationalityID = Obj.NationalityID;
                    CI.ResidentialLocation = Obj.ResidentialLocation;
                    CI.WorkLocation = Obj.WorkLocation;


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();







                }


            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return -1;
            }
        }

        public static List<PersonInformation> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MRPersonalInformationRepository checkerRepository = new DataModel.MRPersonalInformationRepository(DSCLocationIDContext);
                    List<PersonInformation> lstLocation = checkerRepository.GetAll()
                        .OrderBy(a => a.FullName).ToList();
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

    }
}
