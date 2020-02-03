using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class InvoiceItemValidator : DataValidator
    {
        public InvoiceItemValidator(EditInvoiceItemViewModel viewmodel) : base(viewmodel)
        {
        }

        protected override string[] Properties => new string[] { "Content", "Count", "Price" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Content":
                    return Validation.Required(property, ViewModel);
                case "Count":
                case "Price":
                    return Validation.Format(property, ViewModel, true);
                default:
                    return null;
            }
        }
    }
}
