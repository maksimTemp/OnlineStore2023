using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using OnlineStore2023.Commands;
using OnlineStore2023.Managers;
using OnlineStore2023.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Fields
        private MainWindow _mainWindow;
        private object _mainFrame;
        private Dictionary<Guid, int> _cart;
        private int? _itemsInCart;
        #endregion

        #region Properties
        public object MainFrame
        {
            get { return _mainFrame; }
            set
            {
                if (value == _mainFrame) return;

                _mainFrame = value;
                OnPropertyChanged("MainFrame");
            }
        }
        public Dictionary<Guid, int> Cart
        {
            get { return _cart; }
            set
            {
                if (value == _cart) return;

                _cart = value;
                OnPropertyChanged("Cart");

                if (_cart != null)
                    ItemsInCart = _cart.Count;
            }
        }
        public int? ItemsInCart
        {
            get { return (_itemsInCart == 0) ? null : _itemsInCart; }
            set
            {
                if (value == _itemsInCart) return;

                _itemsInCart = value;
                OnPropertyChanged("ItemsInCart");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand CloseWindowCommand { get; set; }
        public SimpleCommand MinimalizeWindowCommand { get; set; }
        public ParameterCommand LoadScreenCommand { get; set; }
        #endregion

        public MainWindowVM(MainWindow main)
        {
            _mainWindow = main;

            CloseWindowCommand = new SimpleCommand(CloseWindow);
            MinimalizeWindowCommand = new SimpleCommand(MinimalizeWindow);
            LoadScreenCommand = new ParameterCommand(LoadScreen);
            Cart = new Dictionary<Guid, int>();
            LoadPage("home");
        }
        public void UpdateCart()
        {
            if (Cart == null) return;

            ItemsInCart = Cart.Count;
            OnPropertyChanged("Cart");
            OnPropertyChanged("ItemsInCart");
        }
        public void LoadPage(string name)
        {
            LoadScreen(name);
        }
        public void LoadProductList(uint? categoryID, string search)
        {
            //MainFrame = new ProductList(this, categoryID, search);
        }
        public void LoadProduct(Guid prodId)
        {
            MainFrame = new ProductPage(this, prodId);
        }
        public void LoadCustomAccountPage(string page)
        {
            if (page != null)
                MainFrame = new AccountPage(this, page);
            else
                MainFrame = new AccountPage(this);
        }
        private void LoadScreen(object param)
        {
            var txt = param as string;
            switch (txt)
            {
                case "home":
                    MainFrame = new Home();
                    break;
                case "product list":
                    MainFrame = new ProductsListPage(this);
                    break;
                case "cart":
                    MainFrame = new CartPage(this);
                    break;
                case "account":
                    if (AccountManager.LoggedId == null)
                        MainFrame = new LoginPage(this);
                    else
                        MainFrame = new AccountPage(this);
                    break;
                case "checkout":
                    MainFrame = new CheckoutPage(this);
                    break;
                default:
                    break;
            }
        }
        public void NavigationGoBack()
        {
            if (_mainWindow.frameMain.CanGoBack)
                _mainWindow.frameMain.GoBack();
        }
        private void CloseWindow()
        {
            _mainWindow.Close();
        }
        private void MinimalizeWindow()
        {
            _mainWindow.WindowState = System.Windows.WindowState.Minimized;
        }
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
