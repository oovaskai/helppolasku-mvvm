using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class EditCompanyViewModel : ContactViewModel
    {
        public EditCompanyViewModel(Company company) : base(company)
        {
            EditEnabled = true;
            Validator = new Validation.CompanyValidator(this);

            if (Model.IsNew)
            {
                DisplayName = "Uusi yritys";
                InvoiceID = 1;
                Model.ReferenceNumber = "0001";
                Model.DefaultTax = Properties.Settings.Default.DefaultTax;
                Model.DefaultExpire = Properties.Settings.Default.DefaultExpire;
                Model.DefaultInterest = Properties.Settings.Default.MaxInterest;
            }
            else
            {
                DisplayName = Name;
            }

            DefaultTax = Model.DefaultTax.ToString();
            DefaultExpire = Model.DefaultExpire.ToString();
            DefaultInterest = Model.DefaultInterest.ToString("0.0");

            LogoManager = new ImageManagerViewModel(System.IO.Directory.GetCurrentDirectory() + @"\userdata\" + company.ID, Model.Logo);
            LogoManager.FileTypes = new List<string> { ".png", ".jpg", ".jpeg" };

            EditID = new CommandViewModel("Muokkaa", OnEditID, CanEditID);
            SaveID = new CommandViewModel("Aseta numero", OnSaveID);
            CancelID = new CommandViewModel("Peruuta", OnCancelID);

            SaveID.Visibility = Visibility.Collapsed;
            CancelID.Visibility = Visibility.Collapsed;
            ReadOnlyID = !Model.IsNew;
        }

        public new Company Model
        {
            get => base.Model as Company;
            set => base.Model = value;
        }

        #region Properties

        public ImageManagerViewModel LogoManager { get; set; }

        public string BIC
        {
            get => Model.BIC;
            set
            {
                if (Model.BIC != value)
                {
                    Model.BIC = value;
                    RaisePropertyChanged("BIC");
                }
            }
        }

        public string IBAN
        {
            get => Model.IBAN;
            set
            {
                if (Model.IBAN != value)
                {
                    Model.IBAN = value;
                    RaisePropertyChanged("IBAN");
                }
            }
        }

        string defaultExpire;

        public string DefaultExpire
        {
            get => defaultExpire;
            set
            {
                if (defaultExpire != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.DefaultExpire = i;

                    defaultExpire = value;
                    RaisePropertyChanged("DefaultExpire");
                }
            }
        }

        string defaultInterest;

        public string DefaultInterest
        {
            get => defaultInterest;
            set
            {
                if (defaultInterest != value)
                {
                    if (double.TryParse(value, out double d))
                        Model.DefaultInterest = d;

                    defaultInterest = value;
                    RaisePropertyChanged("DefaultInterest");
                }
            }
        }

        int invoiceID;

        public int InvoiceID
        {
            get => Model.InvoiceID;
            set
            {
                if (invoiceID != value)
                {
                    if (Model.IsNew)
                        Model.InvoiceID = value;

                    invoiceID = value;

                    RaisePropertyChanged("InvoiceID");
                }
            }
        }

        public string ReferenceNumber
        {
            get => Model.ReferenceNumber;
            set
            {
                if (Model.ReferenceNumber != value)
                {
                    Model.ReferenceNumber = value;
                    RaisePropertyChanged("ReferenceNumber");
                    RaisePropertyChanged("ReferenceCheckNumber");
                }
            }
        }

        public int ReferenceCheckNumber => Model.ReferenceCheck(Model.ReferenceNumber);

        public string DefaultTax
        {
            get => Model.DefaultTax.ToString();
            set
            {
                if (Model.DefaultTax.ToString() != value)
                {
                    Model.DefaultTax = double.Parse(value);
                    RaisePropertyChanged("DefaultTax");
                }
            }
        }

        public string[] TaxRates => Properties.Settings.Default.TaxRates.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);

        #endregion

        #region Edit InvoiceID

        public CommandViewModel EditID { get; private set; }

        void OnEditID()
        {
            if (Views.MainWindow.ConfirmMessage("Kirjanpitolain mukaan laskunumeroinnin pitää olla juokseva ja helposti tunnistettavissa.\n" +
                "Jos kuitenkin haluat muuttaa laskunumerointia, tee se ainoastaan tilikauden vaihtuessa.\n\n" +
                "Haluatko jatkaa ?",
                "Varoitus", MessageBoxImage.Warning))
            {
                ReadOnlyID = false;
                RaisePropertyChanged("ReadOnlyID");
                EditID.Visibility = Visibility.Collapsed;
                SaveID.Visibility = Visibility.Visible;
                CancelID.Visibility = Visibility.Visible;
                IsEnabled = false;
            }
        }

        bool CanEditID()
        {
           return Model.IsNew ? false : true;
        }

        public CommandViewModel SaveID { get; private set; }

        void OnSaveID()
        {
            
            Model.InvoiceID = invoiceID;
            RaisePropertyChanged("InvoiceID");

            ReadOnlyID = true;
            RaisePropertyChanged("ReadOnlyID");

            EditID.Visibility = Visibility.Visible;
            SaveID.Visibility = Visibility.Collapsed;
            CancelID.Visibility = Visibility.Collapsed;
            IsEnabled = true;

        }

        public CommandViewModel CancelID { get; private set; }

        void OnCancelID()
        {
            RaisePropertyChanged("InvoiceID");

            ReadOnlyID = true;
            RaisePropertyChanged("ReadOnlyID");

            EditID.Visibility = Visibility.Visible;
            SaveID.Visibility = Visibility.Collapsed;
            CancelID.Visibility = Visibility.Collapsed;

            IsEnabled = true;
        }

        public bool ReadOnlyID { get; private set; }

        #endregion


        public override void OnSave()
        {
            //Model.Logo = LogoManager.Update();
            base.OnSave();
        }

        public override bool CanSave()
        {
            if (!IsEnabled)
                return false;

            return base.CanSave();
        }
    }
}
