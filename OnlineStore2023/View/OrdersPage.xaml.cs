using OnlineStore2023.ViewModel;
using System.Windows.Controls;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage(AccountVM accountVM)
        {
            InitializeComponent();
            this.DataContext = new AdminOrdersPageVM(accountVM);
        }
    }
}
