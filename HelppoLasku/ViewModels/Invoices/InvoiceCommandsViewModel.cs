using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelppoLasku.ViewModels
{
    public class InvoiceCommandsViewModel : ViewModelBase
    {
        public InvoiceCommandsViewModel(EditInvoiceViewModel invoice)
        {
            Invoice = invoice;
            Send = new CommandViewModel("Laskuta", OnSend, CanSend);
            Pay = new CommandViewModel("Maksettu", OnPay, CanPay);
            Save = new CommandViewModel("Aseta tiedot", OnSave, CanSave);
            Cancel = new CommandViewModel("Peruuta", OnCancel);
        }

        public EditInvoiceViewModel Invoice { get; private set; }

        #region SendCommand

        public bool Sending { get; private set; }

        DateTime sendDate;

        public DateTime SendDate
        {
            get => sendDate;
            set
            {
                if (sendDate != value)
                {
                    sendDate = value;

                    if (sendDate != null)
                        DueDate = sendDate.AddDays(ExpireDays);

                    RaisePropertyChanged("SendDate");
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        int expireDays;

        public int ExpireDays
        {
            get => expireDays;
            set
            {
                if (expireDays != value)
                {
                    expireDays = value;
                    if (SendDate != null)
                        DueDate = SendDate.AddDays(value);

                    RaisePropertyChanged("ExpireDays");
                    RaisePropertyChanged("DueDate");
                }
            }
        }

        public DateTime DueDate { get; set; }

        public CommandViewModel Send { get; private set; }

        void OnSend()
        {
            Sending = true;
            SendDate = DateTime.Now;
            ExpireDays = MainMenuViewModel.SelectedCompany.DefaultExpire;
            Invoice.IsEnabled = false;
            RaisePropertyChanged("SendDate");
            RaisePropertyChanged("SendDateVisibility");
        }

        bool CanSend()
            => Invoice.Paid == null && Invoice.Error == null && !Sending;

        public Visibility SendVisibility => Invoice.Paid == null ? Visibility.Visible : Visibility.Hidden;

        public Visibility SendDateVisibility => Sending ? Visibility.Visible : Visibility.Collapsed;


        #endregion

        #region PayCommand

        public bool Paying { get; private set; }

        public DateTime PayDate { get; set; }

        public CommandViewModel Pay { get; private set; }

        void OnPay()
        {
            PayDate = DateTime.Now;
            Paying = true;
            RaisePropertyChanged("PayDate");
            RaisePropertyChanged("PayDateVisibility");
        }

        bool CanPay()
        {
            return Invoice.Paid == false && !Paying;
        }

        public Visibility PayVisibility => Invoice.Paid == false ? Visibility.Visible : Visibility.Hidden;

        public Visibility PayDateVisibility => Paying ? Visibility.Visible : Visibility.Collapsed;

        #endregion

        #region SaveCommand

        public CommandViewModel Save { get; private set; }

        void OnSave()
        {
            if (Invoice.Date == null)
            {
                Invoice.InvoiceID = MainMenuViewModel.SelectedCompany.InvoiceID;
                Invoice.Interest = MainMenuViewModel.SelectedCompany.DefaultInterest;
                Invoice.Date = SendDate;
                Invoice.DueDate = DueDate;
                Invoice.Paid = false;
                Sending = false;

                RaisePropertyChanged("SendVisibility");
                RaisePropertyChanged("SendDateVisibility");
            }

            else if (Invoice.PayDate == null)
            {
                Invoice.PayDate = PayDate;
                Invoice.Paid = true;
                Paying = false;

                RaisePropertyChanged("PayVisibility");
                RaisePropertyChanged("PayDateVisibility");
            }
        }

        bool CanSave()
        {
            if (SendDate != null && DueDate != null && DueDate >= SendDate)
                return true;
            return false;
        }

        #endregion

        #region CancelCommand

        public CommandViewModel Cancel { get; private set; }

        void OnCancel()
        {
            Sending = false;
            Paying = false;
            RaisePropertyChanged("SendDateVisibility");
            RaisePropertyChanged("PayDateVisibility");

            if (Invoice.Paid == null)
                Invoice.IsEnabled = true;
        }

        #endregion
    }
}
