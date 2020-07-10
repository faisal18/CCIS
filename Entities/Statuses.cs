using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Statuses")]
   public class Statuses
    {

        [Key]
        public int StatusesID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        public string Types { get; set; }

        //public ICollection<Notification> Notifications_FK { get; set; }
      
        //public ICollection<SLADeclarations> SLADeclartions_FK { get; set; }
        ////public ICollection<TicketHistory> TicketIncidentHistories_FK { get; set; }
        //public ICollection<TicketHistory> IncidentTicketSubStatus_FK { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
