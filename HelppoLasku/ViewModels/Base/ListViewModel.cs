using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HelppoLasku.ViewModels
{
    public abstract class ListViewModel : CollectionViewModel
    {
        protected ListViewModel(System.Collections.ObjectModel.ObservableCollection<DataAccess.DataModel> source) : base(source) { }

        CollectionView view;

        public CollectionView View
        {
            get
            {
                if (view == null)
                    view = (CollectionView)CollectionViewSource.GetDefaultView(Items);
                return view;
            }
        }

        FilterViewModel filter;

        public virtual FilterViewModel Filter
        {
            get { return filter; }
            set
            {
                if (filter != value)
                {
                    if (filter != null)
                        filter.FilterChanged -= OnFiltersChanged;

                    filter = value;
                    filter.List = this;
                    filter.FilterChanged += OnFiltersChanged;
                    View.Filter = value.Filter;
                }
            }
        }

        public virtual void OnFiltersChanged(object sender, FilterChangedEventArgs e)
        {
            View.Refresh();
        }

        protected override void OnDispose()
        {
            if (filter != null)
            {
                filter.FilterChanged -= OnFiltersChanged;
            }
            base.OnDispose();
        }
    }
}
