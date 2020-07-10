using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Applications")]
   public class Applications
    {

        [Key]
        public int ApplicationsID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public string Owner { get; set; }
        public string Owner_Email { get; set; }
        public string Contact_Number { get; set; }
        public string Contact_Person  { get; set; }
        public string URL  { get; set; }

        public int ApplicationGroupID { get; set; }
        public Groups ApplicationGroupID_FK { get; set; }

        //public ICollection<UserApplication> userApplications_FK   { get; set; }
        //public ICollection<InquiryDetails> inquiryDetails_FK { get; set; }
        //public ICollection<TicketInformation> ticketInformation_FK { get; set; }
        


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
