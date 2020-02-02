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
    public class EditInvoiceTitleViewModel : InvoiceTitleViewModel
    {
        public EditInvoiceTitleViewModel(InvoiceTitle title) : base(title)
        {
        }

        public new EditInvoiceViewModel Invoice
        {
            get => base.Invoice as EditInvoiceViewModel;
            set => base.Invoice = value;
        }

        protected override void LoadItems(InvoiceTitle title)
        {
            List<EditInvoiceItemViewModel> items = new List<EditInvoiceItemViewModel>();

            if (title.Items != null)
            {
                foreach (InvoiceItem item in title.Items)
                {
                    EditInvoiceItemViewModel viewmodel = new EditInvoiceItemViewModel(item);
                    viewmodel.Title = this;
                    items.Add(viewmodel);
                }
            }

            Items = new ObservableCollection<InvoiceItemViewModel>(items);
        }

        public CommandViewModel DeleteTitle => new CommandViewModel("Poista otsikko", OnDeleteTitle);

        void OnDeleteTitle()
        {
            Invoice.Model.Titles.Remove(Model);
            Invoice.Titles.Remove(this);
            Invoice.OnItemsChanged();
        }

        public CommandViewModel NewItem => new CommandViewModel("Lisää nimike", OnNewItem);

        void OnNewItem()
        {
            InvoiceItem item = new InvoiceItem { Tax = Properties.Settings.Default.DefaultTax };
            item.Title = Model;
            Model.Items.Add(item);


            EditInvoiceItemViewModel viewmodel = new EditInvoiceItemViewModel(item);
            viewmodel.Title = this;
            Items.Add(viewmodel);
            Invoice.OnItemsChanged();
        }

        public override string Error
        {
            get
            {
                foreach (EditInvoiceItemViewModel item in Items)
                {
                    string error = item.Error;
                    if (error != null)
                        return error;
                }
                return base.Error;
            }
        }
    }
}
