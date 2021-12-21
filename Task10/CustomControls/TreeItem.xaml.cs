using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task10.CustomControls {
    /// <summary>
    /// Interaction logic for TreeItem.xaml
    /// </summary>
    public partial class TreeItem : UserControl {
        public TreeItem(FileSystemInfo directoryInfo) {
            InitializeComponent();

            FileName.Text = $"{directoryInfo.Name} | ";
            if (directoryInfo is FileInfo fi) {
                FileSize.Text = $"{fi.Length} Bytes";
            }
        }
    }
}
