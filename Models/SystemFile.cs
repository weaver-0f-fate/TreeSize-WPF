using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Models {
    public class SystemFile : INotifyPropertyChanged  {
        private Size _size;

        #region Properties
        public string Name { get; set; }
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
        public Enums.FileType Type { get; }
        #endregion

        public SystemFile(FileSystemInfo info) {
            Name = info.Name;
            FileSystemInfo = info;
            Type = info switch {
                DirectoryInfo => Enums.FileType.Folder,
                FileInfo => Enums.FileType.File,
                _ => Enums.FileType.Unknown
            };
            Size = info is FileInfo f ? new Size(f.Length) : new Size(0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
