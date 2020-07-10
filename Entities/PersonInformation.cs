using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("PersonInformation")]
  public  class PersonInformation
    {

        [Key]
        public int PersonInformationID { get; set; }
       
        //[ForeignKey("TransactionID")]
     

        [MaxLength(250)]
        public string FullName { get; set; }

        public string ContactNumber { get; set; }
        
        public string Gender { get; set; }

        public int NationalityID { get; set; }
        public Nationality NationalityID_FK { get; set; }


        public string Email { get; set; }
        //[MaxLength(100)]
        
        public int ResidentialLocation { get; set; }
        public AddressLocations ResidentialLocationID_FK{ get; set; }

        public int WorkLocation { get; set; }
        public AddressLocations WorkLocationID_FK { get; set; }


        //public ICollection<TicketInformation> TicketReporter_FK { get; set; }
        //public ICollection<TicketHistory> AssignedTo_FK { get; set; }
        //public ICollection<TicketHistory> AssignedFrom_FK { get; set; }
        //public ICollection<Notification> NotificationSentBy_FK { get; set; }
        //public ICollection<Notification> NotificationRecipient_FK { get; set; }
        //public ICollection<SystemUser> SystemUsers_FK { get; set; }

        //public ICollection<UserApplication> UserApplications_FK { get; set; }
        //public ICollection<UserGroups> UserGroups_FK { get; set; }
        //public ICollection<UserRoles> UserRoles_FK { get; set; }
        

        public string CreatedBy { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
