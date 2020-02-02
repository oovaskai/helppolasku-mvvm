using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;
using HelppoLasku.Models;

namespace HelppoLasku.ViewModels
{
    public class ContactViewModel : DataViewModel
    {
        protected ContactViewModel(Contact contact) : base(contact)
        {
        }

        public new Contact Model
        {
            get => base.Model as Contact;
            set => base.Model = value;
        }

        public virtual string Name
        {
            get => Model.Name;
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        public string CompanyID
        {
            get => Model.CompanyID;
            set
            {
                if (Model.CompanyID != value)
                {
                    Model.CompanyID = value;
                    RaisePropertyChanged("CompanyID");
                }
            }
        }


        public string ContactPerson
        {
            get => Model.ContactPerson;
            set
            {
                if (Model.ContactPerson != value)
                {
                    Model.ContactPerson = value;
                    RaisePropertyChanged("ContactPerson");
                }
            }
        }

        public string Phone
        {
            get => Model.Phone;
            set
            {
                if (Model.Phone != value)
                {
                    Model.Phone = value;
                    RaisePropertyChanged("Phone");
                }
            }
        }

        public string Email
        {
            get => Model.Email;
            set
            {
                if (Model.Email != value)
                {
                    Model.Email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        public string WebPage
        {
            get => Model.WebPage;
            set
            {
                if (Model.WebPage != value)
                {
                    Model.WebPage = value;
                    RaisePropertyChanged("WebPage");
                }
            }
        }

        public string Address
        {
            get => Model.Address;
            set
            {
                if (Model.Address != value)
                {
                    Model.Address = value;
                    RaisePropertyChanged("Address");
                }
            }
        }

        public string PostalCode
        {
            get => Model.PostalCode;
            set
            {
                if (Model.PostalCode != value)
                {
                    Model.PostalCode = value;
                    RaisePropertyChanged("PostalCode");
                }
            }
        }

        public string City
        {
            get => Model.City;
            set
            {
                if (Model.City != value)
                {
                    Model.City = value;
                    RaisePropertyChanged("City");
                }
            }
        }

        public string Country
        {
            get => Model.Country;
            set
            {
                if (Model.Country != value)
                {
                    Model.Country = value;
                    RaisePropertyChanged("Country");
                }
            }
        }

        public string Info
        {
            get => Model.Info;
            set
            {
                if (Model.Info != value)
                {
                    Model.Info = value;
                    RaisePropertyChanged("Info");
                }
            }
        }
    }
}
