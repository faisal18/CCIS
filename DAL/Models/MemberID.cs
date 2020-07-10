using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{  /*
                        Payer type: I=INS  SP=Self Paid   SF=Self Funded
                Insurer ID – 3 Numbers following INS… - / or SP/SF ID – 4 numbers following SP or SF
                TPA ID – 3 Numbers following TPA… -
                First letter of first name – S
                Gender – 1 = male or 0 = female
                DOB – YYYY
                Last letter of last name – K
                ReferenceID
                PolicyID

                 * 
                * */
    [Table("V2_Member")]
    public   class MemberID
    {
        //[Key]

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int MemID { get;  set; }
      //  [Key]
        [Column(Order = 2)]
        public string MemberCode { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        //public  MRHeaderInformation MRHeaderInformation { get; set; }


        //[Required]
        public string PayerType { get; set; }
 
        public string InsurerID { get; set; }
        public string TPAID { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string LastName { get; set; }
        public string ReferrenceID { get; set; }
        [MaxLength(100)]
        public string PolicyID { get; set; }
        public string MemberDataComputed { get; set; }

        public string GeneratedHash { get; set; }

        public bool? isUpdate { get; set; } = false;
        public bool? isDeleted { get; set; } = false;
        public bool? isVerified { get; set; } = true;


      //  public int SubmissionID { get; set; }
        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
