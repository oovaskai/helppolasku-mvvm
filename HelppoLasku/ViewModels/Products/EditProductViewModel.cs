using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class EditProductViewModel : ProductViewModel
    {
        public EditProductViewModel(Product product) : base(product)
        {
            EditEnabled = true;
            Validator = new Validation.ProductValidator(this);

            if (Model.IsNew)
            {
                DisplayName = "Uusi tuote";
                Tax = MainMenuViewModel.SelectedCompany.DefaultTax.ToString();
            }
            else  
            {
                DisplayName = "Muokkaa tuotetta " + Name;
            }

            Price = Model.Price.ToString("0.00");

            GroupList = new ProductGroupListViewModel(Resources.GetModels<ProductGroup>());
                
            if (product.Group != null)
                GroupList.SelectedItem = GroupList.FindByID(product.Group.ID);

            GroupList.SelectionChanged += GroupList_SelectionChanged;
        }

        public bool IsTaxed
        {
            get { return Model.Tax < 0; }
            set
            {
                if (IsTaxed != value)
                {
                    Model.Tax *= -1;

                    RaisePropertyChanged("IsTaxed");
                    RaisePropertyChanged("TotalPrice");
                }
            }
        }

        string price;

        public string Price
        {
            get => price;
            set
            {
                if (double.TryParse(value, out double p))
                    Model.Price = p;

                price = value;
                RaisePropertyChanged("Price");
            }
        }

        public new string Tax
        {
            get { return Model.Tax < 0 ? (Model.Tax * -1).ToString() : Model.Tax.ToString(); }
            set
            {
                if (Model.Tax != double.Parse(value))
                {
                    Model.Tax = IsTaxed ? double.Parse(value) * -1 : double.Parse(value);
                    RaisePropertyChanged("Tax");
                    RaisePropertyChanged("Price");
                    RaisePropertyChanged("TotalPrice");
                }
            }
        }

        public string[] TaxRates => Properties.Settings.Default.TaxRates.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);

        public ProductGroupListViewModel GroupList { get; private set; }

        void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductGroupViewModel viewmodel = e.NewSelection as ProductGroupViewModel;
            if (viewmodel.Name != ProductGroup.All.Name)
                Group = viewmodel;
            else
                Group = null;
        }

        protected override void OnDispose()
        {
            if (GroupList != null)
            {
                GroupList.SelectionChanged -= GroupList_SelectionChanged;
                GroupList.Dispose();
            }
            base.OnDispose();
        }
    }
}
