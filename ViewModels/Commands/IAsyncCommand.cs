using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModels.Commands {
    public interface IAsyncCommand : ICommand {
        Task ExecuteAsync(object parameter);
    }
}
