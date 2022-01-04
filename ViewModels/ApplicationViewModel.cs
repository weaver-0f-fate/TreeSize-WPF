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
        private ICommand _selectDirectoryCommand;
        private IAsyncCommand _analyzeDirectoryCommand;
        private DirectoryFile _rootDirectory;
        private string _selectedDirectoryPath;
        private bool _inProgress;

        public ApplicationViewModel() {
            _analyzeDirectoryCommand ??= new BaseAsyncCommand(GetFilesCommandAsync, CanExecute);
            _selectDirectoryCommand ??= new RelayCommand(x => {
                using (var folderDialog = new FolderBrowserDialog()) {
                    var result = folderDialog.ShowDialog();
                    if (result == DialogResult.OK) {
                        SelectedDirectoryPath = folderDialog.SelectedPath;
                    }
                }
            });
        }

        #region Properties
        public string SelectedDirectoryPath {
            get {
                return _selectedDirectoryPath;
            }
            set {
                _selectedDirectoryPath = value;
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
        public ICommand SelectDirectoryCommand {
            get {
                return _selectDirectoryCommand ??= new RelayCommand(x => {
                    using (var folderDialog = new FolderBrowserDialog()) {
                        var result = folderDialog.ShowDialog();
                        if (result == DialogResult.OK) {
                            SelectedDirectoryPath = folderDialog.SelectedPath;
                        }
                    }
                });
            }
        }

        public IAsyncCommand AnalyzeDirectoryCommand {
            get {
                return _analyzeDirectoryCommand ??= new BaseAsyncCommand(GetFilesCommandAsync, CanExecute);
            }
        }
        private async Task GetFilesCommandAsync() {
            InProgress = true;

            RootDirectory = null;
            GC.Collect();

            var directoryInfo = new DirectoryInfo(SelectedDirectoryPath);
            RootDirectory = new DirectoryFile(directoryInfo);

            await Task.Run(() => ReadDirectoryService.ReadRootDirectory(RootDirectory));

            InProgress = false;
        }
        private bool CanExecute(object parameter) {
            return !string.IsNullOrEmpty(SelectedDirectoryPath) && !InProgress;
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged is not null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
