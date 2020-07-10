using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
   public class MemberRegister
    {

        [Key]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

        public MemberID MemberID { get; set; }
        public MRContracts MRContracts { get; set; }
        public MRFiles MRFiles { get; set; }
        public MREstablishment MREstablishment { get; set; }
        public MRPersonInformation MRPersonInformation { get; set; }
        public MRPhoto MRPhoto { get; set; }

                             
        ////  public int SubmissionID { get; set; }
        //public string CreatedBy { get; set; }
        ////[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime? CreatedDate { get; set; } = DateTime.Now;


        //public string UpdatedBy { get; set; }
        //public DateTime? UpdateDate { get; set; }


        //[Timestamp]
        //public Byte[] TimeStamp { get; set; }
        public MemberRegister ()
        {
            MemberID = new MemberID();
            MRHeaderInformation = new MRHeaderInformation();
            MRPersonInformation = new MRPersonInformation();
            MRPhoto = new MRPhoto();
            MRContracts = new MRContracts();
            MREstablishment = new MREstablishment();
            MRFiles = new MRFiles();

        }


    }
}
