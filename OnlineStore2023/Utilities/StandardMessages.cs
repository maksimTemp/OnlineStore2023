using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.Utilities
{
    public class StandardMessages
    {
        public static void Error(string message)
        {
            MessageBox.Show("Произошла ошибка!\n" + message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
        }
    }
}
