using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SystemSettings")]
   public class SystemSettings
    {

        [Key]
        public int SystemSettingsID { get; set; }
        [MaxLength(100)]
        public string ComponentName { get; set; }
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public bool isActive { get; set; }

       
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
