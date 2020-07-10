using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_MRFiles")]
 public   class MRFiles
    {
        //[Key]

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MRFilesID { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

        //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        public MemberID MemberID { get; set; }

        [MaxLength(450)]
        public string FileID { get; set; } 

        public byte[] FileContent { get; set; }
        public string ValidationReport { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }

        public DateTime? ReportCreationDate { get; set; }

        //  public int SubmissionID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
