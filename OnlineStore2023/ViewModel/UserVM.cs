using OnlineStore2023.Commands;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using OnlineStore2023.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using OnlineStore2023.DataContext.Models;

namespace OnlineStore2023.ViewModel
{
    internal class UserVM : INotifyPropertyChanged
    {
        #region Fields
        private string _firstName;
        private string _surname;
        private string _email;
        private string _phone;
        private string _userLogin;
        private string _jobTitle;
        private string _emailMain;
        private string _oldPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _customerAdressLine;

        private Visibility _loadingScreen;
        private Visibility _employeeSectionVisibility;
        #endregion

        #region Properties
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;

                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                if (value == _surname) return;

                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (value == _email) return;

                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (value == _phone) return;

                _phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public string CustomerAdressLine
        {
            get { return _customerAdressLine; }
            set
            {
                if (value == _customerAdressLine) return;

                _customerAdressLine = value;
                OnPropertyChanged("CustomerAdressLine");
            }
        }
        public string UserLogin
        {
            get { return _userLogin; }
            set
            {
                if (value == _userLogin) return;

                _userLogin = value;
                OnPropertyChanged("UserLogin");
            }
        }
        public string JobTitle
        {
            get { return _jobTitle; }
            set
            {
                if (value == _jobTitle) return;

                _jobTitle = value;
                OnPropertyChanged("JobTitle");
            }
        }
        public string EmailMain
        {
            get { return _emailMain; }
            set
            {
                if (value == _emailMain) return;

                _emailMain = value;
                OnPropertyChanged("EmailMain");
            }
        }
        public string OldPassword
        {
            get { return _oldPassword; }
            set
            {
                if (value == _oldPassword) return;

                _oldPassword = value;
                OnPropertyChanged("OldPassword");
            }
        }
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                if (value == _newPassword) return;

                _newPassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (value == _confirmPassword) return;

                _confirmPassword = value;
                OnPropertyChanged("ConfirmPassword");
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
        #endregion
        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public SimpleCommand SaveCommand { get; set; }
        public SimpleCommand ChangePasswordCommand { get; set; }

        public UserVM()
        {
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));
            //MessageQueue.IgnoreDuplicate = true;

            LoadingScreen = Visibility.Collapsed;
            SaveCommand = new SimpleCommand(Save);
            ChangePasswordCommand = new SimpleCommand(ChangePassword);

