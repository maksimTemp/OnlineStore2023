using OnlineStore2023.Commands;
using OnlineStore2023.Managers;
using OnlineStore2023.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    abstract class AdminAbstractVMPageWithGrid : BaseAbstractVMPage
    {
        #region Fields
        protected AccountVM _accountVM;

        private ObservableCollection<string> _searchCategory;

        private string _currentCategory;
        private string _searchRequest;

        private Visibility _employeeSectionVisibility;
        private Visibility _adminSectionVisibility;
        private Visibility _searchSectionVisibility;
        #endregion

        #region Properties
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

        public ObservableCollection<string> SearchCategory
        {
            get => _searchCategory;
            protected set
            {
                _searchCategory = value;
            }
        }
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
        public Visibility AdminSectionVisibility
        {
            get => _adminSectionVisibility;
            set
            {
                if (value == _adminSectionVisibility) return;

                _adminSectionVisibility = value;
                OnPropertyChanged("AdminSectionVisibility");
            }
        }
        #endregion

        #region Commands
        public SimpleCommand AddCommand { get; set; }
        public SimpleCommand EditCommand { get; set; }
        public SimpleCommand RemoveCommand { get; set; }
        public SimpleCommand ShowSearchSectionCommand { get; set; }
        public SimpleCommand SearchCommand { get; set; }
        #endregion

        protected AdminAbstractVMPageWithGrid(AccountVM accountVM)
        {
            InitializingFieldsAndCommands(accountVM);
            CheckUserPrivileges();

            Refresh();
        }

        protected void InitializingFieldsAndCommands(AccountVM accountVM)
        {
            _accountVM = accountVM;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));

            AddCommand = new SimpleCommand(AddNewEntity);
            EditCommand = new SimpleCommand(EditEntity);
            RemoveCommand = new SimpleCommand(RemoveEntity);

            SearchCommand = new SimpleCommand(SearchEntity);
            ShowSearchSectionCommand = new SimpleCommand(ShowSearchSection);
        }

        abstract protected void AddNewEntity();

        abstract protected void EditEntity();

        abstract protected void RemoveEntity();

        abstract protected void SearchEntity();

        protected void ShowSearchSection()
        {
            SearchSectionVisibility = VisibilitySupport.ChangeVisibility(SearchSectionVisibility);
        }

        abstract protected void SearchByСriteria(string inputString, string type);

        protected bool CheckSearchSectionFields()
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

        protected void CheckUserPrivileges()
        {
            if(AccountManager.UserType == "shop_admin")
            {
                EmployeeSectionVisibility = Visibility.Visible;
                AdminSectionVisibility= Visibility.Visible;
            }
            else if(AccountManager.UserType == "shop_employee")
            {
                EmployeeSectionVisibility = Visibility.Visible;
            }
            else
            {
                EmployeeSectionVisibility = Visibility.Collapsed;
                AdminSectionVisibility = Visibility.Collapsed;
            }
        }
    }
}
