using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Operations
{
    class OpReports
    {

        public static string getOLATime (Entities.TicketInformation _TicketInformationID)
        {
            try
            {
                string OLATime = "";








                return OLATime;


            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }


    }
}
