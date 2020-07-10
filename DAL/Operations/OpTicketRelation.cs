using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpTicketRelation
    {

        public static TicketRelation GetTicketRelation(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var result = entity.TicketRelations.FirstOrDefault(x => x.TicketRelationID == _id);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static List<TicketRelation> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.TicketRelations.OrderBy(a => a.TR_TI_ToID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(TicketRelation ticketRelation)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.TicketRelations.Add(ticketRelation);
                    entity.SaveChanges();

                    int LastInsertedID = ticketRelation.TicketRelationID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int TicketRelationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.TicketRelations.SingleOrDefault(x => x.TicketRelationID == TicketRelationID);
                    DBContext.TicketRelations.Remove(entity);
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

        public static int UpdateEmail(TicketRelation TicketRelations, int TicketRelationID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {

                    TicketRelation CI = GetTicketRelation(TicketRelationID);

                    CI.TR_TI_ID = TicketRelations.TR_TI_ID;
                    CI.TR_RelationTypeID = TicketRelations.TR_RelationTypeID;
                    CI.TR_TI_ToID = TicketRelations.TR_TI_ToID;

                    CI.UpdateDate = TicketRelations.UpdateDate;
                    CI.UpdatedBy = TicketRelations.UpdatedBy;
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
