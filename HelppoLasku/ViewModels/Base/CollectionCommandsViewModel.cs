using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public class CollectionCommandsViewModel : ViewModelBase
    {
        Action OnNew;
        Func<bool> CanNew;
        Action OnEdit;
        Func<bool> CanEdit;
        Action  OnCopy;
        Func<bool> CanCopy;
        Action  OnDelete;
        Func<bool>  CanDelete;

        public CollectionCommandsViewModel(Action onNew, Func<bool> canNew, Action onEdit, Func<bool> canEdit, Action onCopy, Func<bool> canCopy, Action onDelete, Func<bool> canDelete)
        {
            OnNew = onNew;
            CanNew = canNew;
            OnEdit = onEdit;
            CanEdit = canEdit;
            OnCopy = onCopy;
            CanCopy = canCopy;
            OnDelete = onDelete;
            CanDelete = canDelete;
        }

        CommandViewModel newCommand;

        public CommandViewModel New
        {
            get
            {
                if (newCommand == null)
                    newCommand = new CommandViewModel("Luo uusi", OnNew, CanNew);
                return newCommand;
            }
        }

        CommandViewModel editCommand;

        public CommandViewModel Edit
        {
            get
            {
                if (editCommand == null)
                    editCommand = new CommandViewModel("Muokkaa", OnEdit, CanEdit);
                return editCommand;
            }
        }

        CommandViewModel copyCommand;

        public CommandViewModel Copy
        {
            get
            {
                if (copyCommand == null)
                    copyCommand = new CommandViewModel("Kopioi", OnCopy, CanCopy);
                return copyCommand;
            }
        }

        CommandViewModel deleteCommand;

        public CommandViewModel Delete
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new CommandViewModel("Poista", OnDelete, CanDelete);
                return deleteCommand;
            }
        }
    }
}
