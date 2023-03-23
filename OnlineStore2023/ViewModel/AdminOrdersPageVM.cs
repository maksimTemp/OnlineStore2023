using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.Collections.Generic;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class AdminOrdersPageVM : AdminAbstractVMPageWithGrid
    {
        #region Fields
        private List<Order> _orders;
        private object _selectedOrder;

        #endregion

        #region Properties
        public List<Order> Orders
        {
            get { return _orders; }
            set
            {
                if (value == _orders) return;

                _orders = value;
                OnPropertyChanged("Orders");
            }

        }
        public object SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (value == _selectedOrder) return;

                _selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand ShowOrderDetail { get; set; }
        #endregion

        public AdminOrdersPageVM(AccountVM accountVM) : base(accountVM)
        {
            ShowOrderDetail = new SimpleCommand(OpenOrderDetailPage);
        }

        private void DeleteOrder(Order order)
        {
            try
            {
                OrderRepository orderRepository = new();
                orderRepository.DeleteOrderByID(order.OrderId);
                orderRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                string mess = "В процессе удаления произошла ошибка!\n";
                StandardMessages.Error(mess + ex.Message);
            }
            Refresh();
        }

        private void OpenOrderDetailPage()
        {
            var selectedOrder = SelectedOrder;
            if (selectedOrder == null)
            {
                MessageQueue.Enqueue("Выберите один заказ для оторажения деталей!");
                return;
            }
            _accountVM.FrameView = new OrderDetailPage((Order)selectedOrder, _accountVM);
        }

        protected override void AddNewEntity() { }

        protected override void EditEntity()
        {
            var selectedOrder = SelectedOrder;
            if (selectedOrder == null)
            {
                MessageQueue.Enqueue("Выберите один заказ для редактирования!");
                return;
            }
            _accountVM.FrameView = new AdminOrderEditPageVM((Order)selectedOrder, _accountVM);
        }

        protected override void RemoveEntity()
        {
            if (SelectedOrder == null)
            {
                MessageQueue.Enqueue("Выберите хотя бы один товар для удаления!");
                return;
            }
            var selectedOrder = (Order)SelectedOrder;

            if (MessageBox.Show($"Вы точно хотите удалить {selectedOrder.OrderId}?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MessageQueue.Enqueue("Удаление..", null, null, null, false, false, new TimeSpan(0, 0, 2));
                RunInBackground(() => DeleteOrder(selectedOrder));
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
                MessageQueue.Enqueue($"По вашему запросу было найдено {Orders.Count} заказов");
            });
        }

        protected override void SearchByСriteria(string inputString, string type)
        {
            Orders.Clear();

            SqlQueryScreener screener = new();
            InputValueConverter inputValueConverter = new();
            OrderRepository orderRepository = new();
            string errorMessage = null;
            if (screener.IsValidSqlQuery(inputString))
            {
                try
                {
                    switch (type)
                    {
                        case "ID заказа":
                            {
                                var guidTupleValue = inputValueConverter.ParseGuidFromString(inputString);
                                if (guidTupleValue.Item1 != Guid.Empty)
                                {
                                    Orders = orderRepository.GetOrderListById(guidTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = guidTupleValue.Item2;
                                }
                            }
                            break;
                        case "Дата заказа":
                            {
                                var dateTimeTupleValue = inputValueConverter.ParseDateTimeFromString(inputString);
                                if (dateTimeTupleValue.Item1 != DateTime.MinValue)
                                {
                                    Orders = orderRepository.GetOrderListByDate(dateTimeTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = dateTimeTupleValue.Item2;
                                }
                            }
                            break;
                        case "Адресс":
                            Orders = orderRepository.GetOrderListByAddress(inputString);
                            break;
                        case "Статус заказа":
                            Orders = orderRepository.GetOrderListByOrderStatus(inputString);
                            break;
                        case "Итоговая стоимость":
                            {
                                var decimalTupleValue = inputValueConverter.ParseDecimalFromString(inputString);
                                if (decimalTupleValue.Item2 != null)
                                {
                                    errorMessage = decimalTupleValue.Item2;
                                }
                                else
                                {
                                    Orders = orderRepository.GetOrderListByCost(decimalTupleValue.Item1);
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
            else if (Orders.Count == 0)
            {
                MessageQueue.Enqueue("По вашему запросу ничего не найдено");
                return;
            }
            else
            {
                return;
            }
        }

        protected override void LoadData()
        {
            SearchCategory = new()
            {
                "ID заказа",
                "ID сотрудника",
                "ID покупателя",
                "Дата заказа",
                "Статус заказа",
                "Итоговая стоимость",
            };
            try
            {
                OrderRepository orderRepository = new();
                Orders = orderRepository.GetOrderList();
                //Orders = orderRepository.GetOrderListByCustomerId((Guid)AccountManager.LoggedId);
            }
            catch (Exception ex)
            {
                string mess = "Произошла ошибка при получении списка заказов!\n";
                StandardMessages.Error(mess + ex.Message);
            }
        }
    }
}
