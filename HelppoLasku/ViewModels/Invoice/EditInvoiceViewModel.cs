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

            if (Model.IsNew)
            {
                DisplayName = "Uusi lasku";
                if (Model.Titles.Count < 1)
                    NewTitle.Execute();
            }
            else
                DisplayName = "Lasku " + (InvoiceID == null ? Status : InvoiceID.ToString()) + (Customer == null ? null : " - " + Customer.Name);

            if (Paid != null)
                IsEnabled = false;

            CustomerList = new CustomerListViewModel(Resources.GetModels<Customer>());
            if (Model.Customer != null)
                CustomerList.SelectedItem = CustomerList.FindByID(Model.Customer.ID) as CustomerViewModel;

            CustomerList.SelectionChanged += CustomerListSelectionChanged;

            Send = new CommandViewModel("Laskuta", OnSend, CanSend);
            Pay = new CommandViewModel("Merkitse maksupäivä", OnPay, CanPay);
            SavePay = new CommandViewModel("Aseta päiväys", OnSavePay);
            CancelPay = new CommandViewModel("Peruuta", OnCancelPay);
        }

        public ProductFilterViewModel ProductFilter { get; set; }

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

        public CommandViewModel NewTitle => new CommandViewModel("Lisää otsikko", OnNewTitle);

        void OnNewTitle()
        {
            InvoiceTitle title = new InvoiceTitle();
            title.Invoice = Model;
            Model.Titles.Add(title);

            EditInvoiceTitleViewModel viewModel = new EditInvoiceTitleViewModel(title);
            viewModel.Invoice = this;
            viewModel.NewItem.Execute();
            Titles.Add(viewModel);
        }

        public override void OnSave()
        {
            if (statusChanged && !Views.MainWindow.ConfirmMessage("Lasku merkitään lähetetyksi eikä sitä voi enää tallentamisen jälkeen muokata.\n\n" +
                "Haluatko varmasti tallentaa?", "Huomio", System.Windows.MessageBoxImage.Question))
                return;

            if (statusChanged)
            {
                MainMenuViewModel.SelectedCompany.NewInvoice();
                statusChanged = false;
            }  
            base.OnSave();
        }

        public override bool CanSave()
        {
            if (Paying)
                return false;

            return base.CanSave();
        }

        public override bool? Paid
        {
            get => base.Paid;
            set
            {
                if (base.Paid != value)
                {
                    if (base.Paid == null && value == false)
                        statusChanged = true;
                    base.Paid = value;
                }
            }
        }

        #region SendCommand

        public CommandViewModel Send { get; private set; }

        bool statusChanged;

        void OnSend()
        {
            Views.MainWindow.EditDialog(new SendInvoiceViewModel(this), 300, 250);
        }

        bool CanSend()
            => Paid == null && Error == null;

        #endregion

        #region PayCommand

        bool paying;

        public bool Paying
        {
            get => paying;
            set
            {
                if (paying != value)
                {
                    paying = value;
                    RaisePropertyChanged("Paying");
                }
            }
        }

        public CommandViewModel Pay { get; private set; }

        void OnPay()
        {
            PayDate = DateTime.Now;
            Paying = true;
            RaisePropertyChanged("PayDate");
        }

        bool CanPay()
        {
            return Paid != null && !Paying;
        }

        public CommandViewModel SavePay { get; private set; }

        void OnSavePay()
        {
            Paid = true;
            Paying = false;
        }

        public CommandViewModel CancelPay { get; private set; }

        void OnCancelPay()
        {
            Paid = false;
            Paying = false;
        }

        #endregion
    }
}
