using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace OnlineStore2023.Utilities
{
    internal class PasswordHashGenerator
    {
        private readonly string Peper = "Amogus";

        public PasswordHashGenerator()
        {
        }

        public string GenerateHash(string userPassword, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Peper + userPassword + salt);
            var sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
