using OnlineStore2023.DataContext.Models;
using OnlineStore2023.DataContext.Repositories;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using System;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    class HomeVM : BaseAbstractVMPage
    {
        #region Fields
        private UsersDatum _currentUserData;

        private Visibility _userNameSectionVisibility;
        #endregion

        #region Properties
        public UsersDatum CurrentUserData 
        { 
            get => _currentUserData;
            set
            {
                if (value == _currentUserData) return;

                _currentUserData = value;
                OnPropertyChanged("CurrentUserData");
            }
        }
        public string Title { get; set; }

        public Visibility UserNameSectionVisibility
        {
            get => _userNameSectionVisibility;
            set
            {
                if (value == _userNameSectionVisibility) return;

                _userNameSectionVisibility = value;
                OnPropertyChanged("UserNameSectionVisibility");
            }
        }

        #endregion

        #region Commands

        #endregion
        public HomeVM()
        {
            Title = "Добро пожаловать";
            UserNameSectionVisibility = Visibility.Collapsed;

            LoadData();
        }

        protected override void LoadData()
        {
            Guid? id = AccountManager.LoggedId;
            if (id != null)
            {
                try
                {
                    UserDatumRepository userDatumRepository = new();


                    Title = "С возвращением,";
                    var x = userDatumRepository.GetUserDatumById((Guid)id);
                    CurrentUserData = x;
                    UserNameSectionVisibility = VisibilitySupport.ChangeVisibility(UserNameSectionVisibility);
                }
                catch (Exception) { }
            }

        }
    }
}
