using System.IO;

namespace Models {
    public class SystemFile {
        public string Name { get; set; }
        public Size Size { get; set; }
        public Enums.FileType Type { get; }


        public SystemFile(FileSystemInfo info) {
            Name = info.Name;
            Type = info switch {
                DirectoryInfo => Enums.FileType.Folder,
                FileInfo => Enums.FileType.File,
                _ => Enums.FileType.Unknown
            };
        }

    }
}
