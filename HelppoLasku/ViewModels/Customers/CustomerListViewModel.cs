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
        }

        public override DataViewModel NewItem(DataModel model)
            => new CustomerViewModel(model as Customer);
        
        public new CustomerViewModel SelectedItem
        {
            get => base.SelectedItem as CustomerViewModel;
            set => base.SelectedItem = value;
        }

        public override void OnNew()
        {
            Views.MainWindow.EditDialog(new EditCustomerViewModel(new Customer()), 600, 500);
        }

        public override void OnEdit()
        {
            Customer customer = new Customer(SelectedItem.Model);
            Views.MainWindow.EditDialog(new EditCustomerViewModel(customer), 600, 500);
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
