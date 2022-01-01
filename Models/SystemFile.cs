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
        private ObservableCollection<SystemFile> _nestedItems;
        private List<SystemFile> _nestedDirectories;
        private List<SystemFile> _nestedFiles;

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

        public List<SystemFile> NestedDirectories {
            get {
                return _nestedDirectories;
            }
            set {
                _nestedDirectories = value;
                OnPropertyChanged("NestedDirectories");
            }
        }
        public List<SystemFile> NestedFiles {
            get {
                return _nestedFiles;
            }
            set {
                _nestedFiles = value;
                OnPropertyChanged("NestedFiles");
            }
        }
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
            NestedDirectories = new List<SystemFile>();
            NestedFiles = new List<SystemFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public void LoadNestedDirectories() {
            var rootDirectory = FileSystemInfo as DirectoryInfo;

            foreach (var dir in rootDirectory.GetDirectories()) {
                try {
                    var directory = new SystemFile(dir);
                    AddDirectory(directory);
                }
                catch (Exception) { }
            }
        }

        public void LoadNestedFiles() {
            var rootDirectory = FileSystemInfo as DirectoryInfo;

            foreach (var dir in NestedDirectories) {
                try {
                    Size.Amount += dir.Size.Amount;
                }
                catch (Exception) { }
            }

            foreach (var file in rootDirectory.GetFiles()) {
                var nestedFile = new SystemFile(file);
                AddFile(nestedFile);
                Size.Amount += file.Length;
            }
        }

        private void AddDirectory(SystemFile directory) {
            NestedDirectories.Add(directory);
            NestedItems.Add(directory);
        }

        private void AddFile(SystemFile file) {
            NestedFiles.Add(file);
            NestedItems.Add(file);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
