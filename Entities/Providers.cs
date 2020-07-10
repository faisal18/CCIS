using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("Providers")]
   public class Providers
    {
        [Key]
        public int ProviderID { get; set; }
        public int ProviderUID { get; set; }

        [MaxLength(20)]
        public string ProviderLicense { get; set; }
        [MaxLength(150)]
        public string ProviderName { get; set; }
        
        public int ProviderTypeID { get; set; }
        public ProviderType ProviderTypeIDs { get; set; }

        public bool IsActive { get; set; }
        [MaxLength(50)]
        public string Emirate { get; set; }
        [MaxLength(50)]
        public string Source { get; set; }
  

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
