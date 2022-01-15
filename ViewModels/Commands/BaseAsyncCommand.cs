using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.Commands {
    public class BaseAsyncCommand : IAsyncCommand {
        private readonly Func<Task> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public BaseAsyncCommand(Func<Task> execute, Func<object, bool> canExecute = null) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter) {
            await ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter) {
            await _execute();
        }
    }
}
