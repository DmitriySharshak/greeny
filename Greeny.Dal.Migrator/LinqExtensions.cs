namespace Greeny.Dal.Migration
{
    internal static class LinqExtensions
    {
        public static string Concat(this IEnumerable<string> items, string separator)
        {
            return string.Join(separator, items.ToArray());
        }
    }
}
