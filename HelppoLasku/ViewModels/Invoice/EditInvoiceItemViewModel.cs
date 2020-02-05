using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class EditInvoiceItemViewModel : InvoiceItemViewModel
    {
        public EditInvoiceItemViewModel(InvoiceItem item) : base(item)
        {
            Validator = new Validation.InvoiceItemValidator(this);
            ProductList = new ProductListViewModel(Resources.GetModels<Product>());

            Price = Model.Price.ToString("0.00");
            Count = Model.Count.ToString();
        }

        public ProductListViewModel ProductList { get; set; }

        new public EditInvoiceTitleViewModel Title
        {
            get => base.Title as EditInvoiceTitleViewModel;
            set => base.Title = value;
        }

        public bool ItemTemplateOpen { get; set; }

        public ProductViewModel ItemTemplate
        {
            set
            {
                if (value != null)
                {
                    Content = value.Model.Name;
                    Price = value.Model.Price.ToString("0.00");
                    Model.Tax = value.Model.Tax;
                    Unit = value.Unit;
                    RaisePropertyChanged("Content");
                    RaisePropertyChanged("Tax");
                    RaisePropertyChanged("Price");
                    RaisePropertyChanged("Total");
                    RaisePropertyChanged("IsTaxed");
                }
            }
        }

        string count;

        public override string Count
        {
            get => count;
            set
            {
                if (count != value)
                {
                    if (int.TryParse(value, out int i))
                        Model.Count = i;

                    count = value;
                    RaisePropertyChanged("Count");
                    RaisePropertyChanged("Total");
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
                    RaisePropertyChanged("Total");
                }
            }
        }

        public new string Tax
        {
            get { return Model.Tax < 0 ? (Model.Tax * -1).ToString() : Model.Tax.ToString(); }
            set
            {
                Model.Tax = IsTaxed ? double.Parse(value) * -1 : double.Parse(value);
                RaisePropertyChanged("Tax");
                RaisePropertyChanged("Price");
                RaisePropertyChanged("Total");
                RaisePropertyChanged("IsTaxed");
            }
        }

        string price;

        public string Price
        {
            get => price;
            set
            {
                if (price != value)
                {
                    if (double.TryParse(value, out double d))
                        Model.Price = d;

                    price = value;
                    RaisePropertyChanged("Price");
                    RaisePropertyChanged("Total");
                }

            }
        }

        public override double Total => TaxlessPrice * (1 + base.Tax / 100) * Model.Count;

        public string[] TaxRates => Properties.Settings.Default.TaxRates.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);

        public CommandViewModel DeleteItem => new CommandViewModel("Poista nimike", OnDeleteItem);

        void OnDeleteItem()
        {
            Title.Model.Items.Remove(Model);
            Title.Items.Remove(this);
            RaisePropertyChanged("Items");
        }

        protected override void RaisePropertyChanged(string property)
        {
            if (Title != null)
                Title.Invoice.OnItemsChanged();

            base.RaisePropertyChanged(property);
        }
    }
}
