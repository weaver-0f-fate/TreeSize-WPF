using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Models {
    public abstract class AbstractFile : INotifyPropertyChanged {
        private double _size;

        #region Properties
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public double Size {
            get {
                return _size;
            }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }
        #endregion 

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
