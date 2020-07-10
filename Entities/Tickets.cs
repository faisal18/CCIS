using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{

    public class Tickets
    {

        public TicketInformation _TicketInformation { get; set; }
        public TicketHistory _TicketHistory { get; set; }
        public TicketAttachment _TicketAttachment { get; set; }


        public Tickets()
        {
            _TicketHistory = new TicketHistory();
            _TicketAttachment = new TicketAttachment();
            _TicketInformation = new TicketInformation();
        }


    }

    public class TicketLists
    {

        public List<TicketInformation> _TicketInformation { get; set; }
        public List<TicketHistory> _TicketHistory { get; set; }
        public List<TicketAttachment> _TicketAttachment { get; set; }


    }
}
