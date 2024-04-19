namespace Greeny.Core
{
    public class GreenyException: Exception
    {
        public GreenyException(string message) : base(message) { }
        public GreenyException(string message, Exception innerException) : base(message, innerException) { }
        public GreenyException(Exception innerException) : base("Операция не возможна по техническим причинам.", innerException) { }
    }
}
