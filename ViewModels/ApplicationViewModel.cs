using Models;
using Services;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using ViewModels.Commands;

namespace ViewModels {
    public class ApplicationViewModel : INotifyPropertyChanged {
        private DirectoryFile _rootDirectory;
        private string _selectedFolderPath;
        private bool _inProgress;

        #region Properties
        public string SelectedFolderPath {
            get {
                return _selectedFolderPath;
            }
            set {
                _selectedFolderPath = value;
                OnPropertyChanged("SelectedFolderPath");
            }
        }
        public bool InProgress {
            get {
                return _inProgress;
            }
            set {
                _inProgress = value;
                OnPropertyChanged("InProgress");
            }
        }
        public DirectoryFile RootDirectory {
            get {
                return _rootDirectory;
            }
            set {
                _rootDirectory = value;
                OnPropertyChanged("RootDirectory");
            }
        }
        #endregion


        #region Commands
        private ICommand _selectFolderCommand;
        public ICommand SelectFolderCommand {
            get {
                return _selectFolderCommand ??= new RelayCommand(x => {
                    using (var folderDialog = new FolderBrowserDialog()) {
                        var result = folderDialog.ShowDialog();
                        if (result == DialogResult.OK) {
                            SelectedFolderPath = folderDialog.SelectedPath;
                        }
                    }
                });
            }
        }

        private IAsyncCommand _analyzeFolderCommand;
        public IAsyncCommand AnalyzeFolderCommand {
            get {
                return _analyzeFolderCommand ??= new BaseAsyncCommand(GetFilesCommandAsync, CanExecute);
            }
        }
        private async Task GetFilesCommandAsync() {
            InProgress = true;

            RootDirectory = null;
            GC.Collect();

            var directoryInfo = new DirectoryInfo(SelectedFolderPath);
            RootDirectory = new DirectoryFile(directoryInfo);

            await Task.Run(() => ReadAsyncService.ReadRootDirectoryAsync(RootDirectory));

            InProgress = false;
        }
        private bool CanExecute(object parameter) {
            return !string.IsNullOrEmpty(_selectedFolderPath) && !_inProgress;
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
