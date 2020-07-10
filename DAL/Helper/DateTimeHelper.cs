using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Helper
{
    public class DateTimeHelper
    {
        //obselete
        public static string getTimeElapsed(DateTime _FirstDate, DateTime _SecondDate, bool needString = true)
        {
            try
            {
                string dateString = "";
                string IncludeNonWorkingHours = System.Configuration.ConfigurationManager.AppSettings.Get("IncludeNonWorkingHours"); //60" />

                bool _includeNonWorkingHours = bool.Parse(IncludeNonWorkingHours);

                if (_includeNonWorkingHours)
                {



                    TimeSpan span = (_SecondDate - _FirstDate);


                    if (needString)
                    {
                        dateString = String.Format("{0} days {1} hours {2} minutes ",
                            span.Days, span.Hours, span.Minutes);
                        //dateString =  String.Format("{0} days {1} hours {2} minutes {3} seconds",
                        //  span.Days, span.Hours, span.Minutes, span.Seconds);
                    }
                    else
                    {
                        double mins = span.TotalMinutes;
                        dateString = mins.ToString();
                    }

                }
                else
                {
                    TimeSpan _timespan = TimeSpan.FromMinutes(getTimeElapsedforWorkingHoursinMinutes(_FirstDate, _SecondDate));


                    if (needString)
                    {
                        dateString = String.Format("{0} days {1} hours {2} minutes ",
                          _timespan.Days, _timespan.Hours, _timespan.Minutes);
                    }
                    else
                    {
                        double mins = _timespan.TotalMinutes;
                        dateString = mins.ToString();

                    }
                }
                return dateString;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        public static double getTimeElapsedforWorkingHoursinMinutes(DateTime _FirstDate, DateTime _SecondDate)
        {
            try
            {
                int count = 0;

                for (var i = _FirstDate; i < _SecondDate; i = i.AddHours(1))
                {
                    if (i.DayOfWeek != DayOfWeek.Friday && i.DayOfWeek != DayOfWeek.Saturday)
                    {
                        if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours < 17)
                        {
                            count++;
                        }
                    }
                }


                double totalMinutes = count * 60;
                return totalMinutes;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return -1;
            }
        }

        //no change required
        public static string caclulateSLA_Date(List<Entities.TicketHistory> tickethistory_all, string whichdate)
        {
            string result = string.Empty;
            DateTime closing = DateTime.Now, RKB = DateTime.Now, RWKB = DateTime.Now, WTF = DateTime.Now;
            bool isclosed = false, yougot = false;
            try
            {

                int lateststatus = tickethistory_all.Last().IncidentStatusID;
                DateTime creationdate = tickethistory_all.First().CreationDate.Value;

                if (lateststatus == 14 || lateststatus == 39 || lateststatus == 12)
                {
                    isclosed = true;
                }

                if (isclosed)
                {
                    //check if closed
                    if (whichdate == "close")
                    {
                        if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Closed" && x.Comments != "Ticket Closed by the System").LastOrDefault() != null)
                        {
                            closing = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Closed" && x.Comments != "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                            WTF = closing;
                            yougot = true;
                        }
                        if (!yougot)
                        {
                            if (tickethistory_all.Where(x => x.ActivityComments == "TICKET CLOSED").LastOrDefault() != null)
                            {
                                closing = tickethistory_all.Where(x => x.ActivityComments == "TICKET CLOSED").LastOrDefault().CreationDate.Value;
                                WTF = closing;
                                yougot = true;
                            }
                        }
                        if (!yougot)
                        {
                            if (tickethistory_all.Where(x => x.Comments == "Ticket Closed by the System").LastOrDefault() != null)
                            {
                                closing = tickethistory_all.Where(x => x.Comments == "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                                WTF = closing;
                                yougot = true;
                            }
                        }

                        if (!yougot)
                        {
                            if (tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault() != null)
                            {
                                closing = tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault().CreationDate.Value;
                                WTF = closing;
                                yougot = true;
                            }
                        }

                        result = WTF.ToString();

                    }

                    else if (whichdate == "resolve")
                    {
                        //check if resolved with KB
                        if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved with KB" && x.Comments != "Ticket Closed by the System").LastOrDefault() != null)
                        {
                            RKB = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved with KB" && x.Comments != "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                            yougot = true;
                        }
                        //check if resolved without KB
                        if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved without KB" && x.Comments != "Ticket Closed by the System").LastOrDefault() != null)
                        {
                            RWKB = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved without KB" && x.Comments != "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                            yougot = true;
                        }
                        if (!yougot)
                        {
                            if (tickethistory_all.Where(x => x.IncidentStatusID == 39).LastOrDefault() != null)
                            {
                                RKB = tickethistory_all.Where(x => x.IncidentStatusID == 39).LastOrDefault().CreationDate.Value;
                            }

                            if (tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault() != null)
                            {
                                RWKB = tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault().CreationDate.Value;
                            }
                        }


                        if (RKB < RWKB)
                        {
                            WTF = RKB;
                        }
                        else if (RWKB < RKB)
                        {
                            WTF = RWKB;
                        }




                        result = WTF.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }
            return result;
        }

        public static Result_SLAOLA Controller_SLAOLA(List<Entities.TicketHistory> ticketHistories,string ApplicationName,string Priority,string _CurrentAssignedGroup)
        {
            Result_SLAOLA obj = new Result_SLAOLA();
            bool isworkingdays = false;
            string sla_days = string.Empty, ola_days = string.Empty;

            try
            {
                if (ApplicationName.Contains("NUPCO"))
                {
                    isworkingdays = false;
                }
                else
                {
                    isworkingdays = true;
                }


                double sla = calculateSLA(ticketHistories, isworkingdays);
                double sla_2 = SLACalculations(ApplicationName, sla, Priority, _CurrentAssignedGroup, "SLA");

                if (sla_2 < 0)
                {
                    sla_days = getTimeElapsedstringfromMinutes(sla_2, isworkingdays);
                }

                double ola_2 = SLACalculations(ApplicationName, sla, Priority, _CurrentAssignedGroup, "OLA");
                if (ola_2 < 0)
                {
                    ola_days = getTimeElapsedstringfromMinutes(ola_2, isworkingdays);
                }

                obj.SLATime = sla_2;
                obj.DaysOverSLA = sla_days;
                obj.OLATime = ola_2;
                obj.DaysOverOLA = ola_days;




            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return obj;
        }


        public static double calculateSLA(List<Entities.TicketHistory> tickethistory_all,bool workinghours)
        {
            double sla = 0;
            DateTime closing = DateTime.Now, RKB = DateTime.Now, RWKB = DateTime.Now, WTF = DateTime.Now;
            bool isclosed = false;
            try
            {

                int lateststatus = tickethistory_all.Last().IncidentStatusID;
                DateTime creationdate = tickethistory_all.First().CreationDate.Value;

                if (lateststatus == 14 || lateststatus == 39 || lateststatus == 12)
                {
                    isclosed = true;
                }

                if (isclosed)
                {
                    bool yougot = false;
                    //check if closed
                    if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Closed").LastOrDefault() != null)
                    {
                        closing = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Closed").LastOrDefault().CreationDate.Value;
                        yougot = true;
                    }
                    //check if resolved with KB
                    if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved with KB" && x.Comments != "Ticket Closed by the System").LastOrDefault() != null)
                    {
                        RKB = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved with KB" && x.Comments != "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                        yougot = true;

                    }
                    //check if resolved without KB
                    if (tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved without KB" && x.Comments != "Ticket Closed by the System").LastOrDefault() != null)
                    {
                        RWKB = tickethistory_all.Where(x => x.ActivityComments == "Ticket status has been changed to Resolved without KB" && x.Comments != "Ticket Closed by the System").LastOrDefault().CreationDate.Value;
                        yougot = true;

                    }


                    if (!yougot)
                    {
                        if (tickethistory_all.Where(x => x.IncidentStatusID == 39).LastOrDefault() != null)
                        {
                            RKB = tickethistory_all.Where(x => x.IncidentStatusID == 39).LastOrDefault().CreationDate.Value;
                        }
                        if (tickethistory_all.Where(x => x.ActivityComments == "TICKET CLOSED").LastOrDefault() != null)
                        {
                            closing = tickethistory_all.Where(x => x.ActivityComments == "TICKET CLOSED").LastOrDefault().CreationDate.Value;
                        }
                        else if(tickethistory_all.Where(x=>x.IncidentStatusID == 14).LastOrDefault()!= null)
                        {
                            closing = tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault().CreationDate.Value;
                        }
                        if (tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault() != null)
                        {
                            RWKB = tickethistory_all.Where(x => x.IncidentStatusID == 14).LastOrDefault().CreationDate.Value;
                        }


                        
                    }

                    if (closing < RKB && closing < RWKB)
                    {
                        WTF = closing;
                    }
                    else if (RKB < closing && RKB < RWKB)
                    {
                        WTF = RKB;
                    }
                    else if (RWKB < closing && RWKB < RKB)
                    {
                        WTF = RWKB;
                    }




                }
                else
                {
                    WTF = DateTime.Now;
                }

                sla = DAL.Helper.DateTimeHelper.CalculateHourse(creationdate, WTF, workinghours);


            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return sla;
        }
        public static double CalculateHourse(DateTime start_date, DateTime end_date,bool workinghours)
        {
            double count = 0;

            try
            {

                DateTime start = start_date;
                DateTime end = end_date;

                if (workinghours)
                {
                    List<DateTime> holidays = GetPublicHolidays("UAE");
                    for (var i = start; i < end; i = i.AddHours(1))
                    {
                        if (i.DayOfWeek != DayOfWeek.Friday && i.DayOfWeek != DayOfWeek.Saturday)
                        {
                            if (!holidays.Any(x => x.Day == i.Day && x.Month == i.Month && x.Year == i.Year))
                            {
                                if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours <= 17)
                                {
                                    //this is in hours
                                    count++;
                                }
                            }
                        }
                    }
                    TimeSpan value = TimeSpan.FromHours(count);
                    count = value.TotalMinutes;
                }
                else if(!workinghours)
                {
                    // this is in minutes
                    TimeSpan sp1 =  end_date - start_date;
                    count = sp1.TotalMinutes;
                }

            }
            catch (Exception ex)
            {
                DAL.Helper.Log4Net.Error(ex);
            }
            return count;
        }
        public static double SLACalculations(string ApplicationName, double _olaMinutes, string Priority, string _CurrentAssignedGroup, string _Type)
        {

            try
            {
                double result = 0; double _PreferredSlaTime = 0;
                Dictionary<string, string> SLaTimes = DAL.Helper.DateTimeHelper.getSLATime();

                string getType = _Type + "_";
                if (_CurrentAssignedGroup != null)
                {
                    if (_Type == "OLA")
                    {
                        if (_CurrentAssignedGroup.Contains("L1") == true || _CurrentAssignedGroup.Contains("L2") == true || _CurrentAssignedGroup.Contains("L3") == true)
                        {
                            if (_olaMinutes != -1 && _CurrentAssignedGroup.ToUpper().Contains("L2") && _Type == "OLA")
                            {
                                getType = getType + "L2_";
                            }
                            else if (_olaMinutes != -1 && _CurrentAssignedGroup.ToUpper().Contains("L1") && _Type == "OLA")
                            {
                                getType = getType + "L1_";
                            }
                            else if (_olaMinutes != -1 && _CurrentAssignedGroup.ToUpper().Contains("L3") && _Type == "OLA")
                            {
                                getType = getType + "L3_";
                            }

                            _PreferredSlaTime = double.Parse(SLaTimes[getType + Priority]);
                        }
                    }
                    else if (_Type == "SLA")
                    {

                        int itemtypeID = DAL.Operations.OpItemTypes.GetItemTypesbyCategoryName("Priority").Where(x => x.Description == Priority).First().ItemTypesID;

                        if (ApplicationName.Contains("NUPCO"))
                        {

                            _PreferredSlaTime = DAL.Operations.OpSLADeclarations.GetActiveSLAs().Where(x => x.PriorityID == itemtypeID && x.Description == "NUPCO").First().TimeinMinutes;
                        }
                        else
                        {
                            _PreferredSlaTime = DAL.Operations.OpSLADeclarations.GetActiveSLAs().Where(x => x.PriorityID == itemtypeID && x.Description != "NUPCO").First().TimeinMinutes;
                        }
                    }

                    result = (_PreferredSlaTime - _olaMinutes);
                    return result;
                }
                else
                {
                    return result;
                }

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return -1;
            }

        }
        public static string getTimeElapsedstringfromMinutes(double _Minutes,bool workinghours)
        {
            try
            {

                string dateString = "";

                if (workinghours)
                {

                    int newminutest = (int)System.Math.Abs(_Minutes);
                    double working_days = (double)Decimal.Divide(newminutest, 540);
                    working_days = System.Math.Round(System.Math.Abs(working_days), 2);

                    if (working_days.ToString().Contains('.'))
                    {

                        string[] datearray = working_days.ToString().Split('.');
                        char[] char_Array = datearray[1].ToCharArray();
                        if (char_Array.Length > 1)
                        {
                            TimeSpan _timespan = new TimeSpan(int.Parse(datearray[0]), int.Parse(char_Array[0].ToString()), int.Parse(char_Array[1].ToString()), 0);
                            dateString = String.Format("{0} days {1} hours {2} minutes ", _timespan.Days, _timespan.Hours, _timespan.Minutes);
                        }
                        else
                        {
                            TimeSpan _timespan = new TimeSpan(int.Parse(datearray[0]), int.Parse(char_Array[0].ToString()), 0, 0);
                            dateString = String.Format("{0} days {1} hours {2} minutes ", _timespan.Days, _timespan.Hours, _timespan.Minutes);
                        }
                    }
                    else
                    {
                        TimeSpan timeSpan = new TimeSpan(int.Parse(working_days.ToString()), 0, 0, 0, 0);
                        dateString = String.Format("{0} days {1} hours {2} minutes ", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
                    }
                }
                else
                {
                    TimeSpan tS1 = TimeSpan.FromMinutes(System.Math.Abs(_Minutes));
                    dateString = String.Format("{0} days {1} hours {2} minutes ", tS1.Days, tS1.Hours, tS1.Minutes);
                }

                return dateString;
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        public static Dictionary<string, string> getSLATime()
        {
            try
            {

                string SLA_P0 = System.Configuration.ConfigurationManager.AppSettings.Get("SLA_P0"); //60" />
                string SLA_P1 = System.Configuration.ConfigurationManager.AppSettings.Get("SLA_P1"); //300" />
                string SLA_P2 = System.Configuration.ConfigurationManager.AppSettings.Get("SLA_P2"); //2880" />
                string SLA_P3 = System.Configuration.ConfigurationManager.AppSettings.Get("SLA_P3"); //7200" />
                string SLA_P4 = System.Configuration.ConfigurationManager.AppSettings.Get("SLA_P4"); //10080" />
                string OLA_L1_P0 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L1_P0"); //5" />
                string OLA_L1_P1 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L1_P1"); //30" />
                string OLA_L1_P2 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L1_P2"); //120" />
                string OLA_L1_P3 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L1_P3"); //180" />
                string OLA_L1_P4 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L1_P4"); //360" />
                string OLA_L2_P0 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L2_P0"); //25" />
                string OLA_L2_P1 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L2_P1"); //90" />
                string OLA_L2_P2 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L2_P2"); //240" />
                string OLA_L2_P3 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L2_P3"); //360" />
                string OLA_L2_P4 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L2_P4"); //480" />
                string OLA_L3_P0 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L3_P0"); //25" />
                string OLA_L3_P1 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L3_P1"); //150" />
                string OLA_L3_P2 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L3_P2"); //480" />
                string OLA_L3_P3 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L3_P3"); //1440" />
                string OLA_L3_P4 = System.Configuration.ConfigurationManager.AppSettings.Get("OLA_L3_P4"); //1800" />




                Dictionary<string, string> OLATimes = new Dictionary<string, string>();


                OLATimes.Add("SLA_P0", SLA_P0);
                OLATimes.Add("SLA_P1", SLA_P1);
                OLATimes.Add("SLA_P2", SLA_P2);
                OLATimes.Add("SLA_P3", SLA_P3);
                OLATimes.Add("SLA_P4", SLA_P4);
                OLATimes.Add("OLA_L1_P0", OLA_L1_P0);
                OLATimes.Add("OLA_L1_P1", OLA_L1_P1);
                OLATimes.Add("OLA_L1_P2", OLA_L1_P2);
                OLATimes.Add("OLA_L1_P3", OLA_L1_P3);
                OLATimes.Add("OLA_L1_P4", OLA_L1_P4);
                OLATimes.Add("OLA_L2_P0", OLA_L2_P0);
                OLATimes.Add("OLA_L2_P1", OLA_L2_P1);
                OLATimes.Add("OLA_L2_P2", OLA_L2_P2);
                OLATimes.Add("OLA_L2_P3", OLA_L2_P3);
                OLATimes.Add("OLA_L2_P4", OLA_L2_P4);
                OLATimes.Add("OLA_L3_P0", OLA_L3_P0);
                OLATimes.Add("OLA_L3_P1", OLA_L3_P1);
                OLATimes.Add("OLA_L3_P2", OLA_L3_P2);
                OLATimes.Add("OLA_L3_P3", OLA_L3_P3);
                OLATimes.Add("OLA_L3_P4", OLA_L3_P4);


                return OLATimes;

            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        
        public static double CalculateHourse(string date)
        {
            double count = 0;

            try
            {
                date = Convert.ToDateTime(date).ToString("dd/MM/yyyy HH:mm");
                DateTime start = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None);
                DateTime end = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None);

                double elapsed = end.Subtract(start).TotalMinutes;

                List<DateTime> holidays = GetPublicHolidays("UAE");
                for (var i = start; i < end; i = i.AddMinutes(1))
                {
                    if (i.DayOfWeek != DayOfWeek.Friday && i.DayOfWeek != DayOfWeek.Saturday)
                    {
                        if (!holidays.Any(x => x.Day == i.Day && x.Month == i.Month && x.Year == i.Year))
                        {
                            if (i.TimeOfDay.Hours >= 9 && i.TimeOfDay.Hours <= 17)
                            {
                                count++;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DAL.Helper.Log4Net.Error(ex);
            }
            return count;
        }
        public static List<DateTime> GetPublicHolidays(string country)
        {
            List<DateTime> holidays = new List<DateTime>();
            try
            {
                if (country == "UAE")
                {
                    holidays.Add(new DateTime(2020, 05, 24));
                    holidays.Add(new DateTime(2020, 05, 25));
                    holidays.Add(new DateTime(2020, 05, 26));
                    holidays.Add(new DateTime(2020, 07, 30));
                    holidays.Add(new DateTime(2020, 07, 31));
                    holidays.Add(new DateTime(2020, 08, 01));
                    holidays.Add(new DateTime(2020, 08, 02));
                    holidays.Add(new DateTime(2020, 08, 23));
                    holidays.Add(new DateTime(2020, 10, 29));
                    holidays.Add(new DateTime(2020, 12, 01));
                    holidays.Add(new DateTime(2020, 12, 02));
                    holidays.Add(new DateTime(2020, 12, 03));
                }
            }
            catch (Exception ex)
            {
                DAL.Operations.Logger.LogError(ex);
            }

            return holidays;
        }
       
    }

    public class Result_SLAOLA
    {
        public double SLATime { get; set; }
        public string DaysOverSLA { get; set; }

        public double OLATime { get; set; }
        public string DaysOverOLA { get; set; }

    }

}
