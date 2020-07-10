using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Operations
{
    public class OpActionsExecution
    {

        #region Loaders
        public static Entities.SLAExecutionLog GetSLAExecutionLogObj(Entities.TicketLists _TicketLists, string SLA_ActionComments, Entities.SLADeclarations _EligibleSLA, string SLA_Comments)
        {

            try
            {
                //if (GetData(_TicketLists))
                //{
                Entities.SLAExecutionLog sLAExecutionLog = new Entities.SLAExecutionLog
                {
                    ActionComments = SLA_ActionComments,
                    Comments = SLA_Comments,
                    ActionRequiredID = _EligibleSLA.ActionRequiredID,
                    IsBreached = true,

                    SLAID = _EligibleSLA.SLADeclarationsID,

                    SLASequenceID = _EligibleSLA.SLASequenceID,
                    SLATriggerTime = DateTime.Now,
                    SLACheckingTime = DateTime.Now,
                    Triggered = true,

                    TicketInformationID = _TicketLists._TicketInformation.Select(a => a.TicketInformationID).FirstOrDefault(),
                    TicketHistoryID = _TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).Select(b => b.TicketHistoryID).FirstOrDefault(),
                    //  SLAExecutionLogID = SLAExecutionLogID,
                    //   CreatedBy = SessionName,
                    //  CreationDate = DateTime.Now,

                    //   UpdatedBy = SessionName,
                    //  UpdateDate = DateTime.Now
                };
                return sLAExecutionLog;
                //  }
                //else
                //{
                //    Logger.Info("");
                //}
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //   Helper.Log4Net.Error(ex);
                return null;
            }
        }
        public static Entities.TicketHistory GetTicketHistoryObj(Entities.TicketLists _TicketLists, Entities.SLADeclarations _EligibleSLA, string _TickethistoryCOmments)
        {

            try
            {
                //if (GetData(_TicketLists))
                //{
                Entities.TicketHistory _TicketHistoryObj = new Entities.TicketHistory();

                _TicketHistoryObj = _TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).FirstOrDefault();

                _TicketHistoryObj.SLASequenceID = _EligibleSLA.SLADeclarationsID;
                _TicketHistoryObj.SLATriggerTime = DateTime.Now;
                _TicketHistoryObj.CreationDate = DateTime.Now;

                //_TicketHistoryObj.Comments = " SLA TRIGGERED for SLA ID : " + _EligibleSLA.SLADeclarationsID + " For " + SLATitle;
                _TicketHistoryObj.CreatedBy = "IQserviceDesk";
                _TicketHistoryObj.CreationDate = DateTime.Now;
                _TicketHistoryObj.Comments = _TickethistoryCOmments;
                _TicketHistoryObj.Activity = "SLA TRIGGERED TICKET HISTORY";
                _TicketHistoryObj.ActivityComments = "";

                return _TicketHistoryObj;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //   Helper.Log4Net.Error(ex);
                return null;
            }
        }
        public static Entities.Notification GetNotificationObj(Entities.TicketLists _TicketLists, string SLA_Email_Subject_Comment, string toAddress, string ccAdd)
        {

            try
            {
                int TicketInformationID = _TicketLists._TicketInformation.Select(a => a.TicketInformationID).FirstOrDefault();
                string EmailCategory = "Internal_Template_CS_04_INT_Ticket pending";
                Entities.Notification SLANotification = OpNotification.GenerateNotification(TicketInformationID, true, EmailCategory, SLA_Email_Subject_Comment, "EMAIL", false, false, toAddress, ccAdd, true);
                return SLANotification;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                //   Helper.Log4Net.Error(ex);
                return null;
            }
        }
        public static Entities.Notification GetNotificationObj(Entities.TicketLists _TicketLists,string EmailCategory, string SLA_Email_Subject_Comment, string toAddress, string ccAdd)
        {

            try
            {
                int TicketInformationID = _TicketLists._TicketInformation.Select(a => a.TicketInformationID).FirstOrDefault();
                Entities.Notification SLANotification = OpNotification.GenerateNotification(TicketInformationID, true, EmailCategory, SLA_Email_Subject_Comment, "EMAIL", false, false, toAddress, ccAdd, true);
                return SLANotification;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }
        }
        public static bool ProcessNotificationData(int NotificationLevel)
        {
            bool result = false;
            try
            {

                result = true;
            }
            catch (Exception ex)
            {
                //Helper.Log4Net.Error(ex);
                Logger.LogError(ex);
                result = false;
            }

            return result;
        }
        #endregion

        #region Control
        public static void TakeAction(Entities.TicketLists _ticketLIsts, Entities.SLADeclarations _EligibleSLA)
        {
            try
            {
                if (ProcessActions(_ticketLIsts, _EligibleSLA))
                {
                    Logger.Info("SLA ACTION SUCCESSFUL for TicketID : " + _ticketLIsts._TicketInformation.FirstOrDefault().TicketInformationID +
                        " and ticketHistory ID: " + _ticketLIsts._TicketHistory.FirstOrDefault().TicketHistoryID + " for SLA ID :  " + _EligibleSLA.SLADeclarationsID);
                }
                else
                {
                    Logger.LogError("SLA PROCESS ACTION FAILED ", " SLA PROCESS ACTION FAILED ");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
        public static bool ProcessActions(Entities.TicketLists _TicketLists, Entities.SLADeclarations _EligibleSLA)
        {

            try
            {
                int _TicketInformationID = _TicketLists._TicketInformation.Select(a => a.TicketInformationID).FirstOrDefault();
                int _TicketHistoryID = _TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).Select(b => b.TicketHistoryID).FirstOrDefault();
                string _TicketNumber = _TicketLists._TicketInformation.Select(a => a.TicketNumber).FirstOrDefault();
                int _applicationID = _TicketLists._TicketInformation.Select(a => a.ApplicationID).FirstOrDefault();

                // Time Calculations
                //DateTime ticketCreationTime = (DateTime)_TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).Select(b => b.CreationDate).FirstOrDefault();
                DateTime ticketCreationTime = (DateTime)_TicketLists._TicketInformation.Select(b => b.CreationDate).FirstOrDefault();

                //double TimeDifference = 60;
                //15,30,40,60,70,90,1440,10
                double TimeDifference = DAL.Helper.DateTimeHelper.CalculateHourse(ticketCreationTime.ToString());

                //Get Email for the SLA groups
                string cc = string.Empty;
                string to = string.Empty;

                Entities.ItemTypes itemTypes = OpItemTypes.GetRecordbyID(_EligibleSLA.ActionRequiredID);
                string ActionDescription = itemTypes.Description.ToUpper();
                to = itemTypes.Scenario;


                if (_EligibleSLA.Description != "P0 L3 Product Manager")
                {
                    if (to.Contains(';'))
                    {
                        cc = string.Join(";", to.Split(';').Skip(1).ToArray());
                        to = to.Split(';').ToArray()[0];
                    }
                }
                else
                {
                    cc = to;
                    to = OpApplications.GetRecordbyID(_applicationID).Owner_Email;
                }

                //Email type
                string emailtemplate = OpItemTypes.GetRecordbyID(_EligibleSLA.NotificationType).Scenario;

                //Create Execution Log
                OpSLAExecutionLog.InsertRecord(GetSLAExecutionLogObj(_TicketLists, " " + itemTypes.Description.ToUpper() + "  ", _EligibleSLA, "BREACHED TIME in MINUTES: " + Math.Round(TimeDifference).ToString()));

                //Create Ticket History
                string sla_Email_Subject_Comment = "SLA: " + itemTypes.Description.ToUpper() + " Breached for " + Math.Round(TimeDifference).ToString() + " MINUTES for Ticket:  " + _TicketNumber + "";
                OpTicketHistory.InsertRecord(GetTicketHistoryObj(_TicketLists, _EligibleSLA, sla_Email_Subject_Comment));

                //Craete Notification
                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, emailtemplate, sla_Email_Subject_Comment, to, cc));

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return false;
            }

        }
        public static string[] GetAddresses(Entities.TicketLists _TicketLists, int NotificationLevel, bool isGroup, string ROleName = "", string AddressType = "")
        {

            try
            {
                string[] Address = new string[2];

                int TicketHistoryAssignee = _TicketLists._TicketHistory.OrderByDescending(x => x.TicketHistoryID).Select(t => t.AssignedTOID).FirstOrDefault();
                int ticketApplicationID = _TicketLists._TicketInformation.Select(x => x.ApplicationID).Distinct().FirstOrDefault();
                int TicketInformationID = _TicketLists._TicketInformation.Select(x => x.TicketInformationID).Distinct().FirstOrDefault();
                Entities.Notification LastNotification = OpNotification.GetLastInternalNotificationbyTicketInformationID(TicketInformationID);



                if (AddressType == "ASSIGNEE")
                {
                    string AssigneeEmail = OpPersonInformation.GetEmailbyUserId(TicketHistoryAssignee);
                    Address[0] = AssigneeEmail;
                    Address[1] = LastNotification.ToAddress + ";" + LastNotification.CCAddress;
                }
                else if (AddressType == "GROUP")
                {
                    List<Entities.UserApplication> _UserAppROles = OpUserApplication.GetUserApplicationbyPersonnApplication(ticketApplicationID, TicketHistoryAssignee);
                    List<int> groupIDs = _UserAppROles.Select(x => x.GroupID).Distinct().ToList();
                    string GroupEmail = "";
                    foreach (var groupID in groupIDs)
                    {
                        GroupEmail += OpGroups.GetEmailbyGroupID(groupID) + ";";

                    }
                    Address[0] = GroupEmail;
                    Address[1] = LastNotification.ToAddress + ";" + LastNotification.CCAddress;
                }
                else if (AddressType == "ROLE")
                {
                    List<Entities.UserApplication> _UserAppROles = OpUserApplication.GetUserApplicationbyPersonnApplication(ticketApplicationID, TicketHistoryAssignee);
                    List<Entities.Roles> AllRoles = OpRoles.GetAll();
                    List<int> SpecificRoleID = new List<int>();
                    string RoleBasedEmail = "";
                    if (ROleName != "")
                    {
                        SpecificRoleID = AllRoles.Where(d => d.Description.ToUpper() == ROleName.ToUpper()).Select(c => c.RolesID).ToList();
                    }

                    foreach (var _RoleID in SpecificRoleID)
                    {
                        RoleBasedEmail += OpPersonInformation.GetEmailbyUserId(OpUserApplication.GetUserApplicationbyRolenApplication(ticketApplicationID, _RoleID).Select(a => a.PersonID).FirstOrDefault()) + ";";
                    }
                    Address[0] = RoleBasedEmail;
                    Address[1] = LastNotification.ToAddress + ";" + LastNotification.CCAddress;
                }

                return Address;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return null;
            }


        }
        #endregion



        #region OLD
        //public static bool ProcessActions(Entities.TicketLists _TicketLists, Entities.SLADeclarations _EligibleSLA)
        //{

        //    try
        //    {
        //        int _TicketInformationID = _TicketLists._TicketInformation.Select(a => a.TicketInformationID).FirstOrDefault();
        //        int _TicketHistoryID = _TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).Select(b => b.TicketHistoryID).FirstOrDefault();
        //        string _TicketNumber = _TicketLists._TicketInformation.Select(a => a.TicketNumber).FirstOrDefault();

        //        #region TimeCalculations
        //        DateTime currentTime = DateTime.Now;
        //        DateTime ticketCreationTime = (DateTime)_TicketLists._TicketHistory.OrderByDescending(a => a.TicketHistoryID).Select(b => b.CreationDate).FirstOrDefault();
        //        TimeSpan span = currentTime.Subtract(ticketCreationTime);
        //        double TimeDifference = span.TotalMinutes;
        //        #endregion

        //        Entities.ItemTypes itemTypes = OpItemTypes.GetRecordbyID(_EligibleSLA.ActionRequiredID);
        //        string ActionDescription = itemTypes.Description.ToUpper();

        //        OpSLAExecutionLog.InsertRecord(GetSLAExecutionLogObj(_TicketLists, " " + itemTypes.Description.ToUpper() + "  ", _EligibleSLA, " BREACHED TIME in MINUTES: " + Math.Round(TimeDifference).ToString()));
        //        string sla_Email_Subject_Comment = "  SLA: " + itemTypes.Description.ToUpper() + " Breached for " + Math.Round(TimeDifference).ToString() + " MINUTES for Ticket:  " + _TicketNumber + "";
        //        OpTicketHistory.InsertRecord(GetTicketHistoryObj(_TicketLists, _EligibleSLA, sla_Email_Subject_Comment));


        //        if (itemTypes.GroupAction == false)
        //        {


        //            if (ActionDescription == "L1 SUPERVISOR")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 1, false, "", "ASSIGNEE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));
        //            }

        //            else
        //           if (ActionDescription == "Director Technical Support")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 2, false, "", "ASSIGNEE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //           if (ActionDescription == "L2 Supervisor")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 3, false, "", "GROUP");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //           if (ActionDescription == "IT Product Development")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 4, false, "", "GROUP");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //           if (ActionDescription == "L3 Product Manager")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 5, false, "DEVELOPER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //           if (ActionDescription == "Director, IT Product Development")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 6, false, "DEVELOPER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //           if (ActionDescription == "Excellence and Governance Manager,BT")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 7, false, "DEVELOPMENT MANAGER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "VP,Payer Provider Group")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 8, false, "DEVELOPMENT MANAGER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            // else
        //            //if (ActionDescription == "NOTIFICATION TO CTO")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 9, false, "CTO", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            // else
        //            //if (ActionDescription == "REMINDER NOTIFICATION TO CTO")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 10, false, "CTO", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            // else
        //            //if (ActionDescription == "NOTIFICATION TO CEO")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 11, false, "CEO", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            // else
        //            //if (ActionDescription == "REMINDER NOTIFICATION TO CEO")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 12, false, "CEO", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            // else
        //            //if (ActionDescription == "NOTIFICATION TO DEVELOPMENT LEAD")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 13, false, "DEVELOPMENT LEAD", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            // else
        //            //if (ActionDescription == "REMINDER NOTIFICATION TO DEVELOPMENT LEAD")
        //            // {
        //            //     string[] Addresses = GetAddresses(_TicketLists, 14, false, "DEVELOPMENT LEAD", "ROLE");
        //            //     string toAdd = Addresses[0];
        //            //     string ccAdd = Addresses[1];

        //            //     OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            // }
        //            else
        //            {
        //                Operations.Logger.LogError("CRITICIAL", " SLA EXECUTOR NO MATCHING ACTION ", "NON Group .ProcessActions");
        //            }



        //        }
        //        else
        //        {
        //            if (ActionDescription == "NOTIFICATION TO ASSIGNEE")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 100, true, "", "ASSIGNEE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));


        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO ASSIGNEE")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 101, true, "", "ASSIGNEE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO GROUP OF ASSIGNEE")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 102, true, "", "GROUP");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO GROUP OF ASSIGNEE")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 103, true, "", "GROUP");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO APPLICATION DEVELOPER")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 104, true, "DEVELOPER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO APPLICATION DEVELOPER")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 105, true, "DEVELOPER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO APPLICATION DEVELOPMENT MANAGER")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 106, true, "DEVELOPMENT MANAGER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //             if (ActionDescription == "REMINDER NOTIFICATION TO APPLICATION DEVELOPMENT MANAGER")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 107, true, "DEVELOPMENT MANAGER", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO CTO")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 108, true, "CTO", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO CTO")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 109, true, "CTO", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO CEO")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 110, true, "CEO", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO CEO")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 111, true, "CEO", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "NOTIFICATION TO DEVELOPMENT LEAD")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 112, true, "DEVELOPMENT LEAD", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            if (ActionDescription == "REMINDER NOTIFICATION TO DEVELOPMENT LEAD")
        //            {
        //                string[] Addresses = GetAddresses(_TicketLists, 113, true, "DEVELOPMENT LEAD", "ROLE");
        //                string toAdd = Addresses[0];
        //                string ccAdd = Addresses[1];

        //                OpNotification.InsertRecord(GetNotificationObj(_TicketLists, sla_Email_Subject_Comment, toAdd, ccAdd));

        //            }
        //            else
        //            {
        //                Operations.Logger.LogError("CRITICIAL", " SLA EXECUTOR NO MATCHING ACTION FOR GROUP ", "GROUP ACTIVITY Actions");

        //            }

        //        }
        //        return true;
        //    }



        //    catch (Exception ex)
        //    {

        //        Logger.LogError(ex);
        //        //Helper.Log4Net.Error(ex);
        //        return false;
        //    }

        //}
        #endregion
    }
}
