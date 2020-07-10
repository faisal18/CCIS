using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
    public class OpTicketLists
    {

        public static int InsertRecord(Tickets _Tickets)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                   
                    DBContext.TicketInformation.Add(_Tickets._TicketInformation);
                    DBContext.TicketHistory.Add(_Tickets._TicketHistory);
                    DBContext.TicketAttachment.Add(_Tickets._TicketAttachment);
                    DBContext.SaveChanges();
                  
                    int LastInsertedID = _Tickets._TicketInformation.TicketInformationID;
                    string LastInserted = _Tickets._TicketInformation.TicketNumber;
                   
                    return LastInsertedID;
                }


            }
            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static async Task<int> CreateRecordAsyncOp(Tickets _Tickets)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);

                    //checkerRepository.Add(_Tickets);
                    DBContext.TicketInformation.Add(_Tickets._TicketInformation);
                    DBContext.TicketHistory.Add(_Tickets._TicketHistory);
                    DBContext.TicketAttachment.Add(_Tickets._TicketAttachment);

                    await DBContext.SaveChangesAsync ();
                    int LastInsertedID = _Tickets._TicketInformation.TicketInformationID;

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

     
        public static int InsertRecordAsync(Tickets _Tickets)
        {
            try
            {

                Task<int> _result = CreateRecordAsyncOp(_Tickets);
                return _result.Result;

            }



            catch (Exception ex)
            {
                 Logger.LogError(ex);
                return -1;
            }
        }

        public static List<TicketLists> GetAll()
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);

                    var ticketInformation = (from ti in DBContext.TicketInformation
                                             join th in DBContext.TicketHistory on 
                                             ti.TicketInformationID equals th.TicketInformationID
                                             join ta in DBContext.TicketAttachment on
                                             new { a = ti.TicketInformationID, b = th.TicketHistoryID } equals
                                             new { a = ta.TicketInformationID, b = ta.TicketHistoryID }
                                             select new { ti, th, ta }
                                    ).OrderByDescending(a => a.th).ToList().Cast<TicketLists>();




                    List<TicketLists> lstTickets = new List<TicketLists>();
                  

                    lstTickets = ticketInformation.ToList<TicketLists>();
                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return lstTickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        
         public static TicketLists GetTicketsbySubject(string _Subject)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);

     


                    List<TicketInformation> lstTI = DBContext.TicketInformation.Where (x => x.Subject == _Subject)
                        .OrderByDescending(x=>x.CreationDate).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
                    List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {
                        
                        lstTH = (OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID));
                        lstTA = OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID);
                        
                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                    tickets._TicketAttachment = lstTA;

                    
                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

               public static TicketLists GetTicketsbyDateRange(DateTime _from , DateTime _To )
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);

     


                    List<TicketInformation> lstTI = DBContext.TicketInformation.Where (x => x.CreationDate >= _from && x.CreationDate <= _To)
                        .OrderByDescending(x=>x.CreationDate).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
             //       List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {

                        lstTH.AddRange((OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID)));
                   //     lstTA = OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID);
                        
                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                    tickets._TicketAttachment = null;

                    
                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static TicketLists GetTicketsbyAssignee(int _AssigneeID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);
                    List<int> lstTicketInformationIDs = DBContext.TicketHistory.Where(x => x.AssignedTOID == _AssigneeID)
                        .Select(a => a.TicketInformationID).ToList();
                        

                    List<TicketInformation> lstTI = DBContext.TicketInformation
                        .Where(a=> lstTicketInformationIDs.Contains(a.TicketInformationID)).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
                    List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {
                        
                        lstTH = (OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID));
                        lstTA = OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID);
                        
                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                    tickets._TicketAttachment = lstTA;

                    
                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
  public static TicketLists GetOpenTicketsbyAssignee(int _AssigneeID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    List<TicketHistory> _listTicketOpen = DAL.Operations.OpTicketHistory.GetOpenIncidents();
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);
                    List<int> lstTicketInformationIDs = _listTicketOpen.Where(x => x.AssignedTOID == _AssigneeID)
                        .Select(a => a.TicketInformationID).ToList();
                        

                    List<TicketInformation> lstTI = DBContext.TicketInformation
                        .Where(a=> lstTicketInformationIDs.Contains(a.TicketInformationID)).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
               //     List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {
                        
                        lstTH.AddRange( (OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID)));
                 //       lstTA.AddRange( OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID));
                        
                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                 //   tickets._TicketAttachment = lstTA;

                    
                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        //public static List<TicketLists> GetAllAsync()
        //{
        //    try
        //    {


        //        Task<List<TicketLists >> _result = GetAllAsyncCallBack();
        //        return _result.Result;
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.LogError(ex);
        //        return null;
        //    }
        //}


        //public static async Task<List<TicketLists>> GetAllAsyncCallBack()
        //{
        //    try
        //    {
        //        using (var DBContext = new DataModel.DALDbContext())
        //        {

        //            List<TicketLists> lstTickets = new List<TicketLists>();

                    
        //            var query =                        (from ti in DBContext.TicketInformation
        //                 join th in DBContext.TicketHistory on ti.TicketInformationID equals th.TicketInformationID
        //                 join ta in DBContext.TicketAttachment on
        //                 new { a = ti.TicketInformationID, b = th.TicketHistoryID } equals
        //                 new { a = ta.TicketInformationID, b = ta.TicketHistoryID }
        //                 select new { }
        //                            )
        //                            .ToList()
        //                            //.ToListAsync()
        //                            //.ConfigureAwait(false)
        //                           ;
        //            //.ToListAsync()

        //            //.ConfigureAwait(false);
        //            // lstTickets = ticketInformation.ToList<TicketLists>();
        //             //lstTickets = ticketInformation.Select( );

                     

        //            var res = query.Select(x => new { }).ToList();

        //            lstTickets = await Task.WhenAll<TicketLists>(query);
        //            //checkerRepository.Dispose();
        //            //DBContext.Dispose();
        //            return lstTickets;






        //           // List<TicketLists> lstLocation = await DBContext.TicketLists.ToListAsync();
                   
        //            //checkerRepository.Dispose();
        //            //DBContext.Dispose();
        //          //  return lstLocation;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Logger.LogError(ex);
        //        return null;
        //    }
        //}


        public static TicketLists GetRecordbyTicketInformationID(int _TicketsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);




                    List<TicketInformation> lstTI = DBContext.TicketInformation.Where(x => x.TicketInformationID == _TicketsID)
                        .OrderByDescending(x => x.CreationDate).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
                    List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {

                        lstTH = (OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID));
                        lstTA = OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID);

                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                    tickets._TicketAttachment = lstTA;


                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }
        public static TicketLists GetRecordbyTicketHistoryID(int _TicketHistoryID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);


                    TicketHistory th = Operations.OpTicketHistory.GetRecordbyID(_TicketHistoryID);
                   
                 
                    return GetRecordbyTicketInformationID(th.TicketInformationID);
                }
            }
            catch (Exception ex)
            {

                 Logger.LogError(ex);
                return null;
            }
        }

        public static TicketLists GetRecordbyTicketCallerKeyID(int _TicketsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);




                    List<TicketInformation> lstTI = DBContext.TicketInformation.Where(x => x.CallerKeyID == _TicketsID)
                        .OrderByDescending(x => x.CreationDate).ToList();
                    List<TicketHistory> lstTH = new List<TicketHistory>();
                    List<TicketAttachment> lstTA = new List<TicketAttachment>();
                    List<TicketLists> lstTickets = new List<TicketLists>();


                    foreach (TicketInformation item in lstTI)
                    {

                        lstTH = (OpTicketHistory.GetTicketHistorybyTicketID(item.TicketInformationID));
                        lstTA = OpTicketAttachment.GetTicketAttachmentbyTicketID(item.TicketInformationID);

                    }
                    TicketLists tickets = new TicketLists();
                    tickets._TicketInformation = lstTI;
                    tickets._TicketHistory = lstTH;
                    tickets._TicketAttachment = lstTA;


                    //var tickets = Enumerable.Repeat(lstTI, lstTI.Count);











                    //checkerRepository.Dispose();
                    //DBContext.Dispose();
                    return tickets;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }



        //public static bool DeletebyID(int _TicketsID)
        //{
        //    try
        //    {
        //        using (var DBContext = new DataModel.DALDbContext())
        //        {
        //            //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);
        //            Tickets RecordObj = DBContext.Tickets.SingleOrDefault(x => x.UserRoleID == _TicketsID);
        //            //checkerRepository.Dispose();
        //            DBContext.Tickets.Remove(RecordObj);
        //            DBContext.SaveChanges();
        //            // DBContext.Dispose();
        //            return true; ;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //         Logger.LogError(ex);
        //        return false;
        //    }
        //}


        //public static int UpdateRecord(Tickets Obj, int __TicketsID)
        //{
        //    try
        //    {
        //        using (var DBContext = new DataModel.DALDbContext())
        //        {
        //            //DataModel.TicketsRepository checkerRepository = new DataModel.TicketsRepository(DBContext);

        //            Tickets CI = GetRecordbyID(__TicketsID);
        //            CI.UpdateDate = DateTime.Now;
        //            CI.UpdatedBy = Obj.UpdatedBy;
        //            CI.RoleID = Obj.RoleID;
        //            CI.Description = Obj.Description;
        //            CI.PersonID = Obj.PersonID;
                

        //            DBContext.Entry(CI).State = System.Data.Entity.EntityState.Modified;

        //            return DBContext.SaveChanges();
        //            //checkerRepository.Update(Obj, __TicketsID);
        //            ////checkerRepository.Dispose();
        //            //DBContext.Dispose();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //         Logger.LogError(ex);
        //        return -1;
        //    }
        //}
     
    }
}
