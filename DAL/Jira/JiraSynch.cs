using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Entities;
using Newtonsoft.Json.Linq;


namespace DAL.Jira
{
    public class JiraSynch
    {

        #region Sync Methods
        public string Synch_Comments()
        {
            string result = string.Empty;
            try
            {
                JiraComments OBJ = new JiraComments();
                result = OBJ.SynchComments();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        public string Synch_Comments(string key)
        {
            string result = string.Empty;
            try
            {
                JiraComments OBJ = new JiraComments();
                result = OBJ.SynchCommentsbyTicket(key);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        public string Synch_Status()
        {
            string result = string.Empty;
            try
            {
                JiraStatuses OBJ = new JiraStatuses();
                result = OBJ.Synch_Status();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }
        public string Synch_Assignee()
        {
            string result = string.Empty;
            try
            {
                JiraAssignee OBJ = new JiraAssignee();
                result = OBJ.Synch_Assignee();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        } 
        #endregion

        #region HelperFunctions
        public bool Jira_CheckTicketExist(string TicketKey)
        {
            bool exist = false;

            try
            {
                if (TicketKey.Length > 0)
                {
                    string result = DAL.Jira.Rest_API.API("issue/" + TicketKey, "", "GET");
                    JObject result2 = JObject.Parse(result);
                    if (result2["errorMessages"] == null)
                    {
                        if (result2["key"].ToString() == TicketKey)
                        {
                            exist = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return exist;
        }
        public int GetTicketinformationIDByJiraKey(string JiraTicket)
        {
            int id = -1;
            try
            {
                var result = DAL.Operations.OpTicketHistory.GetAll().Where(x => x.JiraNumber == JiraTicket).Select(x => x.TicketInformationID).First().ToString();
                id = int.Parse(result);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return id;
        }
        public List<string> FethcListofJiraTickets()
        {
            List<string> JiraTickets = new List<string>();
            try
            {
                //JiraTickets = DAL.Operations.OpTicketHistory.GetAll().Where(x => x.JiraNumber != null && x.IncidentStatusID != 14).OrderBy(x => x.JiraNumber).Select(x => x.JiraNumber).Distinct().ToList();
                JiraTickets = DAL.Operations.OpTicketHistory.GetOpenIncidents().Where(x => x.JiraNumber != null).OrderBy(x => x.JiraNumber).Select(x => x.JiraNumber).Distinct().ToList();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return JiraTickets;
        }
        public string Jira_EscapeCharacters(string Desc, bool toRemove)
        {
            try
            {
                if (!toRemove)
                {
                    Desc = Desc.Replace("\"", "\\\" "); //double quot
                    Desc = Desc.Replace("\r", "\\r "); // tab
                    Desc = Desc.Replace("\n", "\\n "); // newline
                    Desc = Desc.Replace("\b", "\\b "); // bold
                    Desc = Desc.Replace("\f", "\\f "); // form feed
                    Desc = Desc.Replace("\t", "\\t "); // Tab
                    Desc = Desc.Replace("\v", "\\v "); // vertical Tab
                    Desc = Desc.Replace("\'", "\\' "); // Apostrophie
                    Desc = Desc.Replace("\\", "\\"); // Backslash
                    Desc = Desc.Replace("•", " ");
                }
                else if (toRemove)
                {
                    Desc = Desc.Replace("\"", " "); //double quot
                    Desc = Desc.Replace("\r", " "); // tab
                    Desc = Desc.Replace("\n", " "); // newline
                    Desc = Desc.Replace("\b", " "); // bold
                    Desc = Desc.Replace("\f", " "); // form feed
                    Desc = Desc.Replace("\t", " "); // Tab
                    Desc = Desc.Replace("\v", " "); // vertical Tab
                    Desc = Desc.Replace("\'", " "); // Apostrophie
                    Desc = Desc.Replace("\\", " "); // Backslash
                    Desc = Desc.Replace("•", " ");
                }
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
            }

            return Desc;
        }
        #endregion

        #region OperationalFunctions
        public void CreateTicketHistory(int TicketInformationID, string comments, string Activity)
        {
            try
            {
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                ticketHistoryMain.ActivityComments = comments;
                ticketHistoryMain.CreatedBy = "IqServiceDesk";
                ticketHistoryMain.Activity = Activity;
                ticketHistoryMain.CreationDate = DateTime.Now;
                DAL.Operations.OpTicketHistory.InsertRecord(ticketHistoryMain);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        public void CreateTicketHistory(int TicketInformationID, string comments, string Activity, string Author)
        {
            try
            {
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                ticketHistoryMain.Comments = comments;
                ticketHistoryMain.CreatedBy = "JIRA - " + Author;
                ticketHistoryMain.Activity = Activity;
                ticketHistoryMain.CreationDate = DateTime.Now;
                DAL.Operations.OpTicketHistory.InsertRecord(ticketHistoryMain);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        public void CreateTicketHistory(int TicketInformationID, string comments, bool isclosed, string Activity)
        {
            try
            {
                Entities.TicketHistory ticketHistoryMain = DAL.Operations.OpTicketHistory.GetLatestTicketHistoryByTicketID(TicketInformationID);
                int subStatusId = DAL.Operations.OpStatuses.GetStatusbyNameandType("Sent to TechSupport/L2", "Sub Incident Status").StatusesID;

                ticketHistoryMain.ActivityComments = comments;
                ticketHistoryMain.CreatedBy = "IQServiceDesk";
                ticketHistoryMain.Activity = Activity;
                ticketHistoryMain.CreationDate = DateTime.Now;
                ticketHistoryMain.IncidentSubStatusID = subStatusId;
                DAL.Operations.OpTicketHistory.InsertRecord(ticketHistoryMain);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        public void CreateNotification(int TicketInformationID, string comments)
        {
            try
            {
                string CC_Address = System.Configuration.ConfigurationManager.AppSettings.Get("SupportEmail");
                CreateInternalNotification(TicketInformationID, comments, CC_Address, "Internal_Template_CS_02_INT_Ticket update");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void CreateInternalNotification(int TicketInformationID, string _comments, string ToEmailAddress, string EmailCategory)
        {
            try
            {
                string comments = _comments;
                string To_Address = ToEmailAddress;
                string CC_Address = "";

                Entities.Notification _notification1 = DAL.Operations.OpNotification.GenerateNotification(TicketInformationID, false, EmailCategory, comments, "EMAIL", false, false, To_Address, CC_Address);

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        #endregion

    }
}
