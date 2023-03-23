using OnlineStore2023.ViewModel;
using System.Windows.Controls;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для AdminOrdersPage.xaml
    /// </summary>
    public partial class AdminOrdersPage : Page
    {
        public AdminOrdersPage(AccountVM accountVM)
        {
            InitializeComponent();
            DataContext = new AdminOrdersPageVM(accountVM);
        }
    }
}
