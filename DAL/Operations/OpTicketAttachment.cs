using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpTicketAttachment
    {

        public static int InsertRecord(TicketAttachment _TicketAttachment)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.TicketAttachment.Add(_TicketAttachment);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _TicketAttachment.TicketAttachmentID;
                    string LastInserted = _TicketAttachment.filename;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(TicketAttachment _TicketAttachment)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);

                    //checkerRepository.Add(_TicketAttachment);
                    DBContext.TicketAttachment.Add(_TicketAttachment);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _TicketAttachment.TicketAttachmentID;

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

     
        public static int InsertRecordAsync(TicketAttachment _TicketAttachment)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_TicketAttachment);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<TicketAttachment> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = DBContext.TicketAttachment.OrderBy(a => a.TicketInformationID).ToList();

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


                List<string> lstLocation = GetAll().Select(x => x.TicketAttachmentID + "-" +x.filename ).ToList();
                    return lstLocation;
                
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
         public static List<TicketAttachment> GetTicketAttachmentbyFileName(string _Comments)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = DBContext.TicketAttachment.Where (x => x.filename == _Comments)
                        .OrderBy(x=>x.filename).ToList();

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
        public static List<TicketAttachment> GetTicketAttachmentbyTicketID(int _TicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = DBContext.TicketAttachment.Where(x => x.TicketInformationID == _TicketID)
                        .OrderBy(x => x.TicketInformationID).ToList();

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


        public static List<TicketAttachment> GetTicketAttachmentbyTicketHistoryID(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = DBContext.TicketAttachment.Where(x => x.TicketHistoryID == _TicketHistoryID )
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

        public static List<TicketAttachment> GetTicketAttachmentbyTicketandHistoryID(int _TicketHistoryID, int _TicketInformationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = DBContext.TicketAttachment.Where(x => x.TicketHistoryID == _TicketHistoryID && x.TicketInformationID == _TicketInformationID)
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

        public static List<TicketAttachment> GetAllAsync()
        {
            try
            {


                Task<List<TicketAttachment>> _result = GetAllAsyncCallBack();
                return _result.Result;
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static async Task<List<TicketAttachment>> GetAllAsyncCallBack()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);



                    List<TicketAttachment> lstLocation = await DBContext.TicketAttachment.ToListAsync();
                   
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


        public static TicketAttachment GetRecordbyID(int _TicketAttachmentID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);
                    TicketAttachment RecordObj = DBContext.TicketAttachment.SingleOrDefault(x => x.TicketAttachmentID == _TicketAttachmentID);
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


        public static bool DeletebyID(int _TicketAttachmentID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);
                    TicketAttachment RecordObj = DBContext.TicketAttachment.SingleOrDefault(x => x.TicketAttachmentID == _TicketAttachmentID);
                    //checkerRepository.Dispose();
                    DBContext.TicketAttachment.Remove(RecordObj);
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


        public static int UpdateRecord(TicketAttachment Obj, int __TicketAttachmentID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketAttachmentRepository checkerRepository = new DataModel.TicketAttachmentRepository(DBContext);

                    TicketAttachment CI = GetRecordbyID(__TicketAttachmentID);
                    CI.UpdateDate = DateTime.Now;
                    CI.UpdatedBy = Obj.UpdatedBy;
                    CI.Attachment = Obj.Attachment;
                    CI.filename = Obj.filename;
                    CI.TicketHistoryID = Obj.TicketHistoryID;
                    CI.TicketInformationID = Obj.TicketInformationID;
                   

                  
                  
                    


                    DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

                    return DBContext.SaveChanges();
                    //checkerRepository.Update(Obj, __TicketAttachmentID);
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
