using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ItemTypes")]
   public class ItemTypes
    {

        [Key]
        public int ItemTypesID { get; set; }
        
        public string Description { get; set; }

        public string Categories { get; set; }

        public bool? GroupAction { get; set; }

        public string Scenario { get; set; }

        public string Role { get; set; }


        //public ICollection<TicketInformation> TicketSeverity_FK { get; set; }
        //public ICollection<TicketInformation> TicketPriority_FK { get; set; }
        //public ICollection<TicketInformation> TicketType_FK { get; set; }
        //public ICollection<TicketHistory> THistorySeverity_FK { get; set; }
        //public ICollection<TicketHistory> THistoryPriority_FK { get; set; }
        //public ICollection<SLADeclarations> SLASeverity_FK { get; set; }
        //public ICollection<SLADeclarations> SLAPriority_FK { get; set; }
        //public ICollection<SLADeclarations> SLA_ActionRequired_FK { get; set; }
        //public ICollection<SLAExecutionLog> ItemTypes_ActionIDs_FK { get; set; }

        //public ICollection<Notification> NotificationTypes_FK { get; set; }
      

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
