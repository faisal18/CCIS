using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using DAL.Operations;

namespace DAL.Helper
{
  public  class EmailHelper
    {


        public static void SendTestEmail()
        {
            try
            {
                string SMTPCOnnectionServer = System.Configuration.ConfigurationManager.AppSettings["SMTPCOnnectionServer"];
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
                string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                string SMTPMailFrom = System.Configuration.ConfigurationManager.AppSettings["SMTPMailFrom"];
                string SMTPMailTo = System.Configuration.ConfigurationManager.AppSettings["SMTPMailTo"];
                string SMTPSendEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSendEmail"];


                int sendemailactive = int.Parse(SMTPSendEmail);

                //  bool emailSendingActive = Convert.ToBoolean(SMTPSendEmail);
                bool emailSendingActive = Convert.ToBoolean(sendemailactive);


                if (emailSendingActive == true)
                {
                    MailMessage mail = new MailMessage(SMTPMailFrom, SMTPMailTo);
                    SmtpClient client = new SmtpClient();
                    client.Port = int.Parse(SMTPPort);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = SMTPCOnnectionServer;
                    client.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                    client.EnableSsl = true;
                    mail.Subject = "Techsupport_TEST EMAIL";
                    mail.Body = "Test Email Body!!! ";
                    client.Send(mail);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
               DAL.Operations.Logger.LogError(ex);
                throw;
            }

        }


        public static bool SendEmail(string Subject, string Message, string toAddress)
        {
            try
            {
                string SMTPCOnnectionServer = System.Configuration.ConfigurationManager.AppSettings["SMTPCOnnectionServer"];
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
                string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                string SMTPMailFrom = System.Configuration.ConfigurationManager.AppSettings["SMTPMailFrom"];
                string SMTPMailTo = System.Configuration.ConfigurationManager.AppSettings["SMTPMailTo"];
                string SMTPSendEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSendEmail"];

                int sendemailactive = int.Parse(SMTPSendEmail);

                //  bool emailSendingActive = Convert.ToBoolean(SMTPSendEmail);
                bool emailSendingActive = Convert.ToBoolean(sendemailactive);


                if (emailSendingActive == true)
                {
                    MailMessage mail = new MailMessage(SMTPMailFrom, toAddress);
                    SmtpClient client = new SmtpClient();
                    client.Port = int.Parse(SMTPPort);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = SMTPCOnnectionServer;
                    client.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                    client.EnableSsl = true;
                    mail.Subject = Subject;
                    //mail.Subject = "Techsupport_" + Subject;
                    mail.Body = Message;
                    client.Send(mail);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return false;
            }

        }

        public static bool SendEmailtoSpecific(string Subject, string Message)
        {
            try
            {
                string SMTPCOnnectionServer = System.Configuration.ConfigurationManager.AppSettings["SMTPCOnnectionServer"];
                string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
                string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                string SMTPMailFrom = System.Configuration.ConfigurationManager.AppSettings["SMTPMailFrom"];
                string SMTPMailTo = System.Configuration.ConfigurationManager.AppSettings["SMTPMailTo"];
                string SMTPSendEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSendEmail"];

                int sendemailactive = int.Parse(SMTPSendEmail);

                //  bool emailSendingActive = Convert.ToBoolean(SMTPSendEmail);
                bool emailSendingActive = Convert.ToBoolean(sendemailactive);

                if (emailSendingActive == true)
                {
                    MailMessage mail = new MailMessage(SMTPMailFrom, SMTPMailTo);
                    SmtpClient client = new SmtpClient();
                    client.Port = int.Parse(SMTPPort);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = SMTPCOnnectionServer;
                    client.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                    client.EnableSsl = true;
                    //mail.Subject = "Techsupport_" + Subject;
                    mail.Subject = Subject;
                    mail.Body = Message;
                    client.Send(mail);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return false;
            }
        }


        public static bool SendEmailwithAttachment(string Subject, string Message, string toAddress, string attachmentPath)
        {
            try
            {


                //  string compressedfilepath =   Helpers.CompressionHelper.CompressZip(attachmentPath);

                System.IO.FileInfo f = new System.IO.FileInfo(attachmentPath);

                if (f.Length < 12000000)
                {
                    string SMTPCOnnectionServer = System.Configuration.ConfigurationManager.AppSettings["SMTPCOnnectionServer"];
                    string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
                    string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
                    string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
                    string SMTPMailFrom = System.Configuration.ConfigurationManager.AppSettings["SMTPMailFrom"];
                    string SMTPMailTo = System.Configuration.ConfigurationManager.AppSettings["SMTPMailTo"];
                    string SMTPSendEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSendEmail"];
                    string DimensionEmail = System.Configuration.ConfigurationManager.AppSettings["EmailDomain"];

                    int sendemailactive = int.Parse(SMTPSendEmail);

                    //  bool emailSendingActive = Convert.ToBoolean(SMTPSendEmail);
                    bool emailSendingActive = Convert.ToBoolean(sendemailactive);


                    if (emailSendingActive == true)
                    {
                        MailMessage mail = new MailMessage(SMTPMailFrom, toAddress);
                        SmtpClient client = new SmtpClient();
                        client.Port = int.Parse(SMTPPort);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = SMTPCOnnectionServer;
                        client.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                        client.EnableSsl = true;
                        //mail.Subject = "Techsupport_" +  Subject;
                        mail.Subject = Subject;
                        mail.Body = Message;
                        Attachment emailAttachment = new Attachment(attachmentPath);
                        mail.Attachments.Add(emailAttachment);
                        client.Send(mail);
                        emailAttachment.Dispose();
                        client.Dispose();
                        mail.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    SendEmail(Subject, Message + Environment.NewLine + "!!! File Size is greater than 8 MB !!!!!", "techsupport@dimensions-healthcare.com");
                    DAL.Operations.Logger.LogError("","File Size is greater than 8 MB !!!!! ", "SendEmailwithAttachment");
                    return false;
                }
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return false;
            }
            finally
            {
                //  GC.Collect();
            }
        }

        public static bool SendEmailwithAttachment_MultipleAddresses(string to_Address, string cc_Address, string Subject, string Message, string attachmentPath)
        {
            bool result = false;
            string SMTPCOnnectionServer = System.Configuration.ConfigurationManager.AppSettings["SMTPCOnnectionServer"];
            string SMTPPort = System.Configuration.ConfigurationManager.AppSettings["SMTPPort"];
            string SMTPUsername = System.Configuration.ConfigurationManager.AppSettings["SMTPUsername"];
            string SMTPPassword = System.Configuration.ConfigurationManager.AppSettings["SMTPPassword"];
            string SMTPMailFrom = System.Configuration.ConfigurationManager.AppSettings["SMTPMailFrom"];
            string SMTPMailTo = System.Configuration.ConfigurationManager.AppSettings["SMTPMailTo"];
            string SMTPSendEmail = System.Configuration.ConfigurationManager.AppSettings["SMTPSendEmail"];
            string DimensionEmail = System.Configuration.ConfigurationManager.AppSettings["EmailDomain"];

            int sendemailactive = int.Parse(SMTPSendEmail);
            bool emailSendingActive = Convert.ToBoolean(sendemailactive);

            try
            {
                Logger.Info("SendEmailwithAttachment_MultipleAddresses has been called");
                if (emailSendingActive == true)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(attachmentPath);

                    if (f.Length < 12000000)
                    {

                        MailMessage mail = new MailMessage();
                        SmtpClient client = new SmtpClient();

                        client.Port = int.Parse(SMTPPort);
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Host = SMTPCOnnectionServer;
                        client.Credentials = new System.Net.NetworkCredential(SMTPUsername, SMTPPassword);
                        client.EnableSsl = true;

                        Logger.Info("Sending email to " + to_Address);
                        if (cc_Address != null && cc_Address != "")
                        {
                            Logger.Info("CC-ed are " + cc_Address);
                            mail.CC.Add(cc_Address);
                        }
                        Logger.Info("Attaching file " + attachmentPath);

                        mail.From = new MailAddress(SMTPMailFrom);
                        mail.To.Add(to_Address);
                        mail.Subject = Subject;
                        mail.Body = Message;
                        Attachment emailAttachment = new Attachment(attachmentPath);
                        mail.Attachments.Add(emailAttachment);

                        client.Send(mail);
                        emailAttachment.Dispose();
                        client.Dispose();
                        mail.Dispose();
                        result = true;
                    }
                    else
                    {
                        Logger.Info("File size exceeds the limit of 12000000 bytes");
                    }
                }
                else
                {
                    Logger.Info("emailSendingActive is false");
                }

                return result;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return false;
            }
        }

        public static string getUsernamefromEmail(string userEmail)
        {

            try
            {

                string[] getUsername = userEmail.Split('@');
                string useremail = "";
                if (getUsername.Length > 0)
                {
                    useremail = getUsername[0];

                }

                return useremail;

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
                        
            }
        }
            public static string ProcessEmailText(string _Text)
        {

            try
            {

                string EmailVariables = System.Configuration.ConfigurationManager.AppSettings.Get("EmailVariables");

                string[] emailKeyWords = _Text.Split(new[] { "||" },StringSplitOptions.RemoveEmptyEntries);





                string useremail = "";
                if (emailKeyWords.Length > 0)
                {
                    foreach (string keyword in emailKeyWords)
                    {


                    }

                }

                return useremail;

            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;

            }
        }

        





    }



}
