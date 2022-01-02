using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    public static class ReadAsyncService {
        public static async Task ReadRootDirectoryAsync(SystemFile rootDirectory) {
            await ReadDirectoryAsync(rootDirectory);
            rootDirectory.LoadNested();
        }
        private static async Task ReadDirectoryAsync(SystemFile directory) {
            directory.LoadNestedDirectories();
            var tasks = directory.NestedItems.Select(x => ReadDirectoryAsync(directory, x));
            
            try {
                await Task.WhenAll(tasks);
            }
            catch { }
        }

        private static async Task ReadDirectoryAsync(SystemFile rootDir, SystemFile dir) {
            await ReadDirectoryAsync(dir);
            dir.LoadNested();
            rootDir.Size += dir.Size;
        }
    }
}
