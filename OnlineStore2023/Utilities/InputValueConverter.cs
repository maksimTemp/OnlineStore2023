using System;
using System.Globalization;

namespace OnlineStore2023.Utilities
{
    internal class InputValueConverter
    {
        public Tuple<Guid,string> ParseGuidFromString(string inputString)
        {
            try
            {
                ;
                var inputValue = Guid.Parse(inputString);
                if (inputValue == Guid.Empty || !Guid.TryParse(inputString, out inputValue))
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<Guid, string>(inputValue, null);
            }
            catch (Exception e)
            {
                return new Tuple<Guid, string>(Guid.Empty, e.ToString());
            }
        }

        public Tuple<DateTime, string> ParseDateTimeFromString(string inputString)
        {
            try
            {
                var inputValue = DateTime.ParseExact(inputString, "dd-MM-yyyy", CultureInfo.GetCultureInfo("ru-RU"));
                if (inputValue == DateTime.MinValue || !DateTime.TryParse(inputString, out inputValue))
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<DateTime, string>(inputValue, null);
            }
            catch (Exception e)
            {
                return new Tuple<DateTime, string>(DateTime.MinValue, e.ToString());
            }
        }

        public Tuple<decimal, string> ParseDecimalFromString(string inputString)
        {
            try
            {
                var inputValue = decimal.Parse(inputString);
                if (inputValue < 0 || !decimal.TryParse(inputString, out inputValue))
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<decimal, string>(inputValue, null); ;
            }
            catch (FormatException e)
            {
                return new Tuple<decimal, string>(decimal.MinValue, e.ToString());
            }
        }

        public string CheckStringLength(string inputString, byte inputStringLenth)
        {
            if (inputString.Length < 0 || inputString.Length > inputStringLenth)
                throw new Exception("The value you entered is not valid");
            else
                return inputString;
        }

        public Tuple<int, string> ParseIntFromString(string inputString, int maxSize)
        {
            try
            {
                var inputValue = Convert.ToInt32(inputString);
                if (inputValue < 0 || inputValue > maxSize)
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<int, string>(inputValue, null);
            }
            catch (FormatException e)
            {
                return new Tuple<int, string>(int.MinValue, e.ToString());
            }
        }
        public Tuple<int, string> ParseIntFromString(string inputString)
        {
            try
            {
                var inputValue = Convert.ToInt32(inputString);
                if (inputValue < 0)
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<int, string>(inputValue, null);
            }
            catch (FormatException e)
            {
                return new Tuple<int, string>(int.MinValue, e.ToString());
            }
        }
        public Tuple<bool, string> ParseBoolFromString(string inputString)
        {
            try
            {
                bool inputValue;
                if(!bool.TryParse(inputString,out inputValue))
                    throw new Exception("Введенное вами значение некорректно");
                return new Tuple<bool, string>(inputValue, null);
            }
            catch (FormatException e)
            {
                return new Tuple<bool, string>(false, e.ToString());
            }
        }
    }
}
