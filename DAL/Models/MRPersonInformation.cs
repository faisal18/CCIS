using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_MRPersonInformation")]
  public  class MRPersonInformation
    {

        [Key]
        public int MRPersonInformationID { get; set; }
       
        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

        //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        public MemberID MemberID { get; set; }



        public int memberTypeID { get; set; }
        public MemberType MemberType { get; set; }
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string SecondName { get; set; }
        [MaxLength(50)]
        public string FamilyName { get; set; }
        [MaxLength(50)]
        public string ContactNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        
        public int Gender { get; set; }
        public int NationalityID { get; set; }
        public UNNationality Nationality { get; set; }


        [MaxLength(20)]
        public string PassportNumber { get; set; }

        public int MaritalStatus { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        //[MaxLength(100)]
        public int Emirate { get; set; }

        public int ResidenceLocationID { get; set; }
        public DSCLocations DSCLocations { get; set; }

        public int WorkLocationID { get; set; }
        public DSCLocations DSCLocationsWork { get; set; }

        [MaxLength(50)]
        public string EmiratesIDNumber { get; set; }
        public int UIDNumber { get; set; }
        public string GDRFAFileNumber { get; set; }
        public string BirthCertificateID { get; set; }

        //[MaxLength(100)]
        public int Salary { get; set; }
        [MaxLength(50)]
        public string Commission { get; set; }


        public int Relation { get; set; }
        [MaxLength(30)]
        public string RelationTo { get; set; }



        public int CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        public int UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }


        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
