using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using HelppoLasku.DataAccess;

namespace HelppoLasku.ViewModels
{
    public class AllProductsViewModel : ViewModelBase
    {
        public AllProductsViewModel()
        {
            DisplayName = "Tuotteet";

            ProductList = new ProductListViewModel(Resources.GetModels<Product>());

            ProductFilter = new ProductFilterViewModel();
            ProductList.Filter = ProductFilter;

            GroupList = new ProductGroupListViewModel(Resources.GetModels<ProductGroup>());
            GroupList.SelectionChanged += GroupChanged;
        }

        public ProductListViewModel ProductList { get; private set; }

        public ProductFilterViewModel ProductFilter { get; private set; }

        public ProductGroupListViewModel GroupList { get; private set; }

        void GroupChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.NewSelection == null)
            {
                ProductFilter.Group = "";
                return;
            }

            ProductFilter.Group = (e.NewSelection as ProductGroupViewModel).Name;
        }

        protected override void OnDispose()
        {
            GroupList.SelectionChanged -= GroupChanged;
            base.OnDispose();
        }
    }
}
