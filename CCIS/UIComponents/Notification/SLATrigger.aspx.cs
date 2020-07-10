using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.WebService
{
    public partial class SLATrigger : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SLAProcessing sLAProcessing = new SLAProcessing();
                lbl_result.Text = sLAProcessing.ProcessTickets();
            }
            catch(Exception ex)
            {
                lbl_result.Text = ex.Message;

            }
        }
    }
}