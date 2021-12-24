using Models;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using ViewModels.Commands;

namespace ViewModels {
    public class ApplicationViewModel : INotifyPropertyChanged {
        private SystemFile _rootDirectory;
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

        public SystemFile RootDirectory {
            get {
                return _rootDirectory;
            }
            set {
                _rootDirectory = value;
                OnPropertyChanged("RootDirectory");
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
                return _analyzeFolderCommand ??= new RelayCommand(async x => {
                        var info = new DirectoryInfo(SelectedFolderPath);
                        RootDirectory = await SystemFile.GetSystemFile(info);
                    },
                    x => {
                        return _selectedFolderPath is not null;
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
