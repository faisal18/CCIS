using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MR_Dal.Models
{
    [Table("V2_MREstablishment")]
   public class MREstablishment
    {

        [Key]
        public int MREstablishmentID { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

        //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        public MemberID MemberID { get; set; }


        public int EntityType { get; set; }
        [MaxLength(50)]
        public string EntityID { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }


        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
