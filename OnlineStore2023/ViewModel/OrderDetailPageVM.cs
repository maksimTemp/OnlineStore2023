using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class OrderDetailPageVM : BaseAbstractVMPage
    {
        #region Fields
        private Order _currentOrder;
        private Order _selectedOrder;
        private string _isOrderPaid;

        private readonly ObservableCollection<string> _orderStatus = new()
        {
            "Проверяется сотрудниками",
            "Принято в работу",
            "Передано в доставку",
            "Готово к выдаче",
            "Завершён",
            "Отменён"
        };
        private readonly ObservableCollection<string> _orderPaymentStatus = new()
        {
            "True",
            "False",
        };

        private Visibility _employeeAddButtonSectionVisibility;
        private Visibility _saveButtonSectionVisibility;
        private Visibility _employeeSectionVisibility;
        #endregion

        #region Properties
        public Order CurrentOrder
        {
            get { return _currentOrder; }
            set
            {
                if (value == _currentOrder) return;

                _currentOrder = value;

                if (IfSomethingChanged())
                    SaveButtonSectionVisibility = Visibility.Visible;
                OnPropertyChanged("CurrentOrder");
            }
        }
        public ObservableCollection<string> OrderStatus => _orderStatus;
        public ObservableCollection<string> OrderPaymentStatus => _orderPaymentStatus;
        public string IsOrderPaid
        {
            get => _isOrderPaid;
            set
            {
                if (value == _isOrderPaid) return;

                _isOrderPaid = value;
                OnPropertyChanged("IsActiveString");
            }
        }

        public bool IsComboBoxReadOnly { get; set; }

        public Visibility EmployeeAddButtonSectionVisibility
        {
            get => _employeeAddButtonSectionVisibility;
            set
            {
                if (value == _employeeAddButtonSectionVisibility) return;

                _employeeAddButtonSectionVisibility = value;
                OnPropertyChanged("EmployeeAddButtonSectionVisibility");
            }
        }
        public Visibility SaveButtonSectionVisibility
        {
            get => _saveButtonSectionVisibility;
            set
            {
                if (value == _saveButtonSectionVisibility) return;

                _saveButtonSectionVisibility = value;
                OnPropertyChanged("SaveButtonSectionVisibility");
            }
        }
        public Visibility EmployeeSectionVisibility
        {
            get => _employeeSectionVisibility;
            set
            {
                if (value == _employeeSectionVisibility) return;

                _employeeSectionVisibility = value;
                OnPropertyChanged("EmployeeSectionVisibility");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand AddEmployeeToOrderCommand { get; set; }
        public SimpleCommand SaveOrderCommand { get; set; }

        #endregion
        public OrderDetailPageVM(Order setectedOrder, AccountVM accountVM)
        {
            _selectedOrder = setectedOrder;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));

            LoadData();

            if (IsOrderStatusActive())
            {
                CheckIfEmployeeServingOrder();
            }
            Refresh();
        }

        protected override void LoadData()
        {
            OrderRepository orderRepository = new();


            AddEmployeeToOrderCommand = new SimpleCommand(AddEmployeeToOrder);
            SaveOrderCommand = new SimpleCommand(SaveOrder);
            if(IsEmployee())
            {
                EmployeeSectionVisibility = Visibility.Visible;
            }
            else
            {
                EmployeeSectionVisibility = Visibility.Hidden;
                IsComboBoxReadOnly = false;
            }

            //SaveButtonSectionVisibility = Visibility.Collapsed;
            CurrentOrder = orderRepository.GetOrderByOrderId(_selectedOrder.OrderId);
            IsOrderPaid = CurrentOrder.OrderPaid.ToString();
        }

        private void AddEmployeeToOrder()
        {
            try
            {
                if (AccountManager.UserType == "shop_employee")
                {
                    UserRepository userRepository = new();

                    CurrentOrder.Employee = userRepository.GetSimpleUserById((Guid)AccountManager.LoggedId);
                    CurrentOrder.OrderStatus = "Принято в работу";
                    MessageQueue.Enqueue($"Вы обслуживаете заказ {CurrentOrder.OrderId}");
                }
                else
                {
                    MessageQueue.Enqueue("Ваша роль должна быть \"shop_employee\".");
                    MessageQueue.Enqueue("Для получения права обратитесь к администратору");
                }
            }
            catch (Exception ex)
            {
                MessageQueue.Enqueue(ex);
            }
        }

        private void CheckIfEmployeeServingOrder()
        {
            if (CurrentOrder.EmployeeId != null && AccountManager.UserType == "shop_employee")
            {
                EmployeeAddButtonSectionVisibility = Visibility.Collapsed;
            }
        }

        private bool IsOrderStatusActive()
        {
            if (CurrentOrder.OrderStatus is "Отменён" or "Завершён")
            {
                SaveButtonSectionVisibility = Visibility.Collapsed;
                IsComboBoxReadOnly = true;
                return false;
            }
            else
            {
                IsComboBoxReadOnly = false;
                return true;
            }
        }

        private bool IfSomethingChanged()
        {
            if (_currentOrder.EmployeeId != _selectedOrder.EmployeeId
                   || _currentOrder.OrderStatus != _selectedOrder.OrderStatus
                   || _currentOrder.OrderPaid != _selectedOrder.OrderPaid)
                return true;
            else return false;
        }

        private void SaveOrder()
        {
            MessageQueue.Enqueue("Сохранение..", null, null, null, false, false, new TimeSpan(0, 0, 2));
            RunInBackground(() =>
            {
                try
                {
                    OrderRepository orderRepository = new();
                    orderRepository.Update(CurrentOrder);
                    orderRepository.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageQueue.Enqueue("Сохранение не удалось");
                }
            }, () =>
            {
                MessageQueue.Enqueue("Сохранено!");
                Refresh();
            });
        }

        private bool IsEmployee()
        {
            return AccountManager.UserType == "shop_employee";
        }
    }
}
