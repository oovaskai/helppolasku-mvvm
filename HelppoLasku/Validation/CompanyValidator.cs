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
            "Name", "CompanyID", "Phone", "Email", "Address", "PostalCode", "City",
            "BIC", "IBAN", "ReferenceBase", "InvoiceID",
            "CompanyExpire", "CompanyInterest", "CompanyAnnotation",
            "PersonExpire", "PersonAnnotation"
        };

        public override string Validate(string property)
        {
            switch (property)
            {
                case "Name":
                case "CompanyID":
                    return Validation.Unique(property, Model, true);
                case "Address":
                case "PostalCode":
                case "City":
                case "InvoiceID":
                case "BIC":
                case "IBAN":
                    return Validation.Required(property, ViewModel);
                case "ReferenceBase":
                case "CompanyExpire":
                case "CompanyInterest":
                case "CompanyAnnotation":
                case "PersonExpire":
                case "PersonAnnotation":
                    return Validation.Format(property, ViewModel, true);
                case "Email":
                case "Phone":
                    return Validation.Format(property, ViewModel, false);
                default:
                    return null;
            }
        }
    }
}
