using System.Collections.ObjectModel;
using System.IO;
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

        public void ReadRootDirectory() {
            Initialize();
            foreach (var item in NestedItems) {
                if (item is DirectoryFile dir) {
                    dir.ReadRootDirectory();
                    Size += dir.Size;
                }
            }
        }

        private void Initialize() {
            try {
                LoadNestedDirectories();
                LoadNestedFiles();
            }
            catch { }
        }

        private void LoadNestedDirectories() {
            foreach (var dir in _directoryInfo.GetDirectories()) {
                try {
                    addNested(new DirectoryFile(dir));
                }
                catch { }
            }
        }

        private void LoadNestedFiles() {
            foreach (var file in _directoryInfo.GetFiles()) {
                addNested(new SystemFile(file));
            }
        }

        private void addNested(AbstractFile file) {
            NestedItems.Add(file);
            Size += file.Size;
        }
    }
}
