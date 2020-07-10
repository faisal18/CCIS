using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_MRContracts")]
   public class MRContracts
    {
        /*
         
         */
         [Key]
         public int MRContractID { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

      //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        public MemberID MemberID { get; set; }

        public int ProductOrigin { get; set; }
        [MaxLength(50)]
        public string ProductCode { get; set; }
        public string ProductID { get; set; }
        [MaxLength(50)]
        public string PolicyID { get; set; }
        public int TopUpPolicy { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public DateTime? DeletionDate { get; set; }
        public float GrossPremium { get; set; }
        public float ActualPremium { get; set; }
        public float IntermediaryFee { get; set; }
        [MaxLength(200)]
        public string TPAFee { get; set; }
        public int TPAFeeType { get; set; }
        [MaxLength(200)]
        public string IPCopay { get; set; }
        [MaxLength(200)]
        public string OPCopay { get; set; }
        [MaxLength(200)]
        public string PharmacyCopay { get; set; }
        [MaxLength(200)]
        public string DentalCopay { get; set; }
        [MaxLength(200)]
        public string OpticalCopay { get; set; }
        [MaxLength(200)]
        public string MaternityCopay { get; set; }



        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        [Timestamp]
        public Byte[] TimeStamp { get; set; }

    }
}
