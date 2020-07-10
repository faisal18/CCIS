using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpTicketApplicationProps
    {
        public static TicketApplicationProp GetTicketApplicationProp(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    //var email = entity.Emails.SingleOrDefault(x => x.EmailID == _id);
                    var email = entity.TicketApplicationProps.FirstOrDefault(x => x.TicketApplicationPropID == _id);

                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketApplicationProp> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.TicketApplicationProps.OrderBy(a => a.ApplicationPropID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }


        public static int InsertRecord(TicketApplicationProp ticketApplication)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.TicketApplicationProps.Add(ticketApplication);
                    entity.SaveChanges();

                    int LastInsertedID = ticketApplication.TicketApplicationPropID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int ticketApplicationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.TicketApplicationProps.SingleOrDefault(x => x.TicketApplicationPropID == ticketApplicationID);
                    DBContext.TicketApplicationProps.Remove(entity);
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

        public static int UpdateEmail(TicketApplicationProp ticketApplicationProps, int TickeApplicationPropsID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    TicketApplicationProp CI = GetTicketApplicationProp(TickeApplicationPropsID);

                    CI.ApplicationPropID = ticketApplicationProps.ApplicationPropID;
                    CI.TicketID = ticketApplicationProps.TicketID;

                    CI.UpdateDate = ticketApplicationProps.UpdateDate;
                    CI.UpdatedBy = ticketApplicationProps.UpdatedBy;
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
