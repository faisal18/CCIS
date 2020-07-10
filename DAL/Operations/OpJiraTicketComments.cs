using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpJiraTicketComments
    {
        public static JiraTicketComments GetJiraTicketCommentsById(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var result = entity.jiraTicketComments.FirstOrDefault(x => x.JiraTicketCommentsID == _id);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static JiraTicketComments GetLatestByTicketInformationID(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var MaxID = entity.jiraTicketComments.ToList().Where(x => x.TicketInformationID == _id).Select(x => x.JiraTicketCommentsID).Max();
                    var result = entity.jiraTicketComments.FirstOrDefault(x => x.TicketInformationID == _id && x.JiraTicketCommentsID == MaxID);

                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static JiraTicketComments GetLatestByTicketKey(string Key)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {

                    JiraTicketComments result = new JiraTicketComments();
                        List<Entities.JiraTicketComments> MaxID = entity.jiraTicketComments.Where(x => x.JiraTicketKey == Key).ToList();

                    if (MaxID.Count > 0)
                    {
                        int JiraTicketCommentsMax = MaxID.Select(x => x.JiraTicketCommentsID).Max();
                        //   MaxID = MaxID.Select(x => x.JiraTicketCommentsID).Max();
                       result = entity.jiraTicketComments.Where(x => x.JiraTicketKey == Key && x.JiraTicketCommentsID == JiraTicketCommentsMax).FirstOrDefault();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static List<JiraTicketComments> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.jiraTicketComments.OrderBy(a => a.JiraTicketCommentsID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(JiraTicketComments JiraTicketComments)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.jiraTicketComments.Add(JiraTicketComments);
                    entity.SaveChanges();

                    int LastInsertedID = JiraTicketComments.JiraTicketCommentsID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int JiraTicketCommentsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.jiraTicketComments.SingleOrDefault(x => x.JiraTicketCommentsID == JiraTicketCommentsID);

                    DBContext.jiraTicketComments.Remove(entity);
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

        public static int UpdateJiraTicket(JiraTicketComments JiraTicketComments, int JiraTicketCommentsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    JiraTicketComments CI = GetJiraTicketCommentsById(JiraTicketCommentsID);

                    CI.JiraTicketKey = JiraTicketComments.JiraTicketKey;
                    CI.TicketInformationID = JiraTicketComments.TicketInformationID;
                    CI.Comments = JiraTicketComments.Comments;

                    CI.UpdateDate = JiraTicketComments.UpdateDate;
                    CI.UpdatedBy = JiraTicketComments.UpdatedBy;
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
