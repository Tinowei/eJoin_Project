using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Extensions
{
    public static class HasherExtension
    {
        public static string HashPassword(this string origin)
        {
            byte[] source = Encoding.Default.GetBytes(origin);
            using (var mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(source);
                string result = hashValue.Aggregate(string.Empty, (current, t) => current + t.ToString("X2"));
                return result.ToUpper();
            }
        }
    }
}
