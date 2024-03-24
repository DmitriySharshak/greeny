namespace Greeny.Dal.Migrator.Internal
{
    internal static class NullableHelper
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetUnderlyingType(Type type)
        {
            Type result = null;
            if (IsNullable(type))
            {
                Type genericType = type.GetGenericTypeDefinition();
                if (ReferenceEquals(genericType, typeof(Nullable<>)))
                {
                    result = type.GetGenericArguments()[0];
                }
            }
            return result;
        }
    }
}
