using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Diagnostics;
using System.ComponentModel;

namespace HelppoLasku.ViewModels
{
    public class WorkspaceControlViewModel : ViewModelBase
    {
        ObservableCollection<WorkspaceViewModel> workspaces;

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (workspaces == null)
                {
                    workspaces = new ObservableCollection<WorkspaceViewModel>();
                }
                return workspaces;
            }
        }

        WorkspaceViewModel selectedWorkspace;

        public WorkspaceViewModel SelectedWorkspace
        {
            get { return Workspaces.Count > 0 ? selectedWorkspace : null; }
            set
            {
                selectedWorkspace = value;
                ICollectionView collection = CollectionViewSource.GetDefaultView(Workspaces);
                if (collection != null)
                    collection.MoveCurrentTo(value);
            }
        }

        public void New(ViewModelBase viewmodel, bool unique)
        {
            if (unique)
            { 
                WorkspaceViewModel workspace = ContainsViewModel(viewmodel);
                if (workspace != null)
                {
                    SelectedWorkspace = workspace;
                    return;
                }     
            }   
            WorkspaceViewModel newspace = new WorkspaceViewModel(this, viewmodel);
            Workspaces.Add(newspace);
            SelectedWorkspace = newspace;
        }

        public WorkspaceViewModel ContainsViewModel(ViewModelBase viewmodel)
        {
            foreach(WorkspaceViewModel workspace in Workspaces)
            {
                if (workspace.Content.GetType() == viewmodel.GetType() && workspace.Content.DisplayName == viewmodel.DisplayName)
                    return workspace;
            }
            return null;
        }
    }
}
