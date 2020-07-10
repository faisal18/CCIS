using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DAL.DataModel
{
    internal class MaintenanceLoggingRepository: GenericRepository<MaintenanceLogging>
    {
        public   MaintenanceLoggingRepository(DALDbContext _context):base (_context)
        {
            base.Save();
        }
    }

   
}
