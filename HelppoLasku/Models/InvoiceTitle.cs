using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Models
{
    public class InvoiceTitle : DataModel
    {
        public InvoiceTitle() : base()
        {
            Items = new List<InvoiceItem>();
        }

        public InvoiceTitle(DataModel source) : base(source)
        {
            (source as InvoiceTitle).CopyItems(this);
        }

        public int Index { get; set; }

        public string Header { get; set; }

        public Invoice Invoice { get; set; }

        public List<InvoiceItem> Items { get; set; }

        internal override string[] CopyProperties => new string[] { "Index", "Header", "Invoice" };

        public override void CopyTo(DataModel target, out string[] copiedProperties)
        {
            base.CopyTo(target, out copiedProperties);
            copiedProperties.Concat(new string[] { "Items" });
            CopyItems(target as InvoiceTitle);
        }

        public void CopyItems(InvoiceTitle target)
        {
            target.Items = new List<InvoiceItem>();
            foreach (InvoiceItem item in Items)
            {
                target.Items.Add(new InvoiceItem(item));
            }
        }
    }
}
