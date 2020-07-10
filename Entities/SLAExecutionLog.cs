using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SLAExecutionLog")]
    public class SLAExecutionLog
    {

        [Key]
        public int SLAExecutionLogID { get; set; }

        public string Comments { get; set; }

        public bool Triggered { get; set; }
        public bool IsBreached { get; set; }

        public DateTime? SLACheckingTime { get; set; } = DateTime.Now;

        public int SLASequenceID { get; set; }


        public DateTime? SLATriggerTime { get; set; } 

        public int TicketInformationID { get; set; }
        public TicketInformation Tickets_FK { get; set; }

        public int TicketHistoryID { get; set; }
        public TicketHistory TicketsHistory_FK { get; set; }


        public int SLAID { get; set; }
        public SLADeclarations SLADeclarations_FK { get; set; }
        
        public string ActionComments { get; set; }
        public int ActionRequiredID { get; set; }
        public ItemTypes ItemTypes_ActionIDs_FK { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
