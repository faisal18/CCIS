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
    public class JiraAttachments
    {
        public string UploadAttachments(string TicketKey, int TicketinformationId, string AppData)
        {
            string result = string.Empty;
            try
            {
                result = UploadAttachmentByJiraKey(TicketKey, TicketinformationId, AppData);

                //result = UploadAttachmentByJiraKey("L3POOL-7", 3789, AppData);
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return result;
        }
        public string UploadAttachmentByJiraKey(string key, int TicketInformationId, string AppDataDir)
        {
            string result = string.Empty;

            try
            {

                List<TicketAttachment> ticketAttachments = DAL.Operations.OpTicketAttachment.GetTicketAttachmentbyTicketID(TicketInformationId);
                if (ticketAttachments.Count > 0)
                {
                    foreach (TicketAttachment attachment in ticketAttachments)
                    {
                        //attachment.filename;

                        string filename = attachment.filename;
                        byte[] Byte_File = attachment.Attachment;
                        if (Byte_File.Length > 0 && filename.Length > 0)
                        {
                            string FileDir = AppDataDir + "\\Jira\\" + filename;
                            System.IO.File.WriteAllBytes(FileDir, Byte_File);

                            string yo = DAL.Jira.Rest_API.API("issue/" + key + "/attachments", FileDir);
                            if (yo != "The remote server returned an error: (404) Not Found." && yo != "The remote server returned an error: (403) Forbidden.")
                            {
                                JArray result2 = JArray.Parse(yo);
                                if (result2[0]["filename"].ToString() == filename)
                                {
                                    result += "<br> File " + filename + " Uploaded successfully to JIRA";
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

            return result;
        }
    }
}
