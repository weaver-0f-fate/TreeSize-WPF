using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services {
    public static class ReadAsyncService {

        public static async Task ReadDirectoriesAsync(SystemFile rootDirectory) {
            rootDirectory.LoadNestedDirectories();
            foreach(var dir in rootDirectory.NestedDirectories) {
                try {
                    await ReadDirectoriesAsync(dir);
                }
                catch (Exception) {}
            }
        }

        public static async Task ReadFilesAsync(SystemFile rootDirectory) {
         
            foreach(var dir in rootDirectory.NestedDirectories) {
                try {
                    await ReadFilesAsync(dir);
                }
                catch (Exception) { }
            }
            rootDirectory.LoadNestedFiles();
        }

    }
}
