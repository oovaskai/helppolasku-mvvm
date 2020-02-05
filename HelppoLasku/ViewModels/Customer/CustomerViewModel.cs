using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;


namespace HelppoLasku.ViewModels
{
    public class CustomerViewModel : ContactViewModel
    {
        public CustomerViewModel(Customer customer) : base(customer)
        {
        }

        public new Customer Model
        {
            get => base.Model as Customer;
            set => base.Model = value;
        }

        public string CustomerID
        {
            get => Model.CustomerID;
            set
            {
                if (Model.CustomerID != value)
                {
                    Model.CustomerID = value;
                    RaisePropertyChanged("CustomerID");
                }
            }
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(ContactPerson))
                    return Name;
                return Name + " / " + ContactPerson;
            }
        }

        public virtual bool IsCompany => !string.IsNullOrEmpty(Model.CompanyID);

        public string CustomerType => IsCompany ? "Yritys" : "Henkilö";

        public string Location
        {
            get
            {
                if (string.IsNullOrEmpty(City))
                    return Country;

                if (string.IsNullOrEmpty(Country))
                    return City;

                return City + " / " + Country;
            }
        }

        public override void OnModelChanged(object sender, ModelChangedEventArgs e)
        {
            base.OnModelChanged(sender, e);

            if (e.Properties != null)
            {
                if (e.Properties.Contains("CompanyID"))
                {
                    RaisePropertyChanged("IsCompany");
                    RaisePropertyChanged("CustomerType");
                }

                if (e.Properties.Contains("City") || e.Properties.Contains("Country"))
                {
                    RaisePropertyChanged("Location");
                }

                if (e.Properties.Contains("Name") || e.Properties.Contains("ContactPerson"))
                {
                    RaisePropertyChanged("FullName");
                }
            }   
        }
    }  
}
