using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public class EditCommandsViewModel
    {
        Action OnSave;
        Func<bool> CanSave;

        Action OnCancel;
        Func<bool> CanCancel;

        public EditCommandsViewModel(Action onSave, Func<bool> canSave, Action onCancel, Func<bool> canCancel)
        {
            OnSave = onSave;
            CanSave = canSave;
            OnCancel = onCancel;
            CanCancel = canCancel;
        }

        CommandViewModel saveCommand;

        public CommandViewModel Save
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new CommandViewModel("Tallenna", OnSave, CanSave);
                return saveCommand;
            }
        }

        CommandViewModel cancelCommand;

        public CommandViewModel Cancel
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new CommandViewModel("Peruuta", OnCancel, CanCancel);
                return cancelCommand;
            }
        } 
    }
}
