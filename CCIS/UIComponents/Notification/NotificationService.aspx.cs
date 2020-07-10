using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCIS.UIComponents.Notification
{
    public partial class NotificationService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckNotification();
        }


        static readonly object _object = new object();

        private void CheckNotification()
        {
            try
            {
                lbl_message.Text =  GetAllPendingNotification();
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);

                lbl_message.Text += ex.Message;
            }
        }
        private static string GetAllPendingNotification()
        {
            string result = string.Empty;
            string notification_status_pending = ConfigurationManager.AppSettings.Get("Notification_Status_Pending");
            int pendingstatusid = DAL.Operations.OpStatuses.GetStatusIDbyName(notification_status_pending);

            lock (_object)
            {
                try
                {
                    List<Entities.Notification> notificationslist = DAL.Operations.OpNotification.GetAll().Where(x => x.StatusID == pendingstatusid).ToList();

                    foreach (Entities.Notification notification in notificationslist)
                    {
                        if (SendEmail2(notification))
                        {
                            if (UpdateNotification(notification))
                            {
                                result += "<br/>Email Sent && Status updated for Notification:" + notification.NotificationID;
                                DAL.Operations.Logger.Info("Email Sent && Status updated for Notification:" + notification.NotificationID);
                            }
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    DAL.Operations.Logger.LogError(ex);

                    return ex.Message;
                }
            }
        }
        private static bool UpdateNotification(Entities.Notification notification)
        {
            bool result = false;
            try
            {
                int notification_status_sent_id = int.Parse(DAL.Operations.OpStatuses.GetStatusesbyName(ConfigurationManager.AppSettings.Get("Notification_Status_Sent")).AsEnumerable().Where(x => x.Types == ConfigurationManager.AppSettings.Get("Notification_Status_Type")).Select(x => x.StatusesID).First().ToString());

                //Entities.Notification notification2 = new Entities.Notification
                //{
                //    CallerKeyID = notification.CallerKeyID,
                //    CCAddress = notification.CCAddress,
                //    Comments = notification.Comments,
                //    CreatedBy = notification.CreatedBy,
                //    CreationDate = notification.CreationDate,
                //    EmailTemplateID = notification.EmailTemplateID,
                //    NotificationTypeID = notification.NotificationTypeID,
                //    RecipientID = notification.RecipientID,
                //    isExternal = notification.isExternal,
                //    SentByID = notification.SentByID,
                //    StatusID = notification_status_sent_id,
                //    TicketInformationID = notification.TicketInformationID,
                //    ToAddress = notification.ToAddress,
                //};

                notification.StatusID = notification_status_sent_id;

                if (DAL.Operations.OpNotification.UpdateRecord(notification, notification.NotificationID) > 0)
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
        private static bool SendEmail2(Entities.Notification notification)
        {
            string mailfrom = ConfigurationManager.AppSettings.Get("SMTPMailFrom");
            string username = ConfigurationManager.AppSettings.Get("SMTPUsername");
            string password = ConfigurationManager.AppSettings.Get("SMTPPassword");
            string smtp = ConfigurationManager.AppSettings.Get("SMTPCOnnectionServer");
            string smtp_port = ConfigurationManager.AppSettings.Get("SMTPPort");

            string SendCLientEmail = ConfigurationManager.AppSettings.Get("SendNotificationtoClients");

            bool SendNotificationtoClients = bool.Parse(SendCLientEmail);

            string subject = string.Empty;
            string body = string.Empty;
            string Email_To = string.Empty;
            string Email_CC = string.Empty;
            bool result = false;

            try
            {
                
                if (notification.Subject != null &&   notification.Subject.Length > 0)
                {
                    subject = "[IQService Desk]: " + notification.Subject;
                }
                else
                {
                    string ticketnumber = DAL.Helper.ListToDataset.ToDataSet(DAL.Operations.OpTicketInformation.GetTicketInformationbyTicketInformationID(notification.TicketInformationID)).Tables[0].Rows[0]["TicketNumber"].ToString();
               
                    subject = "[IQService Desk]: " + notification.Comments + ticketnumber;
                }


                if (notification.Body != null &&   notification.Body.Length > 0)
                {
                    body = notification.Body;

                }
                else
                {
                    body = DAL.Operations.OpEmail.GetEmailById(notification.EmailTemplateID).EmailBody;
                }
                Email_To = notification.ToAddress;
                Email_CC = notification.CCAddress;

                if (SendNotificationtoClients == false && notification.isExternal == true)
                {

                }

                else
               
                {
                    if (execute_process(mailfrom, username, password, subject, body, "", Email_To, Email_CC, smtp, smtp_port))
                    {

                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        private static bool execute_process(string emailID, string username, string password, string subject, string body, string directory, string tosent_emailID, string toCC_emailID, string SMTP, string SMTPPort)
        {
            bool result = false;

            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(SMTP);

                mail.From = new MailAddress(emailID);

                if (tosent_emailID != null && tosent_emailID.Contains(';') && tosent_emailID.Length > 0)
                {
                    string[] emailtos = tosent_emailID.Split(';');
                    foreach (string emailto in emailtos)
                    {
                        if (emailto.Length > 0)
                        {
                            mail.To.Add(emailto);
                        }
                    }
                }
                else if (tosent_emailID != null && tosent_emailID.Length > 0)
                {

                    mail.To.Add(tosent_emailID);

                }
                if (!tosent_emailID.Contains("@"))
                {
                    mail.To.Add(ConfigurationManager.AppSettings.Get("SupportEmail"));
                }

                if (toCC_emailID != null &&  toCC_emailID.Contains(';') && toCC_emailID.Length > 0)
                {
                    string[] emailCCs = toCC_emailID.Split(';');
                    foreach (string emailCC in emailCCs)
                    {
                        if (emailCC.Length > 0)
                        {
                            mail.CC.Add(emailCC);
                        }
                    }
                }
                else
                {
                    if (toCC_emailID != null && toCC_emailID.Length > 0)
                    {
                        mail.CC.Add(toCC_emailID);
                    }
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