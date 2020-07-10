using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace DAL.Helper
{
    public static class DatasetToEntity
    {




        private static readonly IDictionary<Type, ICollection<PropertyInfo>> _Properties =
            new Dictionary<Type, ICollection<PropertyInfo>>();

        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static IEnumerable<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
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

                var list = new List<T>(table.Rows.Count);
                // Parallel.ForEach(table.AsEnumerable().Skip(0), row =>
                foreach (var row in table.AsEnumerable().Skip(0))
                {
                    var obj = new T();

                    // Parallel.ForEach(properties, prop =>
                    foreach (var prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var UpdateType = new object();
                            if (prop.Name.ToString() == "Photo" && propType.FullName == "System.Byte[]")
                            {
                                //UpdateType = (byte[])Convert.ChangeType(row[prop.Name], typeof(System.Byte[]));
                                UpdateType = Encoding.Default.GetBytes(row[prop.Name].ToString());
                            }
                            else
                            {
                                UpdateType = Convert.ChangeType(row[prop.Name], propType);
                            }

                            var safeValue = row[prop.Name] == null ? null : UpdateType;

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch (InvalidCastException iEx)
                        {
                            // ignored
                            Operations.Logger.LogError("Warning",iEx.InnerException.ToString(),"Datatable to LIst COnversion -Invalid cast Exception");
                        }
                        catch (Exception ex)
                        {
                            Operations.Logger.LogError(ex);
                        }
                    }
                    // );

                    list.Add(obj);
                }
                //  );
                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }



        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static IEnumerable<T> DataTableToListAsync<T>(this DataTable table) where T : class, new()
        {
            try
            {
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

                var list = new List<T>(table.Rows.Count);
                Parallel.ForEach(table.AsEnumerable().Skip(0), row =>
                //foreach (var row in table.AsEnumerable().Skip(0))
                {
                    var obj = new T();

                    Parallel.ForEach(properties, prop =>
                    // foreach (var prop in properties)
                    {
                        try
                        {
                            var propType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var UpdateType = new object();
                            if (prop.Name.ToString() == "Photo" && propType.FullName == "System.Byte[]")
                            {
                                //UpdateType = (byte[])Convert.ChangeType(row[prop.Name], typeof(System.Byte[]));
                                UpdateType = Encoding.Default.GetBytes(row[prop.Name].ToString());
                            }
                            else
                            {
                                UpdateType = Convert.ChangeType(row[prop.Name], propType);
                            }

                            var safeValue = row[prop.Name] == null ? null : UpdateType;

                            prop.SetValue(obj, safeValue, null);
                        }
                        catch (InvalidCastException iEx)
                        {
                            Operations.Logger.LogError("Warning", iEx.InnerException.ToString(), "Datatable to LIst (ASYNC)COnversion -Invalid cast Exception");

                        }
                        catch (Exception ex)
                        {
                            Operations.Logger.LogError(ex);
                        }
                    }
                     );

                    list.Add(obj);
                }
                 );
                return list;
            }
            catch
            {
                return Enumerable.Empty<T>();
            }
        }

        public static IEnumerable<T> GetListFromDataTable<T>(this DataTable table) where T : class, new()
        {
            try
            {

                var empList = table.AsEnumerable().Select(dataRow => new T(

                    )).ToList();

                return empList;
            }
            catch (Exception exc)
            {
                Operations.Logger.LogError(exc);
                return Enumerable.Empty<T>();
            }
        }
        
        public static List<Entities.PersonInformation> PersonInformationFromDataTable(DataTable table)
        {
            try
            {
              
                if (table != null)
                {

                    List<Entities.PersonInformation> _mRPersonInformation = new List<Entities.PersonInformation>();

                    Entities.PersonInformation MRPI = new Entities.PersonInformation();

                    DataRow row = table.Rows[0];

                   
                    
                    //  if (table.Columns.Contains("MemberTypeID"))
                    //{
                    //    MRPI.MemberTypeID = row["MemberTypeID"].ToString();
                    //}


                    if (table.Columns.Contains("FullName"))
                    {
                        MRPI.FullName = row["FullName"].ToString();
                    }

                   
                    if (table.Columns.Contains("ContactNumber"))
                    {
                        MRPI.ContactNumber = row["ContactNumber"].ToString();
                    }


                  

                    if (table.Columns.Contains("Gender"))
                    {
                        MRPI.Gender = row["Gender"].ToString();
                    }

                    if (table.Columns.Contains("Nationality"))
                    {
                        MRPI.NationalityID = int.Parse(row["Nationality"].ToString());
                    }
                    
                    //if (table.Columns.Contains("NationalityID"))
                    //{
                    //    MRPI.NationalityID = row["NationalityID"].ToString();
                    //}
                    


                    
                    if (table.Columns.Contains("Email"))
                    {
                        MRPI.Email = row["Email"].ToString();
                    }
                     //    //
                   
                    
                    if (table.Columns.Contains("ResidentialLocation"))
                    {
                        MRPI.ResidentialLocation = int.Parse(row["ResidentialLocation"].ToString());
                    }


                    //if (table.Columns.Contains("ResidentialLocationID"))
                    //{
                    //    MRPI.ResidentialLocationID = row["ResidentialLocationID"].ToString();
                    //}
                    

                    if (table.Columns.Contains("WorkLocation"))
                    {
                        MRPI.WorkLocation = int.Parse(row["WorkLocation"].ToString());
                    }
                    
                    //if (table.Columns.Contains("WorkLocationID"))
                    //{
                    //    MRPI.WorkLocationID = row["WorkLocationID"].ToString();
                    //} //  //  WorkLocationID = !dRow.Table.Columns.Contains("WorkLocationID") ? null : dRow.Field<string>("WorkLocationID"),


                   
                    
                   






                    //List<Entities.MRPersonInformation> _mRPersonInformation = table.AsEnumerable().Select(dRow => new Entities.MRPersonInformation
                    //{


                    //    MemberType = !dRow.Table.Columns.Contains("MemberType") ? new int() : dRow.Field<int>("MemberType"),
                    //  //MemberTypeID = !dRow.Table.Columns.Contains("MemberTypeID") ? null : dRow.Field<string>("MemberTypeID"),



                    //    FullName = !dRow.Table.Columns.Contains("FullName") ? null : dRow.Field<string>("FullName"),


                    //    FirstName = !dRow.Table.Columns.Contains("FirstName") ? null : dRow.Field<string>("FirstName"),

                    //    SecondName = !dRow.Table.Columns.Contains("SecondName") ? null : dRow.Field<string>("SecondName"),

                    //    FamilyName = !dRow.Table.Columns.Contains("FamilyName") ? null : dRow.Field<string>("FamilyName"),

                    //    ContactNumber = !dRow.Table.Columns.Contains("ContactNumber") ? null : dRow.Field<string>("ContactNumber"),
                    //    BirthDate = !dRow.Table.Columns.Contains("BirthDate") ? null : dRow.Field<DateTime?>("BirthDate"),

                    //    Gender = !dRow.Table.Columns.Contains("Gender") ? new int() : dRow.Field<int>("Gender"),
                    //    Nationality = !dRow.Table.Columns.Contains("Nationality") ? new int() : dRow.Field<int>("Nationality"),
                    //    //NationalityID = !dRow.Table.Columns.Contains("NationalityID") ? null : dRow.Field<string>("NationalityID"),



                    //    PassportNumber = !dRow.Table.Columns.Contains("PassportNumber") ? null : dRow.Field<string>("PassportNumber"),

                    //    MaritalStatus = !dRow.Table.Columns.Contains("MaritalStatus") ? new int() : dRow.Field<int>("MaritalStatus"),

                    //    Email = !dRow.Table.Columns.Contains("Email") ? null : dRow.Field<string>("Email"),
                    //    //
                    //    Emirate = !dRow.Table.Columns.Contains("Emirate") ? new int() : dRow.Field<int>("Emirate"),

                    //    ResidentialLocation = !dRow.Table.Columns.Contains("ResidentialLocation") ? new int() : dRow.Field<int>("ResidentialLocation"),
                    //   // ResidentialLocationID = !dRow.Table.Columns.Contains("ResidentialLocationID") ? null : dRow.Field<string>("ResidentialLocationID"),

                    //    WorkLocation = !dRow.Table.Columns.Contains("WorkLocation") ? new int() : dRow.Field<int>("WorkLocation"),
                    //  //  WorkLocationID = !dRow.Table.Columns.Contains("WorkLocationID") ? null : dRow.Field<string>("WorkLocationID"),


                    //    EmiratesIDNumber = !dRow.Table.Columns.Contains("EmiratesIDNumber") ? null : dRow.Field<string>("EmiratesIDNumber"),
                    //    UIDNumber = !dRow.Table.Columns.Contains("UIDNumber") ? new int() : dRow.Field<int>("UIDNumber"),
                    //    GDRFAFileNumber = !dRow.Table.Columns.Contains("GDRFAFileNumber") ? null : dRow.Field<string>("GDRFAFileNumber"),
                    //    BirthCertificateID = !dRow.Table.Columns.Contains("BirthCertificateID") ? null : dRow.Field<string>("BirthCertificateID"),

                    //    //
                    //    Salary = !dRow.Table.Columns.Contains("Salary") ? new int() : dRow.Field<int>("Salary"),

                    //    Commission = !dRow.Table.Columns.Contains("Commission") ? null : dRow.Field<string>("Commission"),


                    //    Relation = !dRow.Table.Columns.Contains("Relation") ? new int() : dRow.Field<int>("Relation"),

                    //    RelationTo = !dRow.Table.Columns.Contains("RelationTo") ? null : dRow.Field<string>("RelationTo"),




                    //}).ToList();

                    _mRPersonInformation.Add(MRPI);

                    return _mRPersonInformation;




                }
                return null;
            }
            catch (Exception exc)
            {
                Operations.Logger.LogError(exc);
                return null;
            }
        }

      
      

        //public static List<T> GetListFromDT<T>(this DataTable table) where T : new()
        //{
        //    IList<FieldInfo> fields = typeof(T).GetFields().ToList();
        //    List<T> result = new List<T>();
        //    //if (row.Table.Columns.Contains(field.Name))
        //    //{
        //        foreach (var row in table.Rows)
        //        {
        //            var item = CreateItemFromRow<T>((DataRow)row, fields);
        //            result.Add(item);
        //        }
        //   // }

        //    return result;
        //}

        //private static T CreateItemFromRow<T>(DataRow row, IList<FieldInfo> fields) where T : new()
        //{
        //    T item = new T();

        //    foreach (var field in fields)
        //    {
        //        if (row[field.Name] == DBNull.Value)
        //            field.SetValue(item, null);
        //        else
        //            field.SetValue(item, row[field.Name]);
        //    }
        //    return item;
        //}


        //public static object ChangeType (object value , Type TypetoChange)
        //{
        //    try
        //    {
        //        object UpdateType = null;
        //       if (TypetoChange == typeof(byte[]))
        //        {
        //            UpdateType = (byte) byte.Parse(value);

        //        }
        //       else
        //        {

        //        }
        //    }
        //    catch (Exception)
        //    {


        //    }
        //}
    }
}

