using OnlineStore2023.ViewModel;
using System.Windows.Controls;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для AdminProductListPage.xaml
    /// </summary>
    public partial class AdminProductListPage : Page
    {
        public AdminProductListPage(AccountVM accountVM)
        {
            InitializeComponent();
            this.DataContext = new AdminProductListPageVM(accountVM);
        }
    }
}
