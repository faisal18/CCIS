using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MR_Dal.Models
{
    [Table("V2_Payers")]
   public class Payers
    {
        [Key]
        public int PayerID { get; set; }
        [MaxLength(10)]
        public string PayerCode { get; set; }
        [MaxLength(150)]
        public string PayerName { get; set; }
        
        public int PayerTypeID { get; set; }
        public bool IsActive { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string LoginKey { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public DateTime? LicenseStartDate { get; set; }
        public DateTime? LicenseEndDate { get; set; }
        




        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
