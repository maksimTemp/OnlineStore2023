using System;
using System.Windows.Input;

namespace OnlineStore2023.Commands
{
    public class ParameterCommand : ICommand
    {
        private Action<object> _execute;

        public event EventHandler CanExecuteChanged;

        public ParameterCommand(Action<object> executeMethod)
        {
            _execute = executeMethod;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
