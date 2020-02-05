using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class AllInvoicesViewModel : ViewModelBase
    {
        public AllInvoicesViewModel()
        {
            DisplayName = "Laskut";

            InvoiceList = new InvoiceListViewModel(Resources.GetModels<Invoice>());
            InvoiceList.SelectionChanged += OnSelectedInvoiceChanged;

            InvoiceFilter = new InvoiceFilterViewModel();
            InvoiceList.Filter = InvoiceFilter;
        }

        public InvoiceFilterViewModel InvoiceFilter { get; private set; }

        public InvoiceListViewModel InvoiceList { get; private set; }

        public InvoiceViewModel SelectedInvoice => InvoiceList.SelectedItem;

        void OnSelectedInvoiceChanged(object sender, SelectionChangedEventArgs e)
            => RaisePropertyChanged("SelectedInvoice");

        protected override void OnDispose()
        {
            InvoiceList.SelectionChanged -= OnSelectedInvoiceChanged;
            base.OnDispose();
        }
    }
}
