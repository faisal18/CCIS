using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;

namespace CCIS.WebService
{
    /// <summary>
    /// Summary description for WSAutomation
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]

    public class WSAutomation : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public  List<string> GetPayerCodes(string PayerCodes)
        {
           try { List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

            ep = DAL.Operations.OpCallerInfo.GetAll();
            List<string> Svalues = new List<string>();

            foreach (var item in ep)
            {
                Svalues.Add(item.Name + Environment.NewLine);
            }
            //Context.Response.Write(Svalues);
            return Svalues;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
         
        [WebMethod]
        public  List<string> GetPayerCodesCached(string PayerCodes)
        {
            try
            {

            
           //DataCaching.DataCaching dc = new DataCaching.DataCaching();

           // var ds1 = dc.CallerInformation;
           
           
            //List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

            //  DataRow[]  dr = dc.PayersCashed.Tables[0].Select("Name like '%"+PayerCodes+"%'"); 
            var hCache = HttpContext.Current.Cache.Get("_PayersCashed_");

            DataSet ds = new DataSet();

            if (hCache != null)
            {
                ds = (DataSet)hCache;
            }
            else
            { return null; }

            DataRow[]  dr = ds.Tables[0].Select("PayerCode like '%"+PayerCodes+"%'"); 
           // ep = DAL.Operations.OpCallerInfo.GetAll();
            List<string> Svalues = new List<string>();

            foreach (var item in dr)
            {
                if(item.Table.Columns.Contains("PayerCode") == true)
                {
                Svalues.Add(item["PayerCode"].ToString() + " - " + item["PayerName"].ToString() );
                }
                
            }
            //Context.Response.Write(Svalues);
            Svalues.Sort();
            return Svalues;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        } [WebMethod]
        public  List<string> GetAllLicenses(string LicenseCode)
        {
            try
            {

            
           //DataCaching.DataCaching dc = new DataCaching.DataCaching();

           // var ds1 = dc.CallerInformation;
           
           
            //List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

            //  DataRow[]  dr = dc.PayersCashed.Tables[0].Select("Name like '%"+PayerCodes+"%'"); 
            var hCache = HttpContext.Current.Cache.Get("_GetAllLicenses_");

            DataSet ds = new DataSet();

            if (hCache != null)
            {
                ds = (DataSet)hCache;
            }
            else
            { return null; }

            DataRow[]  dr = ds.Tables[0].Select("AllNames like '%" + LicenseCode + "%'","AllNames ASC"); 
           // ep = DAL.Operations.OpCallerInfo.GetAll();
            List<string> Svalues = new List<string>();
                Svalues = dr.AsEnumerable().Select(x=>x[0].ToString()).Take(10).ToList();
              
              
            //foreach (var item in dr)
            //{
            //    if(item.Table.Columns.Contains("AllNames") == true)
            //    {
            //    Svalues.Add(item["AllNames"].ToString() );
            //    }
                
            //}
            //Context.Response.Write(Svalues);
           
            return Svalues;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
        [WebMethod]
        public List<string> GetTicketsCached(string TicketSubject)
        {
            try
            {


                //DataCaching.DataCaching dc = new DataCaching.DataCaching();

                // var ds1 = dc.CallerInformation;


                //List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

                //  DataRow[]  dr = dc.PayersCashed.Tables[0].Select("Name like '%"+PayerCodes+"%'"); 
                var hCache = HttpContext.Current.Cache.Get("_TicketsCashed_");

                DataSet ds = new DataSet();

                if (hCache != null)
                {
                    ds = (DataSet)hCache;
                }
                else
                { return null; }

                DataRow[] dr = ds.Tables[0].Select("Subject like '%" + TicketSubject + "%'", "Subject ASC");
                // ep = DAL.Operations.OpCallerInfo.GetAll();
                List<string> Svalues = new List<string>();
                Svalues = dr.AsEnumerable().Select(x => x["TicketNumber"].ToString() + " - " + x["Subject"].ToString()).Take(100).ToList();
               
                //foreach (var item in dr)
                //{
                //    if (item.Table.Columns.Contains("Subject") == true)
                //    {
                //        Svalues.Add(item["TicketNumber"].ToString() + " - " + item["Subject"].ToString());
                //    }

                //}
                ////Context.Response.Write(Svalues);
                //Svalues.Sort();
                return Svalues;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        //[WebMethod]
        //public List<Entities.CallerInformation> GetCallerListbyLicenseID(string PayerCodes)
        //{

        //    try
        //    {

        //        List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

        //        ep = DAL.Operations.OpCallerInfo.GetCallerInformationbyLicenseID(PayerCodes);
        //        //  DataRow[]  dr = dc.CallerInformationbyLicenseID.Tables[0].Select("Name like '%"+PayerCodes+"%'"); 


        //        // DataRow[]  dr = ds.Tables[0].Select("PayerCode like '%"+PayerCodes+"%'"); 
        //        //// ep = DAL.Operations.OpCallerInfo.GetAll();
        //        // List<string> Svalues = new List<string>();

        //        // foreach (var item in dr)
        //        // {
        //        //     if(item.Table.Columns.Contains("PayerCode") == true)
        //        //     {
        //        //     Svalues.Add(item["PayerCode"].ToString() + " - " + item["PayerName"].ToString() );
        //        //     }

        //        // }
        //        //Context.Response.Write(Svalues);

        //        //ep.Sort();
        //        return ep;
        //    }
        //    catch (Exception ex)
        //    {

        //        DAL.Operations.Logger.LogError(ex);
        //        return null;
        //    }
        //}




    }
}
