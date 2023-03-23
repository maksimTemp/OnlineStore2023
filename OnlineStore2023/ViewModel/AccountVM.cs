using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    public class AccountVM : INotifyPropertyChanged
    {
        #region Fields
        private MainWindowVM _mainVM;
        private object _frameView;
        private Visibility _adminSectionVisibility;
        private Visibility _employeeSectionVisibility;
        private Visibility _loadingScreen;
        #endregion

        #region Properties
        public object FrameView
        {
            get { return _frameView; }
            set
            {
                if (value == _frameView) return;

                _frameView = value;
                OnPropertyChanged("FrameView");
            }
        }
        public Visibility AdminSectionVisibility
        {
            get { return _adminSectionVisibility; }
            set
            {
                if (value == _adminSectionVisibility) return;

                _adminSectionVisibility = value;
                OnPropertyChanged("AdminSectionVisibility");
            }
        }
        public Visibility EmployeeSectionVisibility
        {
            get { return _employeeSectionVisibility; }
            set
            {
                if (value == _employeeSectionVisibility) return;

                _employeeSectionVisibility = value;
                OnPropertyChanged("EmployeeSectionVisibility");
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
        #endregion

        #region Commands
        public ParameterCommand ChangeViewCommand { get; set; }
        public SimpleCommand LogoutCommand { get; set; }

        #endregion

        public AccountVM(MainWindowVM mvm, string startingPage = "account")
        {
            _mainVM = mvm;

            AdminSectionVisibility = Visibility.Collapsed;
            LogoutCommand = new SimpleCommand(Logout);
            ChangeViewCommand = new ParameterCommand(ChangeView);

            FrameView = new UserPage();

            LoadingScreenProcess(() =>
            {
                CheckUserRole();
            }, () =>
            {
                ChangeView(startingPage);
            });
        }
        private void CheckUserRole()
        {
            try
            {
                var userRepo = new UserRepository();
                var user = userRepo.GetSimpleUserById((Guid)AccountManager.LoggedId);
                if (user.UserType == "shop_admin")
                {
                    AdminSectionVisibility = Visibility.Visible;
                    EmployeeSectionVisibility = Visibility.Visible;
                }
                else if (user.UserType == "shop_employee")
                {
                    EmployeeSectionVisibility = Visibility.Visible;
                }
                else
                {
                    AdminSectionVisibility = Visibility.Collapsed;
                    EmployeeSectionVisibility = Visibility.Collapsed;
                }
            }
            catch (Exception e)
            {
                string mess = "В процессе авторизации произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }

        private void ChangeView(object param)
        {
            string change = param as string;

            switch (change)
            {
                case "account":
                    FrameView = new UserPage();
                    break;
                case "orders":
                    FrameView = new OrdersPage(this);
                    break;
                case "adminStaff":
                    FrameView = new AdminStaffPage(this);
                    break;
                case "adminOrders":
                    FrameView = new AdminOrdersPage(this);
                    break;
                case "adminProduct":
                    FrameView = new AdminProductListPage(this);
                    break;
                case "adminCustomers":
                    FrameView = new AdminCustomersPage();
                    break;
                default:
                    break;
            }
        }
        private void Logout()
        {
            AccountManager.Logout();
            _mainVM.Cart.Clear();
            _mainVM.LoadPage("account");
            
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

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
