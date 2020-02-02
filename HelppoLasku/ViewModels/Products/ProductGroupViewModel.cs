using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ProductGroupViewModel : DataViewModel
    {
        public ProductGroupViewModel(DataAccess.DataModel dataModel) : base(dataModel) { }

        public new ProductGroup Model => base.Model as ProductGroup;

        public int Index
        {
            get { return Model.Index; }
            set
            {
                if (Model.Index != value)
                {
                    Model.Index = value;
                    RaisePropertyChanged("Index");
                }
            }
        }

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

        public override bool EditEnabled
        {
            get => base.EditEnabled;
            set
            {
                if (base.EditEnabled != value)
                {
                    if (value)
                        Validator = new Validation.ProductGroupValidator(this);
                    else
                        Validator = null;

                    base.EditEnabled = value;
                }

            }
        }
    }
}
