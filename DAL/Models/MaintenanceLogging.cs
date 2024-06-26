﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MR_Dal.Models
{   [Table("V2_MaintenanceLogging")]
    public class MaintenanceLogging
    {
        [Key]
        public int MLogID { get; set; }

        //[Required]
        public string ApplicationName { get; set; }
        public string AppPath { get; set; }
        public string MaintenanceLogDetails { get; set; }
        public string Status { get; set; }
        public string ErrorDetails { get; set; }
        public string ErrorLevel { get; set; }
        //[NotMapped]
        //public Exception Exception { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;


        [Timestamp]
        public Byte[] TimeStamp { get; set; }


    }
}
