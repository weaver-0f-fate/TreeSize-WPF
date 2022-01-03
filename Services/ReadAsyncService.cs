using Models;
using System.Threading.Tasks;

namespace Services {
    public static class ReadAsyncService {
        public static void ReadRootDirectoryAsync(DirectoryFile rootDirectory) {
            ReadDirectoryAsync(rootDirectory);
            rootDirectory .LoadNestedFiles();
        }
        private static void ReadDirectoryAsync(DirectoryFile rootDirectory) {
            rootDirectory.LoadNestedDirectories();

            Parallel.ForEach(rootDirectory.NestedItems, dir => {
                if (dir is DirectoryFile dirFile) {
                    try {
                        ReadDirectoryAsync(dirFile);
                        dirFile.LoadNestedFiles();
                        rootDirectory.Size += dirFile.Size;
                    }
                    catch { }
                }
            });
        }
    }
}
