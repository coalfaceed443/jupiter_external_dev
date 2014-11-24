using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace CRM.Code.Utils.Enumeration
{
    public static class Enumeration
    {
        public static IDictionary<int, string> GetAll<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Enumeration type is expected");

            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (var val in Enum.GetValues(enumType))
            {
                string name = GetStringValue<TEnum>(Convert.ToInt32(val));
                dictionary.Add(Convert.ToInt32(val), name);
            }

            return dictionary;
        }

        public static IDictionary<string, string> GetAllNames<TEnum>() where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Enumeration type is expected");

            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (var val in Enum.GetValues(enumType))
            {
                string stringvalue = GetStringValue<TEnum>(Convert.ToInt32(val));
                string name = Enum.GetName(enumType, val);
                dictionary.Add(stringvalue, name);
            }

            return dictionary;
        }

        public static string GetStringValue<TEnum>(int val) where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Enumeration type is expected");

            return StringEnum.GetStringValue<TEnum>(Convert.ToInt32(val)) ?? Enum.GetName(enumType, val);
        }

        public static int GetEnumValueByName<TEnum>(string name) where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new ArgumentException("Enumeration type is expected");

            foreach (var val in Enum.GetValues(enumType))
            {
                string enumName = GetStringValue<TEnum>(Convert.ToInt32(val));
                if (enumName == name)
                    return Convert.ToInt32(val);
            }

            throw new ArgumentOutOfRangeException("Enumeration value not found");
        }
    }
}
