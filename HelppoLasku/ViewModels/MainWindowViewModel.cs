using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static WorkspaceControlViewModel WorkspaceControl { get; private set; }

        public MainWindowViewModel()
        {
            Resources.Initialize();

            DisplayName = "HelppoLasku";

            WorkspaceControl = new WorkspaceControlViewModel();
            MainMenu = new MainMenuViewModel();   
        }

        public MainMenuViewModel MainMenu { get; private set; }

        public void OnClosed(object sender, EventArgs e)
        {
            MainMenu.Dispose();
            WorkspaceControl.Dispose();

            Dispose();
        }

        bool saved = true;

        public void OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = saved ? false : MessageBox.Show("Exit ?", "Confirm", MessageBoxButton.OKCancel) != MessageBoxResult.OK;
        }
    }
}
