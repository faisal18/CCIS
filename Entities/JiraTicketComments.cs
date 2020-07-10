using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Entities
{
    [Table("JiraTicketComments")]
    public class JiraTicketComments
    {
        [Key]
        public int JiraTicketCommentsID { get; set; }
        public int TicketInformationID { get; set; }
        public string JiraTicketKey { get; set; }
        public string Comments { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
