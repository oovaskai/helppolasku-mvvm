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
using System.ComponentModel;
using Microsoft.Win32;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Views
{
    public partial class MainWindow : Window
    {
        static Window mainWindow;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel window = new MainWindowViewModel();
            Closing += new CancelEventHandler(window.OnClosing);
            Closed += new EventHandler(window.OnClosed);

            DataContext = window;

            mainWindow = this;
        }

        public static bool? EditDialog(DataViewModel content, double height, double width)
        {
            Controls.Dialog dialog = new Controls.Dialog();

            content.PropertyChanged += (object sender, PropertyChangedEventArgs e) => 
            {
                if (e.PropertyName == "EditEnabled" && content.EditEnabled == false)
                    dialog.Close();
            };

            dialog.DataContext = content;
            dialog.Height = height;
            dialog.Width = width;
            dialog.Owner = mainWindow;
            dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            return dialog.ShowDialog();
        }

        #region MessageBox Methods

        public static void Message(string message)
        {
            MessageBox.Show(message);
        }

        public static void Message(string message, string caption, MessageBoxImage icon)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
        }

        public static bool ConfirmMessage(string message, string caption, MessageBoxImage icon)
        {
            if (MessageBox.Show(message, caption, MessageBoxButton.YesNo, icon) == MessageBoxResult.Yes)
                return true;
            return false;
        }

        #endregion

        #region OpenFileDialog

        public static string[] OpenFileDialog(string filter, string initDir, bool multiSelect = false)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = filter;
            dialog.InitialDirectory = initDir;
            dialog.Multiselect = multiSelect;

            if (dialog.ShowDialog() == true)
                return dialog.FileNames;
            return null;
        }

        public class OpenFileDialogFilter
        {
            public OpenFileDialogFilter(string filter)
            {
                Title = filter.Split('.')[0];
                Extension = filter.Split('.')[1];
            }

            public string Title { get; private set; }

            public string Extension { get; private set; }

            public string Filter => Title + " (*." + Extension + ")|*." + Extension;


        }

        #endregion
    }
}
