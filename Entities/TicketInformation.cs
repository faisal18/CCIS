using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("TicketInformation")]
    public class TicketInformation
    {

        [Key]
        public int TicketInformationID { get; set; }
       
        public int CallerKeyID { get; set; }
        public CallerInformation CallerInfo_FK { get; set; }

        public string TicketNumber   { get; set; }
        public Guid TicketGUIDKey   { get; set; }
     

        [MaxLength(1000)]
        public string Subject   { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }

        public int ApplicationID { get; set; }
        public Applications Applications_FK { get; set; }

        public int SeverityID { get; set; }
        public ItemTypes Severity_FK { get; set; }

        public int PriorityID { get; set; }
        public ItemTypes Priority_FK { get; set; }

        public int TicketType { get; set; }
        public ItemTypes TicketTypes_FK { get; set; }

        public int ReporterID { get; set; }
        public PersonInformation Reporter_FK { get; set; }


        //Business Comments
        public string Comments { get; set; }
        public string CallerSource { get; set; }


        // SMART FORMS FIELDS
        public DateTime? DueDate { get; set; }
        public int? ParentTicketID { get; set; }
        public string ContingencyPlan  { get; set; }


        /// <summary>
        ///  Business REport & Business COmments
        /// </summary>
        /// 
        public string ResolutionSummary { get; set; }
        public string IssueDescription { get; set; }
        public string ResolutionActions { get; set; }
        



        /// <summary>
        /// Approval PRocess Fields
        /// 
        /// </summary>
        public string ProblemJustification { get; set; }
        public bool ApprovedbyL1 { get; set; }
        public bool ApprovedbyL2 { get; set; }



        //public ICollection<TicketHistory> TicketHistories_FK { get; set; }
        //public ICollection<TicketAttachment> TicketAttachments_FK { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
