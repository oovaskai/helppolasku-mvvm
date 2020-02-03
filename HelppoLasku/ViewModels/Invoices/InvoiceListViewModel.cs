using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class InvoiceListViewModel : ListViewModel
    {
        public InvoiceListViewModel(ObservableCollection<DataModel> source) : base(source)
        {
        }

        public override DataViewModel NewItem(DataModel model)
            => new InvoiceViewModel(model as Invoice);

        public new InvoiceViewModel SelectedItem
        {
            get => base.SelectedItem as InvoiceViewModel;
            set => base.SelectedItem = value;
        }

        public double Total
        {
            get
            {
                double total = 0;
                foreach (InvoiceViewModel invoice in View)
                    total += invoice.Total;
                return total;
            }
        }

        public double Taxed
        {
            get
            {
                double total = 0;
                foreach (InvoiceViewModel invoice in View)
                    total += invoice.Taxed;
                return total;
            }
        }

        public double Taxless
        {
            get
            {
                double total = 0;
                foreach (InvoiceViewModel invoice in View)
                    total += invoice.Taxless;
                return total;
            }
        }

        public override void OnFiltersChanged(object sender, FilterChangedEventArgs e)
        {
            RaisePropertyChanged("Total");
            RaisePropertyChanged("Taxed");
            RaisePropertyChanged("Taxless");
            base.OnFiltersChanged(sender, e);
        }

        public override void OnNew()
        {
            Invoice invoice = new Invoice();
            InvoiceTitle title = new InvoiceTitle();
            invoice.Titles.Add(title);
            title.Items.Add(new InvoiceItem { Tax = MainMenuViewModel.SelectedCompany.Tax });
            MainWindowViewModel.WorkspaceControl.New(new EditInvoiceViewModel(invoice), false);
        }

        public override void OnEdit()
        {
            MainMenuViewModel.EditInvoice(new Invoice(SelectedItem.Model));
        }

        public override void OnCopy()
        {
            Invoice copy = new Invoice();
            SelectedItem.Model.CopyTo(copy);
            copy.Date = null;
            copy.DueDate = null;
            copy.PayDate = null;
            copy.Paid = null;
            copy.InvoiceID = null;
            copy.Reference = null;
            copy.AnnotationTime = null;
            copy.Interest = null;

            MainMenuViewModel.EditInvoice(copy);
        }

        public override bool CanDelete()
        {
            if (!base.CanDelete())
                return false;

            return SelectedItem.Paid == null;
        }
    }
}
