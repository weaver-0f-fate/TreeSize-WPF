using Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Services {
    public static class ReadDirectoryService {
        public static void ReadRootDirectory(DirectoryFile rootDirectory) {
            PrintCurrentLevel(rootDirectory);
        }

        public static void PrintCurrentLevel(DirectoryFile root) {
            //root.Initialize();
            foreach(var item in root.NestedItems) {
                if(item is DirectoryFile dir) {
                    //Task.Run(() => PrintCurrentLevel(dir));
                    PrintCurrentLevel(dir);
                    root.Size += dir.Size;
                }
            }
        }
    }
}
