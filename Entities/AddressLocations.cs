using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AddressLocations")]
   public class AddressLocations
    {

        [Key]
        public int AddressLocationID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public string DescriptionAR { get; set; }

        //public ICollection<PersonInformation> ResidenceLocation_FK { get; set; }
        //public ICollection<PersonInformation> WorkLocation_FK { get; set; }



        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
