using Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ViewModels {
    public class SystemFileViewModel : INotifyPropertyChanged {
        private ObservableCollection<SystemFileViewModel> _nestedItems;

        public SystemFile Root { get; set; }
        public ObservableCollection<SystemFileViewModel> NestedItems { 
            get {
                return _nestedItems;
            }
            set {
                _nestedItems = value;
                OnPropertyChanged("NestedItems");
            }
        }

        public SystemFileViewModel(FileSystemInfo fileSystemInfo) {
            Root = new SystemFile(fileSystemInfo);
            NestedItems = new ObservableCollection<SystemFileViewModel>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

   

        public async Task ReadRootDirectory() {
            var fileSystemInfo = Root.FileSystemInfo;

            if (fileSystemInfo is FileInfo info) {
                NestedItems.Add(new SystemFileViewModel(info));
            }
            if (fileSystemInfo is DirectoryInfo directoryInfo) {

                foreach (var directory in directoryInfo.GetDirectories()) {
                    try {
                        var dir = new SystemFileViewModel(directory);
                        NestedItems.Add(dir);
                        await dir.ReadRootDirectory();
                        Root.Size.Amount += dir.Root.Size.Amount;
                    }
                    catch (Exception) {

                    }
                }


                foreach (var sysFile in directoryInfo.GetFiles()) {
                    var file = new SystemFileViewModel(sysFile);
                    file.Root.Size.Amount += sysFile.Length;
                    Root.Size.Amount += sysFile.Length;
                    NestedItems.Add(file);
                }
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
