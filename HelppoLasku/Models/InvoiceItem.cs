using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelppoLasku.Models
{
    public class InvoiceItem : DataAccess.DataModel
    {
        public InvoiceItem() : base()
        {
        }

        public InvoiceItem(DataAccess.DataModel source) : base(source)
        {
        }

        public InvoiceTitle Title { get; set; }

        public int Index { get; set; }

        public string Content { get; set; }

        public double Count { get; set; }

        public string Unit {  get; set; }

        public double Price { get; set; }

        public double Tax { get; set; }

        public string Info { get; set; }

        internal override string[] CopyProperties => new string[] { "Title", "Index", "Content", "Count", "Unit", "Price", "Tax", "Info" };
    }
}
