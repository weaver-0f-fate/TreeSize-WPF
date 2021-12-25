using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
    public class Size {

        public double Amount { get; set; }
        public string Text { get; set; }

        public Size(double size) {
            Amount = size;

            const long GbSize = 1000000000;
            const int MbSize = 1000000;
            const int KbSize = 1000;

            Text = size switch {
                >= GbSize => $"{Math.Round(size / GbSize, 2)} GB",
                >= MbSize => $"{Math.Round(size / MbSize, 2)} MB",
                >= KbSize => $"{Math.Round(size / KbSize, 2)} KB",
                _ => $"{size} Byte",
            };
        }
    }
}
