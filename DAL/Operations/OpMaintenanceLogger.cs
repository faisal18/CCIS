using System;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
  public  class Logger
    {

        public static async Task<int> CreateLogAsyncOp(MaintenanceLogging _LogID)
        {
            try
            {
                using (var MemberIDContext = new DataModel.DALDbContext())
                {
                    DataModel.MaintenanceLoggingRepository checkerRepository = new DataModel.MaintenanceLoggingRepository(MemberIDContext);

                    checkerRepository.Add(_LogID);
                    await checkerRepository.SaveAsync();
                    int LastInsertedID = _LogID.MLogID;

                    checkerRepository.Dispose();
                    MemberIDContext.Dispose();
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Helper.Log4Net.Error(ex.ToString());
                return -1;
            }
        }


        public static int InsertLogAsync(MaintenanceLogging _LogID)
        {
            try
            {

                Task<int> _result = CreateLogAsyncOp(_LogID);
                return _result.Result;

            }



            catch (Exception ex)
            {
             //   Helper.Log4Net lg = new Helper.Log4Net();
                Helper.Log4Net.Error("Critical--- InsertLogAsync --- \n" + ex);
                return -1;
            }
        }


        public static void Log(string ApplicationName, string apppath, string ErrorLevel, string MaintenanceDetails, string status, string ErrorDetails)
            {

            MaintenanceLogging mLog = new MaintenanceLogging();
            mLog.ApplicationName = ApplicationName;
            mLog.AppPath = apppath;
            mLog.MaintenanceLogDetails = MaintenanceDetails;
            mLog.Status = status;
            mLog.ErrorDetails = ErrorDetails;
            mLog.ErrorLevel = ErrorLevel;

            InsertLogAsync(mLog);

            }


        public static void LogError( string ErrorLevel,  string ErrorDetails, string methodname = "Calling Method")
        {

            MaintenanceLogging mLog = new MaintenanceLogging();
            mLog.ApplicationName = "MemberRegister_DAL";
            mLog.ErrorDetails = ErrorDetails;
            mLog.ErrorLevel = ErrorLevel;
            mLog.MaintenanceLogDetails = methodname;

            InsertLogAsync(mLog);
        }

        public static InsertResponse LogError (Exception exc)
        {
             MaintenanceLogging mLog = new MaintenanceLogging();
             
            InsertResponse responseMessage = new InsertResponse();
            responseMessage.responseCode = -4;
            if (exc.InnerException != null)
            {
                responseMessage.ErrorMessage = exc.InnerException.ToString();
                mLog.ErrorDetails = exc.InnerException.ToString();

            }
            else
            {
                responseMessage.ErrorMessage = exc.Message;
                mLog.ErrorDetails = " INNER EXCEPTION IS NULL ";
            }
            try
            {
                mLog.ApplicationName = exc.Source;
                mLog.ErrorLevel ="CRITICAL";
                mLog.MaintenanceLogDetails = exc.StackTrace;
                mLog.AppPath = exc.Message;


                InsertLogAsync(mLog);
               
                return responseMessage;
            }
            catch (Exception ex)
            {
                Helper.Log4Net.Info(ex);
                //throw;
                return responseMessage;
            }
        }


        public static void Info( string MaintenanceDetails)
        {

            MaintenanceLogging mLog = new MaintenanceLogging();
            mLog.ApplicationName = "MemberRegister_DAL_";
            mLog.MaintenanceLogDetails = MaintenanceDetails;
            mLog.ErrorLevel = "Info";

            InsertLogAsync(mLog);

            //Helper.Log4Net.Info(MaintenanceDetails);

        }


    }
}
