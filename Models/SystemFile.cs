using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;


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


        public SystemFile(FileSystemInfo fileInfo) {

            if(fileInfo is DirectoryInfo directoryInfo) {
                var list = new ObservableCollection<SystemFile>();
                long size = 0;

                foreach (var directory in directoryInfo.GetDirectories()) {
                    var folderFile = new SystemFile(directory);
                    size += folderFile.Size;
                    list.Add(folderFile);
                }

                foreach (var systemFile in directoryInfo.GetFiles()) {
                    var f = new SystemFile(systemFile);
                    size += f.Size;
                    list.Add(f);
                }


                Name = directoryInfo.Name;
                NestedItems = list;
                Size = size;
                Type = FileType.Folder;
            }
            if(fileInfo is FileInfo file) {
                Name = file.Name;
                Size = file.Length;
                Type = FileType.File;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
