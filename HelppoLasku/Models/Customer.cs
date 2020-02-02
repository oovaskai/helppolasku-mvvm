using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Models
{
    public class Customer : Contact
    {
        public Customer() : base() { }

        public Customer(DataModel source) : base(source) { }

        public string CustomerID { get; set; }

        internal override string[] CopyProperties
        {
            get
            {
                string[] props = base.CopyProperties.Concat(new string[] { "CustomerID" }).ToArray();

                return props;
            }
        }
    }
}
