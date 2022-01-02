using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Models {
    public abstract class AbstractFile : INotifyPropertyChanged {
        private double _size;
        private string _sizeText;

        #region Properties
        public string Name { get; set; }
        public bool IsDirectory { get; set; }
        public string SizeText {
            get {
                return _sizeText;
            }
            set {
                _sizeText = value;
                OnPropertyChanged("SizeText");
            }
        }
        public double Size {
            get {
                return _size;
            }
            set {
                _size = value;
                SizeText = setText(value);
            }
        }
        #endregion 

        public AbstractFile(FileSystemInfo info) {
            Name = info.Name;
            Size = default;
        }

        private string setText(double value) {
            const long GbSize = 1000000000;
            const int MbSize = 1000000;
            const int KbSize = 1000;

            return value switch {
                >= GbSize => $"{Math.Round(value / GbSize, 2)} GB",
                >= MbSize => $"{Math.Round(value / MbSize, 2)} MB",
                >= KbSize => $"{Math.Round(value / KbSize, 2)} KB",
                _ => $"{value} Byte",
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
