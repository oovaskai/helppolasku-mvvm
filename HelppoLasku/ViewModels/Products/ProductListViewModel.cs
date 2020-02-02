using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ProductListViewModel : ListViewModel
    {
        public ProductListViewModel(System.Collections.ObjectModel.ObservableCollection<DataAccess.DataModel> source) : base(source) { }

        public new ProductViewModel SelectedItem
        {
            get => base.SelectedItem as ProductViewModel;
            set => base.SelectedItem = value;
        }

        public new ProductFilterViewModel Filter
        {
            get => base.Filter as ProductFilterViewModel;
            set => base.Filter = value;
        }

        public override DataViewModel NewItem(DataAccess.DataModel model)
            => new ProductViewModel(model as Product);

        public override void OnNew()
        {
            Product product = new Product { Name = Filter.Name, ProductID = Filter.ProductID, Unit = Filter.Unit };
            Views.MainWindow.EditDialog(new EditProductViewModel(product), 500, 350);
        }

        public override void OnEdit()
        {
            Views.MainWindow.EditDialog(new EditProductViewModel(new Product(SelectedItem.Model)), 500, 350);
        }

        public override void OnCopy()
        {
            Product copy = new Product();
            SelectedItem.Model.CopyTo(copy);

            Views.MainWindow.EditDialog(new EditProductViewModel(copy), 500, 350);
        }

        public override void OnDelete()
        {
            if (Views.MainWindow.ConfirmMessage("Haluatko varmasti poistaa tuotteen " + SelectedItem.Name + " ?", "Varoitus", System.Windows.MessageBoxImage.Question))
            {
                SelectedItem.Model.Delete();
                SelectedItem = null;
            }
        }

    }
}
