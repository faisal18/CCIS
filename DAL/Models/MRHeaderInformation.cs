using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_MRHeaderInformation")]
  public  class MRHeaderInformation
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }
        [MaxLength(100)]
        public string SenderID { get; set; }
        [MaxLength(100)]
        public string ReceiverID { get; set; }

        //[NotMapped]
        [MaxLength(100)]
        public string PayerID { get; set; }
        //[NotMapped]
        [MaxLength(100)]
        public string TPAID { get; set; }
        //[NotMapped]
        [MaxLength(100)]
        public string IntermediaryID { get; set; }
        //[NotMapped]
        public string FileID { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

        public DateTime? TransactionDate { get; set; }
        public int RecordCount { get; set; }

       //   [ForeignKey("MemID")]
        public string MemID { get; set; }
        public MemberID MemberID { get; set; }

        //[NotMapped]
        [MaxLength(100)]
        public string DispositionFlag { get; set; }
        public bool? IsViewd { get; set; } = false;
        public bool? IsProcessed { get; set; } = false;

        //public int SubmissionID { get; set; }
        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("CreationDate")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        [Timestamp]
        public Byte[] TimeStamp { get; set; }

    }
}
