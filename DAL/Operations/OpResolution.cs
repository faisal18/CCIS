using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpResolution
    {

        public static List<Resolution> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Resolutions.OrderBy(a => a.TicketInformationID).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static Resolution GetResolutionById(int id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Resolutions.FirstOrDefault(x => x.ResolutionID == id);
                    return email;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }
        public static Resolution GetResolutionByTicketInformationID(int id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Resolutions.FirstOrDefault(x => x.TicketInformationID == id);
                    return email;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }
        public static Resolution GetResolutionByCategoryID(int id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.Resolutions.FirstOrDefault(x => x.CategoryID == id);
                    return email;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(Resolution resolution)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.Resolutions.Add(resolution);
                    entity.SaveChanges();

                    int LastInsertedID = resolution.ResolutionID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int ResolutionID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.Resolutions.SingleOrDefault(x => x.ResolutionID == ResolutionID);
                    DBContext.Resolutions.Remove(entity);
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

        public static int UpdateResolution(Resolution resolution, int resolutionID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    Resolution CI = GetResolutionById(resolutionID);
                    //CI.ResolutionID = resolution.ResolutionID;
                    CI.TicketInformationID = resolution.TicketInformationID;
                    CI.CategoryID = resolution.CategoryID;
                    CI.Steps = resolution.Steps;
                    CI.RootCause = resolution.RootCause;

                    CI.UpdateDate = resolution.UpdateDate;
                    CI.UpdatedBy = resolution.UpdatedBy;

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
