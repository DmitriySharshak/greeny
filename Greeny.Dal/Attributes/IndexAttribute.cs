using System;

namespace Greeny.Dal
{
    /// <summary>
    /// Атрибут, задающий индекс для таблицы
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class IndexAttribute : Attribute
    {
        public IndexAttribute(string name, params string[] columns)
        {
            Name = name;
            Columns = columns;
        }

        public string Name { get; set; }

        public string Using { get; set; }

        public string[] Columns { get; set; }

        public bool IsUnique { get; set; }
    }
}
