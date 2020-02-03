using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public static Company SelectedCompany { get; private set; }

        public MainMenuViewModel()
        {
            CompanyMenu = new CompanyMenuViewModel(Resources.GetModels<Company>());
            CompanyMenu.SelectionChanged += OnSelectedCompanyChanged;

            CompanyMenuItemViewModel company = CompanyMenu.FindByID(Properties.Settings.Default.SelectedCompany) as CompanyMenuItemViewModel;
            if (company != null)
            {
                CompanyMenu.SelectedItem = company;
                Invoices.Command.Execute(null);
            }  

            if (CompanyMenu.Items.Count < 1)
                CompanyMenu.OnNew();
        }

        public CompanyMenuViewModel CompanyMenu { get; private set; }

        public bool CompanySelected => CompanyMenu.SelectedItem != null;

        void OnSelectedCompanyChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.NewSelection == null)
                return;

            SelectedCompany = (e.NewSelection as CompanyMenuItemViewModel).Model;
            Properties.Settings.Default.SelectedCompany = SelectedCompany.ID;
            Properties.Settings.Default.Save();

            //Resources.Initialize();

            RaisePropertyChanged("CompanySelected");
        }

        #region Commands

        public CommandViewModel Close => new CommandViewModel("Lopeta", () => System.Windows.Application.Current.MainWindow.Close());

        public CommandViewModel Products => new CommandViewModel("Selaa tuotteita", () => MainWindowViewModel.WorkspaceControl.New(new AllProductsViewModel(), true));

        public CommandViewModel NewProduct => new CommandViewModel("Uusi tuote", () => EditProduct(new Product()));

        public CommandViewModel Customers => new CommandViewModel("Selaa asiakkaita", () => MainWindowViewModel.WorkspaceControl.New(new CustomerListViewModel(Resources.GetModels<Customer>()), true));

        public CommandViewModel NewCustomer => new CommandViewModel("Uusi asiakas", () => EditCustomer(new Customer()));

        public CommandViewModel Invoices => new CommandViewModel("Selaa laskuja", () => MainWindowViewModel.WorkspaceControl.New(new AllInvoicesViewModel(), true));

        public CommandViewModel NewInvoice => new CommandViewModel("Uusi lasku", () => EditInvoice(new Invoice()));

        public static void EditProduct(Product product)
            => Views.MainWindow.EditDialog(new EditProductViewModel(product), 500, 350);

        public static void EditCustomer(Customer customer)
            => Views.MainWindow.EditDialog(new EditCustomerViewModel(customer), 600, 500);

        public static void EditInvoice(Invoice invoice)
            => MainWindowViewModel.WorkspaceControl.New(new EditInvoiceViewModel(invoice), !invoice.IsNew);

        #endregion

        protected override void OnDispose()
        {
            CompanyMenu.SelectionChanged -= OnSelectedCompanyChanged;
            base.OnDispose();
        }
    }
}
