using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class InvoiceValidator : DataValidator
    {
        public InvoiceValidator(EditInvoiceViewModel viewmodel) : base(viewmodel)
        {
        }

        protected override string[] Properties => new string[] { "Customer" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Customer":
                    return Validation.Required(property, Model);
                default:
                    return null;
            }
        }
    }
}
