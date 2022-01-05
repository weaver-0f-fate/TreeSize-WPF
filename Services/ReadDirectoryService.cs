using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services {
    public static class ReadDirectoryService {
        public static async Task ReadRootDirectory(DirectoryFile rootDirectory) {
            await ReadDirectory(rootDirectory);
            rootDirectory.LoadNestedFiles();
        }

        private static async Task ReadDirectory(DirectoryFile directory) {
            directory.LoadNestedDirectories();

            var tasks = new List<Task>();

            foreach (DirectoryFile dir in directory.NestedItems) {
                //tasks.Add(Task.Run(() => ReadDirectoryAsync(directory, dir)));
                tasks.Add(ReadDirectoryAsync(directory, dir));
            }

            await Task.WhenAll(tasks);

            //Parallel.ForEach(directory.NestedItems, dir => {
            //    try {
            //        ReadDirectory((DirectoryFile)dir);
            //        ((DirectoryFile)dir).LoadNestedFiles();
            //        directory.Size += ((DirectoryFile)dir).Size;
            //    }
            //    catch { }
            //});
        }

        private static async Task ReadDirectoryAsync(DirectoryFile rootDirectory, DirectoryFile directory) {
            try {
                await ReadDirectory(directory);
                directory.LoadNestedFiles();
                rootDirectory.Size += directory.Size;
            }
            catch { }
        }
    }
}
