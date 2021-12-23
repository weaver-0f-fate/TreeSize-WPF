using System.IO;

namespace Models {
    public class File : AbstractFile{
        public File(FileInfo file) {
            Name = file.Name;
            Size = file.Length;
            Type = FileType.File;
        }
    }
}
