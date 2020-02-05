using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelppoLasku.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;

namespace HelppoLasku.PDF.Templates
{
    public class DefaultInvoiceTemplate : InvoiceTemplate
    {
        Unit GetWidth(Section section) => section.PageSetup.PageWidth - section.PageSetup.LeftMargin - section.PageSetup.RightMargin;

        Unit GetHeight(Section section) => section.PageSetup.PageHeight - section.PageSetup.TopMargin - section.PageSetup.BottomMargin;

        public DefaultInvoiceTemplate(Company company, Invoice invoice) : base(company, invoice)
        {
            Name = "ovasoft-Laskupohja";
        }

        protected override void Setup(Section section)
        {
            section.PageSetup.PageFormat = PageFormat.A4;

            PageSetup.GetPageSize(section.PageSetup.PageFormat, out Unit pageWidth, out Unit pageHeight);
            section.PageSetup.PageWidth = pageWidth;
            section.PageSetup.PageHeight = pageHeight;

            section.PageSetup.LeftMargin = "2.2cm";
            section.PageSetup.RightMargin = "2.2cm";
            section.PageSetup.TopMargin = "2.2cm";
            section.PageSetup.BottomMargin = "2.2cm";

            var header = section.Headers.Primary.AddParagraph();
            header.Style = "PageHeader";
            header.Format.Alignment = ParagraphAlignment.Right;
            header.AddText("Sivu ");
            header.AddPageField();
            header.AddText(" / ");
            header.AddNumPagesField();
        }

        protected override void Top(Section section)
        {
            Table table = section.AddTable();
            table.Borders.Visible = false;

            table.AddColumn(GetWidth(section) * 0.6 );
            table.AddColumn(GetWidth(section) * 0.2);
            table.AddColumn(GetWidth(section) *0.2);

            Row row = table.AddRow();
            row.Cells[0].AddParagraph(Company.Name); // logo?
            row.Cells[0].Style = "CompanyName";
            row.Cells[1].AddParagraph("LASKU");
            row.Cells[1].Style = "InvoiceTitle";

            Row companyRow = table.AddRow();

            row = table.AddRow();
            row.Cells[1].AddParagraph("Päivämäärä");
            row.Cells[2].AddParagraph(Date.ToShortDateString());

            row = table.AddRow();
            row.Cells[1].AddParagraph("Laskunumero");
            row.Cells[2].AddParagraph(Invoice.InvoiceID.ToString());

            companyRow.Cells[0].MergeDown = 3;
            if (Company.ContactPerson != null)
                companyRow.Cells[0].AddParagraph(Company.ContactPerson);
            companyRow.Cells[0].AddParagraph(Company.Address);
            companyRow.Cells[0].AddParagraph(Company.PostalCode + " " + Company.City);

            row = table.AddRow();
            row.Cells[1].AddParagraph("Viitenumero");
            row.Cells[2].AddParagraph(Invoice.Reference);

            row = table.AddRow();
            row.Cells[1].AddParagraph("Maksuehto");
            row.Cells[2].AddParagraph($"{Expire} pv netto");

            Row customerRow = table.AddRow();
            customerRow.Cells[1].AddParagraph("Eräpäivä");
            customerRow.Cells[2].AddParagraph(DueDate.ToShortDateString());

            row = table.AddRow();
            row.Cells[1].AddParagraph("Huomautusaika");
            row.Cells[2].AddParagraph($"{Invoice.AnnotationTime.ToString()} vrk");

            row = table.AddRow();
            row.Cells[1].AddParagraph("Viivästyskorko");
            row.Cells[2].AddParagraph($"{Invoice.Interest.ToString()} %");

            row = table.AddRow();

            customerRow.Cells[0].MergeDown = 3;
            customerRow.Cells[0].AddParagraph(Invoice.Customer.Name);
            if (Invoice.Customer.ContactPerson != null)
                customerRow.Cells[0].AddParagraph(Invoice.Customer.ContactPerson);
            customerRow.Cells[0].AddParagraph(Invoice.Customer.Address);
            customerRow.Cells[0].AddParagraph(Invoice.Customer.PostalCode + " " + Invoice.Customer.City + (Invoice.Customer.Country != null ?  " / " + Invoice.Customer.Country : null));

            section.AddParagraph();
        }

