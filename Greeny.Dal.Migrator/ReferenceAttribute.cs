namespace Greeny.Dal.Migration
{
    /// <summary>
    /// Атрибут, задающий индекс для таблицы
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class ReferenceAttribute : Attribute
    {
        public ReferenceAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string Parent { get; set; }

        public string ParentKey { get; set; }
        public string ChildKey { get; set; }

    }
}
