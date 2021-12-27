using Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace ViewModels {
    public class SystemFileViewModel {
        public SystemFile Root { get; set; }
        public ObservableCollection<SystemFileViewModel> NestedItems { get; set; }


        private SystemFileViewModel(SystemFile file, double size = 0, ObservableCollection<SystemFileViewModel> list = null) {
            file.Size = new Size(size);
            Root = file;
            NestedItems = list ?? new ObservableCollection<SystemFileViewModel>();
        }

        public static async Task<SystemFileViewModel> GetSystemFileViewModelAsync(FileSystemInfo fileInfo) {
            var systemFile = new SystemFile(fileInfo);

            if (fileInfo is DirectoryInfo directoryInfo) {
                var list = new ObservableCollection<SystemFileViewModel>();
                double size = 0;

                foreach (var directory in directoryInfo.GetDirectories()) {
                    try {
                        var dir = await GetSystemFileViewModelAsync(directory);
                        size += dir.Root.Size.Amount;
                        list.Add(dir);
                    }
                    catch (Exception) {
                        list.Add(new SystemFileViewModel(systemFile));
                    }
                }


                foreach (var sysFile in directoryInfo.GetFiles()) {
                    var file = await GetSystemFileViewModelAsync(sysFile);
                    size += sysFile.Length;
                    list.Add(file);

                }
                return new SystemFileViewModel(systemFile, size, list);
            }

            if (fileInfo is FileInfo f) {
                return new SystemFileViewModel(systemFile, f.Length);
            }

            throw new Exception();
        }
    }
}
