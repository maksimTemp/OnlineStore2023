using OnlineStore2023.Commands;
using System;
using System.Windows;

namespace OnlineStore2023.ViewModel
{
    abstract class AdminAbstractVMAddEditPage : BaseAbstractVMPage
    {
        #region Fields
        protected AccountVM _accountVM;
        #endregion

        #region Properties

        #endregion

        #region Commands
        public SimpleCommand SaveCommand { get; set; }
        public SimpleCommand BackCommand { get; set; }
        #endregion
        protected AdminAbstractVMAddEditPage(object inputEntity, AccountVM accountVM)
        {
            _accountVM = accountVM;
            MessageQueue = new MaterialDesignThemes.Wpf.SnackbarMessageQueue(new TimeSpan(0, 0, 4));

            SaveCommand = new SimpleCommand(SaveEntity);
            BackCommand = new SimpleCommand(GoBack);
        }
        abstract protected void GoBack();
        abstract protected void SaveEntity();
        abstract protected void SaveInputValues();
        abstract protected bool CheckInputValues();
    }
}
