using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public class SendInvoiceViewModel : DataViewModel
    {
        public SendInvoiceViewModel(EditInvoiceViewModel invoice) : base(invoice.Model)
        {
            EditEnabled = true;
            Validator = new Validation.SendInvoiceValidator(this);

            DisplayName = "Laskuta";
            Invoice = invoice;

            Date = DateTime.Now;
            ExpireDays = Invoice.Customer.IsCompany ? MainMenuViewModel.SelectedCompany.CompanyExpire.ToString() : MainMenuViewModel.SelectedCompany.PersonExpire.ToString();
            AnnotationTime = Invoice.Customer.IsCompany ? MainMenuViewModel.SelectedCompany.CompanyAnnotation.ToString() : MainMenuViewModel.SelectedCompany.PersonAnnotation.ToString();
            Interest = Invoice.Customer.IsCompany ? MainMenuViewModel.SelectedCompany.CompanyInterest.ToString() : MainMenuViewModel.SelectedCompany.PersonInterest.ToString();
        }

        public EditInvoiceViewModel Invoice { get; private set; }

        public int InvoiceID => MainMenuViewModel.SelectedCompany.InvoiceID;

        public string Reference => MainMenuViewModel.SelectedCompany.Reference;

        DateTime date;

        public DateTime Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;

                    if (date != null && ExpireDays != null)
                        DueDate = date.AddDays(int.Parse(ExpireDays));

                    RaisePropertyChanged("Date");
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        string expireDays;

        public string ExpireDays
        {
            get => expireDays;
            set
            {
                if (expireDays != value)
                {
                    if (int.TryParse(value, out int days) && Date != null)
                        DueDate = Date.AddDays(days);

                    expireDays = value;
                    RaisePropertyChanged("ExpireDays");
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        DateTime dueDate;

        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                if (dueDate != value)
                {
                    dueDate = value;

                    if (DueDate != null && Date != null)
                        ExpireDays =  (DueDate - Date).TotalDays.ToString();

                    RaisePropertyChanged("DueDate");
                    RaisePropertyChanged("ExpireDays");
                }
            }
        }

        public string AnnotationTime { get; set; }

        public string Interest { get; set; }

        public override void OnSave()
        {
            Invoice.InvoiceID = InvoiceID;
            Invoice.Reference = Reference;
            Invoice.Interest = double.Parse(Interest);
            Invoice.AnnotationTime = int.Parse(AnnotationTime);
            Invoice.Date = Date;
            Invoice.DueDate = DueDate;
            Invoice.Paid = false;
            Invoice.IsEnabled = false;

            EditEnabled = false;
        }

    }
}
