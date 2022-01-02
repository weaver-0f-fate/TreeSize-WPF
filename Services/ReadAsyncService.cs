using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    public static class ReadAsyncService {

        public static async Task ReadRootDirectoryAsync(SystemFile rootDirectory) {
            rootDirectory.LoadNestedDirectories();
            var tasks = new List<Task>();

            foreach (var dir in rootDirectory.NestedItems) {
                tasks.Add(ReadRootDirectoryAsync(dir));
            }

            try {
                await Task.WhenAll(tasks);
            }
            catch { }

            rootDirectory.LoadNestedFiles();
        }
    }
}
