using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CallerInformation")]
   public class CallerInformation
    {

        [Key]
        public int CallerInformationID { get; set; }
        [MaxLength(100)]
        public string CallerKeyID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string CallerLicense { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }


       

        [MaxLength(100)]
        public string Email { get; set; }

        public bool isContactPerson { get; set; }
        public bool isOwner { get; set; }


        // 
        // SMART FORMS FIELDS
        [MaxLength(100)]
        public string Department { get; set; }
        [MaxLength(100)]
        public string Location { get; set; }

        [MaxLength(100)]
        public string OperatingSystem { get; set; }
        [MaxLength(100)]
        public string MachineName { get; set; }


        //public ICollection<InquiryDetails> Inquiries_FK { get; set; }
        //public ICollection<TicketInformation> TicketInformations_FK { get; set; }
        //public ICollection<Notification> Notifications_FK { get; set; }







        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
