using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class AdminProductAddEditPageVM : AdminAbstractVMAddEditPage
    {
        #region Fields
        private Product _selectedProduct;
        private Product _curProduct;

        private readonly ObservableCollection<string> _productArchiveStates = new()
        {
            "True",
            "False",
        };

        private Visibility _deletePhotoSectionVisibility;
        private Visibility _addPhotoSectionVisibility;
        #endregion

        #region Properties
        public Product CurProduct
        {
            get => _curProduct;
            set
            {
                if (value == _curProduct) return;

                _curProduct = value;
                OnPropertyChanged("CurProduct");
            }
        }

        public string IsArchiveString
        {
            get
            {
                return CurProduct.IsArchive.ToString();
            }
            set
            {
                CurProduct.IsArchive = Convert.ToBoolean(value);
                OnPropertyChanged("IsActiveString");
            }
        }
        public ObservableCollection<string> ProductArchiveStates => _productArchiveStates;

        public Visibility DeletePhotoSectionVisibility
        {
            get { return _deletePhotoSectionVisibility; }
            set
            {
                if (value == _deletePhotoSectionVisibility) return;

                _deletePhotoSectionVisibility = value;
                OnPropertyChanged("DeletePhotoSectionVisibility");
            }
        }
        public Visibility AddPhotoSectionVisibility
        {
            get { return _addPhotoSectionVisibility; }
            set
            {
                if (value == _addPhotoSectionVisibility) return;

                _addPhotoSectionVisibility = value;
                OnPropertyChanged("AddPhotoSectionVisibility");
            }
        }

        #endregion

        #region Commands
        public SimpleCommand AddCommand { get; set; }
        public SimpleCommand DeleteCommand { get; set; }
        #endregion

        public AdminProductAddEditPageVM(Product setectedProduct, AccountVM accountVM) : base(setectedProduct, accountVM)
        {
            _selectedProduct = setectedProduct;

            //AddCommand = new SimpleCommand(AddProductPhoto);
            //DeleteCommand = new SimpleCommand(DeleteProductPhoto);

            Refresh();
            LoadingScreen = Visibility.Collapsed;
        }

        protected override void LoadData()
        {
            if (_selectedProduct == null)
            {
                _curProduct = new Product();
            }
            else
            {
                _curProduct = _selectedProduct;
            }

            if (_curProduct.ProductPhoto != null)
            {
                DeletePhotoSectionVisibility = Visibility.Visible;
                AddPhotoSectionVisibility = Visibility.Collapsed;
            }
            else
            {
                DeletePhotoSectionVisibility = Visibility.Collapsed;
                AddPhotoSectionVisibility = Visibility.Visible;
            }
        }

        protected override void SaveEntity()
        {
            MessageQueue.Clear();

            if (CheckInputValues())
            {
                MessageQueue.Enqueue("Сохранение...");
                RunInBackground(() =>
                {
                    SaveInputValues();
                }, () =>
                {
                    MessageQueue.Enqueue("Сохранено!");
                    Thread.Sleep(2000);
                    _accountVM.FrameView = new AdminProductListPage(_accountVM);
                });
            }
        }

        protected override bool CheckInputValues()
        {
            if (string.IsNullOrWhiteSpace(CurProduct.ProductName))
            {
                MessageQueue.Enqueue("Название товара не может быть пустым");
                return false;
            }
            if (CurProduct.ProductPrice == 0)
            {
                MessageQueue.Enqueue("Заполните цену товара");
                return false;
            }

            CurProduct.ProductName = CurProduct.ProductName.Trim();
            CurProduct.ProductName = char.ToUpper(CurProduct.ProductName[0]) + CurProduct.ProductName.Substring(1);

            if (!InputStringValidator.ValidateProductName(CurProduct.ProductName))
            {
                MessageQueue.Enqueue("Название товара не корректное");
                return false;
            }
            if (CurProduct.ProductPrice < 0)
            {
                MessageQueue.Enqueue("Цена товара не может быть отрицательной");
                return false;
            }
            if (CurProduct.ProductWeight < 0)
            {
                MessageQueue.Enqueue("Вес товара не может быть отрицательным");
                return false;
            }
            if (CurProduct.ProductVolume < 0)
            {
                MessageQueue.Enqueue("Объём товара не может быть отрицательным");
                return false;
            }

            return true;
        }

        protected override void SaveInputValues()
        {
            try
            {
                var productRepo = new ProductRepository();

                if (_selectedProduct != null)
                {
                    productRepo.Products.Update(_curProduct);
                }
                else
                {
                    productRepo.Products.Add(_curProduct);
                }
                productRepo.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Во время загрузки данных произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
                return;
            }
        }

        protected override void GoBack()
        {
            _accountVM.FrameView = new AdminProductListPage(_accountVM);
        }
    }
}
