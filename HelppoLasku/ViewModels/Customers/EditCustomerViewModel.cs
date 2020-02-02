using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class EditCustomerViewModel : CustomerViewModel
    {
        public EditCustomerViewModel(Customer customer) : base(customer)
        {
            EditEnabled = true;
            Validator = new Validation.CustomerValidator(this);

            DisplayName = Model.IsNew ? "Uusi asiakas" : "Muokkaa asiakasta " + Name;

            IsCompany = !string.IsNullOrEmpty(CompanyID);
        }

        bool isCompany;

        public new bool IsCompany
        {
            get => isCompany; 
            set
            {
                if (IsCompany != value)
                {
                    isCompany = value;
                    RaisePropertyChanged("IsCompany");
                    RaisePropertyChanged("CompanyID");
                }
            }
        }

        public override string Error
        {
            get
            {
                if (this["CompanyID"] != null)
                    return this["CompanyID"];

                return base.Error;
            }
        }

        public override string this[string property]
        {
            get
            {
                if (property == "CompanyID")
                    if (IsCompany && string.IsNullOrEmpty(CompanyID))
                        return "CompanyID ei voi olla tyhjä.";
                return base[property];
            }
        }

        public override void OnSave()
        {
            if (!IsCompany)
            {
                CompanyID = null;
                ContactPerson = null;
            }

            base.OnSave();
        }
    }
}
