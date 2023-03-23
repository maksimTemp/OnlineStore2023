using OnlineStore2023.DataContext.Models;
using OnlineStore2023.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Логика взаимодействия для OrderDetailPage.xaml
    /// </summary>
    public partial class OrderDetailPage : Page
    {
        public OrderDetailPage(Order setectedOrder, AccountVM accountVM)
        {
            InitializeComponent();
            this.DataContext = new OrderDetailPageVM(setectedOrder, accountVM);
        }
    }
}
