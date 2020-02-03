using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class SendInvoiceValidator : DataValidator
    {
        public SendInvoiceValidator(DataViewModel viewmodel) : base(viewmodel)
        {
        }

        protected override string[] Properties => new string[] { "Date", "DueDate", "Interest", "ExpireDays", "AnnotationTime" };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Date":
                case "DueDate":
                    return Validation.Required(property, ViewModel);
                case "Interest":
                case "ExpireDays":
                case "AnnotationTime":
                    return Validation.Format(property, ViewModel, true);   
                default:
                    return null;
            }
        }
    }
}
