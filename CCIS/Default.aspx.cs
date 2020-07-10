using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace CCIS
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //[WebMethod]
        //public static List<string> GetPayerCodes(string PayerCode)
        //{
        //    List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

        //    ep = DAL.Operations.OpCallerInfo.GetAll();
        //    List<string> Svalues = new List<string>();

        //    foreach (var item in ep)
        //    {
        //        Svalues.Add(item.Name + Environment.NewLine);
        //    }
        //    //Context.Response.Write(Svalues);
        //    return Svalues;
        //}

        //[WebMethod]
        //public static List<string> GetPayerCodesCached(string PayerCode)
        //{

        //    DataCaching.DataCaching dc = new DataCaching.DataCaching();

        //  //  dc.PayersCashed.Tables[0].Rows.Count();

        //   var gKey =  HttpContext.Current.Cache.Get("_PayersCashed_");

        //    List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();

        //    DataRow[] dr = dc.PayersCashed.Tables[0].Select("Name like '%" + PayerCode + "%'");
        //    // ep = DAL.Operations.OpCallerInfo.GetAll();
        //    List<string> Svalues = new List<string>();

        //    foreach (var item in dr)
        //    {
        //        //  Svalues.Add(item.ItemArray.Contains("Name") ? item.ItemArray["Name"].ToString() : "" + Environment.NewLine);
        //    }
        //    //Context.Response.Write(Svalues);
        //    return Svalues;
        //}
        [WebMethod]
        public static  List<Entities.CallerInformation> GetCallerListbyLicenseID(string PayerCodes)
        {

            try
            {

                List<Entities.CallerInformation> ep = new List<Entities.CallerInformation>();




                ep = DAL.Operations.OpCallerInfo.GetCallerInformationbyLicenseID(PayerCodes);
                //  DataRow[]  dr = dc.CallerInformationbyLicenseID.Tables[0].Select("Name like '%"+PayerCodes+"%'"); 


                // DataRow[]  dr = ds.Tables[0].Select("PayerCode like '%"+PayerCodes+"%'"); 
                //// ep = DAL.Operations.OpCallerInfo.GetAll();
                // List<string> Svalues = new List<string>();

                // foreach (var item in dr)
                // {
                //     if(item.Table.Columns.Contains("PayerCode") == true)
                //     {
                //     Svalues.Add(item["PayerCode"].ToString() + " - " + item["PayerName"].ToString() );
                //     }

                // }
                //Context.Response.Write(Svalues);

                //ep.Sort();
                return ep;
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
          List<string> ls =   DAL.Operations.OpLookups.getAllLicenses();

        }
    }
}