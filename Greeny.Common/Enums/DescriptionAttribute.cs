namespace Greeny.Common.Enums
{
    public class DescriptionAttribute: Attribute
    {
        private string _description = null;
        private string _shortName = null;

        public DescriptionAttribute(string description)
        {
            _description = description;
            _shortName = string.Empty;
        }

        public DescriptionAttribute(string description, string shortName)
        {
            _description = description;
            _shortName = shortName;
        }

        public string Description
        {
            get { return _description; }
        }

        public string ShortName
        {
            get { return _shortName; }
        }
    }
}
