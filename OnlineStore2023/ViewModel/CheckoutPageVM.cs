using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class CheckoutPageVM : BaseAbstractVMPage
    {

        #region Fields
        private MainWindowVM _mainVM;

        private ObservableCollection<CartModel> _cart;
        private decimal _productSum;
        private string _address;
        private Customer _currentCustomer;
        #endregion

        #region Properties
        public ObservableCollection<CartModel> Cart
        {
            get { return _cart; }
            set
            {
                if (value == _cart) return;

                _cart = value;
                OnPropertyChanged("Cart");
            }
        }
        public decimal ProductSum
        {
            get { return _productSum; }
            set
            {
                if (value == _productSum) return;

                _productSum = value;
                OnPropertyChanged("ProductSum");
                OnPropertyChanged("AllSum");
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;

                _address = value;
                OnPropertyChanged("Address");
            }
        }
        public Customer CurrentCustomer
        {
            get { return _currentCustomer; }
            set
            {
                if (value == _currentCustomer) return;

                _currentCustomer = value;
                OnPropertyChanged("CurrentCustomer");
            }
        }
        public decimal AllSum
        {
            get { return _productSum; }
        }
        #endregion

        #region Commands
        public SimpleCommand ConfirmCommand { get; set; }
        #endregion

        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }
        public CheckoutPageVM(MainWindowVM mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));

            ConfirmCommand = new SimpleCommand(Confirm);

            LoadingScreenProcess(() =>
            {
                Cart = new ObservableCollection<CartModel>(GetCartList());
                ProductSum = SumProducts();
                GetUserDefaultData();
            });
        }
        private void Confirm()
        {
            if (string.IsNullOrWhiteSpace(Address))
            {
                MessageQueue.Enqueue("Введите адресс");
                return;
            }

            Address = Address.Trim();

            Order o = new();
            o.CustomerId = AccountManager.LoggedId;
            o.OrderStatus = "Проверяется сотрудниками";
            o.DeliveryAddress = Address;
            o.TotalPrice = ProductSum;

            OrderRepository orderRepository = new();
            ProductRepository productRepository = new();

            try
            {
                foreach (var p in Cart)
                {
                    o.ProductOrders.Add(new ProductOrder() { ProductId = p.ProductId, SelectedQuantity = p.Quantity, ProductsInOrderTotalPrice = p.PriceSum });
                    var prod = productRepository.GetProductById(p.ProductId);
                    prod.ProductQuantity -= p.Quantity;
                    productRepository.Update(prod);
                    productRepository.SaveChanges();
                }
                orderRepository.Orders.Add(o);
                orderRepository.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Произошла ошибка при сохранении заказа!\n";
                StandardMessages.Error(mess + e.Message);
                return;
            }

            _mainVM.Cart.Clear();
            _mainVM.ItemsInCart = 0;
            //Load orders screen!
            _mainVM.LoadCustomAccountPage("orders");
        }
        private decimal SumProducts()
        {
            decimal output = 0;

            foreach (var prod in Cart)
                output += prod.Price * prod.Quantity;

            return output;
        }
        private List<CartModel> GetCartList()
        {
            List<CartModel> output = new List<CartModel>();

            try
            {
                ProductRepository productRepository = new();
                foreach (var key in _mainVM.Cart.Keys)
                {
                    CartModel prod = new CartModel();
                    prod.ProductId = key;
                    prod.ProductName = productRepository.Products.FirstOrDefault(x => x.ProductId == key).ProductName;
                    prod.Price = productRepository.Products.FirstOrDefault(x => x.ProductId == key).ProductPrice;
                    prod.Quantity = _mainVM.Cart[key];
                    output.Add(prod);
                }
            }
            catch (Exception e)
            {
                string mess = "При получении списка товаров из корзины произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void GetUserDefaultData()
        {
            try
            {
                CustomerRepository customerRepository = new();
                CurrentCustomer = customerRepository.GetCustomerById((Guid)AccountManager.LoggedId);
                Address = CurrentCustomer.CustomerAdressLine;
            }
            catch (Exception e)
            {
                string mess = "Произошла ошибка при получении данных клиента по умолчанию!\n";
                StandardMessages.Error(mess + e.Message);
            }

        }
        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }

        protected override void LoadData()
        {
            throw new NotImplementedException();
        }
    }
}
