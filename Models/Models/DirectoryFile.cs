using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Models {
    public class DirectoryFile : AbstractFile {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryFile(DirectoryInfo info) : base(info) {
            _directoryInfo = info;
            IsDirectory = true;
            NestedItems = new ObservableCollection<AbstractFile>();
            BindingOperations.EnableCollectionSynchronization(NestedItems, new object());
        }

        public async Task ReadRootDirectoryWithMultiThread() {
            LoadNestedDirectories();

            var tasks = new List<Task>();

            foreach (var item in NestedItems) {
                tasks.Add(Task.Run(() => ((DirectoryFile)item).ReadRootDirectory()));
            }
            await Task.WhenAll(tasks);

            LoadNestedFiles();
        }


        public void ReadRootDirectory() {
            LoadNestedDirectories();

            foreach (DirectoryFile dir in NestedItems) {
                dir.ReadRootDirectory();
                Size += dir.Size;
            }

            LoadNestedFiles();
        }

   

        private void LoadNestedDirectories() {
            try {
                foreach (var dir in _directoryInfo.GetDirectories()) {
                    try {
                        NestedItems.Add(new DirectoryFile(dir));
                    }
                    catch { }
                }
            }
            catch { } 
        }

        private void LoadNestedFiles() {
            try {
                foreach (var file in _directoryInfo.GetFiles()) {
                    NestedItems.Add(new SystemFile(file));
                    Size += file.Length;
                }
            }
            catch {}
        }
    }
}
