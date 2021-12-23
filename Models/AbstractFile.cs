using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models {
    public abstract class AbstractFile : INotifyPropertyChanged {
        private string _name;
        private long _size;
        private FileType _type;

        public string Name {
            get { return _name; }
            set {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public long Size {
            get { return _size; }
            set {
                _size = value;
                OnPropertyChanged("Size");
            }
        }

        public FileType Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
