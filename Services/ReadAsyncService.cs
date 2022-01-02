using Models;
using System.Linq;
using System.Threading.Tasks;

namespace Services {
    public static class ReadAsyncService {
        public static async Task ReadRootDirectoryAsync(DirectoryFile rootDirectory) {
            await ReadDirectoryAsync(rootDirectory);
            rootDirectory.LoadNestedFiles();
        }
        private static async Task ReadDirectoryAsync(DirectoryFile rootDirectory) {
            rootDirectory.LoadNestedDirectories();

            var tasks = rootDirectory.NestedItems.Select(nestedDirectory => ReadDirectoryAsync(rootDirectory, (DirectoryFile)nestedDirectory));
            try {
                await Task.WhenAll(tasks);
            }
            catch { }
            
        }
        private static async Task ReadDirectoryAsync(DirectoryFile rootDir, DirectoryFile dir) {
            await ReadDirectoryAsync(dir);
            dir.LoadNestedFiles();
            rootDir.Size += dir.Size;
        }
    }
}
