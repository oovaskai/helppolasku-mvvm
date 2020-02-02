using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HelppoLasku.ViewModels
{
    public class CommandViewModel : ViewModelBase
    {
        public CommandViewModel(string displayName, Action onExecute) : this(displayName, displayName, onExecute)
        {
        }

        public CommandViewModel(string displayName, string tooltip, Action onExecute) : this(displayName, tooltip, onExecute, null)
        {
        }

        public CommandViewModel(string displayName, Action onExecute, Func<bool> canExecute) : this(displayName, displayName, onExecute, canExecute)
        {
        }

        public CommandViewModel(string displayName, string tooltip, Action onExecute, Func<bool> canExecute)
        {
            DisplayName = displayName;
            ToolTip = tooltip;

            if (canExecute == null)
                Command = new CustomCommand(parameter => onExecute(), null);
            else
                Command = new CustomCommand(parameter => onExecute(), parameter => canExecute());
        }

        public CustomCommand Command { get; private set; }

        public string ToolTip { get; private set; }


        System.Windows.Visibility visibility;

        public System.Windows.Visibility Visibility
        {
            get => visibility;
            set
            {
                if (visibility != value)
                {
                    visibility = value;
                    RaisePropertyChanged("Visibility");
                }
            }
        }
    }
}
