using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Notification
{
    public partial class NotificationList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session.Keys.Count > 0)
                {
                    Session["Name"].ToString();
                }
                else
                {
                    System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                }

                if (!IsPostBack)
                {
                    Populate_Grid();
                }
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
        }


        public void Populate_Grid()
        {
            try
            {
                repeater_NotificationList.DataSource = GetData();
                repeater_NotificationList.DataBind();
            }
            catch (Exception ex)
            {
                lbl_message.Text = ex.Message;
            }
        }


        private DataTable GetData()
        {
            DataTable result = new DataTable();
            result.Columns.Add("Description");
            
            try
            {

                List<string> sb = new  List<string>();
                var notificationslist = DAL.Operations.OpNotification.GetTodayNotification();

                for (int i = 0; i < notificationslist.Count; i++) 
                {
                    string SentById = notificationslist[i].SentByID.ToString();
                    string RecipientId = notificationslist[i].RecipientID.ToString();
                    string CreatedBy = notificationslist[i].CreatedBy.ToString();
                    string TicketNumber = notificationslist[i].TicketInformationID.ToString();
                    string comments = notificationslist[i].Comments.ToString();
                    string CreationDate = notificationslist[i].CreationDate.ToString();

                    //SentById = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(int.Parse(SentById)).First().FullName;
                    //RecipientId = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(int.Parse(RecipientId)).First().FullName;
                    string anchortag = "~/UIComponents/Ticket/ViewTicket.aspx?TicketInformationID=" + TicketNumber;
                    TicketNumber = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(int.Parse(TicketNumber)).First().TicketNumber;

                    string result_string = comments.Trim() + " on ticket " + TicketNumber + " added by " + CreatedBy + " at " + CreationDate;



                    Label newline = new Label();
                    newline.Text = comments.Trim() + " on ticket ";

                    HyperLink hyperLink = new HyperLink();
                    hyperLink.Text = TicketNumber;
                    hyperLink.NavigateUrl = anchortag;

                    Label heading = new Label();
                    heading.Text = " added by " + CreatedBy + " at " + CreationDate + "<br/>";

                    CommentsContainer.Controls.Add(newline);
                    CommentsContainer.Controls.Add(hyperLink);
                    CommentsContainer.Controls.Add(heading);


                    sb.Add(result_string);
                }

                //foreach (var array in sb.ToList())
                //{
                //    result.Rows.Add(array);
                //}

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }



    }
}