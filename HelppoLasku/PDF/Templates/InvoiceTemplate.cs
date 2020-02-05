using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using HelppoLasku.Models;

namespace HelppoLasku.PDF.Templates
{
    public abstract class InvoiceTemplate : Template
    {
        public InvoiceTemplate(Company company, Invoice invoice)
        {
            Company = company;
            Invoice = invoice;
        }

        public Company Company { get; private set; }
        public Invoice Invoice { get; private set; }

        protected DateTime Date => DateTime.Parse(Invoice.Date.ToString());
        protected DateTime DueDate => DateTime.Parse(Invoice.DueDate.ToString());

        protected double TotalTaxless = 0;
        protected double TotalPrice = 0;
        protected double TotalTax => TotalPrice - TotalTaxless;
        protected int Expire => (int)(DueDate - Date).TotalDays;
        
        protected override void CreateContent(Section section)
        {
            Top(section);
            Body(section);
            Bottom(section);
        }

        protected override abstract void Setup(Section section);

        protected abstract void Top(Section section);

        protected abstract void Body(Section section);

        protected abstract void Bottom(Section section);
    }
}
