using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HelppoLasku.ViewModels
{
    public class CustomCommand : ICommand
    {
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        public CustomCommand(Action<object> executeMethod) : this(executeMethod, null)
        {
        }

        public CustomCommand(Action<object> executeMethod, Predicate<object> canExecuteMethod)
        {
            execute = executeMethod ?? throw new ArgumentNullException("executeMethod");
            canExecute = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null)
                return canExecute(parameter);
            if (execute != null)
                return true;
            return false;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
