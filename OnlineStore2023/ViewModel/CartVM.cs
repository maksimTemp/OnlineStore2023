using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class CartVM : BaseAbstractVMPage
    {
        #region Fields
        private MainWindowVM _mainVM;

        private ObservableCollection<CartModel> _cart;
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
        #endregion

        #region Commands
        public SimpleCommand OrderProcessCommand { get; set; }
        public ParameterCommand RemoveElementCommand { get; set; }
        #endregion


        public CartVM(MainWindowVM mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue();

            OrderProcessCommand = new SimpleCommand(ProcessOrder);
            RemoveElementCommand = new ParameterCommand(RemoveElement);

            LoadingScreenProcess(() =>
            {
                Cart = new ObservableCollection<CartModel>(GetCartList());
            });
        }

        private void ProcessOrder()
        {
            MessageQueue.Clear();

            if (Cart.Count == 0)
            {
                MessageQueue.Enqueue("Корзина пуста!", null, null, null, false, false, new TimeSpan(0, 0, 4));
                return;
            }
            if (AccountManager.LoggedId == null)
            {
                MessageQueue.Enqueue("Вы должны войти в систему, чтобы разместить заказ", null, null, null, false, false, new TimeSpan(0, 0, 4));
                return;
            }

            ProductRepository productRepository = new();

            foreach (var element in Cart)
            {
                _mainVM.Cart[element.ProductId] = element.Quantity;
                var productFromDb = productRepository.GetProductById(element.ProductId);
                if (productFromDb.ProductQuantity < element.Quantity)
                {
                    MessageQueue.Enqueue($"Выбранное колличество товара {element.ProductName} превосходит допустимое", null, null, null, false, false, new TimeSpan(0, 0, 4));
                    MessageQueue.Enqueue($"Допустимое колличество можно найти в карточке товара", null, null, null, false, false, new TimeSpan(0, 0, 4));
                    return;
                }

            }


            _mainVM.LoadPage("checkout");
        }
        private List<CartModel> GetCartList()
        {
            List<CartModel> output = new List<CartModel>();

            try
            {
                var prodRepo = new ProductRepository();
                foreach (var key in _mainVM.Cart.Keys)
                {
                    var nameAndPrice = prodRepo.GetProductNameAndPriceById(key);
                    CartModel prod = new CartModel();
                    prod.ProductId = key;
                    prod.ProductName = nameAndPrice.Item1;
                    prod.Price = nameAndPrice.Item2;
                    prod.Quantity = _mainVM.Cart[key];
                    output.Add(prod);
                }
            }
            catch (Exception e)
            {
                string mess = "Произошла ошибка при извлечении списка товаров из корзины!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void RemoveElement(object param)
        {
            var element = param as CartModel;
            Cart.Remove(element);
            _mainVM.Cart.Remove(element.ProductId);
            _mainVM.UpdateCart();
        }
        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }

        protected override void LoadData() { }
    }
}
