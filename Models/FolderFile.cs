using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
    public class FolderFile : AbstractFile{
        public ObservableCollection<AbstractFile> NestedItems { get; set; }

        public FolderFile(DirectoryInfo directoryInfo) {
            var list = new ObservableCollection<AbstractFile>();
            long size = 0;

            foreach (var directory in directoryInfo.GetDirectories()) {
                var folderFile = new FolderFile(directory);
                size += folderFile.Size;
                list.Add(folderFile);
            }

            foreach (var file in directoryInfo.GetFiles()) {
                var f = new File(file);
                size += f.Size;
                list.Add(f);
            }


            Name = directoryInfo.Name;
            NestedItems = list;
            Size = size;
            Type = FileType.Folder;
        }
    }
}
