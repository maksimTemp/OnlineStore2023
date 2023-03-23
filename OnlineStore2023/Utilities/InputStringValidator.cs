using System;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace OnlineStore2023.Utilities
{
    class InputStringValidator
    {
        public static bool ValidateEmail(string email)
        {
            string simplePattern = @"^\b[\w\.-]+@[\w\.-]+\.\w{2,}\b$";
            if (!Regex.IsMatch(email, simplePattern)) return false;
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool ValidatePhoneNumber(string phone)
        {
            string pattern = @"^(\d{9})$|^([+]\d{11})$";
            return Regex.IsMatch(phone, pattern);
        }
        public static bool ValidateName(string name)
        {
            string pattern = @"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$";
            return Regex.IsMatch(name, pattern);
        }
        public static bool ValidateNumber(string inputString, int inputStringLength)
        {
            string pattern = null;
            //to check the Russian passport series
            if (inputStringLength == 4)
            {
                pattern = @"\d{4}";
            }
            //to check the Russian passport number
            else if (inputStringLength == 6)
            {
                pattern = @"\d{6}";
            }

            return Regex.IsMatch(inputString, pattern);
        }
        public static bool ValidateNumber(int inputValue, int inputStringLength)
        {
            string inputStr = inputValue.ToString();
            string pattern = null;
            //to check the Russian passport series
            if (inputStringLength == 4)
            {
                pattern = @"\d{4}";
            }
            //to check the Russian passport number
            else if (inputStringLength == 6)
            {
                pattern = @"\d{6}";
            }

            return Regex.IsMatch(inputStr, pattern);
        }
        public static bool ValidateLoginOrPassword(string inputString)
        {
            //The conditions for a regular expression are: 
            //1) length of 8 to 50 characters.
            //2) there must be at least 1 small letter, 1 capital letter, 1 number, 1 character from(. or _)
            //3) the login must start with a lowercase or uppercase letter

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[._])[a-zA-Z][a-zA-Z0-9._]{7,49}$";
            return Regex.IsMatch(inputString, pattern);
        }

        public static bool ValidateProductName(string inputString)
        {
            //The conditions for a regular expression are: 
            //1) length of 8 to 50 characters.
            //2) there must be at least 1 small letter, 1 capital letter, 1 number, 1 character from(. or _)
            //3) the login must start with a lowercase or uppercase letter

            string pattern = "^(?=.{8,100})(?=.*[a-zA-Zа-яА-Я])[a-zA-Zа-яА-Я.,/\\-_\\!?$\"'*:#;\\[\\]=+()]+";
            return Regex.IsMatch(inputString, pattern);
        }
    }
}
