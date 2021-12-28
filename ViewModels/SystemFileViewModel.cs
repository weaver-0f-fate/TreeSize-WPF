using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewModels {
    public class SystemFileViewModel : INotifyPropertyChanged {
        private ObservableCollection<SystemFileViewModel> _nestedItems;
        private SystemFile _root;

        public SystemFileViewModel(FileSystemInfo fileSystemInfo) {
            Root = new SystemFile(fileSystemInfo);
            NestedItems = new ObservableCollection<SystemFileViewModel>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        #region Properties
        public SystemFile Root {
            get {
                return _root;
            }
            set {
                _root = value;
                OnPropertyChanged("Root");
            }
        }
        public ObservableCollection<SystemFileViewModel> NestedItems {
            get {
                return _nestedItems;
            }
            set {
                _nestedItems = value;
                OnPropertyChanged("NestedItems");
            }
        }
        #endregion


        public async Task LoadFilesAsync() {
            await LoadFilesAsync(Root.FileSystemInfo as DirectoryInfo);
        }
        private async Task LoadFilesAsync(DirectoryInfo rootDirectoryInfo) {



            foreach (var nestedDirectoryInfo in rootDirectoryInfo.GetDirectories()) {

                try {
                    var dir = new SystemFileViewModel(nestedDirectoryInfo);

                    await dir.LoadFilesAsync();

                    NestedItems.Add(dir);
                    
                    Root.Size.Amount += dir.Root.Size.Amount;
                }
                catch (Exception) { }
            }

            foreach (var sysFile in rootDirectoryInfo.GetFiles()) {
                var file = new SystemFileViewModel(sysFile);
                Root.Size.Amount += sysFile.Length;
                NestedItems.Add(file);
            }

          

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
