using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FpcFramework.Converter
{
    public class Converter
    {
        public static T ConvertTo<T>(object value, T defaultValue)
        {
            if (Converter.IsNull(value))
            {
                return defaultValue;
            }

            if (value is T)
            {
                return (T)value;
            }
            else
            {
                try
                {
                    if (typeof(T) == System.Type.GetType("System.Boolean"))
                    {
                        bool boolValue;
                        if (bool.TryParse(value.ToString(), out boolValue))
                            value = boolValue;
                        else
                        {
                            int intValue;
                            if (int.TryParse(value.ToString(), out intValue))
                                value = intValue;
                        }
                    }

                    Type t = typeof(T);
                    t = Nullable.GetUnderlyingType(t) ?? t;

                    if (t.IsEnum)
                    {
                        if (value == null || DBNull.Value.Equals(value) || !t.IsEnumDefined(value))
                            throw new Exception("Enum {value} is not defined");

                        return (T)Enum.Parse(t, value.ToString());
                    }
                    else
                    {
                        return (value == null || DBNull.Value.Equals(value)) ? default(T) : (T)Convert.ChangeType(value, t);
                    }
                }
                catch (Exception e)
                {
                    //  return defaultValue;
                    throw new Exception("ConvertTo() An exception occured while converting value " + value.ToString() + " to " + typeof(T).ToString() + e.ToString());
                }
            }
        }

        public static T ConvertTo<T>(object value)
        {
            T defaultValue = default(T);

            // Default value for string must be empty string and not null
            if (typeof(T) == System.Type.GetType("System.String"))
            {
                defaultValue = (T)Convert.ChangeType("", typeof(T));
            }

            return Converter.ConvertTo<T>(value, defaultValue);
        }

        public static bool IsNull(object value)
        {
            return (value == null || value == DBNull.Value);
        }
    }
}
