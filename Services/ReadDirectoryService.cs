using Models;
using System.Threading.Tasks;

namespace Services {
    public static class ReadDirectoryService {
        public static void ReadRootDirectory(DirectoryFile rootDirectory) {
            ReadDirectory(rootDirectory);
            rootDirectory.LoadNestedFiles();
        }

        private static void ReadDirectory(DirectoryFile directory) {
            directory.LoadNestedDirectories();

            Parallel.ForEach(directory.NestedItems, dir => {
                try {
                    ReadDirectory((DirectoryFile)dir);
                    ((DirectoryFile)dir).LoadNestedFiles();
                    directory.Size += ((DirectoryFile)dir).Size;
                }
                catch { }
            });
        }
    }
}
