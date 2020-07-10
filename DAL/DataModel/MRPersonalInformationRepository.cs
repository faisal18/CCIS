using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataModel
{
   internal  class MRPersonalInformationRepository : GenericRepository<Entities.PersonInformation>
    {

       
            public MRPersonalInformationRepository(DALDbContext _context) : base(_context)
            {
                base.Save();


            }
        
    }
}
