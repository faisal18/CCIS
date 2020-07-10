using System;
using System.Collections.Generic;
using System.Data;



namespace CCIS.DataCaching
{
    public class DataCaching
    {
        //public DataCaching ()
        //{

        //    //GetPayersCashed();
        //    //GetCallerInformation();
        //}

        public int CacheReloadTimeForTickets
        {
            get
            {
                try
                {


                    string TicketCacheTime = System.Configuration.ConfigurationManager.AppSettings["CacheReloadTimeForTickets"];
                    int TicketCacheTimeInt = int.Parse(TicketCacheTime);
                    return TicketCacheTimeInt;
                }
                catch (Exception ex)
                {

                    DAL.Operations.Logger.LogError(ex);
                    return 1000;
                }
            }
            set
            {
                try
                {


                    string TicketCacheTime = System.Configuration.ConfigurationManager.AppSettings["CacheReloadTimeForTickets"];
                    int TicketCacheTimeInt = int.Parse(TicketCacheTime);
                    //  return TicketCacheTimeInt;
                }
                catch (Exception ex)
                {

                    DAL.Operations.Logger.LogError(ex);
                    //  return 1000;
                }
            }
        }
        public int CacheReloadTime
        {
            get
            {
                try
                {


                    string TicketCacheTime = System.Configuration.ConfigurationManager.AppSettings["CacheReloadTime"];
                    int TicketCacheTimeInt = int.Parse(TicketCacheTime);
                    return TicketCacheTimeInt;
                }
                catch (Exception ex)
                {

                    DAL.Operations.Logger.LogError(ex);
                    return 1000;
                }
            }
            set
            {
                try
                {


                    string TicketCacheTime = System.Configuration.ConfigurationManager.AppSettings["CacheReloadTime"];
                    int TicketCacheTimeInt = int.Parse(TicketCacheTime);
                    //  return TicketCacheTimeInt;
                }
                catch (Exception ex)
                {

                    DAL.Operations.Logger.LogError(ex);
                    //  return 1000;
                }
            }
        }

        public DataSet TicketsCashed
        {

            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_TicketsCashed_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_TicketsCashed_", GetTicketsCashed(), null, DateTime.Now.AddMinutes(CacheReloadTimeForTickets), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_TicketsCashed_"];
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_TicketsCashed_", GetTicketsCashed(), null, DateTime.Now.AddMinutes(CacheReloadTimeForTickets), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    
                }
            }
        }
        public DataSet GetAllLicenses
        {
            
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_GetAllLicenses_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_GetAllLicenses_", GetAllLicensesCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_GetAllLicenses_"];
                }
                catch(Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_GetAllLicenses_", GetAllLicensesCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                }
            }
        }
        public DataSet PayersCashed
        {

            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_PayersCashed_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_PayersCashed_", GetPayersCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_PayersCashed_"];
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_PayersCashed_", GetPayersCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                }
            }
        }
        public DataSet CallerInformation
        {

            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_CallerInformation_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_CallerInformation_", GetCallerInformation(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_CallerInformation_"];
                }
                catch (Exception ex)
                {

                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_CallerInformation_", GetCallerInformation(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                }
            }
        }
        public DataSet NationalityCashed
        {

            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_NationalityCashed_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_NationalityCashed_", GetNationalityCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_NationalityCashed_"];
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_NationalityCashed_", GetNationalityCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                }
            }
        }
        public DataSet PersonResidentialLocationCashed
        {

            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Cache["_PersonResidentialLocationCashed_"] == null)
                    {
                        System.Web.HttpContext.Current.Cache.Insert("_PersonResidentialLocationCashed_", GetPersonResidentialLocationCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                    }

                    return (DataSet)System.Web.HttpContext.Current.Cache["_PersonResidentialLocationCashed_"];
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Cache.Insert("_PersonResidentialLocationCashed_", GetPersonResidentialLocationCashed(), null, DateTime.Now.AddMinutes(CacheReloadTime), TimeSpan.Zero);
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);
                }
            }
        }

        public DataSet GetCallerInformation()
        {

            //  DAL.Operations.OpPayers.
            //  var callee = this.CallerInformation;
            try
            {
                List<Entities.CallerInformation> payersList = DAL.Operations.OpCallerInfo.GetAllAsync();
                return DAL.Helper.ListToDataset.ToDataSet(payersList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet CallerInformationbyLicenseID(string _licenseID)
        {

            //  DAL.Operations.OpPayers.
            //  var callee = this.CallerInformation;
            try
            {
                List<Entities.CallerInformation> payersList = DAL.Operations.OpCallerInfo.GetCallerInformationbyLicenseID(_licenseID);
                return DAL.Helper.ListToDataset.ToDataSet(payersList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet GetPayersCashed()
        {

            //  DAL.Operations.OpPayers.
            try
            {
                List<Entities.Payers> payersList = DAL.Operations.OpPayers.GetAllAsync();
                return DAL.Helper.ListToDataset.ToDataSet(payersList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet GetTicketsCashed()
        {

            //  DAL.Operations.OpPayers.
            try
            {
                List<Entities.TicketInformation> payersList = DAL.Operations.OpTicketInformation.GetAllAsync();
                return DAL.Helper.ListToDataset.ToDataSet(payersList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet GetNationalityCashed()
        {
            try
            {
                List<Entities.Nationality> uNNationalityList = DAL.Operations.OpNationality.GetAll();
                return DAL.Helper.ListToDataset.ToDataSet(uNNationalityList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet GetPersonResidentialLocationCashed()
        {
            try
            {
                List<Entities.AddressLocations> locationList = DAL.Operations.OpAddressLocations.GetAll();
                return DAL.Helper.ListToDataset.ToDataSet(locationList);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public DataSet GetAllLicensesCashed()
        {
            try
            {
                List<string> getAllLicenses = DAL.Operations.OpLookups.getAllLicenses();
                return DAL.Helper.ListToDataset.ToDataSet(getAllLicenses);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }



    }
}