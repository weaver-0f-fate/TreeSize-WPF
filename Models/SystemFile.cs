using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Models {
    public class SystemFile : INotifyPropertyChanged{
        private string _name;
        private long _size;

        #region Properties
        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public long Size {
            get { return _size; }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }
        public FileType Type { get; }
        public ObservableCollection<SystemFile> NestedItems { get; set; }
        #endregion

        private SystemFile(string name, long size, FileType type, ObservableCollection<SystemFile> collection) {
            Name = name;
            Size = size;
            Type = type;
            NestedItems = collection;
        }


        public static async Task<SystemFile> GetSystemFile(FileSystemInfo fileInfo) {

            if (fileInfo is DirectoryInfo directoryInfo) {
                List<Task<SystemFile>> tasks = new List<Task<SystemFile>>();


                foreach (var directory in directoryInfo.GetDirectories()) {
                    tasks.Add(GetSystemFile(directory));
                }
                

                foreach (var systemFile in directoryInfo.GetFiles()) {
                    tasks.Add(GetSystemFile(systemFile));
                }


                var results = await Task.WhenAll(tasks);
                var list = new ObservableCollection<SystemFile>(results);


                long size = 0;
                foreach (var i in list) {
                    size += i.Size;
                }


                return new SystemFile(fileInfo.Name, size, FileType.Folder, list);
            }
            
            if (fileInfo is FileInfo file) {
                return new SystemFile(file.Name, file.Length, FileType.File, new ObservableCollection<SystemFile>());
            }

            throw new Exception();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
