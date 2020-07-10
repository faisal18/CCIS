using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SystemUser")]
   public class SystemUser
    {

        [Key]
        public int SystemUserID { get; set; }
        [MaxLength(100)]
        public string username { get; set; }
        public string password { get; set; }

        public bool isAdmin { get; set; }
        public bool isActive { get; set; }

        public int PersonID { get; set; }
        public PersonInformation PersonInformationIDs_FK { get; set; }


     


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
