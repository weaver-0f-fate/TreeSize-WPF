using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Models {
    public class SystemFile : INotifyPropertyChanged  {
        public string Name { get; set; }
        public Size Size { get; set; }
        public FileSystemInfo FileSystemInfo { get; set; }
        public Enums.FileType Type { get; }


        public SystemFile(FileSystemInfo info) {
            Name = info.Name;
            Size = new Size(0);
            FileSystemInfo = info;
            Type = info switch {
                DirectoryInfo => Enums.FileType.Folder,
                FileInfo => Enums.FileType.File,
                _ => Enums.FileType.Unknown
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
