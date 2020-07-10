using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModel
{
  internal  class NationalityRepository : GenericRepository<Entities.Nationality>
    {


        
            public NationalityRepository(DALDbContext _context) : base(_context)
            {
                base.Save();


            }
       

    }
}
