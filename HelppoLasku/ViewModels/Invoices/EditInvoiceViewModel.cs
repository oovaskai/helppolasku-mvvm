using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class EditInvoiceViewModel : InvoiceViewModel
    {
        public EditInvoiceViewModel(Invoice invoice) : base(invoice)
        {
            EditEnabled = true;
            Validator = new Validation.InvoiceValidator(this);

            DisplayName = Model.IsNew ? "Uusi lasku" : "Muokkaa laskua " + InvoiceID;

            if (Paid != null)
                IsEnabled = false;

            CustomerList = new CustomerListViewModel(Resources.GetModels<Customer>());
            if (Model.Customer != null)
                CustomerList.SelectedItem = CustomerList.FindByID(Model.Customer.ID) as CustomerViewModel;

            CustomerList.SelectionChanged += CustomerListSelectionChanged;

            StatusCommands = new InvoiceCommandsViewModel(this);  
        }

        public ProductFilterViewModel ProductFilter { get; set; }

        public int Count
        {
            get
            {
                int count = 0;
                foreach (var title in Titles)
                    count += title.Items.Count;
                
                return count;
            }
        }

        protected override void LoadTitles(Invoice invoice)
        {
            List<EditInvoiceTitleViewModel> titles = new List<EditInvoiceTitleViewModel>();

            if (invoice.Titles != null)
            {
                foreach (InvoiceTitle title in invoice.Titles)
                {
                    EditInvoiceTitleViewModel viewmodel = new EditInvoiceTitleViewModel(title);
                    viewmodel.Invoice = this;
                    titles.Add(viewmodel);
                }
            }
            Titles = new ObservableCollection<InvoiceTitleViewModel>(titles);
        }

        public CustomerListViewModel CustomerList { get; private set; }

        void CustomerListSelectionChanged(object sender, SelectionChangedEventArgs e)
            => Customer = e.NewSelection as CustomerViewModel;

        protected override void OnDispose()
        {
            if (CustomerList != null)
                CustomerList.SelectionChanged -= CustomerListSelectionChanged;
            base.OnDispose();
        }

        public void OnItemsChanged()
        {
            RaisePropertyChanged("Count");
            RaisePropertyChanged("Taxless");
            RaisePropertyChanged("Taxed");
            RaisePropertyChanged("Total");
        }

        public CommandViewModel NewTitle => new CommandViewModel("Lisää otsikko", OnNewTitle);

        void OnNewTitle()
        {
            InvoiceTitle title = new InvoiceTitle();
            title.Invoice = Model;
            Model.Titles.Add(title);

            EditInvoiceTitleViewModel viewModel = new EditInvoiceTitleViewModel(title);
            viewModel.Invoice = this;
            Titles.Add(viewModel);
        }

        public InvoiceCommandsViewModel StatusCommands { get; private set; }

        public override void OnSave()
        {
            if (Model.IsNew || Model.InvoiceID > MainMenuViewModel.SelectedCompany.InvoiceID)
                MainMenuViewModel.SelectedCompany.InvoiceID++;

            if (!string.IsNullOrEmpty(Model.Reference))
                MainMenuViewModel.SelectedCompany.ReferenceNumber = Model.Reference.Remove(Model.Reference.Length -1);

            base.OnSave();
        }

        public override bool CanSave()
        {
            if (StatusCommands.Sending || StatusCommands.Paying)
                return false;

            return base.CanSave();
        }

        public override string Error
        {
            get
            {
                foreach (EditInvoiceTitleViewModel title in Titles)
                {
                    string error = title.Error;
                    if (error != null)
                        return error;
                }
                return base.Error;
            }
        }
    }
}
