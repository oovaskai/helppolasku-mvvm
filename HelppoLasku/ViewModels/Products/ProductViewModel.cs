using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ProductViewModel : DataViewModel
    {
        public ProductViewModel(Product product) : base(product)
        {
            if (product.Group != null)
            {
                Group = new ProductGroupViewModel(product.Group);
                Group.Model.ModelChanged += OnModelChanged;
            }
        }

        #region Properties

        public new Product Model => base.Model as Product;

        ProductGroupViewModel group;

        public virtual ProductGroupViewModel Group
        {
            get { return group; }
            set
            {
                if (group != value)
                {
                    if (group != null)
                        group.Model.ModelChanged -= OnModelChanged;

                    group = value;

                    if (value != null)
                        group.Model.ModelChanged += OnModelChanged;

                    Model.Group = value != null ? value.Model : null;
                    RaisePropertyChanged("Group");
                }
            }
        }

        public string DetailName => Name + " | " + TaxlessPrice.ToString("0.00 €") + " / " + Unit + " + " + Tax + " % " + " = " + TotalPrice.ToString("0.00 €" + " | " + Group.Name);

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string ProductID
        {
            get { return Model.ProductID; }
            set
            {
                if (Model.ProductID != value)
                {
                    Model.ProductID = value;
                    RaisePropertyChanged("ProductID");
                }
            }
        }

        public virtual double TaxlessPrice => Model.Tax < 0 ? Model.Price - (Model.Price * Tax / (Tax + 100)) : Model.Price;

        public double TotalPrice => Model.Tax > 0 ? Model.Price * (1 + Tax / 100) : Model.Price;

        public virtual double Tax
        {
            get { return Model.Tax < 0 ? Model.Tax * -1 : Model.Tax; }
            set
            {
                if (Model.Tax != value)
                {
                    Model.Tax = value;
                    RaisePropertyChanged("Tax");
                    RaisePropertyChanged("TaxlessPrice");
                    RaisePropertyChanged("TotalPrice");
                }
            }
        }

        public string Unit
        {
            get { return Model.Unit; }
            set
            {
                if (Model.Unit != value)
                {
                    Model.Unit = value;
                    RaisePropertyChanged("Unit");
                }
            }
        }

        public string Info
        {
            get { return Model.Info; }
            set
            {
                if (Model.Info != value)
                {
                    Model.Info = value;
                    RaisePropertyChanged("Info");
                }
            }
        }

        #endregion // Properties

        public override void OnModelChanged(object sender, DataAccess.ModelChangedEventArgs e)
        {
            if (Group != null && sender.Equals(Group.Model) && e.Type == DataAccess.ModelChangedEventArgs.EventType.Delete)
            {
                Group = null;
            }
            else if (e.Type == DataAccess.ModelChangedEventArgs.EventType.Update && e.Properties.Contains("Group"))
            {
                if ((sender as Product).Group == null)
                    Group = null;
                else
                Group = new ProductGroupViewModel(Model.Group);
            }
            else if(e.Type == DataAccess.ModelChangedEventArgs.EventType.Update && (e.Properties.Contains("Price") || e.Properties.Contains("Tax")))
            {
                RaisePropertyChanged("TaxlessPrice");
                RaisePropertyChanged("TotalPrice");
            }
            base.OnModelChanged(sender, e);
        }

        protected override void OnDispose()
        {
            if (Group != null)
                Group.Model.ModelChanged -= OnModelChanged;

            base.OnDispose();
        }
    }
}
