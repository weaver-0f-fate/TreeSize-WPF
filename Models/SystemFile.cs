using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Models {
    public class SystemFile : INotifyPropertyChanged {
        private DirectoryInfo _directoryInfo;
        private double _size;
        private string _sizeText;

        #region Properties
        public ObservableCollection<SystemFile> NestedItems { get; set; }
        public bool IsDirectory { get; }
        public string Name { get; }
        public string SizeText { 
            get {
                return _sizeText;
            }
            set {
                _sizeText = value;
                OnPropertyChanged("SizeText");
            }
        }
        public double Size {
            get {
                return _size;
            }
            set {
                _size = value;
                SizeText = setText(value);
            } 
        }
        #endregion

        public SystemFile(FileSystemInfo info) {
            Name = info.Name;

            if (info is DirectoryInfo dirInfo) {
                _directoryInfo = dirInfo;
                IsDirectory = true;
                NestedItems = new ObservableCollection<SystemFile>();
                BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
            }

            Size = info is FileInfo f ? f.Length : default;
            
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
                try {
                    Size += dir.Size;
                }
                catch { }
            }

            foreach (var file in _directoryInfo.GetFiles()) {
                try {
                    var nestedFile = new SystemFile(file);
                    NestedItems.Add(nestedFile);
                    Size += file.Length;
                }
                catch { }
            }
        }

        public void LoadNested() {
            foreach (var file in _directoryInfo.GetFiles()) {
                try {
                    var nestedFile = new SystemFile(file);
                    NestedItems.Add(nestedFile);
                    Size += file.Length;
                }
                catch { }
            }
        }


        private string setText(double value) {
            const long GbSize = 1000000000;
            const int MbSize = 1000000;
            const int KbSize = 1000;

            return value switch {
                >= GbSize => $"{Math.Round(value / GbSize, 2)} GB",
                >= MbSize => $"{Math.Round(value / MbSize, 2)} MB",
                >= KbSize => $"{Math.Round(value / KbSize, 2)} KB",
                _ => $"{value} Byte",
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
