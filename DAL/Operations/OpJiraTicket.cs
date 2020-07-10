using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpJiraTicket
    {
        public static JiraTicket GetJiraTicketById(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var result = entity.JiraTickets.FirstOrDefault(x => x.JiraTicketID == _id);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static JiraTicket GetLatestByTicketInformationID(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var MaxID = entity.JiraTickets.ToList().Where(x => x.TicketInformationID == _id).Select(x => x.JiraTicketID).Max();
                    var result = entity.JiraTickets.FirstOrDefault(x => x.TicketInformationID == _id && x.JiraTicketID == MaxID);

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static JiraTicket GetLatestByTicketKey(string Key)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {


                    JiraTicket result = new JiraTicket();
                    List<Entities.JiraTicket> MaxID = entity.JiraTickets.Where(x => x.JiraTicketKey == Key).ToList();

                    if (MaxID.Count > 0)
                    {
                        int JiraTicketsMax = MaxID.Select(x => x.JiraTicketID).Max();
                        //   MaxID = MaxID.Select(x => x.JiraTicketCommentsID).Max();
                        result = entity.JiraTickets.Where(x => x.JiraTicketKey == Key && x.JiraTicketID == JiraTicketsMax).FirstOrDefault();
                    }
                    return result;



                    //var MaxID = entity.JiraTickets.ToList().Where(x => x.JiraTicketKey == Key).Select(x => x.JiraTicketID).Max();
                    //var result = entity.JiraTickets.FirstOrDefault(x => x.JiraTicketKey == Key && x.JiraTicketID == MaxID);

                    //return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static List<JiraTicket> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.JiraTickets.OrderBy(a => a.JiraTicketID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(JiraTicket JiraTicket)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.JiraTickets.Add(JiraTicket);
                    entity.SaveChanges();

                    int LastInsertedID = JiraTicket.JiraTicketID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int JiraTicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.JiraTickets.SingleOrDefault(x => x.JiraTicketID == JiraTicketID);

                    DBContext.JiraTickets.Remove(entity);
                    DBContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

        }

        public static int UpdateJiraTicket(JiraTicket JiraTicket, int JiraTicketID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    JiraTicket CI = GetJiraTicketById(JiraTicketID);

                    CI.JiraTicketKey = JiraTicket.JiraTicketKey;
                    CI.TicketInformationID = JiraTicket.TicketInformationID;
                    CI.Status = JiraTicket.Status;
                    CI.Assignee = JiraTicket.Assignee;

                    CI.UpdateDate = JiraTicket.UpdateDate;
                    CI.UpdatedBy = JiraTicket.UpdatedBy;
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
