using OnlineStore2023.ViewModel;
using System.Windows.Controls;

namespace OnlineStore2023.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
            this.DataContext = new HomeVM();
        }
    }
}
