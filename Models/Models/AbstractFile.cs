using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Models {
    public abstract class AbstractFile : INotifyPropertyChanged {
        private double _size;

       
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public ObservableCollection<AbstractFile> NestedItems { get; set; }
        public double Size {
            get {
                return _size;
            }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }

        public AbstractFile(FileSystemInfo info) {
            Name = info.Name;
            Size = default;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
