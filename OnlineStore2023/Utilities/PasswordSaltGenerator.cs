using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.Utilities
{
    public class PasswordSaltGenerator
    {
        public string GetRandomString(int length)
        {
            Random rnd = new Random();
            StringBuilder sb = new StringBuilder(length - 1);
            string alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            int Position = 0;

            for (int i = 0; i < length; i++)
            {
                Position = rnd.Next(0, alphabet.Length - 1);
                sb.Append(alphabet[Position]);
            }

            return sb.ToString();
        }
    }
}
