using MaterialDesignThemes.Wpf;
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
    /// Логика взаимодействия для ProductsListPage.xaml
    /// </summary>
    public partial class ProductsListPage : Page
    {
        public ProductsListPage(MainWindowVM mvm, string search = null)
        {
            InitializeComponent();
            this.DataContext = new ProductListVM(mvm, search);
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    DrawerHost.CloseDrawerCommand.Execute(Dock.Left, leftDrawer);
        //}
    }
}
