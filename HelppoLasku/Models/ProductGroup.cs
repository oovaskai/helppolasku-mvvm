using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.Models
{
    public class ProductGroup : DataAccess.DataModel
    {
        static ProductGroup all;

        public static ProductGroup All
        {
            get
            {
                if (all == null)
                    all = new ProductGroup { Index = 0, Name = "Kaikki" };
                return all;
            }
        }

        public ProductGroup() : base() { }

        public ProductGroup(DataAccess.DataModel source) : base(source) { }

        public int Index { get; set; }

        public string Name { get; set; }

        internal override string[] CopyProperties => new string[] { "Index", "Name" };

        public override void Save()
        {
            if (!Equals(All))
                base.Save();
        }

        public override void Delete()
        {
            foreach (DataAccess.DataModel dataModel in DataAccess.Resources.GetModels<Product>())
            {
                Product product = dataModel as Product;
                if (product.Group != null && product.Group.Equals(this))
                {
                    product.Group = null;
                    product.Save();
                }      
            }
            base.Delete();
        }
    }
}
