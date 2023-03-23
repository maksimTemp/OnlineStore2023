using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    class AdminStaffPageVM : INotifyPropertyChanged
    {
        #region Fields
        private AccountVM _accountVM;

        private List<Employee> _employees;
        private object _selectedEmployee;

        private readonly ObservableCollection<string> _searchCategory = new()
        {
            "ID работника",
            "Логин",
            "Фамилия",
            "Должность",
            "Активность",
        };
        private string _currentCategory;
        private string _searchRequest;

        private Visibility _loadingScreen;
        private Visibility _searchSectionVisibility;
        #endregion

        #region Properties
        public List<Employee> Employees
        {
            get => _employees;
            set
            {
                if (value == _employees) return;

                _employees = value;
                OnPropertyChanged("Employees");
            }
        }

        public object SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                if (value == _selectedEmployee) return;

                _selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }
        public string SearchRequest
        {
            get => _searchRequest;
            set
            {
                if (value == _searchRequest) return;

                _searchRequest = value;
                OnPropertyChanged("SearchRequest");
            }
        }

        public ObservableCollection<string> SearchCategory => _searchCategory;
        public string CurrentCategory
        {
            get => _currentCategory;
            set
            {
                if (value == _currentCategory) return;

                _currentCategory = value;
                OnPropertyChanged("CurrentCategory");
            }
        }

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
        public Visibility SearchSectionVisibility
        {
            get => _searchSectionVisibility;
            set
            {
                if (value == _searchSectionVisibility) return;

                _searchSectionVisibility = value;
                OnPropertyChanged("SearchSectionVisibility");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand AddCommand { get; set; }
        public SimpleCommand EditCommand { get; set; }
        public SimpleCommand RemoveCommand { get; set; }
        public SimpleCommand ShowOrdersCommand { get; set; }
        public SimpleCommand ShowSearchSectionCommand { get; set; }
        public SimpleCommand SearchCommand { get; set; }
        #endregion

        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public AdminStaffPageVM(AccountVM accountVM)
        {
            _accountVM = accountVM;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));

            LoadingScreen = Visibility.Collapsed;
            SearchSectionVisibility = Visibility.Collapsed;

            AddCommand = new SimpleCommand(AddNewEmployee);
            EditCommand = new SimpleCommand(EditEmployee);
            RemoveCommand = new SimpleCommand(RemoveEmployee);
            ShowOrdersCommand = new SimpleCommand(ShowOrdersList);
            SearchCommand = new SimpleCommand(SearchEmployee);
            ShowSearchSectionCommand = new SimpleCommand(ShowSearchSection);

            Refresh();
        }

        private void ShowSearchSection()
        {
            SearchSectionVisibility = VisibilitySupport.ChangeVisibility(SearchSectionVisibility);
        }

        private void SearchEmployee()
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
                MessageQueue.Enqueue($"По вашему запросу было найдено {Employees.Count} работников");
            });
        }

        private void ShowOrdersList()
        {
            //NavigationService.Navigate(new AdminStaffAddEditPage(null));
        }

        private void RemoveEmployee()
        {
            if (SelectedEmployee == null)
            {
                MessageQueue.Enqueue("Выберите хотя бы одного работника для удаления!");
                return;
            }
            var selectedEmployee = (Employee)SelectedEmployee;

            if (MessageBox.Show($"Вы точно хотите удалить {selectedEmployee.User.UserLogin}?", "Внимание!",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MessageQueue.Enqueue("Удаление..", null, null, null, false, false, new TimeSpan(0, 0, 2));
                RunInBackground(() => DeleteEmployee(selectedEmployee));
            }
        }

        private void EditEmployee()
        {
            var selectedEmployee = SelectedEmployee;
            if (selectedEmployee == null)
            {
                MessageQueue.Enqueue("Выберите одного работника для редактирования!");
                return;
            }
            _accountVM.FrameView = new AdminStaffAddEditPage((Employee)selectedEmployee, _accountVM);
        }

        private void AddNewEmployee()
        {
            _accountVM.FrameView = new AdminStaffAddEditPage(null, _accountVM);
        }

        private void Refresh()
        {
            LoadingScreenProcess(() =>
            {
                LoadData();
            });
        }

        private void LoadData()
        {
            try
            {
                EmployeeRepository employeeRepository = new();
                Employees = employeeRepository.GetEmployeeList();
            }
            catch (Exception ex)
            {
                string mess = "Произошла ошибка при получении списка Работников!\n";
                StandardMessages.Error(mess + ex.Message);
            }
        }

        private void DeleteEmployee(Employee employee)
        {
            try
            {
                var repo = new EmployeeRepository();
                repo.Remove(employee);
                repo.SaveChanges();
            }
            catch (Exception ex)
            {
                string mess = "В процессе удаления произошла ошибка!\n";
                StandardMessages.Error(mess + ex.Message);
            }
            Refresh();
        }

        private void SearchByСriteria(string inputString, string type)
        {
            Employees.Clear();

            InputValueConverter inputValueConverter = new();
            EmployeeRepository employeeRepository = new();
            SqlQueryScreener screener = new();
            string errorMessage = null;
            if (screener.IsValidSqlQuery(inputString))
            {
                try
                {
                    switch (type)
                    {
                        case "ID работника":
                            {
                                var guidTupleValue = inputValueConverter.ParseGuidFromString(inputString);
                                if (guidTupleValue.Item1 != Guid.Empty)
                                {
                                    Employees = employeeRepository.GetEmployeeListById(guidTupleValue.Item1);
                                }
                                else
                                {
                                    errorMessage = guidTupleValue.Item2;
                                }
                            }
                            break;
                        case "Логин":
                            Employees = employeeRepository.GetEmployeeListByLogin(inputString);
                            break;
                        case "Фамилия":
                            Employees = employeeRepository.GetEmployeeListBySurname(inputString);
                            break;
                        case "Должность":
                            Employees = employeeRepository.GetEmployeeListByJobTitle(inputString);
                            break;
                        case "Активность":
                            {
                                var boolTupleValue = inputValueConverter.ParseBoolFromString(inputString);
                                if (boolTupleValue.Item2 != null)
                                {
                                    errorMessage = boolTupleValue.Item2;
                                }
                                else
                                {
                                    Employees = employeeRepository.GetEmployeeListByActive(boolTupleValue.Item1);
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
            else if (Employees.Count == 0)
            {
                MessageQueue.Enqueue("По вашему запросу ничего не найдено");
                return;
            }
            else
            {
                return;
            }
        }

        private bool CheckSearchSectionFields()
        {
            if (string.IsNullOrWhiteSpace(CurrentCategory))
            {
                MessageQueue.Enqueue("Выберите критерий поиска");
                return false;
            }
            if (string.IsNullOrWhiteSpace(SearchRequest))
            {
                MessageQueue.Enqueue("Введите значение для поиска");
                return false;
            }
            return true;
        }

        private async void LoadingScreenProcess(Action action, Action onMain = null)
        {
            LoadingScreen = Visibility.Visible;

            await Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });

            if (onMain != null)
                onMain();
        }

        private async void RunInBackground(Action action, Action onMain = null)
        {
            await Task.Factory.StartNew(action);
            if (onMain != null)
                onMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }

}
