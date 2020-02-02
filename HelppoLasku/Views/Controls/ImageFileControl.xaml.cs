using System;
using System.Collections.Generic;
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

namespace HelppoLasku.Views.Controls
{
    /// <summary>
    /// Interaction logic for ImageFileControl.xaml
    /// </summary>
    public partial class ImageFileControl : UserControl
    {
        public ImageFileControl()
        {
            InitializeComponent();
        }

        private void File_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                    (DataContext as ViewModels.FileManagerViewModel).AddFile(file);
            }
            (sender as Grid).Background = Brushes.Transparent;
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            (sender as Grid).Background = Brushes.LightGreen;
        }

        private void Grid_DragLeave(object sender, DragEventArgs e)
        {
            (sender as Grid).Background = Brushes.Transparent;
        }
    }
}
