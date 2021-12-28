using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models {
    public class Size : INotifyPropertyChanged{
        private double _amount;
        private string _text;
        public double Amount {
            get {
                return _amount;
            } 
            set {
                _amount = value;
                Text = setText();
                OnPropertyChanged("Amount");
            }
        }
        public string Text {
            get {
                return _text;
            } 
            set {
                _text = value;
                OnPropertyChanged("Text");
            } 
        }

        public Size(double size) {
            Amount = size;
            setText();
        }

        private string setText() {
            const long GbSize = 1000000000;
            const int MbSize = 1000000;
            const int KbSize = 1000;

            return _amount switch {
                >= GbSize => $"{Math.Round(_amount / GbSize, 2)} GB",
                >= MbSize => $"{Math.Round(_amount / MbSize, 2)} MB",
                >= KbSize => $"{Math.Round(_amount / KbSize, 2)} KB",
                _ => $"{_amount} Byte",
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
