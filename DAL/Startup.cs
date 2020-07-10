using DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace DAL
{
    public class Startup
    {
        public Startup()
        {
            try
            {

                Operations.Logger.Info("-- Lets begin the story -- ");
                

                Operations.Logger.Info("-- Lets End the story -- ");

            }

            catch (Exception ex)
            {

                Operations.Logger.LogError(ex);


            }

        }
    }
}

