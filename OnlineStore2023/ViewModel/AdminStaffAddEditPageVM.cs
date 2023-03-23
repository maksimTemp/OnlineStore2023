using OnlineStore2023.Commands;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using OnlineStore2023.View;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    internal class AdminStaffAddEditPageVM : AdminAbstractVMAddEditPage
    {
        #region Fields
        private Employee _selectedEmployee;
        private Employee _curEmployee;

        private string _isActive;
        private string _passwordRegistration;
        private string _newPassword;
        private string _confirmPassword;

        private readonly ObservableCollection<string> _storeEmployeeRoles = new()
        {
            "simple_user",
            "shop_employee",
            "shop_admin",
        };
        private readonly ObservableCollection<string> _userStates = new()
        {
            "True",
            "False",
        };
        private readonly ObservableCollection<string> _employeeJobTitle = new()
        {
            "simple_employee",
            "simple_seller",
            "shop_admin",
        };

        private Visibility _regPasswordSectionVisibility;
        private Visibility _changePasswordSectionVisibility;
        #endregion

        #region Properties
        public Employee CurEmployee
        {
            get => _curEmployee;
            set
            {
                if (value == _curEmployee) return;

                _curEmployee = value;
                OnPropertyChanged("CurEmployee");
            }
        }

        public string IsActiveString
        {
            get => _isActive;
            set
            {
                if (value == _isActive) return;

                _isActive = value;
                OnPropertyChanged("IsActiveString");
            }
        }
        public string PasswordRegistration
        {
            get { return _passwordRegistration; }
            set
            {
                if (value == _passwordRegistration) return;

                _passwordRegistration = value;
                OnPropertyChanged("PasswordRegistration");
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

        public ObservableCollection<string> Roles => _storeEmployeeRoles;
        public ObservableCollection<string> UserStates => _userStates;
        public ObservableCollection<string> EmployeeJobTitle => _employeeJobTitle;

        public Visibility ChangePasswordSectionVisibility
        {
            get { return _changePasswordSectionVisibility; }
            set
            {
                if (value == _changePasswordSectionVisibility) return;

                _changePasswordSectionVisibility = value;
                OnPropertyChanged("ChangePasswordSectionVisibility");
            }
        }
        public Visibility RegPasswordSectionVisibility
        {
            get { return _regPasswordSectionVisibility; }
            set
            {
                if (value == _regPasswordSectionVisibility) return;

                _regPasswordSectionVisibility = value;
                OnPropertyChanged("RegPasswordSectionVisibility");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand ChangePasswordCommand { get; set; }
        #endregion

        public AdminStaffAddEditPageVM(Employee selectedEmployee, AccountVM accountVM) : base(selectedEmployee, accountVM)
        {
            _selectedEmployee = selectedEmployee;


            ChangePasswordCommand = new SimpleCommand(ChangePassword);

            Refresh();
            LoadingScreen = Visibility.Collapsed;
        }

        private void ChangePassword()
        {
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
                MessageQueue.Enqueue("Пароли должны совпадать!");
                return;
            }

            MessageQueue.Enqueue("Сохранение..", null, null, null, false, false, new TimeSpan(0, 0, 2));
            RunInBackground(() =>
            {
                if (!AccountManager.ChangePassword(NewPassword, CurEmployee.UserId))
                    MessageQueue.Enqueue("Что то не так!");
                else
                {
                    MessageQueue.Enqueue("Пароль был изменен");
                    NewPassword = null;
                    ConfirmPassword = null;
                }
            });
        }

        protected override void GoBack()
        {
            throw new NotImplementedException();
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
                    _accountVM.FrameView = new AdminStaffPage(_accountVM);
                });
            }
        }

        protected override void SaveInputValues()
        {
            try
            {
                var employeeRepo = new EmployeeRepository();

                if (_selectedEmployee != null)
                {
                    employeeRepo.Employees.Update(_curEmployee);
                }
                else
                {
                    PasswordHashGenerator passwordHashGenerator = new();
                    PasswordSaltGenerator passwordSaltGenerator = new();
                    var passwordSalt = passwordSaltGenerator.GetRandomString(10);

                    _curEmployee.User.UserPasswordSalt = passwordSalt;
                    _curEmployee.User.UserPasswordHash = passwordHashGenerator.GenerateHash(PasswordRegistration, passwordSalt);
                    _curEmployee.User.IsActive = Convert.ToBoolean(IsActiveString);

                    employeeRepo.Employees.Add(_curEmployee);
                }
                employeeRepo.SaveChanges();
            }
            catch (Exception e)
            {
                string mess = "Во время загрузки данных произошла ошибка!\n";
                StandardMessages.Error(mess + e.Message);
                return;
            }
        }

        protected override bool CheckInputValues()
        {
            if (string.IsNullOrWhiteSpace(CurEmployee.User.UsersDatum.UserFirstName))
            {
                MessageQueue.Enqueue("Имя не может быть пустым");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurEmployee.User.UsersDatum.UserLastName))
            {
                MessageQueue.Enqueue("Фамилия не может быть пустой");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurEmployee.User.UserLogin))
            {
                MessageQueue.Enqueue("Логин не может быть пустой");
                return false;
            }
            if (string.IsNullOrWhiteSpace(PasswordRegistration))
            {
                MessageQueue.Enqueue("Пароль не может быть пустым");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurEmployee.JobTitle))
            {
                MessageQueue.Enqueue("Выберите должность работника");
                return false;
            }
            if (string.IsNullOrWhiteSpace(CurEmployee.User.UserType))
            {
                MessageQueue.Enqueue("Выберите тип пользователя");
                return false;
            }
            if (CurEmployee.EmployeePassportSeries == 0)
            {
                MessageQueue.Enqueue("Заполните серию паспорта");
                return false;
            }
            if (CurEmployee.EmployeePassportNumber == 0)
            {
                MessageQueue.Enqueue("Заполните номер паспорта");
                return false;
            }

            CurEmployee.User.UsersDatum.UserFirstName = CurEmployee.User.UsersDatum.UserFirstName.Trim();
            CurEmployee.User.UsersDatum.UserFirstName = char.ToUpper(CurEmployee.User.UsersDatum.UserFirstName[0]) + CurEmployee.User.UsersDatum.UserFirstName.Substring(1);
            CurEmployee.User.UsersDatum.UserLastName = CurEmployee.User.UsersDatum.UserLastName.Trim();
            CurEmployee.User.UsersDatum.UserLastName = char.ToUpper(CurEmployee.User.UsersDatum.UserLastName[0]) + CurEmployee.User.UsersDatum.UserLastName.Substring(1);
            CurEmployee.User.UserLogin = CurEmployee.User.UserLogin.Trim();
            PasswordRegistration = PasswordRegistration.Trim();

            if (!InputStringValidator.ValidateName(CurEmployee.User.UsersDatum.UserFirstName))
            {
                MessageQueue.Enqueue("Имя не корректное");
                return false;
            }
            if (!InputStringValidator.ValidateName(CurEmployee.User.UsersDatum.UserLastName))
            {
                MessageQueue.Enqueue("Фамилия некорректна");
                return false;
            }
            if (CurEmployee.User.UsersDatum.UserPhoneNumber != null)
            {
                CurEmployee.User.UsersDatum.UserPhoneNumber = CurEmployee.User.UsersDatum.UserPhoneNumber.Trim();
                if (CurEmployee.User.UsersDatum.UserPhoneNumber == string.Empty)
                    CurEmployee.User.UsersDatum.UserPhoneNumber = null;
                else
                {
                    if (!InputStringValidator.ValidatePhoneNumber(CurEmployee.User.UsersDatum.UserPhoneNumber))
                    {
                        MessageQueue.Enqueue("Неправильный телефон");
                        return false;
                    }
                }
            }
            if (!InputStringValidator.ValidateLoginOrPassword(CurEmployee.User.UserLogin))
            {
                MessageQueue.Enqueue("Логин должен быть длиной от 8 до 50 символов");
                MessageQueue.Enqueue("начинаться с заглавной или маленькой буквы");
                MessageQueue.Enqueue("содержать как минимум букву нижнего и верхнего регистров");
                MessageQueue.Enqueue("а также спецсимвол ('.' или '_')");
                return false;
            }
            if (!InputStringValidator.ValidateLoginOrPassword(PasswordRegistration))
            {
                MessageQueue.Enqueue("Пароль должен быть длиной от 8 до 50 символов");
                MessageQueue.Enqueue("начинаться с заглавной или маленькой буквы");
                MessageQueue.Enqueue("содержать как минимум букву нижнего и верхнего регистров");
                MessageQueue.Enqueue("а также спецсимвол ('.' или '_')");
                return false;
            }
            if (!InputStringValidator.ValidateNumber(CurEmployee.EmployeePassportSeries, 4))
            {
                MessageQueue.Enqueue("Серия паспорта должна содержать 4 цифры");
                return false;
            }
            if (!InputStringValidator.ValidateNumber(CurEmployee.EmployeePassportNumber, 6))
            {
                MessageQueue.Enqueue("Номер паспорта должен содержать 6 цифр");
                return false;
            }
            return true;
        }

        protected override void LoadData()
        {
            if (_selectedEmployee == null)
            {
                _curEmployee = new Employee();
                _curEmployee.User = new();
                _curEmployee.User.UsersDatum = new();
                IsActiveString = "False";

                ChangePasswordSectionVisibility = Visibility.Collapsed;
                RegPasswordSectionVisibility = Visibility.Visible;
            }
            else
            {
                _curEmployee = _selectedEmployee;
                IsActiveString = CurEmployee.User.IsActive.ToString();

                ChangePasswordSectionVisibility = Visibility.Visible;
                RegPasswordSectionVisibility = Visibility.Collapsed;
            }
        }
    }
}
