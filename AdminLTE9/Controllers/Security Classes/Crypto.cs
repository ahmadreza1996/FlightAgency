using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace AdminLTE9
{
    public static class Crypto
    {
        // Create standard hash from password string.
        public static string Hash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return Convert.ToBase64String(algorithm.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}