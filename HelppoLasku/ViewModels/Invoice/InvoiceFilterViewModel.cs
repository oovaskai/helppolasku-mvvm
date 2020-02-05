using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public class InvoiceFilterViewModel : FilterViewModel
    {
        public InvoiceFilterViewModel()
        {
        }

        public override bool Filter(object item)
        {
            InvoiceViewModel invoice = item as InvoiceViewModel;

            if (StartDate == null || StartDate <= invoice.Date)
                if (EndDate == null || EndDate >= invoice.Date)
                    if (string.IsNullOrEmpty(Customer) || invoice.Customer.Name.IndexOf(Customer, StringComparison.OrdinalIgnoreCase) >= 0)
                        if (All || (invoice.Paid == null && Open) || (invoice.Paid == true && Paid) || (invoice.Paid == false && NotPaid))
                            return true;

            return false;
        }

        public override void OnClear()
        {
            StartDate = null;
            EndDate = null;
            Customer = null;
            All = true;
        }

        #region Properties

        DateTime? startDate;

        public DateTime? StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }

        DateTime? endDate;

        public DateTime? EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    RaisePropertyChanged("EndDate");
                }
            }
        }

        string customer;

        public string Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    RaisePropertyChanged("Customer");
                }
            }
        }

        bool paid;

        public bool Paid
        {
            get { return paid; }
            set
            {
                if (paid != value)
                {
                    paid = value;
                    RaisePropertyChanged("Paid");
                    RaisePropertyChanged("All");
                }
            }
        }

        bool notPaid;

        public bool NotPaid
        {
            get { return notPaid; }
            set
            {
                if (notPaid != value)
                {
                    notPaid = value;
                    RaisePropertyChanged("NotPaid");
                    RaisePropertyChanged("All");
                }
            }
        }

        bool open;

        public bool Open
        {
            get => open;
            set
            {
                if (open != value)
                {
                    open = value;
                    RaisePropertyChanged("Open");
                    RaisePropertyChanged("All");
                }
            }
        }

        public bool All
        {
            get => Paid && NotPaid && Open;
            set
            {
                Paid = value;
                NotPaid = value;
                Open = value;
            }
        }

        #endregion
    }
}
