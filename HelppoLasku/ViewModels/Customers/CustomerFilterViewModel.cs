using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.ViewModels
{
    public class CustomerFilterViewModel : FilterViewModel
    {
        public override bool Filter(object item)
        {
            CustomerViewModel customer = item as CustomerViewModel;

            if (string.IsNullOrEmpty(CustomerID) || (!string.IsNullOrEmpty(customer.CustomerID) && customer.CustomerID.IndexOf(CustomerID, StringComparison.OrdinalIgnoreCase) >= 0))
                if (string.IsNullOrEmpty(Location) || (!string.IsNullOrEmpty(customer.Location) && customer.Location.IndexOf(Location, StringComparison.OrdinalIgnoreCase) >= 0))
                    if (string.IsNullOrEmpty(Name) || customer.FullName.IndexOf(Name, StringComparison.OrdinalIgnoreCase) >= 0)
                            if ((IsCompany && customer.IsCompany) || (IsPerson && !customer.IsCompany))
                                return true;
            
            return false;
        }

        public override void OnClear()
        {
            CustomerID = null;
            Name = null;
            Location = null;
            IsPerson = true;
            IsCompany = true;
        }

        #region Properties

        string customerID;

        public string CustomerID
        {
            get { return customerID; }
            set
            {
                if (customerID != value)
                {
                    customerID = value;
                    RaisePropertyChanged(CustomerID);
                }
            }
        }

        string name;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        string location;

        public string Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    RaisePropertyChanged("Location");
                }
            }
        }

        bool isCompany;

        public bool IsCompany
        {
            get => isCompany;
            set
            {
                if (isCompany != value)
                {
                    isCompany = value;
                    RaisePropertyChanged("IsCompany");

                    if (!value && !IsPerson)
                        IsPerson = true;
                }
            }
        }

        bool isPerson;

        public bool IsPerson
        {
            get => isPerson;
            set
            {
                if (isPerson != value)
                {
                    isPerson = value;
                    RaisePropertyChanged("IsPerson");

                    if (!value && !IsCompany)
                        IsCompany = true;
                }
            }
        }

        #endregion
    }
}
