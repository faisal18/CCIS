using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Operations
{
  public  class OpLookups
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> getAllLicenses ()
        {
            try
            {
                List<string> lstLicenses = new List<string>();
                lstLicenses.AddRange (OpProviders.GetAllString());
                lstLicenses.AddRange (OpPayers.GetAllString());
                return lstLicenses;
            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                    return null;
            }


        }

        public static List<string> getAllLicensesNoSplit ()
        {
            try
            {
                List<string> lstLicenses = new List<string>();

                lstLicenses.AddRange (OpProviders.GetAllStringNoSplit());
                lstLicenses.AddRange (OpPayers.GetAllStringNoSplit());
                return lstLicenses;
            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                    return null;

            }


        }




    }
}
