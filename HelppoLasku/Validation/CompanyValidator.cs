using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.ViewModels;

namespace HelppoLasku.Validation
{
    public class CompanyValidator : DataValidator
    {
        public CompanyValidator(EditCompanyViewModel viewmodel) : base(viewmodel)
        {
        }

        protected override string[] Properties => new string[] 
        {
            "Name", "CompanyID", "Email", "Address", "PostalCode", "City", "BIC", "IBAN", "ReferenceNumber", "InvoiceID", "DefaultExpire", "DefaultInterest"
        };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Name":
                case "CompanyID":
                    return Validation.Required(property, ViewModel) ?? Validation.Unique(property, Model);
                case "Address":
                case "PostalCode":
                case "City":
                case "InvoiceID":
                case "BIC":
                case "IBAN":
                    return Validation.Required(property, ViewModel);
                case "DefaultExpire":
                case "ReferenceNumber":
                case "DefaultInterest":
                    return Validation.Required(property, ViewModel) ?? Validation.Format(property, ViewModel);
                case "Email":
                    return Validation.Format(property, ViewModel);
                default:
                    return null;
            }
        }
    }
}
