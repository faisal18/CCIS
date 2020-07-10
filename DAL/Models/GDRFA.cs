using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_GDRFA_Logs")]
 public   class GDRFA
    {
        //[Key]

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GDRFAID { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        //public MRHeaderInformation MRHeaderInformation { get; set; }

        //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        //public MemberID MemberID { get; set; }

        public int Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PassportNumber { get; set; }


        public string Request { get; set; }
        public string Response { get; set; }


        public string  Error { get; set; }
        public string ErrorDetails { get; set; }



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
