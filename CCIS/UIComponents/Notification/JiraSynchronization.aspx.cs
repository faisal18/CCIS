using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Notification
{
    public partial class JiraSynchronization : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DAL.Jira.JiraSynch jiraSynch = new DAL.Jira.JiraSynch();

                //lbl_message.Text += jiraSynch.Synch_StatusNAssignee();
                //lbl_message.Text += jiraSynch.SynchComments();


                //Thread Status = new Thread(() => { jiraSynch.Synch_Status(); });
                //Status.Start();

                //Thread Assignee = new Thread(() => { jiraSynch.Synch_Assignee(); });
                //Assignee.Start();

                Thread Comments = new Thread(() => { jiraSynch.Synch_Comments(); });
                Comments.Start();

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                lbl_message.Text = ex.Message;
            }
        }
    }
}