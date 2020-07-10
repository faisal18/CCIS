using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;


namespace DAL.Operations
{
  public  class OpSearch
    {
        public static List<string> GetAll()
        {
            try
            {
                


                    List<string> lstLocation = new List<string>();
                    
                    lstLocation.AddRange(OpCallerInfo.GetAllString());
                lstLocation.AddRange(OpPayers.GetAllString());
                lstLocation.AddRange(OpProviders.GetAllString());



             


                //checkerRepository.Dispose();
                //DBContext.Dispose();
                return lstLocation;
                
            }
            catch (Exception ex)
            {

                DAL.Operations.Logger.LogError(ex);
                return null;
            }
        }
    }
}
