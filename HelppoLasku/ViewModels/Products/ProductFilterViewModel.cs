using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ProductFilterViewModel : FilterViewModel
    {
        public override bool Filter(object item)
        {
            ProductViewModel product = item as ProductViewModel;

            if (string.IsNullOrEmpty(Group) || Group == ProductGroup.All.Name || (product.Group != null && product.Group.Name == Group))
                if (string.IsNullOrEmpty(ProductID) || (!string.IsNullOrEmpty(product.ProductID) && product.ProductID.IndexOf(ProductID, StringComparison.OrdinalIgnoreCase) >= 0))
                    if (string.IsNullOrEmpty(Name) || product.Name.IndexOf(Name, StringComparison.OrdinalIgnoreCase) >= 0)
                        if (string.IsNullOrEmpty(Unit) || (!string.IsNullOrEmpty(product.Unit) && product.Unit.IndexOf(Unit, StringComparison.OrdinalIgnoreCase) >= 0))
                            if (string.IsNullOrEmpty(Tax) || product.Tax.ToString().IndexOf(Tax, StringComparison.OrdinalIgnoreCase) >= 0)
                                return true;
            return false;
        }

        public override void OnClear()
        {

            Group = "";
            ProductID = "";
            Name = "";
            Unit = "";
            Tax = "";
        }

        #region Properties

        string group;

        public string Group
        {
            get { return group; }
            set
            {
                if (group != value)
                {
                    group = value;
                    RaisePropertyChanged("Group");
                }
            }
        }

        string productID;

        public string ProductID
        {
            get { return productID; }
            set
            {
                if (productID != value)
                {
                    productID = value;
                    RaisePropertyChanged("ProductID");
                }
            }
        }

        string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        string unit;

        public string Unit
        {
            get { return unit; }
            set
            {
                if (unit != value)
                {
                    unit = value;
                    RaisePropertyChanged("Unit");
                }
            }
        }

        string tax;

        public string Tax
        {
            get { return tax; }
            set
            {
                if (tax != value)
                {
                    tax = value;
                    RaisePropertyChanged("Tax");
                }
            }
        }

        public string[] TaxRates => Properties.Settings.Default.TaxRates.Split('%');

        #endregion
    }
}
