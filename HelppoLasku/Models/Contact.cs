using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Models
{
    public class Contact : DataModel
    {
        public Contact() : base()
        {
        }

        public Contact(DataModel source) : base(source)
        {
        }

        public string Name { get; set; }

        public string CompanyID { get; set; }

        public string ContactPerson { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string WebPage { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Info { get; set; }

        internal override string[] CopyProperties => new string[] { "Name", "CompanyID", "ContactPerson", "Phone", "Email", "WebPage", "Address", "PostalCode", "City", "Country", "Info" };
    }
}
