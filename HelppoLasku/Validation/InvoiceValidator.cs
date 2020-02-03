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

        new EditInvoiceViewModel ViewModel => base.ViewModel as EditInvoiceViewModel;

        protected override string[] Properties => new string[] { "Customer", "Items" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Customer":
                    {
                        if (ViewModel.Paid != null)
                            return null;
                        return Validation.Required(property, Model);
                    }
                    
                case "Items":
                    {
                        if (ViewModel.ItemCount == 0)
                            return "Laskulla pitää olla sisältöä.";
                        foreach (EditInvoiceTitleViewModel title in ViewModel.Titles)
                            if (title.Error != null)
                                return title.Error;
                        return null;
                    }
                default:
                    return null;
            }
        }
    }
}
