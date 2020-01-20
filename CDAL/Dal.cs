using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace CDAL
{
    public class Dal
    {


        public static SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Test"].ConnectionString);
        public static SqlCommand cmd = new SqlCommand("", cnx);
        /// <summary>
        /// Inserts a new item on the table represented with the instance of the class object.
        /// </summary>
        /// <param name="objPoco">The instance of the POCO Class.</param>
        /// <remarks>Insert the record with the information loaded in <paramref name="objPOco"/>.
        /// </remarks>
        public static bool ObjAdd<T>(T objPoco)
        {
            try
            {

                string datef = GetDefaultDate();
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string Sql = "";
                string insert = "";
                int cont = 0;
                string tablename = GetSchema(typeobjpoco);
                //if (GetExist(tablename) == false)
                //{
                dt = GetSql(String.Format(@"SELECT C.name as ColumnName, C.is_identity, C.is_nullable FROM SYS.COLUMNS AS C 
                                          INNER JOIN sys.objects AS O on o.object_id = c.object_id
                                          INNER JOIN sys.schemas AS S on S.schema_id = O.schema_id
                                          WHERE OBJECT_NAME(C.OBJECT_ID) = '{0}' and s.name = '{1}'
                                          ORDER BY C.column_id", tablename.Substring(tablename.IndexOf(".") + 1), tablename.Substring(0, tablename.IndexOf("."))));
                //    SerializeDt(dt, tablename);
                //}
                //else
                //{
                //    dt = DeSerializeDt(tablename);
                //}
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);

                foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                {
                    object typ = prop.GetValue(objPoco);
                    if (dt.Rows[cont][0].ToString().ToUpper() == prop.Name.ToUpper())
                    {
                        if (prop.GetValue(objPoco) == null && Convert.ToBoolean(dt.Rows[cont][2]) == true)
                        {
                            return false;
                        }
                        if (Convert.ToBoolean(dt.Rows[cont][1]) == false)
                        {
                            if (prop.GetValue(objPoco) == null)
                            {
                                Sql += "Null,";
                            }
                            else
                            {
                                if (typ.GetType().ToString() == "System.DateTime")
                                {
                                    if (prop.GetValue(objPoco).ToString() == GetDefaultDate())
                                    {
                                        Sql += "Null,";
                                    }
                                    else
                                    {
                                        Sql += "'" + DateSet(prop.GetValue(objPoco).ToString()) + "',";
                                    }

                                }
                                else
                                {
                                    if (typ.GetType().ToString() == "System.String" || typ.GetType().ToString() == "System.DateTime" || typ.GetType().ToString() == "System.Boolean")
                                    {


                                        Sql += "'" + prop.GetValue(objPoco).ToString() + "',";


                                    }
                                    else
                                    {
                                        if (typ.GetType() == typeof(System.Decimal) || typ.GetType() == typeof(System.Double))
                                        {
                                            Sql += prop.GetValue(objPoco).ToString().Replace(",", ".") + ",";
                                        }
                                        else
                                        {
                                            Sql += prop.GetValue(objPoco).ToString() + ",";
                                        }

                                    }
                                }

                            }
                        }
                        else
                        {
                            Sql += "DEFAULT,";
                        }

                    }
                    cont++;
                }
                Sql = Sql.Remove(Sql.Length - 1);
                insert = "INSERT INTO " + tablename + " VALUES (" + Sql + ")";
                if (ExecuteSql(insert))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// Update a record on the table represented with the instance of the class object based on the table primary key.
        /// </summary>
        /// <param name="objPoco">The instance of the POCO Class.</param>
        /// <remarks>Update the record with the information loaded in <paramref name="objPOco"/>.
        /// </remarks>
        public static bool ObjUpdate<T>(T objPoco)
        {
            try
            {
                string Pk = "";
                string key = "";
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string Sql = "";
                string Update = "";
                int cont = 0;
                string tablename = GetSchema(typeobjpoco);
                //if (GetExist(tablename) == false)
                //{
                dt = GetSql(String.Format(@"SELECT C.name as ColumnName, C.is_identity, C.is_nullable FROM SYS.COLUMNS AS C 
                                          INNER JOIN sys.objects AS O on o.object_id = c.object_id
                                          INNER JOIN sys.schemas AS S on S.schema_id = O.schema_id
                                          WHERE OBJECT_NAME(C.OBJECT_ID) = '{0}' and s.name = '{1}'
                                          ORDER BY C.column_id", tablename.Substring(tablename.IndexOf(".") + 1), tablename.Substring(0, tablename.IndexOf("."))));
                //    SerializeDt(dt, tablename);
                //}
                //else
                //{
                //    dt = DeSerializeDt(tablename);
                //}
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);

                Sql = "SET ";
                foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                {


                    object typ = prop.GetValue(objPoco);
                    if (typeobjpoco.GetProperty(prop.Name).GetCustomAttribute(typeof(KeyAttribute), false) != null)
                    {
                        if (typeobjpoco.GetProperty(prop.Name).GetCustomAttribute(typeof(KeyAttribute), false).GetType().Equals(typeof(KeyAttribute)))
                        {
                            Pk += prop.Name + "=";
                            if (prop.GetValue(objPoco) == null && Convert.ToBoolean(dt.Rows[cont][0]) == false)
                            {
                                return false;
                            }
                            else
                            {
                                if (typ.GetType() == typeof(System.String) || typ.GetType() == typeof(System.DateTime))
                                {
                                    Pk += "'" + prop.GetValue(objPoco).ToString() + "' And ";
                                }
                                else
                                {
                                    if (typ.GetType() == typeof(System.Decimal) || typ.GetType() == typeof(System.Double))
                                    {
                                        Pk += prop.GetValue(objPoco).ToString().Replace(",", ".") + " And ";
                                    }
                                    else
                                    {
                                        Pk += prop.GetValue(objPoco).ToString() + " And ";
                                    }

                                }
                            }
                        }
                    }

                    else
                    {
                        if (Convert.ToBoolean(dt.Rows[cont][1]) == false)
                        {
                            Sql += prop.Name + " = ";
                            if (prop.GetValue(objPoco) == null)
                            {
                                Sql += "Null,";
                            }
                            else
                            {
                                if (prop.GetValue(objPoco).ToString() == GetDefaultDate() && typ.GetType() == typeof(System.DateTime))
                                {
                                    Sql += "Null,";
                                }
                                else
                                {
                                    if (typ.GetType() == typeof(System.String) || typ.GetType() == typeof(System.DateTime) || typ.GetType() == typeof(System.Boolean))
                                    {
                                        if (typ.GetType() == typeof(System.DateTime))
                                        {
                                            Sql += "'" + DateSet(prop.GetValue(objPoco).ToString()) + "',";
                                        }
                                        else
                                        {
                                            Sql += "'" + prop.GetValue(objPoco).ToString() + "',";
                                        }

                                    }
                                    else
                                    {
                                        if (typ.GetType() == typeof(System.Decimal) || typ.GetType() == typeof(System.Double))
                                        {
                                            Sql += prop.GetValue(objPoco).ToString().Replace(",", ".") + ",";
                                        }
                                        else
                                        {
                                            Sql += prop.GetValue(objPoco).ToString() + ",";
                                        }

                                    }
                                }


                            }
                        }
                    }
                    cont++;
                }
                Sql = Sql.Remove(Sql.Length - 1);
                Pk = Pk.Remove(Pk.Length - 4);
                Update = "UPDATE " + tablename + " " + Sql + " WHERE " + Pk;
                ExecuteSql(Update);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        /// <summary>
        /// Returns an object loaded according to its primary key.
        /// </summary>
        /// <param name="objPoco">The type of the POCO Class.</param>
        /// <param name="Id">The identifier to find.</param>
        /// <remarks>Find a record in table type of <paramref name="objPOco"/>.
        /// </remarks>
        public static T ObjFind<T>(T objPoco)
        {
            try
            {
                string Pk = "";
                string key = "";
                string Sql = "";
                int cont = 0;
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string tablename = GetSchema(typeobjpoco);
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);
                //if (GetExist(tablename) == false)
                //{
                dt = GetSql(String.Format(@"SELECT C.name as ColumnName, C.is_identity, C.is_nullable FROM SYS.COLUMNS AS C 
                                          INNER JOIN sys.objects AS O on o.object_id = c.object_id
                                          INNER JOIN sys.schemas AS S on S.schema_id = O.schema_id
                                          WHERE OBJECT_NAME(C.OBJECT_ID) = '{0}' and s.name = '{1}'
                                          ORDER BY C.column_id", tablename.Substring(tablename.IndexOf(".") + 1), tablename.Substring(0, tablename.IndexOf("."))));
                //    SerializeDt(dt, tablename);
                //}
                //else
                //{
                //    dt = DeSerializeDt(tablename);
                //}


                foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                {

                    object typ = prop.GetValue(objPoco);
                    if (typeobjpoco.GetProperty(prop.Name).GetCustomAttribute(typeof(KeyAttribute), false) != null)
                    {
                        if (typeobjpoco.GetProperty(prop.Name).GetCustomAttribute(typeof(KeyAttribute), false).GetType().Equals(typeof(KeyAttribute)))
                        {
                            Pk += prop.Name + "=";
                            if (prop.GetValue(objPoco) == null && Convert.ToBoolean(dt.Rows[cont][0]) == false)
                            {
                                //return false;
                            }
                            else
                            {
                                if (typ.GetType() == typeof(System.String) || typ.GetType() == typeof(System.DateTime))
                                {
                                    Pk += "'" + prop.GetValue(objPoco).ToString() + "' And ";
                                }
                                else
                                {
                                    Pk += prop.GetValue(objPoco).ToString() + " And ";
                                }
                            }
                        }
                    }

                    //if (Pk.ToUpper() == prop.Name.ToUpper())
                    //{

                    //    if (typ.GetType().ToString().ToUpper() == Id.GetType().ToString().ToUpper())
                    //    {
                    //        if (typ.GetType().ToString() == "System.String" || typ.GetType().ToString() == "System.DateTime")
                    //        {
                    //            key = "'" + Id.ToString() + "'";
                    //        }
                    //        else
                    //        {
                    //            key = Id.ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return default(T);
                    //    }
                    //}
                    cont++;
                }
                Pk = Pk.Remove(Pk.Length - 4);
                Sql = "SELECT * FROM " + tablename + " " + Sql + " WHERE " + Pk;

                dt = GetSql(Sql);
                cont = 0;
                var obj = Activator.CreateInstance(typeof(T));
                if (dt.Rows.Count > 0)
                {
                    foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                    {
                        if ((dt.Rows[0][cont].GetType()).ToString() == "System.DBNull")
                        {
                            prop.SetValue(obj, null);
                        }
                        else
                        {
                            prop.SetValue(obj, dt.Rows[0][cont]);
                        }

                        cont++;
                    }
                }
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// Returns the next id from the table.
        /// </summary>
        /// <param name="objPoco">The type of the POCO Class.</param>
        /// <remarks>get the next id in the table <paramref name="objPOco"/>.
        /// </remarks>
        public static string LastID<T>(T objPoco)
        {
            try
            {
                string Pk = "";
                Type typeobjpoco = typeof(T);
                string tablename = GetSchema(typeobjpoco);
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);
                Pk = SingleData(String.Format("SELECT a.attname " +
                                          "FROM pg_index i " +
                                          "JOIN   pg_attribute a ON a.attrelid = i.indrelid AND a.attnum = ANY(i.indkey) " +
                                          "WHERE  i.indrelid = '{0}'::regclass AND i.indisprimary;", tablename));
                if (Pk == "")
                {
                    return "";
                }
                else
                {
                    string result = SingleData(string.Format("SELECT {0} + 1 FROM {1} ORDER BY {0} DESC FETCH FIRST 1 ROW ONLY", Pk, tablename));
                    if (string.IsNullOrEmpty(result))
                    {
                        return "1";
                    }
                    else
                    {
                        return result;
                    }

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Returns a typed list by performing a query of all fields to a table
        /// </summary>
        /// <param name="objPoco">The type of the POCO Class.</param>
        /// <param name="Opt">Set 0 for Select * of a table, Set 1 for indicate a view and a where clause.</param>
        /// <param name="vw">If Opt is set to 1, Inicates The name of the view .</param>
        /// <param name="Fields">If Opt is set to 1, Indicate fields and values to be filtered, You can filter as many fields as the table or view contains.</param>
        /// <remarks>
        /// </remarks>
        public static List<T> DbToObj<T>(T ObjPoco, int Opt = 0, string vw = null, string Fields = null)
        {
            try
            {
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string tablename = GetSchema(typeobjpoco);
                dt = GetQuery(tablename, Opt, vw, Fields);
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);
                var listobj = new List<T>();
                int cont = 0;
                for (int i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    var obj = Activator.CreateInstance(typeof(T));
                    foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                    {
                        if ((dt.Rows[i][cont].GetType()).ToString() == "System.DBNull")
                        {
                            prop.SetValue(obj, null);
                        }
                        else
                        {
                            prop.SetValue(obj, dt.Rows[i][cont]);
                        }

                        cont++;
                    }
                    listobj.Add((T)Convert.ChangeType(obj, typeof(T)));
                    cont = 0;
                }
                return listobj;
            }
            catch (Exception e)
            {
                //return default(List<T>);
                throw e;
            }

        }

        /// <summary>
        /// Execute a store procedure and return its result in a list .
        /// </summary>
        /// <param name="objPoco">The type of the POCO Class.</param>
        /// <remarks>
        /// </remarks>
        public static List<T> SpToObj<T>(T objPoco)
        {
            try
            {
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string tablename = GetSchema(typeobjpoco);
                dt = GetSql("Select * from  " + tablename);
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);
                var listobj = new List<T>();
                int cont = 0;
                for (int i = 0; i < (dt.Rows.Count - 1); i++)
                {
                    var obj = Activator.CreateInstance(typeof(T));
                    foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                    {
                        if ((dt.Rows[i][cont].GetType()).ToString() == "System.DBNull")
                        {
                            prop.SetValue(obj, null);
                        }
                        else
                        {
                            prop.SetValue(obj, dt.Rows[i][cont]);
                        }

                        cont++;
                    }
                    listobj.Add((T)Convert.ChangeType(obj, typeof(T)));
                    cont = 0;
                }
                return listobj;
            }
            catch (Exception e)
            {
                //return default(List<T>);
                throw e;
            }
        }

        public static List<T> DbToList<T>(T objPoco, string Sql)
        {
            try
            {
                DataTable dt = new DataTable();
                Type typeobjpoco = typeof(T);
                string tablename = GetSchema(typeobjpoco);
                dt = GetSql(Sql);
                PropertyDescriptorCollection PropiedadesObjPoco = TypeDescriptor.GetProperties(typeobjpoco);
                var listobj = new List<T>();
                int cont = 0;
                for (int i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    var obj = Activator.CreateInstance(typeof(T));
                    foreach (PropertyDescriptor prop in PropiedadesObjPoco)
                    {
                        if ((dt.Rows[i][cont].GetType()).ToString() == "System.DBNull")
                        {
                            prop.SetValue(obj, null);
                        }
                        else
                        {
                            prop.SetValue(obj, dt.Rows[i][cont]);
                        }

                        cont++;
                    }
                    listobj.Add((T)Convert.ChangeType(obj, typeof(T)));
                    cont = 0;
                }
                return listobj;
            }
            catch (Exception e)
            {
                //return default(List<T>);
                throw e;
            }
        }

        /// <summary>
        /// Execute a query and return its result in a datable .
        /// </summary>
        /// <param name="Sql">The Sql query.</param>
        /// <remarks>
        /// </remarks>
        public static DataTable GetSql(string Sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dta = new SqlDataAdapter();
            string query = Sql;
            try
            {
                dta.SelectCommand = new SqlCommand(query, cnx);
                if (cnx.State == ConnectionState.Closed)
                {
                    cnx.Open();
                }
                dt.Clear();
                dta.Fill(dt);
                cnx.Close();
                return dt;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }

        }

        public static DataTable GetSqlReport(string Sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dta = new SqlDataAdapter();
            string query = Sql;
            try
            {
                SqlCommand con = new SqlCommand(query, cnx);
                con.CommandTimeout = 0;
                dta.SelectCommand = con;

                cnx.Open();
                dt.Clear();
                dta.Fill(dt);
                cnx.Close();
                return dt;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }

        }

        /// <summary>
        /// Execute a query or store procedure against the database .
        /// </summary>
        /// <param name="Sql">The Sql query.</param>
        /// <remarks>
        /// </remarks>
        public static bool ExecuteSql(string Sql)
        {
            try
            {
                if (cnx.State == ConnectionState.Closed)
                {
                    cnx.Open();
                }
                cmd.CommandText = Sql;
                cmd.ExecuteNonQuery();
                cnx.Close();
                return true;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }
        }
        /// <summary>
        /// Returns a single field of a query .
        /// </summary>
        /// <param name="Sql">The Sql query.</param>
        /// <remarks>
        /// </remarks>
        public static string SingleData(string Sql)
        {
            string result = "";
            try
            {
                cmd.CommandText = Sql;
                if (cnx.State == ConnectionState.Closed)
                {
                    cnx.Open();
                }
                result = Convert.ToString(cmd.ExecuteScalar());
                cnx.Close();
                return result;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                    result = "";
                }
                throw e;
            }
        }

        private static DataTable GetSpInfo(string Sp)
        {

            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            SqlDataAdapter dta = new SqlDataAdapter();
            string query = "Exec sp_help '" + Sp + "'";
            try
            {
                dta.SelectCommand = new SqlCommand(query, cnx);
                cnx.Open();
                dt.Clear();
                dta.Fill(ds);
                dt = ds.Tables[1];
                cnx.Close();
                return dt;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }



        }

        private static DataTable GetQuery(string Table, int Opt = 1, string vw = null, string Fields = null)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dta = new SqlDataAdapter();
            string query = "";
            if (Opt == 1 && vw != null && Fields == null)
            {
                query = "SELECT * FROM " + vw;
            }
            else if (Opt == 1 && vw != null && Fields != null)
            {
                query = "SELECT * FROM " + vw + " WHERE " + Fields;
            }
            else if (Opt == 1 && vw == null && Fields != null)
            {
                query = "SELECT * FROM " + Table + " WHERE " + Fields;
            }
            else
            {
                query = "SELECT * FROM " + Table;
            }
            try
            {
                dta.SelectCommand = new SqlCommand(query, cnx);
                cnx.Open();
                dt.Clear();
                dta.Fill(dt);
                cnx.Close();
                return dt;
            }
            catch (Exception e)
            {
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }

        }

        private static string GetSchema(Type ObjPoco)
        {
            try
            {
                string table = "";

                if (ObjPoco.FullName.Substring((ObjPoco.FullName.IndexOf(".") + 1)).IndexOf("+") > 0)
                {
                    table = ObjPoco.FullName.Substring((ObjPoco.FullName.IndexOf(".") + 1)).Replace("+", ".");
                }
                else
                {
                    table = "dbo." + ObjPoco.FullName.Substring((ObjPoco.FullName.IndexOf(".") + 1));
                }
                return table;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static bool SerializeDt(DataTable dt, string table)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(SetName(table), System.IO.FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, dt);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static DataTable DeSerializeDt(string table)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(SetName(table), System.IO.FileMode.Open);

                BinaryFormatter bf = new BinaryFormatter();
                DataTable dt = (DataTable)bf.Deserialize(fs);
                fs.Close();
                return dt;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static string SetName(string table)
        {
            try
            {
                string result = Environment.CurrentDirectory + "\\vmo\\" + CrypName(table) + "\\" + DateTime.Today.ToString("dd") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy") + ".bin";
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static bool GetExist(string Name)
        {
            try
            {
                string crname = CrypName(Name);
                if (System.IO.Directory.Exists(Environment.CurrentDirectory + "\\vmo") == false)
                {
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\vmo");
                }
                if (System.IO.Directory.Exists(Environment.CurrentDirectory + "\\vmo\\" + crname) == false)
                {
                    System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + "\\vmo\\" + crname);
                }
                if (System.IO.File.Exists(Environment.CurrentDirectory + "\\vmo\\" + crname + "\\" + DateTime.Today.ToString("dd") + DateTime.Today.ToString("MM") + DateTime.Today.ToString("yy") + ".bin") == false)
                {
                    string[] filePaths = System.IO.Directory.GetFiles(Environment.CurrentDirectory + "\\vmo\\" + crname);
                    foreach (string filePath in filePaths)
                        System.IO.File.Delete(filePath);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static string CrypName(string Name)
        {
            try
            {
                string result = string.Empty;
                byte[] encryted = System.Text.Encoding.ASCII.GetBytes(Name);
                result = Convert.ToBase64String(encryted);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static string DecrypName(string Name)
        {
            try
            {
                string result = string.Empty;
                byte[] decryted = Convert.FromBase64String(Name);
                result = System.Text.Encoding.ASCII.GetString(decryted);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static string GetDefaultDate()
        {
            try
            {
                string defaultdate = "";
                DateTime myemptydate = new DateTime();
                defaultdate = myemptydate.ToString();
                return defaultdate;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private static string DateSet(string fecha)
        {
            try
            {
                string datefomat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                string Sep = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
                string yearformat = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.YearMonthPattern.Substring(System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.YearMonthPattern.IndexOf(",") + 2);
                if (yearformat.Length > 4)
                {
                    yearformat = yearformat.Substring(yearformat.Length - 4);
                }
                string datedb = SingleData("SHOW datestyle");
                datedb = datedb.Substring(datedb.IndexOf(",") + 2);
                string fechaf = "";
                string dia = "";
                string mes = "";
                string año = "";

                if (datefomat.Substring(0, 1).ToUpper() == datedb.Substring(0, 1).ToUpper())
                {
                    return fecha;
                }
                else
                {
                    if (datefomat.Substring(0, 1).ToUpper() == "M" && datedb.Substring(0, 1).ToUpper() == "D")
                    {
                        mes = fecha.Substring(0, fecha.IndexOf(Sep));
                        dia = fecha.Substring(fecha.IndexOf(Sep) + 1).Substring(0, fecha.Substring(fecha.IndexOf(Sep) + 1).IndexOf(Sep));
                        año = fecha.Substring(fecha.IndexOf(Sep) + 1).Substring(fecha.Substring(fecha.IndexOf(Sep) + 1).IndexOf(Sep) + 1).Substring(0, yearformat.Length);
                        fechaf = dia + Sep + mes + Sep + año;
                    }
                    else if (datefomat.Substring(0, 1).ToUpper() == "D" && datedb.Substring(0, 1).ToUpper() == "M")
                    {
                        dia = fecha.Substring(0, fecha.IndexOf(Sep));
                        mes = fecha.Substring(fecha.IndexOf(Sep) + 1).Substring(0, fecha.Substring(fecha.IndexOf(Sep) + 1).IndexOf(Sep));
                        año = fecha.Substring(fecha.IndexOf(Sep) + 1).Substring(fecha.Substring(fecha.IndexOf(Sep) + 1).IndexOf(Sep) + 1).Substring(0, yearformat.Length);
                        fechaf = mes + Sep + dia + Sep + año;
                    }

                }
                return fechaf;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public static bool ExecuteSqlMasivos(List<string> Sql)
        {
            //var trans;
            SqlTransaction trans = null;
            try
            {
                cnx.Open();
                trans = cnx.BeginTransaction();
                foreach (var consulta in Sql)
                {
                    cmd.CommandText = consulta;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                cnx.Close();
                return true;
            }
            catch (Exception e)
            {
                if (trans != null)
                {
                    trans.Rollback();
                }
                if (cnx.State == System.Data.ConnectionState.Open)
                {
                    cnx.Close();
                }
                throw e;
            }
        }




    }
}
