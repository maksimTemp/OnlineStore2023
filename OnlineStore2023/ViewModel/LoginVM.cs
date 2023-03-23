using OnlineStore2023.Commands;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using System;
using System.Windows;


namespace OnlineStore2023.ViewModel
{
    internal class LoginVM : BaseAbstractVMPage
    {
        #region Fields
        private MainWindowVM _mainVM;
        #endregion

        #region Properties
        public string UserLogin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        #endregion

        #region Commands
        public SimpleCommand LoginCommand { get; set; }
        public SimpleCommand RegisterCommand { get; set; }

        #endregion

        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }

        public LoginVM(MainWindowVM mvm)
        {
            _mainVM = mvm;

            LoadingScreen = Visibility.Collapsed;

            LoginCommand = new SimpleCommand(Login);
            RegisterCommand = new SimpleCommand(Register);

            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 2));
        }

        private void Login()
        {
            //UserLogin = "AdminWildZoneHub123";
            //Password = "AdminWildZoneHub123Pass";
            //UserLogin = "NosovTheBest_123";
            //Password = "EmployeeWildZoneHub1Pass1";
            if (UserLogin == null || UserLogin == string.Empty)
            {
                MessageQueue.Enqueue("Поле логина не должно быть пустым");
                return;
            }

            if (Password == null || Password == string.Empty)
            {
                MessageQueue.Enqueue("Пароль не должен быть пустым");
                return;
            }

            UserLogin = UserLogin.Trim();

            LoadingScreenProcess(() =>
            {
                if (!AccountManager.Login(UserLogin, Password))
                    MessageQueue.Enqueue("Неверный пароль или имя пользователя!");
            }, () =>
            {
                if (AccountManager.LoggedId != null)
                    _mainVM.LoadPage("account");
            });
        }
        private void Register()
        {
            if (Firstname == null || Firstname == string.Empty)
            {
                MessageQueue.Enqueue("Имя не может быть пустым");
                return;
            }
            if (Surname == null || Surname == string.Empty)
            {
                MessageQueue.Enqueue("Фамилия не должна быть пустой");
                return;
            }
            if (UserLogin == null || UserLogin == string.Empty)
            {
                MessageQueue.Enqueue("Логин пользователя не должен быть пустым");
                return;
            }
            if (Email == null || Email == string.Empty)
            {
                MessageQueue.Enqueue("Электронная почта не должна быть пустой");
                return;
            }
            if (Password == null || Password == string.Empty || RepeatPassword == null || RepeatPassword == string.Empty)
            {
                MessageQueue.Enqueue("Пароль не должен быть пустым");
                return;
            }
            if (Password != RepeatPassword)
            {
                MessageQueue.Enqueue("Пароли не совпадают");
                return;
            }

            Firstname = Firstname.Trim();
            Firstname = char.ToUpper(Firstname[0]) + Firstname.Substring(1);
            Surname = Surname.Trim();
            Surname = char.ToUpper(Surname[0]) + Surname.Substring(1);
            Email = Email.Trim();

            if (!InputStringValidator.ValidateEmail(Email))
            {
                MessageQueue.Enqueue("Электронная почта не является корректной");
                return;
            }
            if (!InputStringValidator.ValidateName(Firstname))
            {
                MessageQueue.Enqueue("Имя не соответствует действительности");
                return;
            }
            if (!InputStringValidator.ValidateName(Surname))
            {
                MessageQueue.Enqueue("Фамилия не соответствует действительности");
                return;
            }


            LoadingScreenProcess(() =>
            {
                string messOut = string.Empty;
                if (!AccountManager.Register(UserLogin, Email, Password, Firstname, Surname, out messOut))
                    MessageQueue.Enqueue(messOut);
                else
                {
                    if (!AccountManager.Login(UserLogin, Password))
                        MessageQueue.Enqueue("Возникла проблема с входом в систему");
                }
            }, () =>
            {
                _mainVM.LoadPage("account");
            });
        }

        protected override void LoadData() { }
    }
}
