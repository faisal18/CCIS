using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("TicketApplicationProp")]
    public class TicketApplicationProp
    {
        [Key]
        public int TicketApplicationPropID { get; set; }

        public int ApplicationPropID { get; set; }

        public int TicketID { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
