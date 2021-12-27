using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.Commands {
    public class BaseAsyncCommand : IAsyncCommand {
        private readonly Func<Task> execute;
        private readonly Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public BaseAsyncCommand(Func<Task> execute, Func<object, bool> canExecute = null) {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public async void Execute(object parameter) {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter) {
            await execute();
        }
    }
}
