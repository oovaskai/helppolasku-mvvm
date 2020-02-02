using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public abstract class FilterViewModel : ViewModelBase
    {
        protected FilterViewModel()
        {
            OnClear();
        }

        ListViewModel list;

        public ListViewModel List
        {
            get { return list; }
            set
            {
                if (list != value)
                {
                    list = value;
                    value.Filter = this;
                    RaisePropertyChanged("List");
                }
            }
        }

        public abstract bool Filter(object item);

        CommandViewModel clearCommand;

        public virtual CommandViewModel ClearCommand
        {
            get
            {
                if (clearCommand == null)
                    clearCommand = new CommandViewModel("Tyhjennä", OnClear, CanClear);
                return clearCommand;
            }
        }

        public abstract void OnClear();

        public virtual bool CanClear()
            => true;

        public EventHandler<FilterChangedEventArgs> FilterChanged;

        protected override void RaisePropertyChanged(string property)
        {
            base.RaisePropertyChanged(property);
            FilterChanged?.Invoke(this, new FilterChangedEventArgs(property));
        }
    }

    public class FilterChangedEventArgs : EventArgs
    {
        public FilterChangedEventArgs(string filterName)
        {
            FilterName = filterName;
        }

        public string FilterName { get; private set; }
    }
}
