using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.Models
{
    public class Product : DataAccess.DataModel
    {
        public Product() : base()
        {
        }

        public Product(DataAccess.DataModel source) : base(source) { }

        public string Name { get; set; }

        public string ProductID { get; set; }

        public ProductGroup Group { get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        public string Unit { get; set; }

        public string Info { get; set; }

        internal override string[] CopyProperties => new string[] { "Name", "ProductID", "Group", "Price", "Tax", "Unit", "Info" };
    }
}
