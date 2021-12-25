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
        private Size _size;

        #region Properties
        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public Size Size {
            get { return _size; }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }
        public Enums.FileType Type { get; }
        public ObservableCollection<SystemFile> NestedItems { get; set; }
        #endregion

        private SystemFile(string name, double size, Enums.FileType type, ObservableCollection<SystemFile> collection) {
            Name = name;
            Size = new Size(size);
            Type = type;
            NestedItems = collection;
        }


        public static async Task<SystemFile> GetSystemFile(FileSystemInfo fileInfo) {

            if (fileInfo is DirectoryInfo directoryInfo) {
                var list = new ObservableCollection<SystemFile>();
                double size = 0;


                foreach (var directory in directoryInfo.GetDirectories()) {
                    var dir = await GetSystemFile(directory);
                    size += dir.Size.Amount;
                    list.Add(dir);
                }
                

                foreach (var systemFile in directoryInfo.GetFiles()) {
                    var file = await GetSystemFile(systemFile);
                    size += file.Size.Amount;
                    list.Add(file);
                }
                return new SystemFile(fileInfo.Name, size, Enums.FileType.Folder, list);
            }
            
            if (fileInfo is FileInfo f) {
                return new SystemFile(f.Name, f.Length, Enums.FileType.File, new ObservableCollection<SystemFile>());
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
