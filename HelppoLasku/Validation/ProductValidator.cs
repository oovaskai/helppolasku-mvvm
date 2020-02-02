using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class ProductValidator : DataValidator
    {
        public ProductValidator(EditProductViewModel viewmodel) : base(viewmodel)
        {
        }

        EditProductViewModel Product => ViewModel as EditProductViewModel;

        protected override string[] Properties => new string[] { "Name", "ProductID", "Price" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Name":
                    return Validation.Required(property, ViewModel);
                case "Price":
                    return Validation.Required(property, ViewModel) ?? Validation.Format(property, ViewModel);
                case "ProductID":
                    return Validation.Unique(property, Model);
                default:
                    return null;

            }
        }
    }
}
