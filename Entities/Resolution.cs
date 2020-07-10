using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Resolution")]
    public class Resolution
    {
        [Key]
        public int ResolutionID { get; set; }

        public int TicketInformationID { get; set; }
        public TicketInformation TicketInformation_FK { get; set; }

        public int CategoryID { get; set; }
        public string Steps { get; set; }
        public string RootCause { get; set; }
        public string MergeToResolutionID { get; set; }
        public bool ShowinKB { get; set; }
        public bool ShowforL1 { get; set; }
        public bool ShowinTicket { get; set; }

                          
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
