using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OnlineStore2023.Commands
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _execute;

        public SimpleCommand(Action executeMethod) 
        {
            _execute = executeMethod;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke();
        }
    }
}
