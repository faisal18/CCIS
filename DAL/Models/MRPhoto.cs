using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MR_Dal.Models
{
    [Table("V2_MRPhoto")]
  public  class MRPhoto
    {

        [Key]
        public int MRPhotoID { get; set; }

        //[ForeignKey("TransactionID")]
        public int TransactionID { get; set; }
        public MRHeaderInformation MRHeaderInformation { get; set; }

        //  [ForeignKey("MemID")]
        public string MemberCode { get; set; }
        public MemberID MemberID { get; set; }


        public byte[] PhotoAttachment { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
