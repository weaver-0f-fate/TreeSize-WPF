using System.IO;

namespace Models {
    public class SystemFile : AbstractFile{

        public SystemFile(FileInfo fileInfo) : base(fileInfo) {
            Size = fileInfo.Length;
        }

    }
}
