using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace IS.UI.Command
{

    public class ActionCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Func<object, bool> canExecute = p => true;

        public ActionCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            this.action = action;
            if (canExecute != null)
                this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }

}
