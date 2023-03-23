using OnlineStore2023.DataContext;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Utilities;
using OnlineStore2023.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для AdminAddEditProductPage.xaml
    /// </summary>
    public partial class AdminProductAddEditPage : Page
    {
        public AdminProductAddEditPage(Product setectedProduct, AccountVM accountVM)
        {
            InitializeComponent();
            this.DataContext = new AdminProductAddEditPageVM(setectedProduct, accountVM);
        }

    }
}
