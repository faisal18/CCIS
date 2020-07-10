using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpAddressLocations
    {

        public static int InsertRecord(AddressLocations AddressLocations)
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);
                    //  AddressLocations = Helper.DSCLocationHashCode.GenerateDSCLocationData(AddressLocations);

                    checkerRepository.Add(AddressLocations);
                    checkerRepository.Save();
                    int LastInsertedID = AddressLocations.AddressLocationID;
                    string LastInserted = AddressLocations.Description;
                    checkerRepository.Dispose();
                    DSCLocationIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateDSCLocationAsyncOp(AddressLocations AddressLocations)
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);

                    checkerRepository.Add(AddressLocations);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = AddressLocations.AddressLocationID;

                    checkerRepository.Dispose();
                    DSCLocationIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return -1;
            }
        }

        public static int InsertRecordAsync(AddressLocations AddressLocations)
        {
            try
            {

                Task<int> _result = CreateDSCLocationAsyncOp(AddressLocations);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }



        public static List<AddressLocations> GetAll()
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);
                    List<AddressLocations> lstLocation = checkerRepository.GetAll().OrderBy(a => a.Description).ToList();
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

        public static AddressLocations GetLocationbyID(int AddressLocationsID)
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);
                    AddressLocations DSCLocationObj = checkerRepository.Get(AddressLocationsID);
                    checkerRepository.Dispose();
                    DSCLocationIDContext.Dispose();
                    return DSCLocationObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }

        public static AddressLocations GetRecordbyName(string _Name)
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);
                    AddressLocations DSCLocationObj = checkerRepository.Find(x => x.Description.Contains(_Name));

                    checkerRepository.Dispose();
                    DSCLocationIDContext.Dispose();
                    return DSCLocationObj;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }

        public static bool DeletebyID(int AddressLocationsID)
        {
            try
            {
                using (var DSCLocationIDContext = new DataModel.DALDbContext())
                {
                    CallerInformation callerInformation = DSCLocationIDContext.CallerInformation.SingleOrDefault(x => x.CallerInformationID == AddressLocationsID);
                    DSCLocationIDContext.CallerInformation.Remove(callerInformation);
                    DSCLocationIDContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return false;
            }
        }

        public static int UpdateLocation(AddressLocations Obj, int _AddressLocationsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {


                  
                        AddressLocations CI = GetLocationbyID(_AddressLocationsID);
                        CI.UpdateDate = DateTime.Now;
                        CI.UpdatedBy = Obj.UpdatedBy;

                        CI.Description = Obj.Description;
                        CI.DescriptionAR = Obj.DescriptionAR;
                      

                        DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                        return DBContext.SaveChanges();
                        //checkerRepository.Update(Obj, __UserRolesID);
                        ////checkerRepository.Dispose();
                        //DBContext.Dispose();
                







                    //DataModel.AddressLocationsRepository checkerRepository = new DataModel.AddressLocationsRepository(DSCLocationIDContext);
                    //checkerRepository.Update(DSCLocation, _AddressLocationsID);
                    //checkerRepository.Dispose();
                    //DSCLocationIDContext.Dispose();
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
