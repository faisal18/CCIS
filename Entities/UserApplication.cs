using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("UserApplication")]
   public class UserApplication
    {

        [Key]
        public int UserApplicationID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        public int GroupID { get; set; }
        public Groups GroupsID_FK { get; set; }

        public int RoleID { get; set; }
        public Roles RolesID_FK { get; set; }
        public int PersonID { get; set; }
        public PersonInformation PersonInformationIDs_FK { get; set; }
        public int ApplicationID { get; set; }
        public Applications ApplicationsIDs_FK { get; set; }


     


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
