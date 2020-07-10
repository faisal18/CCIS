using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Notification
{
    public partial class TicketClosure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CloseTickets();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        private void CloseTickets()
        {
            var result_msg = string.Empty;
            try
            {
                //fetch list of resolved and not-closed issues
                var list = ListofClosedTickets();

                if (list.Count > 0)
                {
                    foreach (int ticketid in list)
                    {
                        Entities.TicketInformation ticketInformation = DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(ticketid).First();
                        string TicketNumber = ticketInformation.TicketNumber;

                        if (CloseTicket(ticketid))
                        {
                            if (SendNotification(ticketid))
                            {
                                lbl_message.Text += "<br/>Ticket Number: " + TicketNumber + " status has been closed and the Notification has been sent";
                            }
                            else
                            {
                                lbl_message.Text += "<br/>Something went wrong while sending notification of Ticket Number: " + TicketNumber;
                            }
                        }
                        else
                        {
                            lbl_message.Text += "<br/>Something went wrong while changing status of Ticket Number: " + TicketNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        private List<int> ListofClosedTickets()
        {
            List<int> List = new List<int>();
            try
            {
                List = DAL.Operations.OpTicketHistory.GetResolvedIncidents().Where(x => x.CreationDate < DateTime.Now.AddDays(-5)).OrderBy(x => x.TicketInformationID).Select(x => x.TicketInformationID).ToList();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return List;
        }

        private bool CloseTicket(int TicketInformationID)
        {
            var result = false;
            try
            {
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);

                ticketHistoryMain.Comments = "Ticket Closed by the System";
                ticketHistoryMain.CreatedBy = "IQServiceDesk";
                ticketHistoryMain.CreationDate = DateTime.Now;
                ticketHistoryMain.IncidentStatusID = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("IncidentStatusClosedID"));
                ticketHistoryMain.Activity = "TICKET CLOSED";
                int result_insert = DAL.Operations.OpTicketHistory.InsertRecord(ticketHistoryMain);

                if (result_insert > 0)
                {
                    result = true;
                }


            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }

        private bool SendNotification(int TicketInformationID)
        {

            try
            {

                string EmailCategory = "Customer_Template_CS_04_Ticket closed";
                string EmailCategory_Int = "Internal_Template_CS_02_INT_Ticket update";
                string comments = "Ticket status changed to CLOSED Automatically.";
                string Toaddress = DAL.Operations.OpPersonInformation.GetPersonInformationbyPersonID(DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID).AssignedTOID).First().Email;
                SendExternalNotification(TicketInformationID,EmailCategory, comments);
                SendInternalNotification(TicketInformationID, EmailCategory_Int, comments, Toaddress);

                return true;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return false;
            }

        }

        private void SendInternalNotification(int TicketInformationId, string Category, string Comments,string ToAddress)
        {
            try
            {
                if (Category.Length > 1)
                {
                    Entities.Notification _notificationCS = WebService.SLAProcessing.GenerateNotification(TicketInformationId, false, Category, Comments, "EMAIL", false, false,ToAddress);
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void SendExternalNotification(int TicketInformationId, string Category, string Comments)
        {
            try
            {
                if (Category.Length > 1)
                {
                    Entities.Notification _notificationCS = WebService.SLAProcessing.GenerateNotification(TicketInformationId, false, Category, Comments, "EMAIL", true, false);
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
    }
}