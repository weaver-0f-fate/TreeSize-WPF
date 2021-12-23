using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ViewModels.Commands;

namespace ViewModels {
    public class ApplicationViewModel : INotifyPropertyChanged {


        private string _selectedFolderPath;
        public string SelectedFolderPath {
            get {
                return _selectedFolderPath;
            } 
            set {
                _selectedFolderPath = value;
                OnPropertyChanged("SelectedFolderPath");
            } 
        }



        #region Commands
        private RelayCommand _selectFolderCommand;
        public RelayCommand SelectFolderCommand {
            get {
                return _selectFolderCommand ??= new RelayCommand(x => {
                        using var folderDialog = new FolderBrowserDialog();
                        var result = folderDialog.ShowDialog();
                        if (result == DialogResult.OK) {
                            SelectedFolderPath = folderDialog.SelectedPath;
                        }
                    });
            }
        }

        private RelayCommand _analyzeFolderCommand;
        public RelayCommand AnalyzeFolderCommand {
            get {
                return _analyzeFolderCommand ??= new RelayCommand(x => { 
                        //TODO analyzeCommand logic
                    });
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
/*
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
 */
