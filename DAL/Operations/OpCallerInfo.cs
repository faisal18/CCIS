using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpCallerInfo
    {

        public static int InsertRecord(CallerInformation _CallerInformation)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.CallerInformation.Add(_CallerInformation);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _CallerInformation.CallerInformationID;
                    string LastInserted = _CallerInformation.CallerKeyID;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(CallerInformation _CallerInformation)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);

                    //checkerRepository.Add(_CallerInformation);
                    DBContext.CallerInformation.Add(_CallerInformation);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _CallerInformation.CallerInformationID;

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

        public static string generateCallerID (string license, int CallerInformationID)
        {
            try
            {

                string CallerKeyID = "";

                CallerKeyID = license + " -" + CallerInformationID;


                UpdateCallerID(CallerKeyID, CallerInformationID);

                return CallerKeyID;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static int InsertRecordAsync(CallerInformation _CallerInformation)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_CallerInformation);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<CallerInformation> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<CallerInformation> lstLocation = DBContext.CallerInformation.OrderBy(a => a.CallerLicense).ThenBy(b => b.CallerKeyID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.CallerKeyID + "-" +x.CallerLicense + "-" + x.Name).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<CallerInformation> GetCallerInformationbyLicenseID(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<CallerInformation> lstLocation = DBContext.CallerInformation.Where (x => x.CallerLicense == _licenseID)
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

        public static List<CallerInformation> GetAllAsync()
        {
            try
            {


                Task<List<CallerInformation>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<CallerInformation>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<CallerInformation> lstLocation = await DBContext.CallerInformation.ToListAsync();
                   
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


        public static List<string> GetCallerEmailbyLicenseID(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<string> lstLocation = DBContext.CallerInformation
                        .Where(x => x.CallerLicense == _licenseID)
                        .Select(a=>a.Email).ToList();

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

        public static List<string> GetCallerEmailbyCallerKeyID(string _CallerKeyID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);



                    List<string> lstLocation = DBContext.CallerInformation
                        .Where(x => x.CallerKeyID == _CallerKeyID)
                        .Select(a=>a.Email).ToList();

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

        public static string GetEmailStringbyLicenseID(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                  
                    List<string> lstLocation = DBContext.CallerInformation
                        .Where(x => x.CallerLicense == _licenseID)
                        .Select(a=>a.Email).ToList();

                    string emailaddresses = "";

                    foreach (string email in lstLocation)
                    {
                        emailaddresses += email + ";";
                    }
                    
                    return emailaddresses;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static string GetEmailStringbyCallerKeyID(string _CallerKeyID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                  
                    List<string> lstLocation = DBContext.CallerInformation
                        .Where(x => x.CallerKeyID == _CallerKeyID)
                        .Select(a=>a.Email).ToList();

                    string emailaddresses = "";

                    foreach (string email in lstLocation)
                    {
                        emailaddresses += email + ";";
                    }
                    
                    return emailaddresses;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static CallerInformation GetRecordbyID(int _CallerInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);
                    CallerInformation RecordObj = DBContext.CallerInformation.SingleOrDefault(x => x.CallerInformationID == _CallerInformationID);
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


        public static CallerInformation GetRecordbyCallerKey(string _CallInfoCode)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);
                    CallerInformation RecordObj = DBContext.CallerInformation.SingleOrDefault(x => x.CallerKeyID == _CallInfoCode);
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



        public static bool DeletebyID(int _CallerInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);
                    CallerInformation RecordObj = DBContext.CallerInformation.SingleOrDefault(x => x.CallerInformationID == _CallerInformationID);
                    //checkerRepository.Dispose();
                    DBContext.CallerInformation.Remove(RecordObj);
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

        public static bool DeletebyCallerKey(string _CallerKey)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);
                    CallerInformation RecordObj = GetRecordbyCallerKey(_CallerKey);

                    DBContext.CallerInformation.Remove(RecordObj);
                    DBContext.SaveChanges();
                    //checkerRepository.Delete(RecordObj);
                    ////checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return false;
            }
        }


        public static int UpdateRecord(CallerInformation Obj, int __CallerInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);

                    CallerInformation CI = GetRecordbyID(__CallerInformationID);

                    CI.CallerKeyID = Obj.CallerKeyID;
                    CI.CallerLicense = Obj.CallerLicense;
                    CI.Email = Obj.Email;
                    CI.isContactPerson = Obj.isContactPerson;
                    CI.isOwner = Obj.isOwner;
                    CI.Name = Obj.Name;
                    CI.MachineName = Obj.MachineName;
                    CI.Location = Obj.Location;
                    CI.OperatingSystem = Obj.OperatingSystem;
                    CI.Department = Obj.Department;
                    CI.PhoneNumber = Obj.PhoneNumber;
                    CI.UpdateDate = Obj.UpdateDate;
                    CI.UpdatedBy = Obj.UpdatedBy;

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
        public static int UpdateCallerID(string CallerKeyID, int __CallerInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.CallerInformationRepository checkerRepository = new DataModel.CallerInformationRepository(DBContext);

                    CallerInformation CI = GetRecordbyID(__CallerInformationID);
                    CI.UpdateDate = DateTime.Now;
                    CI.CallerKeyID = CallerKeyID;

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
