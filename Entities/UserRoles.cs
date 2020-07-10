using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("UserRoles")]
   public class UserRoles
    {

        [Key]
        public int UserRoleID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        public int RoleID { get; set; }
        public Roles Roles_FK { get; set; }
        public int PersonID { get; set; }
        public PersonInformation PersonInformation_FK { get; set; }


     


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
