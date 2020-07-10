using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL.Helper
{
 public   class ObjectCompare
    {


        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
            new Dictionary<Type, ICollection<PropertyInfo>>();

        public static string compareobj<T>(T OldOBj , T NewObj) where T:class
        {
            try
            {
                string UpdatedFields = "";




                var objType = typeof(T);
                ICollection<PropertyInfo> properties;

                lock (_Properties)
                {
                    if (!_Properties.TryGetValue(objType, out properties))
                    {
                        properties = objType.GetProperties().Where(property => property.CanWrite).ToList();
                        _Properties.Add(objType, properties);
                    }
                }

               
                    
                    if (OldOBj.Equals(NewObj))
                    {
                        UpdatedFields = "HISTORY \n" + Environment.NewLine;


                    }
                    else
                    {
                    foreach (var prop in properties)
                    {
                        object Objold = OldOBj;
                        object Objnew = NewObj;
                        var x = NewObj.GetType().GetProperty(prop.Name).GetValue(Objnew, null) == null ? "NULL" : NewObj.GetType().GetProperty(prop.Name).GetValue(Objnew, null).ToString();
                        var y = OldOBj.GetType().GetProperty(prop.Name).GetValue(Objold, null) == null ? "NULL" : OldOBj.GetType().GetProperty(prop.Name).GetValue(Objold, null).ToString();
                        if (x == y)
                        {

                        }
                        else
                        {
                            if (prop.Name.ToString() == "Comments" || prop.Name.ToString() == "TicketInformationID" || prop.Name.ToString() == "TicketGUIDKey" || prop.Name.ToString() == "CreatedBy" || prop.Name.ToString() == "CreationDate" || prop.Name.ToString() == "UpdatedBy" || prop.Name.ToString() == "UpdateDate" || prop.Name.ToString() == "TimeStamp" || prop.Name.ToString() == "TicketHistoryID" )
                            {

                            }
                            else
                            {
                                UpdatedFields += "|| " + prop.Name.ToString() + " Value changed  from  [" + y + "]  to [" + x + "];\n\r" + Environment.NewLine;
                            }
                        }
                    }
                   }




                    return UpdatedFields;

            }
            catch (Exception ex)
            {
                Operations.Logger.LogError(ex);
                return null;
                
            }

        }
       




    }
}
