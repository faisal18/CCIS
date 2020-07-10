using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpInquiryDetails
    {

        public static int InsertRecord(InquiryDetails _InquiryDetails)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    DBContext.InquiryDetails.Add(_InquiryDetails);
                    DBContext.SaveChanges();

                    int LastInsertedID = _InquiryDetails.InquiryDetailsID;
                    string LastInserted = _InquiryDetails.Description;

                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(InquiryDetails _InquiryDetails)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);

                    //checkerRepository.Add(_InquiryDetails);
                    DBContext.InquiryDetails.Add(_InquiryDetails);

                    await DBContext.SaveChangesAsync();
                    int LastInsertedID = _InquiryDetails.InquiryDetailsID;

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


        public static int InsertRecordAsync(InquiryDetails _InquiryDetails)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_InquiryDetails);
                return _result.Result;

            }



            catch (Exception ex)
            {
                Logger.LogError(ex);
                return -1;
            }
        }

        public static List<InquiryDetails> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);



                    List<InquiryDetails> lstLocation = DBContext.InquiryDetails.OrderByDescending(a => a.CreationDate).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.InquiryDetailsID + "-" + x.CallerKeyID + "-" + x.Description).ToList();
                return lstLocation;

            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static List<InquiryDetails> GetInquiryDetailsbyCallerKeyID(string _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);

                    int _CallerKeyID = OpCallerInfo.GetRecordbyCallerKey(_licenseID).CallerInformationID;

                    List<InquiryDetails> lstLocation = DBContext.InquiryDetails.Where(x => x.CallerKeyID == _CallerKeyID)
                        .OrderByDescending(x => x.CreationDate).ToList();

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

        public static List<InquiryDetails> GetInquiryDetailsbyCallerKeyIDNew(int _licenseID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    List<InquiryDetails> lstLocation = DBContext.InquiryDetails.Where(x => x.CallerKeyID == _licenseID).ToList();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<InquiryDetails> GetInquiryDetailsbyApplicationID(int Application)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    List<InquiryDetails> lstLocation = DBContext.InquiryDetails.Where(x => x.ApplicationID == Application && x.NewInquiry == true).ToList();
                    return lstLocation;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static List<InquiryDetails> GetAllAsync()
        {
            try
            {


                Task<List<InquiryDetails>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<InquiryDetails>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);



                    List<InquiryDetails> lstLocation = await DBContext.InquiryDetails.ToListAsync();

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




        public static InquiryDetails GetRecordbyID(int _InquiryDetailsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);
                    InquiryDetails RecordObj = DBContext.InquiryDetails.SingleOrDefault(x => x.InquiryDetailsID == _InquiryDetailsID);
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



        public static bool DeletebyID(int _InquiryDetailsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);
                    InquiryDetails RecordObj = DBContext.InquiryDetails.SingleOrDefault(x => x.InquiryDetailsID == _InquiryDetailsID);
                    //checkerRepository.Dispose();
                    DBContext.InquiryDetails.Remove(RecordObj);
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



        public static int UpdateRecord(InquiryDetails Obj, int __InquiryDetailsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.InquiryDetailsRepository checkerRepository = new DataModel.InquiryDetailsRepository(DBContext);

                    InquiryDetails CI = GetRecordbyID(__InquiryDetailsID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;


                    CI.CallerKeyID = Obj.CallerKeyID;
                    CI.Description = Obj.Description;
                    CI.ActionTaken = Obj.ActionTaken;

                    CI.ApplicationID = Obj.ApplicationID;
                    CI.NewInquiry = Obj.NewInquiry;
                    CI.NewIssue = Obj.NewIssue;
                    CI.FollowUp = Obj.FollowUp;

                    CI.TicketNumber = Obj.TicketNumber;
                    //   CI.CallerInfo_FK = Obj.CallerInfo_FK;

                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;



                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __InquiryDetailsID);
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
