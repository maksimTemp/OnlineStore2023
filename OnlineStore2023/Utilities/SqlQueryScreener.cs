using System.Linq;
using System.Text.RegularExpressions;

namespace OnlineStore2023.Utilities
{
    public class SqlQueryScreener
    {
        private readonly string[] _bannedWords = { "DROP", "DELETE", "INSERT", "UPDATE", "ALTER", "EXEC", "UNION", "--", "xp_" };
        private readonly Regex _sqlInjectionRegex = new Regex(@"(;|--|;|\/\*|\*\/|\+|\@|\[|\]|\(|\)|\$|\%|\^|\&|\*|\!|\|)", RegexOptions.IgnoreCase);
        public bool IsValidSqlQuery(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return false;
            }
            // Check if input string contains any banned keywords
            if (_bannedWords.Any(inputString.Contains))
            {
                return false;
            }
            // Check if input string matches any SQL injection regex patterns
            if (_sqlInjectionRegex.IsMatch(inputString))
            {
                return false;
            }
            return true;
        }
    }
}

