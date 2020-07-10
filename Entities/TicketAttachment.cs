using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("TicketAttachment")]
    public class TicketAttachment
    {

        [Key]
        public int TicketAttachmentID { get; set; }

        public string filename { get; set; }
        public byte[] Attachment { get; set; }


        public int TicketInformationID { get; set; }
        public TicketInformation TicketsInformationIDs_FK { get; set; }

        public int TicketHistoryID { get; set; }
        public TicketHistory TicketHistoryIDs_FK { get; set; }

       


     
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
