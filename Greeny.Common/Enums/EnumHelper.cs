using System.Reflection;

namespace Greeny.Common.Enums
{
    public static class EnumHelper
    {
        private static DescriptionAttribute GetDescriptionAttribute(object value)
        {
            DescriptionAttribute attribute = null;

            if (value == null)
                return null;

            Type enumType = value.GetType();

            if (!enumType.IsEnum)
                throw new ArgumentException("Argument type is not Enum.", "value");

            FieldInfo fieldInfo = enumType.GetField(value.ToString(), BindingFlags.Static | BindingFlags.Public);

            if (fieldInfo != null)
            {
                attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                if (attribute == null)
                    throw new ArgumentException(string.Format("Enum '{0}' value '{1}' does not have 'DescriptionAttribute' attribute.", enumType, value), "value");
            }
            else
                throw new ArgumentException(string.Format("Argument is not '{0}' Enum value. ({1})", enumType, value), "value");

            return attribute;
        }

        public static string GetDescription(object value)
        {
            var attr = GetDescriptionAttribute(value);

            return attr == null ? string.Empty : attr.Description;
        }

        public static string GetShortName(object value)
        {
            var attr = GetDescriptionAttribute(value);

            return attr == null ? string.Empty : attr.ShortName;
        }

        public static string GetDescription(this Enum val)
        {
            return GetDescription((object)val);
        }

    }
}
