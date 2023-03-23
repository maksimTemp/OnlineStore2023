using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    abstract class BaseAbstractVMPage : INotifyPropertyChanged
    {
        #region Fields
        private Visibility _loadingScreen;
        #endregion

        #region Properties
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

        public MaterialDesignThemes.Wpf.SnackbarMessageQueue MessageQueue { get; set; }
        #endregion

        #region Commands

        #endregion

        protected void Refresh()
        {
            LoadingScreenProcess(() =>
            {
                LoadData();
            });
        }

        abstract protected void LoadData();

        protected async void LoadingScreenProcess(Action action, Action onMain = null)
        {
            LoadingScreen = Visibility.Visible;

            await Task.Factory.StartNew(action).ContinueWith((Task) =>
            {
                LoadingScreen = Visibility.Collapsed;
            });

            if (onMain != null)
                onMain();
        }

        protected async void RunInBackground(Action action, Action onMain = null)
        {
            await Task.Factory.StartNew(action);
            if (onMain != null)
                onMain();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
