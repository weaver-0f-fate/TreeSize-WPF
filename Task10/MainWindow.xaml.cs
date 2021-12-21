using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task10.CustomControls;

namespace Task10 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private string _selectedFolderPath;

        public MainWindow() {
            InitializeComponent();
        }

        private void SelectFolder(object sender, RoutedEventArgs e) {
            using var folderDialog = new FolderBrowserDialog(); 
            
            var result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                _selectedFolderPath = folderDialog.SelectedPath;
                FileURL.Text = $" | {folderDialog.SelectedPath}";
            }
        }

        private void Analyze(object sender, RoutedEventArgs e) {
            if (_selectedFolderPath is null) {
                return;
            }

            ListDirectory(_selectedFolderPath);
        }

        private void ListDirectory(string path) {
            DirectoryTree.Items.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            DirectoryTree.Items.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo) {
            var directoryNode = new TreeViewItem { Header = directoryInfo.Name };


            foreach (var directory in directoryInfo.GetDirectories()) {
                directoryNode.Items.Add(CreateDirectoryNode(directory));
            }

            foreach (var file in directoryInfo.GetFiles()) {
                directoryNode.Items.Add(new TreeViewItem() { Header = new TreeItem(file) });
            }

            return directoryNode;

        }
    }
}
