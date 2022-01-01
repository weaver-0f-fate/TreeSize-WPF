using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Models {
    public class SystemFile : INotifyPropertyChanged {
        private Size _size;
        private ObservableCollection<SystemFile> _nestedItems;

        #region Properties
        public Size Size {
            get {
                return _size;
            }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }
        public FileSystemInfo FileSystemInfo { get; set; }
        public ObservableCollection<SystemFile> NestedItems {
            get {
                return _nestedItems;
            }
            set {
                _nestedItems = value;
                OnPropertyChanged("NestedItems");
            }
        }
        public Enums.FileType FileType { get; set; }
        public string Name { get; set; }
        #endregion

        public SystemFile(FileSystemInfo info) {
            Name = info.Name;
            FileSystemInfo = info;
            FileType = info switch {
                DirectoryInfo => Enums.FileType.Folder,
                FileInfo => Enums.FileType.File,
                _ => Enums.FileType.Unknown
            };
            Size = info is FileInfo f ? new Size(f.Length) : new Size(0);
            NestedItems = new ObservableCollection<SystemFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public static async Task LoadNestedDirectories(SystemFile root) {
            var rootDirectory = root.FileSystemInfo as DirectoryInfo;

            foreach(var dir in rootDirectory.GetDirectories()) {
                try {
                    var directory = new SystemFile(dir);
                    
                    await Task.Run(() => LoadNestedDirectories(directory));
                    root.NestedItems.Add(directory);
                }
                catch (Exception) { }
            }
        }



        public static async Task<double> LoadNestedFiles(SystemFile root) {
            var rootDirectory = root.FileSystemInfo as DirectoryInfo;

            foreach(var dir in root.NestedItems) {
                try {
                    root.Size.Amount += await Task.Run(() => LoadNestedFiles(dir));
                }
                catch (Exception) { }
            }

            foreach (var file in rootDirectory.GetFiles()) {
                var nestedFile = new SystemFile(file);
                root.NestedItems.Add(nestedFile);
                root.Size.Amount += file.Length;
            }

            return root.Size.Amount;
        }

        private static async Task LoadNested(SystemFile root, SystemFile nested) {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
