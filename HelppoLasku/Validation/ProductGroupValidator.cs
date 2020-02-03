using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class ProductGroupValidator : DataValidator
    {
        public ProductGroupValidator(ProductGroupViewModel viewmodel) : base(viewmodel)
        {
        }

        protected override string[] Properties => new string[] { "Name" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Name":
                    return Validation.Unique(property, Model, true);
                default:
                    return null;
            }
        }
    }
}