            Refresh();
        }

        private void Refresh()
        {
            LoadingScreenProcess(() =>
            {
                LoadSettings();
            });
        }
        private void LoadSettings()
        {
            CheckUserRole();
            User user = new ();
            UsersDatum userData = new ();
            Customer customer = new ();
            Employee employee = new ();
            try
            {
                OnlineShop2022Context dataContext = new();
                user = dataContext.Users.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                var x = dataContext.UsersData.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                userData = x;
                customer = dataContext.Customers.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                employee = dataContext.Employees.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
            }
            catch (Exception e)
            {
                string mess = "Во время загрузки данных произошла ошибка\n";
                StandardMessages.Error(mess + e.Message);
            }

            UserLogin = user.UserLogin;
            if(userData != null)
            {
                FirstName = userData.UserFirstName;
                Surname = userData.UserLastName;
                Phone = userData.UserPhoneNumber;
            }
            if(customer != null)
            {
                Email = customer.CustomerEmailAdress;
                EmailMain = customer.CustomerEmailAdress;
                CustomerAdressLine = customer.CustomerAdressLine;
            }
            if(employee != null)
            {
                JobTitle = employee.JobTitle;
            }

        }
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageQueue.Enqueue("Имя не может быть пустым");
                return;
            }
            if (string.IsNullOrWhiteSpace(Surname))
            {
                MessageQueue.Enqueue("Фамилия не может быть пустой");
                return;
            }

            FirstName = FirstName.Trim();
            Surname = Surname.Trim();
            if (Email != null)
            {
                Email = Email.Trim();
                if (Email == string.Empty)
                    Email = null;
                else
                {
                    if (!InputStringValidator.ValidateEmail(Email))
                    {
                        MessageQueue.Enqueue("Электронная почта неверна");
                        return;
                    }
                }
            }
            if (Phone != null)
            {
                Phone = Phone.Trim();
                if (Phone == string.Empty)
                    Phone = null;
                else
                {
                    if (!InputStringValidator.ValidatePhoneNumber(Phone))
                    {
                        MessageQueue.Enqueue("Неправильный телефон");
                        return;
                    }
                }
            }
            if (!InputStringValidator.ValidateName(FirstName))
            {
                MessageQueue.Enqueue("Имя не корректное");
                return;
            }
            if (!InputStringValidator.ValidateName(Surname))
            {
                MessageQueue.Enqueue("Фамилия некорректна");
                return;
            }

            MessageQueue.Enqueue("Сохранение...");
            RunInBackground(() =>
            {
                try
                {
                    User user = new ();
                    UsersDatum usersDatum = new ();
                    Customer customer = new ();
                    OnlineShop2022Context dataContext = new();
                    user = dataContext.Users.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                    usersDatum = dataContext.UsersData.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                    customer = dataContext.Customers.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                    if (user != null)
                    {
                        usersDatum.UserLastName = Surname;
                        usersDatum.UserFirstName = FirstName;
                        usersDatum.UserPhoneNumber = Phone;

                        customer.CustomerAdressLine = CustomerAdressLine;
                        customer.CustomerEmailAdress = Email;

                        user.UsersDatum = usersDatum;
                        user.Customer = customer;

                        //user.UsersDatum.UserPhoneNumber = Phone;
                        //user.UsersDatum.UserFirstName = FirstName;
                        //user.UsersDatum.UserLastName = Surname;
                        //user.Customer.CustomerAdressLine = CustomerAdressLine;
                        //user.Customer.CustomerEmailAdress = Email;

                        dataContext.Users.Update(user);
                        dataContext.SaveChanges();
                    }

                    //dataContext.UsersData.Update(usersDatum);
                }
                catch (Exception e)
                {
                    string mess = "Во время загрузки данных произошла ошибка!\n";
                    StandardMessages.Error(mess + e.Message);
                }
            }, () =>
            {
                MessageQueue.Enqueue("Сохранено!");
            });
        }
        private void ChangePassword()
        {
            if (string.IsNullOrEmpty(OldPassword))
            {
                MessageQueue.Enqueue("Необходимо ввести старый пароль!");
                return;
            }
            if (string.IsNullOrEmpty(NewPassword))
            {
                MessageQueue.Enqueue("Необходимо ввести новый пароль!");
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPassword))
            {
                MessageQueue.Enqueue("Пароль должен быть повторен!");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                MessageQueue.Enqueue("Пароль должен быть повторен!");
                return;
            }

            MessageQueue.Enqueue("Сохранение..", null, null, null, false, false, new TimeSpan(0, 0, 2));
            RunInBackground(() =>
            {
                if (!AccountManager.ChangePassword(OldPassword, NewPassword))
                    MessageQueue.Enqueue("Введенный вами пароль неверен!");
                else
                {
                    MessageQueue.Enqueue("Пароль был изменен");
                    OldPassword = null;
                    NewPassword = null;
                    ConfirmPassword = null;
                }
            });
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
        private void CheckUserRole()
        {
            try
            {
                var dataContext = new OnlineShop2022Context();
                var user = dataContext.Users.FirstOrDefault(x => x.UserId == AccountManager.LoggedId);
                if (user.UserType == "shop_employee" || user.UserType == "shop_admin")
                {
                    EmployeeSectionVisibility = Visibility.Visible;
                }
                else
                {
                    EmployeeSectionVisibility = Visibility.Collapsed;
                }
            }
            catch (Exception e)
            {
                string mess = "В процессе авторизации произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
