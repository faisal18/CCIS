using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class InsertResponse
    {


        public int responseCode { get; set; }
        public string memberCode { get; set; }
        public string ErrorMessage { get; set; }

    }
}
