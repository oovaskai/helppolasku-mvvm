using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.DataAccess;

namespace HelppoLasku.Models
{
    public class Invoice : DataModel
    {
        public Invoice() : base()
        {
            Titles = new List<InvoiceTitle>();
        }

        public Invoice(DataModel source) : base(source)
        {
            (source as Invoice).CopyTitles(this);
        }

        public int? InvoiceID { get; set; }

        public Customer Customer { get; set; }

        public DateTime? Date { get; set; }

        public bool? Paid { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? PayDate { get; set; }

        public string Reference { get; set; }

        public double? Interest { get; set; }

        public int? AnnotationTime { get; set; }

        public List<InvoiceTitle> Titles { get; set; }

        public string Info { get; set; }

        internal override string[] CopyProperties => new string[] { "InvoiceID", "Customer", "Date", "Paid", "DueDate", "PayDate", "Reference", "Interest", "AnnotationTime", "Info" };

        public override void CopyTo(DataModel target, out string[] copiedProperties)
        {
            base.CopyTo(target, out copiedProperties);
            copiedProperties.Concat(new string[] { "Titles" });
            CopyTitles(target as Invoice);
        }

        public override void CopyTo(DataModel target)
        {
            base.CopyTo(target);
            CopyTitles(target as Invoice);
        }

        public void CopyTitles(Invoice target)
        {
            target.Titles = new List<InvoiceTitle>();
            foreach (InvoiceTitle title in Titles)
            {
                target.Titles.Add(new InvoiceTitle(title));
            }
        }
    }
}
