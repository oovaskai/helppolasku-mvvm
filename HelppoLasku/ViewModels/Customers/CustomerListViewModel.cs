using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class CustomerListViewModel : ListViewModel
    {
        public CustomerListViewModel(ObservableCollection<DataModel> source) : base(source)
        {
            DisplayName = "Asiakkaat";
            Filter = new CustomerFilterViewModel();

            Invoice = new CommandViewModel("Laskuta", "Luo uusi lasku tälle asiakkaalle.", OnInvoice, CanEdit);
            Commands.Copy.Visibility = System.Windows.Visibility.Collapsed;
        }

        public override DataViewModel NewItem(DataModel model)
            => new CustomerViewModel(model as Customer);
        
        public new CustomerViewModel SelectedItem
        {
            get => base.SelectedItem as CustomerViewModel;
            set => base.SelectedItem = value;
        }

        public CommandViewModel Invoice { get; private set; }

        void OnInvoice()
        {
            Invoice invoice = new Invoice { Customer = SelectedItem.Model };
            MainMenuViewModel.EditInvoice(invoice);
        }

        public override void OnNew()
        {
            MainMenuViewModel.EditCustomer(new Customer());
        }

        public override void OnEdit()
        {
            MainMenuViewModel.EditCustomer(new Customer(SelectedItem.Model));
        }

        public override void OnDelete()
        {
            if (Views.MainWindow.ConfirmMessage("Haluatko varmasti poistaa asiakkaan " + SelectedItem.Name + " ?", "Varoitus", System.Windows.MessageBoxImage.Question))
            {
                SelectedItem.Model.Delete();
                SelectedItem = null;
            }
        }
    }
}
