using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    class AdminProductListPageVM : AdminAbstractVMPageWithGrid
    {
        #region Fields

        private List<Product> _products;
        private object _selectedProduct;

        #endregion

        #region Properties
        public List<Product> Products
        {
            get => _products;
            set
            {
                if (value == _products) return;

                _products = value;
                OnPropertyChanged("Products");
            }
        }
        public object SelectedProduct
        {
            get => _selectedProduct; set
            {
                if (value == _selectedProduct) return;

                _selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }
        #endregion

        public AdminProductListPageVM(AccountVM accountVM) : base(accountVM)
        {
            Refresh();
        }

        private void DeleteProduct(Product product)
        {
            try
            {
                var repo = new ProductRepository();
                repo.Remove(product);
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                string mess = "В процессе удаления произошла ошибка!\n";
                StandardMessages.Error(mess + ex.Message);
            }
            Refresh();
        }

        protected override void AddNewEntity()
        {
            _accountVM.FrameView = new AdminProductAddEditPage(null, _accountVM);
        }

        protected override void EditEntity()
        {
            var selectedProduct = SelectedProduct;
            if (selectedProduct == null)
            {
                MessageQueue.Enqueue("Выберите одного работника для редактирования!");
                return;
            }
            _accountVM.FrameView = new AdminProductAddEditPage((Product)selectedProduct, _accountVM);
        }

        protected override void RemoveEntity()
        {
            if (SelectedProduct == null)
            {
                MessageQueue.Enqueue("Выберите хотя бы один товар для удаления!");
                return;
            }
            var selectedProduct = (Product)SelectedProduct;

            if (MessageBox.Show($"Вы точно хотите удалить {selectedProduct.ProductName}?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MessageQueue.Enqueue("Удаление..", null, null, null, false, false, new TimeSpan(0, 0, 2));
                RunInBackground(() => DeleteProduct(selectedProduct));
            }
        }

        protected override void SearchEntity()
        {
            if (!CheckSearchSectionFields())
            { return; }
            LoadingScreen = VisibilitySupport.ChangeVisibility(LoadingScreen);
            MessageQueue.Enqueue("Поиск...");
            RunInBackground(() =>
            {
                SearchByСriteria(SearchRequest, CurrentCategory);
            }, () =>
            {
                LoadingScreen = VisibilitySupport.ChangeVisibility(LoadingScreen);
                MessageQueue.Enqueue($"По вашему запросу было найдено {Products.Count} продуктов");
            });
        }

        protected override void LoadData()
        {
            SearchCategory = new()
            {
                "ID Товара",
                "Название",
                "Цена",
                "Вес",
                "Цвет",
                "Архивный"
            };

            try
            {
                ProductRepository productRepository = new();
                Products = productRepository.GetProductList();
            }
            catch (Exception ex)
            {
                string mess = "Произошла ошибка при получении списка Работников!\n";
                StandardMessages.Error(mess + ex.Message);
            }
        }

        protected override void SearchByСriteria(string inputString, string type)
        {
            Products.Clear();

            InputValueConverter inputValueConverter = new();
            ProductRepository productRepository = new();
            SqlQueryScreener screener = new();
            string errorMessage = null;
            if (screener.IsValidSqlQuery(inputString))
            {
                try
                {
                    switch (type)
                    {
                        case "ID Товара":
                            {
                                var guidTupleValue = inputValueConverter.ParseGuidFromString(inputString);
                                if (guidTupleValue.Item1 != Guid.Empty)
                                {
                                    Products = productRepository.GetProductListById(guidTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = guidTupleValue.Item2;
                                }
                            }
                            break;
                        case "Название":
                            Products = productRepository.GetProductListByName(inputString);
                            break;
                        case "Цена":
                            {
                                var decimalTupleValue = inputValueConverter.ParseDecimalFromString(inputString);
                                if (decimalTupleValue.Item1 != decimal.MinValue)
                                {
                                    Products = productRepository.GetProductListByPrice(decimalTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = decimalTupleValue.Item2;
                                }
                            }
                            break;
                        case "Вес":
                            {
                                var decimalTupleValue = inputValueConverter.ParseDecimalFromString(inputString);
                                if (decimalTupleValue.Item1 != decimal.MinValue)
                                {
                                    Products = productRepository.GetProductListByWeight(decimalTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = decimalTupleValue.Item2;
                                }
                            }
                            break;
                        case "Цвет":
                            Products = productRepository.GetProductListByColor(inputString);
                            break;
                        case "Архивный":
                            {
                                var boolTupleValue = inputValueConverter.ParseBoolFromString(inputString);
                                if (boolTupleValue.Item2 != null)
                                {
                                    errorMessage = boolTupleValue.Item2;
                                }
                                else
                                {
                                    Products = productRepository.GetProductListByStatus(boolTupleValue.Item1);
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageQueue.Enqueue(ex.ToString());
                    return;
                }
            }
            else
            {
                MessageQueue.Enqueue("АТАТА ИНЪЕКЦИИ ПИСАТЬ!");
                return;
            }
            if (errorMessage != null)
            {
                MessageQueue.Enqueue(errorMessage);
                return;
            }
            else if (Products.Count == 0)
            {
                MessageQueue.Enqueue("По вашему запросу ничего не найдено");
                return;
            }
            else
            {
                return;
            }
        }
    }
}
