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

        public async Task LoadNestedDirectoriesAsync() {
            var rootDirectory = FileSystemInfo as DirectoryInfo;

            foreach(var dir in rootDirectory.GetDirectories()) {
                try {
                    var directory = new SystemFile(dir);
                    
                    await Task.Run(() => directory.LoadNestedDirectoriesAsync());
                    NestedItems.Add(directory);
                }
                catch (Exception) { }
            }
        }



        public async Task<double> LoadNestedFiles() {
            var rootDirectory = FileSystemInfo as DirectoryInfo;

            foreach(var dir in NestedItems) {
                try {
                  Size.Amount += await Task.Run(() => dir.LoadNestedFiles());
                }
                catch (Exception) { }
            }

            foreach (var file in rootDirectory.GetFiles()) {
                var nestedFile = new SystemFile(file);
                NestedItems.Add(nestedFile);
                Size.Amount += file.Length;
            }

            return Size.Amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
