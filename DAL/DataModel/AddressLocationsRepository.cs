using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModel
{
  internal  class AddressLocationsRepository : GenericRepository<Entities.AddressLocations>
    {


        
            public AddressLocationsRepository(DALDbContext _context) : base(_context)
            {
                base.Save();


            }
       

    }
}
