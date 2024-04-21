namespace Greeny.Common.Models
{
    public sealed class CategoryHash
    {
        public int Hash { get; init; }
        public IReadOnlyCollection<Category> List { get; init; }
    }
}
