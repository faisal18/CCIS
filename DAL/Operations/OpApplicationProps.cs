using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.Operations
{
    public class OpApplicationProps
    {
        public static ApplicationProp GetApplicationPropById(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var result = entity.ApplicationProps.FirstOrDefault(x => x.ApplicationPropID == _id);
                    return result;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<ApplicationProp> GetApplicationPropByApplicationId(int _id)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var result = entity.ApplicationProps.Where(x => x.ApplicationsID == _id).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static List<ApplicationProp> GetAll()
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    var email = entity.ApplicationProps.OrderBy(a => a.Property).ToList();
                    return email;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return null;
            }
        }

        public static int InsertRecord(ApplicationProp applicationProp)
        {
            try
            {
                using (var entity = new DataModel.DALDbContext())
                {
                    entity.ApplicationProps.Add(applicationProp);
                    entity.SaveChanges();

                    int LastInsertedID = applicationProp.ApplicationPropID;
                    return LastInsertedID;
                }
            }
            catch (Exception ex)
            {

                Logger.LogError(ex);
                return -1;
            }
        }

        public static bool DeleteByID(int resolutionID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    var entity = DBContext.ApplicationProps.SingleOrDefault(x => x.ApplicationPropID == resolutionID);
                    DBContext.ApplicationProps.Remove(entity);
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

        public static int Update(ApplicationProp applicationProp, int applicationPropID)
        {
            try
            {
                using (var DBContext = new DataModel.DALDbContext())
                {
                    ApplicationProp CI = GetApplicationPropById(applicationPropID);
                    CI.ApplicationsID = applicationProp.ApplicationsID;
                    CI.Property = applicationProp.Property;
                    CI.Value = applicationProp.Value;

                    CI.UpdateDate = applicationProp.UpdateDate;
                    CI.UpdatedBy = applicationProp.UpdatedBy;

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
