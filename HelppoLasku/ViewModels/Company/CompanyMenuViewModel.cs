using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class CompanyMenuViewModel : CollectionViewModel
    {
        public CompanyMenuViewModel(ObservableCollection<DataAccess.DataModel> source) : base(source)
        {
            DisplayName = "Yritys";
        }

        public new CompanyMenuItemViewModel SelectedItem
        {
            get => base.SelectedItem as CompanyMenuItemViewModel;
            set
            {
                if (base.SelectedItem != value)
                {
                    if (value == null)
                        DisplayName = "Yritys";
                    else
                        DisplayName = value.Name;

                    base.SelectedItem = value;
                }
            }
        }

        public override bool IsEnabled => Items.Count > 0;

        public override void OnSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("IsEnabled");
            base.OnSourceCollectionChanged(sender, e);

            if (e.NewItems != null && e.NewItems.Count > 0)
                foreach (Company company in e.NewItems)
                    SelectedItem = FindByID(company.ID) as CompanyMenuItemViewModel;
        }

        public override DataViewModel NewItem(DataAccess.DataModel model)
            => new CompanyMenuItemViewModel(model as Company, this);

        public override void OnNew()
        {
            MainWindowViewModel.WorkspaceControl.New(new EditCompanyViewModel(new Company()), false);
        }

        public override void OnEdit()
        {
            MainWindowViewModel.WorkspaceControl.New(new EditCompanyViewModel(new Company(SelectedItem.Model)), true);
        }
    }
}
