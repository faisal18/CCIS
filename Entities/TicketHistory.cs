using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("TicketHistory")]
    public class TicketHistory
    {

        [Key]
        public int TicketHistoryID { get; set; }

        public string JiraNumber { get; set; }

        public string Comments { get; set; }
        public string ActivityComments { get; set; }

        public string Activity { get; set; }

        public int SLASequenceID { get; set; }
        public DateTime? SLATriggerTime { get; set; }


        public int TicketInformationID { get; set; }
        public TicketInformation Tickets_FK { get; set; }

        public int AssignedTOID { get; set; }
        public PersonInformation AssignedTo_FK { get; set; }

        public int AssignedFromID { get; set; }
        public PersonInformation AssignedFrom_FK { get; set; }

        public int SeverityID { get; set; }
        public ItemTypes Severity_FK { get; set; }

        public int PriorityID { get; set; }
        public ItemTypes Priority_FK { get; set; }

        public int IncidentStatusID { get; set; }
        public Statuses IncidentStatusesIDs_FK { get; set; }

        public int IncidentSubStatusID { get; set; }
        public Statuses IncidentSubStatusID_FK { get; set; }

        public int SupportTypeID { get; set; }


        
        //public ICollection<TicketAttachment> TicketAttachments_FK { get; set; }


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
