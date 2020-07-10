using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ApplicationProp")]
    public class ApplicationProp
    {
        [Key]
        public int ApplicationPropID { get; set; }

        public int ApplicationsID { get; set; }

        public int Property { get; set; }
        public string Value { get; set; }


        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        [Timestamp]
        public Byte[] TimeStamp { get; set; }
    }
}
