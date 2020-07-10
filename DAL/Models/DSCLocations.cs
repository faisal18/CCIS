using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_DSCLocations")]
   public class DSCLocations
    {

        [Key]
        public int DSCLocationsID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public string DescriptionAR { get; set; }


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
