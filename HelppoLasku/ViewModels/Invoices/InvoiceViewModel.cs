using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class InvoiceViewModel : DataViewModel
    {
        public InvoiceViewModel(Invoice invoice) : base(invoice)
        {
            if (invoice.Customer != null)
                Customer = new CustomerViewModel(invoice.Customer);

            LoadTitles(invoice);
        }

        public new Invoice Model
        {
            get => base.Model as Invoice;
            set => base.Model = value;
        }

        #region Properties

        public ObservableCollection<InvoiceTitleViewModel> Titles { get; set; }

        CustomerViewModel customer;

        public virtual CustomerViewModel Customer
        {
            get => customer;
            set
            {
                if (customer != value)
                {
                    customer = value;
                    Model.Customer = value.Model;
                    RaisePropertyChanged("Customer");
                }
            }
        }

        public int? InvoiceID
        {
            get => Model.InvoiceID;
            set
            {
                if (Model.InvoiceID != value)
                {
                    Model.InvoiceID = value;
                    RaisePropertyChanged("InvoiceID");
                }
            }
        }

        public DateTime? Date
        {
            get => Model.Date;
            set
            {
                if (Model.Date != value)
                {
                    Model.Date = value;
                    RaisePropertyChanged("Date");
                }
            }
        }

        public bool? Paid
        {
            get => Model.Paid;
            set
            {
                if (Model.Paid != value)
                {
                    Model.Paid = value;

                    if (value == false)
                        PayDate = null;
                    RaisePropertyChanged("Paid");
                    RaisePropertyChanged("Status");
                    RaisePropertyChanged("StatusDate");
                    RaisePropertyChanged("IsLate");
                }
            }
        }

        public DateTime? DueDate
        {
            get => Model.DueDate;
            set
            {
                if (Model.DueDate != value)
                {
                    Model.DueDate = value;
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        public DateTime? PayDate
        {
            get => Model.PayDate;
            set
            {
                if (Model.PayDate != value)
                {
                    Model.PayDate = value;
                    RaisePropertyChanged("PayDate");
                }
            }
        }

        public string Reference
        {
            get => Model.Reference;
            set
            {
                if (Model.Reference != value)
                {
                    Model.Reference = value;
                    RaisePropertyChanged("Reference");
                }
            }
        }

        public double? Interest
        {
            get => Model.Interest;
            set
            {
                if (Model.Interest != value)
                {
                    Model.Interest = value;
                    RaisePropertyChanged("Interest");
                }
            }
        }

        public string Status
        {
            get
            {
                if (Paid == null)
                    return "Avoin";
                if (Paid == true)
                    return "Maksettu";
                if (IsLate)
                    return "Erääntynyt";
                return "Laskutettu";
            }
        }

        public DateTime? StatusDate
        {
            get
            {
                if (Paid == false)
                    return IsLate ? DueDate : Date;
                
                if (Paid == true)
                    return PayDate;

                return null;
            }
        }

        public bool IsLate => Paid == false && DueDate != null && ((DateTime)DueDate).AddDays(1) < DateTime.Now;

        public double Taxless => GetTaxless();

        public double Taxed => GetTotal() - GetTaxless();

        public double Total => GetTotal();

        public string File => Model.File?.Remove(0, Model.File.LastIndexOf(@"\"));

        #endregion

        #region Methods

        protected virtual void LoadTitles(Invoice invoice)
        {
            if (invoice.Titles != null)
            {
                Titles = new ObservableCollection<InvoiceTitleViewModel>();

                foreach (InvoiceTitle title in invoice.Titles)
                {
                    InvoiceTitleViewModel viewmodel = new InvoiceTitleViewModel(title);

                    viewmodel.Invoice = this;
                    Titles.Add(viewmodel);
                }
            }    
        }

        protected double GetTotal()
        {
            double total = 0;
            foreach (InvoiceTitleViewModel title in Titles)
                total += title.Total;
            return total;

        }

        protected virtual double GetTaxless()
        {
            double taxless = 0;
            foreach (InvoiceTitleViewModel title in Titles)
                taxless += title.Taxless;
            return taxless;

        }

        #endregion
    }
}
