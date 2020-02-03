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

            MainMenuViewModel.SelectedCompany.ModelChanged += OnModelChanged;

            if (Model.IsNew)
            {
                DisplayName = "Uusi yritys";

                Model.InvoiceID = Properties.Settings.Default.DefaultInvoiceID;
                Model.ReferenceBase = Properties.Settings.Default.DefaultReference;

                Model.Tax = Properties.Settings.Default.DefaultTax;

                Model.CompanyExpire = Properties.Settings.Default.MinCompanyExpire;
                Model.CompanyInterest = Properties.Settings.Default.MaxInterest;
                Model.CompanyAnnotation = Properties.Settings.Default.DefaultAnnotation;

                Model.PersonExpire = Properties.Settings.Default.MinPersonExpire;
                Model.PersonInterest = Properties.Settings.Default.MaxInterest;
                Model.PersonAnnotation = Properties.Settings.Default.DefaultAnnotation;
            }
            else
            {
                DisplayName = Name;
            }

            Tax = Model.Tax.ToString();

            CompanyExpire = Model.CompanyExpire.ToString();
            CompanyInterest = Model.CompanyInterest.ToString("0.0");
            CompanyAnnotation = Model.CompanyAnnotation.ToString();

            PersonExpire = Model.PersonExpire.ToString();
            PersonInterest = Model.PersonInterest.ToString("0.0");
            PersonAnnotation = Model.PersonAnnotation.ToString();

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

        public ImageManagerViewModel LogoManager { get; set; }

        #region Invoicing

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

        public string ReferenceBase
        {
            get => Model.ReferenceBase;
            set
            {
                if (Model.ReferenceBase != value)
                {
                    Model.ReferenceBase = value;
                    RaisePropertyChanged("ReferenceBase");
                    RaisePropertyChanged("ReferenceCheck");
                }
            }
        }

        public int ReferenceCheck => Model.ReferenceCheck(Model.ReferenceBase);

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

        #region Customers

        string companyAnnotation;

        public string CompanyAnnotation
        {
            get => companyAnnotation;
            set
            {
                if (companyAnnotation != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.CompanyAnnotation = i;

                    companyAnnotation = value;
                    RaisePropertyChanged("CompanyAnnotation");
                }
            }
        }

        string companyExpire;

        public string CompanyExpire
        {
            get => companyExpire;
            set
            {
                if (companyExpire != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.CompanyExpire = i;

                    companyExpire = value;
                    RaisePropertyChanged("CompanyExpire");
                }
            }
        }

        string companyInterest;

        public string CompanyInterest
        {
            get => companyInterest;
            set
            {
                if (companyInterest != value)
                {
                    if (double.TryParse(value, out double d))
                        Model.CompanyInterest = d;

                    companyInterest = value;
                    RaisePropertyChanged("CompanyInterest");
                }
            }
        }

        string personAnnotation;

        public string PersonAnnotation
        {
            get => personAnnotation;
            set
            {
                if (personAnnotation != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.PersonAnnotation = i;

                    personAnnotation = value;
                    RaisePropertyChanged("PersonAnnotation");
                }
            }
        }

        string personExpire;

        public string PersonExpire
        {
            get => personExpire;
            set
            {
                if (personExpire != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.PersonExpire = i;

                    personExpire = value;
                    RaisePropertyChanged("PersonExpire");
                }
            }
        }

        string personInterest;

        public string PersonInterest
        {
            get => personInterest;
            set
            {
                if (personInterest != value)
                {
                    if (double.TryParse(value, out double d))
                        Model.PersonInterest = d;

                    personInterest = value;
                    RaisePropertyChanged("PersonInterest");
                }
            }
        }

        #endregion

        #region Products

        public string[] TaxRates => Properties.Settings.Default.TaxRates.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);

        public string Tax
        {
            get { return Model.Tax < 0 ? (Model.Tax * -1).ToString() : Model.Tax.ToString(); }
            set
            {
                if (Model.Tax != double.Parse(value))
                {
                    Model.Tax = IsTaxed ? double.Parse(value) * -1 : double.Parse(value);
                    RaisePropertyChanged("Tax");
                    RaisePropertyChanged("IsTaxed");
                }
            }
        }

        public bool IsTaxed
        {
            get => Model.Tax < 0;
            set
            {
                if (IsTaxed != value)
                {
                    Model.Tax *= -1;
                    RaisePropertyChanged("IsTaxed");
                }
            }
        }

        #endregion

        #region Base Overrides

        public override void OnModelChanged(object sender, ModelChangedEventArgs e)
        {
            if (e.Type == ModelChangedEventArgs.EventType.Update && sender == MainMenuViewModel.SelectedCompany)
            {
                ReferenceBase = (sender as Company).ReferenceBase;
                Model.InvoiceID = (sender as Company).InvoiceID;
                RaisePropertyChanged("InvoiceID");
            }
            base.OnModelChanged(sender, e);
        }

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

        protected override void OnDispose()
        {
            MainMenuViewModel.SelectedCompany.ModelChanged -= OnModelChanged;
            base.OnDispose();
        }

        #endregion
    }
}
