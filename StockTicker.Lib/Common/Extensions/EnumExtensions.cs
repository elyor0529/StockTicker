using System;
using System.ComponentModel;

namespace StockTicker.Lib.Common.Extensions
{
    public static class EnumExtensions
    {
         
        public static string GetDescription(this Enum value)
        { 
            if(value==null)
                return String.Empty;

            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetName(this Enum value)
        { 
            var type = value.GetType();

            return Enum.GetName(type, value);
        }

        public static T GetValue<T>(this string description) where T : struct
        {
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                if (attribute != null)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            return default(T);
        }

    }
}