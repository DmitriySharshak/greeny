using System.Security.Cryptography;
using System.Text;

namespace Greeny.Core.Security
{
    internal static class CryptoHelper
    {
        public static string GetRandomString(int bytes)
        {
            var rndGenerator = new RNGCryptoServiceProvider();
            var rnd = new byte[bytes];
            rndGenerator.GetBytes(rnd);
            return Convert.ToBase64String(rnd);
        }

        public static string GetBase64Hash(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var hasher = new MD5CryptoServiceProvider();
            var result = hasher.ComputeHash(data);

            var hash = Convert.ToBase64String(result);

            hash = hash.Replace('+', '_');
            hash = hash.Replace('/', '_');
            return hash.TrimEnd('=');
        }
    }
}
