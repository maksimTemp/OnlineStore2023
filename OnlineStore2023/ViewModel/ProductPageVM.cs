using OnlineStore2023.Commands;
using OnlineStore2023.DataContext;
using OnlineStore2023.Managers;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing.Imaging;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;

namespace OnlineStore2023.ViewModel
{
    public class ProductPageVM : INotifyPropertyChanged
    {
        #region Fields
        private MainWindowVM _mainVM;
        private string _name;
        private byte[] _image;
        private decimal _price;
        private ObservableCollection<SpecifitationModel> _specification;
        private string _description;
        private int _noOfAvaible;
        private ObservableCollection<Tuple<int, SimpleListModel>> _navList;
        private Visibility _loadingScreen;
        private bool _inWishlist;
        private View.ProductPage _productPage;
        #endregion

        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;

                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public byte[] Image
        {
            get { return _image; }
            set
            {
                if (value == _image) return;

                _image = value;
                OnPropertyChanged("Images");
            }
        }
        public decimal Price
        {
            get { return _price; }
            set
            {
                if (value == _price) return;

                _price = value;
                OnPropertyChanged("Price");
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;

                _description = value;
                OnPropertyChanged("Description");
            }
        }
        public ObservableCollection<SpecifitationModel> Specification
        {
            get { return _specification; }
            set
            {
                if (value == _specification) return;

                _specification = value;
                OnPropertyChanged("Specification");
            }
        }
        public int NoOfAvaible
        {
            get { return _noOfAvaible; }
            set
            {
                if (value == _noOfAvaible) return;

                _noOfAvaible = value;
                OnPropertyChanged("NoOfAvaible");
            }
        }
        public ObservableCollection<Tuple<int, SimpleListModel>> NavList
        {
            get { return _navList; }
            set
            {
                if (value == _navList) return;

                _navList = value;
                OnPropertyChanged("NavList");
            }
        }
        public Guid CurrentProductId { get; set; }
        public Visibility LoadingScreen
        {
            get { return _loadingScreen; }
            set
            {
                if (value == _loadingScreen) return;

                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
        }
        public bool InWhishlist
        {
            get { return _inWishlist; }
            set
            {
                if (value == _inWishlist) return;

                _inWishlist = value;
                OnPropertyChanged("InWhishlist");
            }
        }
        #endregion

        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public SimpleCommand GoBackCommand { get; set; }
        public SimpleCommand GoHomeCommand { get; set; }
        public SimpleCommand AddToCartCommand { get; set; }
        public ParameterCommand ChangeImageCommand { get; set; }
        public ParameterCommand ChangeCategoryCommand { get; set; }

        public ProductPageVM()
        {
            GoBackCommand = new SimpleCommand(GoBack);
            GoHomeCommand = new SimpleCommand(GoHome);
            AddToCartCommand = new SimpleCommand(AddToCart);

            Name = "Name";
            //Image = new byte[];
            Price = 0.0M;
            Description = "Description";
            NavList = new ObservableCollection<Tuple<int, SimpleListModel>>();
            Specification = new ObservableCollection<SpecifitationModel>();
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 2));
        }
        public ProductPageVM(View.ProductPage page, MainWindowVM mvm, Guid productId) : this()
        {
            if (productId == Guid.Empty)
            {
                string mess = "При загрузке продукта произошла ошибка!\n";
                StandardMessages.Error(mess + "Неправильный номер товара!");
                return;
            }
            _productPage = page;
            _mainVM = mvm;
            LoadingScreenProcess(() =>
            {
                GetInfo(productId);
            });
        }

        private void AddToCart()
        {
            if (CurrentProductId == Guid.Empty) return;

            if (_mainVM.Cart.ContainsKey(CurrentProductId))
            {
                MessageQueue.Enqueue("Продукт уже добавлен в вашу корзину");
            }
            else
            {
                _mainVM.Cart[CurrentProductId] = 1;
                _mainVM.UpdateCart();
                MessageQueue.Enqueue("Продукт добавлен в вашу корзину");
            }
        }

        private void GoHome()
        {
            _mainVM.LoadPage("categories");
        }

        private void GoBack()
        {
            _mainVM.NavigationGoBack();
        }

        private void GetInfo(Guid prodId)
        {
            Product obj = new Product();

            try
            {
                var dataContext = new OnlineShop2022Context();
                obj = dataContext.Products.Where(x => x.ProductId == prodId).ToList()[0];

            }
            catch (Exception e)
            {
                string mess = "При получении объекта продукта произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }

            CurrentProductId = obj.ProductId;
            Name = obj.ProductName;
            //Image = 
            var x = obj.ProductPhoto;
            Price = obj.ProductPrice;
            Description = obj.ProductDescription;
            Specification = new ObservableCollection<SpecifitationModel>(GetProductSpecification(prodId));
            NoOfAvaible = (int)obj.ProductQuantity;
        }
        private List<SpecifitationModel> GetProductSpecification(Guid prodId)
        {
            List<SpecifitationModel> output = new List<SpecifitationModel>();

            try
            {
                ProductRepository productRepository = new();
                output = productRepository.GetProductSpecifitation(prodId);
            }
            catch (Exception e)
            {
                string mess = "При получении характеристик продукта произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }

            return output;
        }
        private void LoadingScreenProcess(Action action)
        {
            LoadingScreen = Visibility.Visible;

            Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