        protected override void Body(Section section)
        {
            Table table = section.AddTable();
            table.Borders.Visible = false;

            table.AddColumn(GetWidth(section) * 0.012);
            table.AddColumn(GetWidth(section) * 0.488);
            table.AddColumn(GetWidth(section) * 0.12);

            Column column = table.AddColumn(GetWidth(section) * 0.12);
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn(GetWidth(section) * 0.09);
            column.Format.Alignment = ParagraphAlignment.Right;
            column = table.AddColumn(GetWidth(section) * 0.17);
            column.Format.Alignment = ParagraphAlignment.Right;

            Row row = table.AddRow();
            row.Height = "0.6cm";
            row.Style = "BodyColumnHeader";
            row.Cells[0].AddParagraph("Nimike");
            row.Cells[0].MergeRight = 1;
            row.Cells[2].AddParagraph("Määrä");
            row.Cells[3].AddParagraph("á-hinta");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[4].AddParagraph("ALV");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[5].AddParagraph("Yhteensä");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
            row.Borders.Bottom.Style = BorderStyle.Single;

            table.AddRow();

            foreach (InvoiceTitle title in Invoice.Titles)
            {
                row = table.AddRow();
                row.Height = "0.5cm";
                row.Style = "BodyTitle";
                row.Cells[0].AddParagraph(title.Header ?? "");
                row.Cells[0].MergeRight = 5;

                foreach (InvoiceItem item in title.Items)
                {
                    double tax = item.Tax > 0 ? item.Tax : item.Tax * -1;
                    double price = item.Tax < 0 ? item.Price - (item.Price * tax / (tax + 100)) : item.Price;
                    double total = price * (1 + tax / 100) * item.Count;

                    row = table.AddRow();
                    row.Style = "BodyItem";
                    row.Cells[1].AddParagraph(item.Content);
                    row.Cells[2].AddParagraph(item.Count.ToString() + " " + item.Unit);
                    row.Cells[3].AddParagraph(price.ToString("C", System.Globalization.CultureInfo.CurrentCulture));
                    row.Cells[4].AddParagraph(tax.ToString("00") + " %");
                    row.Cells[5].AddParagraph(total.ToString("C", System.Globalization.CultureInfo.CurrentCulture));

                    TotalTaxless += price * item.Count;
                    TotalPrice += total;
                }
            }
            table.Rows[table.Rows.Count - 1].KeepWith = 1;
            table.AddRow();
        }

        protected override void Bottom(Section section)
        {
            var textFrame = section.AddTextFrame();
            textFrame.RelativeVertical = RelativeVertical.Page;
            textFrame.Top = ShapePosition.Bottom;
            //textFrame.Width = section.PageSetup.PageWidth;
            //textFrame.Height = "5.5cm";

            Table table = textFrame.AddTable();
            table.Borders.Visible = false;

            table.AddColumn(GetWidth(section) * 0.2);
            table.AddColumn(GetWidth(section) * 0.3);
            table.AddColumn(GetWidth(section) * 0.24);
            table.AddColumn(GetWidth(section) * 0.1);
            table.AddColumn(GetWidth(section) * 0.16);

            Row row = table.AddRow();
            row.Height = "0.5cm";
            row.Format.Alignment = ParagraphAlignment.Right;
            row.Style = "BottomTaxes";
            row.Cells[0].MergeRight = 3;
            row.Cells[0].AddParagraph("Arvonlisäveroton hinta");
            row.Cells[4].AddParagraph(TotalTaxless.ToString("C", System.Globalization.CultureInfo.CurrentCulture));

            row = table.AddRow();
            row.Height = "0.5cm";
            row.Format.Alignment = ParagraphAlignment.Right;
            row.Style = "BottomTaxes";
            row.Cells[0].MergeRight = 3;
            row.Cells[0].AddParagraph("Arvonlisävero yhteensä");
            row.Cells[4].AddParagraph(TotalTax.ToString("C", System.Globalization.CultureInfo.CurrentCulture));

            row.Borders.Bottom.Style = BorderStyle.Single;;

            row = table.AddRow();
            row.Style = "BottomHeader";
            row.Cells[0].AddParagraph("Eräpäivä");
            row.Cells[1].AddParagraph("Viitenumero");
            row.Height = "0.6cm";

            row.Cells[2].AddParagraph("Maksettava yhteensä");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[2].MergeRight = 1;
            row.Cells[2].Style = "BottomTotal";

            row.Cells[4].AddParagraph(TotalPrice.ToString("C", System.Globalization.CultureInfo.CurrentCulture));
            row.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[4].Style = "BottomValue";

            row = table.AddRow();
            row.Style = "Heading3";
            row.Cells[0].AddParagraph(DueDate.ToShortDateString());
            row.Cells[1].AddParagraph(Invoice.Reference);

            row.Borders.Bottom.Style = BorderStyle.Single;

            Row row1 = table.AddRow();
            row1.Style = "BottomHeader";
            row1.Cells[0].AddParagraph("BIC");
            row1.Cells[1].AddParagraph("IBAN");
            row1.Height = "0.6cm";

            row = table.AddRow();
            row.Style = "BottomValue";
            row.Cells[0].AddParagraph(Company.BIC);
            row.Cells[1].AddParagraph(Company.IBAN);
            row.Cells[1].MergeRight = 1;

            table.AddRow();

            row1.Cells[2].MergeDown = 2;
            row1.Cells[2].Style = "BottomInfo";
            row1.Cells[2].AddParagraph(Company.Name);
            if (Company.ContactPerson != null)
                row1.Cells[2].AddParagraph(Company.ContactPerson);
            row1.Cells[2].AddParagraph(Company.Address);
            row1.Cells[2].AddParagraph($"{Company.PostalCode} {Company.City}" + (Company.Country != null ? " / " + Company.Country : ""));

            row1.Cells[3].MergeDown = 2;
            row1.Cells[3].MergeRight = 1;
            row1.Cells[3].Style = "BottomInfo";
            if (Company.Phone != null)
                row1.Cells[3].AddParagraph(Company.Phone);
            if (Company.Email != null)
                row1.Cells[3].AddParagraph(Company.Email);
            if (Company.WebPage != null)
                row1.Cells[3].AddParagraph(Company.WebPage);
            row1.Cells[3].AddParagraph("Y-tunnus " + Company.CompanyID);

            table.AddRow().Height = "0.5cm";

            Unit tableHeight = 0;
            foreach (Row r in table.Rows)
                tableHeight += r.Height;

            textFrame.Height = tableHeight + section.PageSetup.BottomMargin;
        }
    }
}
