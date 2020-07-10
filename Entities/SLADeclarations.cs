using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SLADeclarations")]
   public class SLADeclarations
    {

        [Key]
        public int SLADeclarationsID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }


        public int SLASequenceID { get; set; }

        public bool isActive { get; set; }
        public bool isRepeatable { get; set; }

        public int TimeinMinutes { get; set; }


        public int NotificationType { get; set; }
        public ItemTypes ItemTypes_NotificationType_FK { get; set; }


        public int SeverityID { get; set; }
        public ItemTypes Severity_FK { get; set; }

        public int PriorityID { get; set; }
        public ItemTypes Priority_FK { get; set; }

        public int ActionRequiredID { get; set; }
        public ItemTypes ItemTypes_ActionIDs_FK { get; set; }

        public int StatusID { get; set; }
        public Statuses StatusesIDs_FK { get; set; }

        public int SubStatusID { get; set; }
        public Statuses SubStatusesIDs_FK { get; set; }

        public int ApplicationID { get; set; }
        public Applications ApplicationID_FK { get; set; }

        public int TicketTypeID { get; set; }
        public Statuses TicketTypeIDs_FK { get; set; }



        //  public ICollection<SLAExecutionLog> SLAExecutionLogs_FK { get; set; }


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
