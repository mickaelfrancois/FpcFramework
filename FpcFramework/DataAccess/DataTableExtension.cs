using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.DataAccess
{
    public static class DataTableExtension
    {
        internal static string TableName<T>(this T item) where T : IDBModel
        {
            string name = "";
            var attributes = item.GetType().GetCustomAttributes(false);
            foreach (var attribute in attributes)
            {
                if (attribute is TableAttribute)
                {
                    var attr = attribute as TableAttribute;
                    name = attr.Name;
                }
            }

            return name;
        }


        internal static string FindPrimaryKey<T>(this T item) where T : IDBModel
        {
            if (item == null)
                return null;

            string primaryKey = "";

            // Just grabbing this to get hold of the type name:
            var type = item.GetType();

            // Get the PropertyInfo object:
            var properties = item.GetType().GetProperties();

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(false);
                var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(PrimaryKeyAttribute));
                if (columnMapping != null)
                {
                    primaryKey = property.Name;
                    return primaryKey;
                }
            }

            return primaryKey;
        }


        public static int Insert(this IDBModel model, IDatabaseAccess cnx)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder fields = new StringBuilder();
            StringBuilder values = new StringBuilder();
            var type = model.GetType();
            var properties = type.GetProperties();

            string sep = "";
            string tableName = model.TableName();
            string primaryKey = model.FindPrimaryKey();

            query.Append($"INSERT INTO `{tableName}` ");

            var command = cnx.CreateCommand("");

            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                {
                    string columnName = property.Name;
                    if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                    {
                        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                        columnName = columnAttribute.Name;
                    }

                    if (string.Compare(columnName, primaryKey, true) != 0)
                    {
                        fields.Append($"{sep} `{columnName}`");
                        values.Append($"{sep} @{columnName}");
                        object value = property.GetValue(model, null);

                        if (value is string)
                        {
                            if (value.ToString() == "")
                                value = DBNull.Value;
                        }

                        cnx.AddParameter(command, $"@{columnName}", value);

                        sep = ",";
                    }
                }
            }

            query.Append($"( {fields.ToString()} ) VALUES ( {values.ToString()} )");
            command.CommandText = query.ToString();
            return cnx.ExecuteNonQuery(command);
        }



        public static int Update(this IDBModel model, IDatabaseAccess cnx)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder fields = new StringBuilder();      
            StringBuilder where = new StringBuilder();
            var type = model.GetType();
            var properties = type.GetProperties();

            string sep = "";
            string sepWhere = "";
            string tableName = model.TableName();
            string primaryKey = model.FindPrimaryKey();

            query.Append($"UPDATE `{tableName}` ");

            var command = cnx.CreateCommand("");

            foreach (var property in properties)
            {               
                if (!Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                {
                    string columnName = property.Name;
                    if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                    {
                        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                        columnName = columnAttribute.Name;
                    }

                    if (string.Compare(columnName, primaryKey, true) != 0)
                    {
                        fields.Append($"{sep} `{columnName}` = @{columnName}");
                        object value = property.GetValue(model, null);

                        if (value is string)
                        {
                            if (value.ToString() == "")
                                value = DBNull.Value;
                        }

                        cnx.AddParameter(command, $"@{columnName}", value);

                        sep = ",";
                    }
                    else
                    {
                        where.Append($"{sepWhere} `{columnName}` = @{columnName}");
                        object value = property.GetValue(model, null);

                        if (value is string)
                        {
                            if (value.ToString() == "")
                                value = DBNull.Value;
                        }

                        cnx.AddParameter(command, $"@{columnName}", value);

                        sepWhere = ",";
                    }
                }
            }

            query.Append($"SET {fields.ToString()} WHERE {where.ToString()}");
            command.CommandText = query.ToString();
            return cnx.ExecuteNonQuery(command);
        }


        public static int Delete(this IDBModel model, IDatabaseAccess cnx)
        {
            StringBuilder query = new StringBuilder();          
            StringBuilder where = new StringBuilder();
            var type = model.GetType();
            var properties = type.GetProperties();

         
            string sepWhere = "";
            string tableName = model.TableName();
            string primaryKey = model.FindPrimaryKey();

            query.Append($"DELETE FROM `{tableName}` ");

            var command = cnx.CreateCommand("");

            foreach (var property in properties.Where(prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute))))
            {
                string columnName = property.Name;
                if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                {
                    var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                    columnName = columnAttribute.Name;
                }

                if (string.Compare(columnName, primaryKey, true) == 0)
                {                   
                    where.Append($"{sepWhere} `{columnName}` = @{columnName}");
                    object value = property.GetValue(model, null);

                    if (value is string)
                    {
                        if (value.ToString() == "")
                            value = DBNull.Value;
                    }

                    cnx.AddParameter(command, $"@{columnName}", value);

                    sepWhere = ",";
                }
            }

            query.Append($" WHERE {where.ToString()}");
            command.CommandText = query.ToString();
            return cnx.ExecuteNonQuery(command);
        }


        public static ICollection<T> To<T>(this DataTable table) where T:IDBModel
        {
            ICollection<T> list = new List<T>();
            var type = typeof(T);
            var properties = type.GetProperties();
            MethodInfo methodConvertTo = typeof(FpcFramework.Converter.Converter).GetMethod("ConvertTo", new[] { typeof(object) });

            foreach (DataRow row in table.Rows)
            {
                T item = Activator.CreateInstance(typeof(T), null) as T;
                if (item != null)
                {
                    foreach (var property in properties)
                    {                        
                        if (!Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                        {
                            string columnName = property.Name;
                            if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                            {
                                var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                                columnName = columnAttribute.Name;
                            }

                            if (table.Columns.IndexOf(property.Name) >= 0)
                            {
                                Type pType = property.PropertyType;
                                MethodInfo method = methodConvertTo.MakeGenericMethod(pType);
                                var result = method.Invoke(null, new object[] { row[property.Name] });
                                property.SetValue(item, result, null);
                            }
                        }
                    }
                }

                list.Add(item);
            }

            return list;
        }


        public static int CreatTable(this IDBModel model, IDatabaseAccess cnx)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder queryPK = new StringBuilder();
            var type = model.GetType();
            var properties = type.GetProperties();
            string tableName = model.TableName();
            string sep = "";
            string sepPK = "";
       
            query.Append($"CREATE TABLE `{tableName}` ( ");
            queryPK.Append("PRIMARY KEY (");

            foreach (var property in properties)
            {
                if (!Attribute.IsDefined(property, typeof(NotMappedAttribute)))
                {
                    string columnName = property.Name;
                    
                    if (Attribute.IsDefined(property, typeof(ColumnAttribute)))
                    {
                        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
                        columnName = columnAttribute.Name;
                    }

                    query.Append($"{sep}`{columnName}` ");

                    var propertyType = property.PropertyType;
                                     

                    if( propertyType == typeof(int) || propertyType == typeof(bool) || propertyType.IsEnum)
                    {
                        query.Append("INTEGER ");
                    }
                    else if (propertyType == typeof(double) || propertyType == typeof(decimal))
                    {
                        query.Append("REAL ");
                    }
                    else if (propertyType == typeof(DateTime)|| propertyType == typeof(TimeSpan))
                    {
                        query.Append("NUMERIC ");
                    }
                    else if (propertyType == typeof(byte[]) )
                    {
                        query.Append("BLOB ");
                    }
                    else
                    {
                        query.Append("TEXT ");
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        query.Append("NULL");
                    }


                    if (Attribute.IsDefined(property, typeof(PrimaryKeyAttribute)))
                    {
                        var columnAttribute = property.GetCustomAttribute<PrimaryKeyAttribute>();

                        queryPK.Append($"{sepPK}`{columnName}`");
                        if (columnAttribute.AutoIncrement)
                            queryPK.Append(" AUTOINCREMENT");

                        sepPK = ", ";
                    }

                    sep = ", ";
                }
             }

            queryPK.Append(")");

            query.Append(sep + queryPK.ToString() + ")");

            var command = cnx.CreateCommand("");
            command.CommandText = query.ToString();
            return cnx.ExecuteNonQuery(command);
        }
    }
}
