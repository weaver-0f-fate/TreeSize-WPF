using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Models {
    public class DirectoryFile : AbstractFile {
        private DirectoryInfo _directoryInfo;
        public ObservableCollection<AbstractFile> NestedItems { get; set; }
     

        public DirectoryFile(DirectoryInfo info) : base(info) {
            _directoryInfo = info;
            IsDirectory = true;
            NestedItems = new ObservableCollection<AbstractFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public void LoadNestedDirectories() {
            foreach (var dir in _directoryInfo.GetDirectories()) {
                try {
                    var directory = new DirectoryFile(dir);
                    NestedItems.Add(directory);
                }
                catch (Exception) { }
            }
        }

        public void LoadNestedFiles() {
            foreach (var file in _directoryInfo.GetFiles()) {
                try {
                    var nestedFile = new SystemFile(file);
                    NestedItems.Add(nestedFile);
                    Size += file.Length;
                }
                catch { }
            }
        }
    }
}
