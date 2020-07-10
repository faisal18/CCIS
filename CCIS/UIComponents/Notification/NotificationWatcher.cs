using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading;
using System.Web;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Enums;

namespace CCIS.UIComponents.Notification
{
    public class NotificationWatcher
    {
        private SqlTableDependency<Entities.Notification> tableDependency;

        public void WatchTable()
        {
            try
            { 
                var connectionstring = ConfigurationManager.ConnectionStrings["DefaultDBConnection"].ConnectionString;
                var tablename = "Notification";
                var mapper = new ModelToTableMapper<Entities.Notification>().AddMapping(model => model.NotificationID, "NotificationID");

                tableDependency = new SqlTableDependency<Entities.Notification>(connectionstring, tablename, null, mapper);
                tableDependency.OnChanged += TableDependency_OnChanged;
                tableDependency.OnError += TableDependency_OnError;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        public void StartWatchTable()
        {
            try
            {
                tableDependency.Start();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        public void StopWatchTable()
        {
            try
            {
                tableDependency.Stop();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            try
            {
                DAL.Operations.Logger.Info("Exception");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Entities.Notification> e)
        {
            try
            {

                switch (e.ChangeType)
                {
                    case ChangeType.Delete:
                        //SendEmail(ChangeType.Delete.ToString(), e.Entity.NotificationID);
                        break;

                    case ChangeType.Update:
                        //SendEmail(ChangeType.Update.ToString(), e.Entity.NotificationID);
                        break;

                    case ChangeType.Insert:
                        //DoInsert(e.ChangeType.ToString(), e.Entity);
                        break;
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }


        private static void DoInsert(string changetype, Entities.Notification notification)
        {

            Thread thread = Thread.CurrentThread;


            bool breached = false;
            bool updated = false;
            int count = 0;
            int statusID = notification.StatusID;
            string notification_status_pending = ConfigurationManager.AppSettings.Get("Notification_Status_Pending");
            string status_desc = DAL.Operations.OpStatuses.GetStatusById(notification.StatusID).Description;

            try
            {
                if (!breached)
                {
                    breached = true;

                    if (notification_status_pending.ToLower() == status_desc.ToLower())
                    {
                        count++;
                        updated = true;

                        if (updated)
                        {
                            if (count == 1)
                            {
                                lock (new object())
                                {
                                    if (UpdateNotification(notification))
                                    {
                                        SendEmail2(changetype, notification);
                                    } 
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private static bool UpdateNotification(Entities.Notification notification)
        {
            bool result = false;
            try
            {
                int notification_status_sent_id = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(ConfigurationManager.AppSettings.Get("Notification_Status_Sent")).AsEnumerable().Where(x => x.Types == ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());

                Entities.Notification notification2 = new Entities.Notification
                {
                    CallerKeyID = notification.CallerKeyID,
                    CCAddress = notification.CCAddress,
                    Comments = notification.Comments,
                    CreatedBy = notification.CreatedBy,
                    CreationDate = notification.CreationDate,
                    EmailTemplateID = notification.EmailTemplateID,
                    NotificationTypeID = notification.NotificationTypeID,
                    RecipientID = notification.RecipientID,
                    SentByID = notification.SentByID,
                    StatusID = notification_status_sent_id,
                    TicketInformationID = notification.TicketInformationID,
                    ToAddress = notification.ToAddress,
                };

                if (DAL.Operations.OpNotification.UpdateRecord(notification2, notification.NotificationID) > 0)
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
        private static void SendEmail2(string ChangeType, Entities.Notification notification)
        {
            string mailfrom = ConfigurationManager.AppSettings.Get("SMTPMailFrom");
            string username = ConfigurationManager.AppSettings.Get("SMTPUsername");
            string password = ConfigurationManager.AppSettings.Get("SMTPPassword");
            string smtp = ConfigurationManager.AppSettings.Get("SMTPCOnnectionServer");
            string smtp_port = ConfigurationManager.AppSettings.Get("SMTPPort");

            string subject = string.Empty;
            string body = string.Empty;
            string Email_To = string.Empty;
            string Email_CC = string.Empty;

            try
            {
                string ticketnumber = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(notification.TicketInformationID)).Tables[0].Rows[0]["TicketNumber"].ToString();
                subject = "CCIS: " + notification.Comments + ticketnumber;
                body = DAL.Operations.OpEmail.GetEmailById(notification.EmailTemplateID).EmailBody;
                Email_To = notification.ToAddress;
                Email_CC = notification.CCAddress;

                if (execute_process(mailfrom, username, password, subject, body, "", Email_To, Email_CC, smtp, smtp_port))
                {
                    DAL.Operations.Logger.Info("Email Sent");
                }

                //DAL.Operations.Logger.Info("JUST TRUE");
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }

        private static void SendEmail(string ChangeType, int notificaionID)
        {
            string mailfrom = ConfigurationManager.AppSettings.Get("SMTPMailFrom");
            string username = ConfigurationManager.AppSettings.Get("SMTPUsername");
            string password = ConfigurationManager.AppSettings.Get("SMTPPassword");
            string smtp = ConfigurationManager.AppSettings.Get("SMTPCOnnectionServer");
            string smtp_port = ConfigurationManager.AppSettings.Get("SMTPPort");

            string notification_status_type = ConfigurationManager.AppSettings.Get("Notification_Status_Type");
            string notification_status_sent = ConfigurationManager.AppSettings.Get("Notification_Status_Sent");
            string notification_status_pending = ConfigurationManager.AppSettings.Get("Notification_Status_Pending");

            string subject = string.Empty;
            string body = string.Empty;
            string Email_To = string.Empty;
            string Email_CC = string.Empty;



            try
            {
                Entities.Notification notification = DAL.Operations.OpNotification.GetRecordbyID(notificaionID);
                Entities.Statuses statuses = DAL.Operations.OpStatuses.GetStatusById(notification.StatusID);

                int notification_status_sent_id = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Sent")).AsEnumerable().Where(x => x.Types == System.Configuration.ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());

                if (statuses.Description.ToLower() == notification_status_pending)
                {
                    string ticketnumber = DAL.Helper.ListToDataset.ToDataSet<Entities.TicketInformation>(DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(notification.TicketInformationID)).Tables[0].Rows[0]["TicketNumber"].ToString();
                    subject = "Notification of " + ChangeType + " in Ticket: " + ticketnumber;
                    body = DAL.Operations.OpEmail.GetEmailById(notification.EmailTemplateID).EmailBody + notification.Comments;
                    Email_To = notification.ToAddress;
                    Email_CC = notification.CCAddress;

                    if (execute_process(mailfrom, username, password, subject, body, "", Email_To, Email_CC, smtp, smtp_port))
                    {
                        DAL.Operations.Logger.Info("Email Sent");
                    }
                    if (true)
                    {
                        DAL.Operations.Logger.Info("Email sent for notification id " + notificaionID);
                        Entities.Notification notification2 = new Entities.Notification
                        {
                            CallerKeyID = notification.CallerKeyID,
                            CCAddress = notification.CCAddress,
                            Comments = notification.CCAddress,
                            CreatedBy = notification.CreatedBy,
                            CreationDate = notification.CreationDate,
                            EmailTemplateID = notification.EmailTemplateID,
                            NotificationTypeID = notification.NotificationTypeID,
                            RecipientID = notification.RecipientID,
                            SentByID = notification.SentByID,
                            StatusID = notification_status_sent_id,
                            TicketInformationID = notification.TicketInformationID,
                            ToAddress = notification.ToAddress,
                        };

                        if (DAL.Operations.OpNotification.UpdateRecord(notification2, notification.NotificationID) > 0)
                        {
                            DAL.Operations.Logger.Info("Notification updated");
                        }
                        else
                        {
                            DAL.Operations.Logger.Info("Notification failed");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
        }
        private static bool execute_process(string emailID, string username, string password, string subject, string body, string directory, string tosent_emailID, string toCC_emailID, string SMTP, string SMTPPort)
        {
            bool result = false;

            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);

                mail.From = new MailAddress(emailID);

                if (tosent_emailID.Contains(';'))
                {
                    string[] emailtos = tosent_emailID.Split(';');
                    foreach (string emailto in emailtos)
                    {
                        mail.To.Add(emailto);
                    }
                }
                else
                {
                    mail.To.Add(tosent_emailID);
                }

                if (toCC_emailID.Contains(';'))
                {
                    string[] emailCCs = toCC_emailID.Split(';');
                    foreach (string emailCC in emailCCs)
                    {
                        mail.CC.Add(emailCC);
                    }
                }
                else
                {
                    mail.CC.Add(toCC_emailID);
                }

                mail.CC.Add(ConfigurationManager.AppSettings.Get("CC_Gorup"));
                mail.IsBodyHtml = true;
                mail.Subject = subject;
                mail.Body = body;

                if (directory.Length > 0)
                {
                    string[] files = Directory.GetFiles(directory);

                    for (int i = 0; i < files.Length; i++)
                    {
                        mail.Attachments.Add(new Attachment(files[i]));
                    }
                }

                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Host = SMTP;

                SmtpServer.Port = Convert.ToInt32(SMTPPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                mail.Attachments.Dispose();
                result = true;



            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                result = false;
            }

            return result;
        }


    }

}