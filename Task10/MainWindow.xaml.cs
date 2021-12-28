using System.Windows;
using ViewModels;

namespace Task10 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            DataContext = new ApplicationViewModel();
            InitializeComponent();
        }
    }
}
