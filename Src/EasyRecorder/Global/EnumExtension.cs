﻿using System.ComponentModel;
using System.Reflection;

namespace ESR.Global
{
    public static class EnumExtension
    {
        public static string ToDescription(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
