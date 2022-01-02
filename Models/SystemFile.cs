using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Models {
    public class SystemFile : INotifyPropertyChanged {
        private Size _size;
        private DirectoryInfo _directoryInfo;

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
        public ObservableCollection<SystemFile> NestedItems { get; set; }
        public Enums.FileType FileType { get; set; }
        public string Name { get; set; }
        #endregion

        public SystemFile(FileSystemInfo info) {
            Name = info.Name;

            if (info is DirectoryInfo dirInfo) {
                _directoryInfo = dirInfo;
                FileType = Enums.FileType.Directory;
            }
            else {
                FileType = Enums.FileType.File;
            }

            Size = info is FileInfo f ? new Size(f.Length) : new Size(0);
            NestedItems = new ObservableCollection<SystemFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public void LoadNestedDirectories() {
            foreach (var dir in _directoryInfo.GetDirectories()) {
                try {
                    var directory = new SystemFile(dir);
                    NestedItems.Add(directory);
                }
                catch (Exception) { }
            }
        }
        public void LoadNestedFiles() {

            foreach (var dir in NestedItems) {
                Size.Amount += dir.Size.Amount;
                
            }

            foreach (var file in _directoryInfo.GetFiles()) {
                var nestedFile = new SystemFile(file);
                NestedItems.Add(nestedFile);
                Size.Amount += file.Length;
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
