using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Groups")]
   public class Groups
    {

        [Key]
        public int GroupsID { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }

        public string GroupEmail { get; set; }


        //public ICollection<UserGroups> UserGroups_FK { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
