using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class InvoiceTitleViewModel : DataViewModel
    {
        public InvoiceTitleViewModel(InvoiceTitle title) : base(title)
        {
            LoadItems(title);
        }

        public new InvoiceTitle Model
        {
            get => base.Model as InvoiceTitle;
            set => base.Model = value;
        }

        #region Properties

        public ObservableCollection<InvoiceItemViewModel> Items { get; set; }

        public InvoiceViewModel Invoice { get; set; }

        public int Index
        {
            get => Model.Index;
            set
            {
                if (Model.Index != value)
                {
                    Model.Index = value;
                    RaisePropertyChanged("Index");
                }
            }
        }

        public string Header
        {
            get => Model.Header;
            set
            {
                if (Model.Header != value)
                {
                    Model.Header = value;
                    RaisePropertyChanged("Header");
                }
            }
        }

        public double Total => GetTotal();

        public double Taxed => GetTotal() - GetTaxless();

        public double Taxless => GetTaxless();

        #endregion

        #region Methods

        protected virtual void LoadItems(InvoiceTitle title)
        {
            if (title.Items != null)
            {
                Items = new ObservableCollection<InvoiceItemViewModel>();
                foreach (InvoiceItem item in title.Items)
                {
                    InvoiceItemViewModel viewmodel = new InvoiceItemViewModel(item);
                    viewmodel.Title = this;
                    Items.Add(viewmodel);
                }
            } 
        }

        protected virtual double GetTotal()
        {
            double total = 0;
            foreach (InvoiceItemViewModel item in Items)
                total += item.Total;
            return total;
        }

        protected virtual double GetTaxless()
        {
            double taxless = 0;
            foreach (InvoiceItemViewModel item in Items)
                taxless += item.Taxless;
            return taxless;
        }

        #endregion
    }
}
