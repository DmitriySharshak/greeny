namespace Greeny.Dal.Migration
{
    public static class NullableHelper
    {
        public static bool IsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static Type GetUnderlyingType(Type nullableType)
        {
            Type result = null;
            if (IsNullable(nullableType))
            {
                Type genericType = nullableType.GetGenericTypeDefinition();
                if (Object.ReferenceEquals(genericType, typeof(Nullable<>)))
                {
                    result = nullableType.GetGenericArguments()[0];
                }
            }
            return result;
        }
    }
}
