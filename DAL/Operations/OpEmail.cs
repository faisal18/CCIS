using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpEmail
    {
        public static Email GetEmailById(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    //var email = entity.Emails.SingleOrDefault(x => x.EmailID == _id);
                    var email = entity.Emails.FirstOrDefault(x => x.TemplateID == _id);

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
        public static Email GetEmailObjectByCategory(string  _Category)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    //var email = entity.Emails.SingleOrDefault(x => x.EmailID == _id);
                    var email = entity.Emails.FirstOrDefault(x => x.Category == _Category);

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }
  public static Email GetEmailObjectByCategoryandNotificationType(string  _Category,string _NotificationType)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    //var email = entity.Emails.SingleOrDefault(x => x.EmailID == _id);
                    var email = entity.Emails.FirstOrDefault(x => x.Category == _Category && x.TemplateType.ToUpper() == _NotificationType.ToUpper());

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<Email> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Emails.OrderBy(a => a.TemplateID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<Email> GetEmailByCategory(string _category)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Emails.Where(x => x.Category == _category).ToList();

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int GetEmailIDByCategory(string _category)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Emails.Where(x => x.Category == _category)
                        .Select(a=> a.TemplateID).FirstOrDefault();

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static int InsertRecord(Email _Email)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.Emails.Add(_Email);
                    entity.SaveChanges();

                    int LastInsertedID = _Email.TemplateID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int _EmailID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.Emails.SingleOrDefault(x => x.TemplateID == _EmailID);
                    DBContext.Emails.Remove(entity);
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

        public static int UpdateEmail(Email _Email,int _EmailID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    Email CI = GetEmailById(_EmailID);

                    CI.TemplateID = _Email.TemplateID;
                    CI.Category = _Email.Category;
                    CI.EmailBody = _Email.EmailBody;
                    CI.EmailSubject = _Email.EmailSubject;
                    CI.UpdatedBy = _Email.UpdatedBy;
                    CI.UpdateDate = DateTime.Now;

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
