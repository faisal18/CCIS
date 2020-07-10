using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponenets.UserControl
{
    public partial class UserControlTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //List<Entities.CallerInformation> payersList = DAL.Operations.OpCallerInfo.GetAllAsync();
            //MasterGrid.GridBinding( DAL.Helper.ListToDataset.ToDataSet(payersList));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
          Entities.TicketLists xyz =  DAL.Operations.OpTicketLists.GetRecordbyTicketInformationID(227);
            string x = DAL.Operations.OpTicketHistory.GetAllCommentsbyTicketInformationID(227);
            WebService.SLAProcessing.GenerateNotification(xyz._TicketInformation[0].TicketInformationID, true, "Client Ticket Creation", "Checking Email notification ", "Email", true, true); ;

        }
    }
}