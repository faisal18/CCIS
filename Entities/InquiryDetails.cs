using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("InquiryDetails")]
    public class InquiryDetails
    {

        [Key]
        public int InquiryDetailsID { get; set; }
       
        public int CallerKeyID { get; set; }
        public CallerInformation CallerInfo_FK { get; set; }

        public string Description { get; set; }

        [MaxLength(1000)]
        public string ActionTaken { get; set; }

        public int CallerSource { get; set; }
        public ItemTypes CallerSource_FK { get; set; }


        //public int StatusID { get; set; }
        //public Statuses Statuses { get; set; }

        [MaxLength(100)]
        public string TicketNumber { get; set; }

        public int ApplicationID { get; set; }
        public Applications Applications_FK { get; set; }

        public bool NewInquiry { get; set; }
        public bool NewIssue { get; set; }
        public bool FollowUp { get; set; }


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
