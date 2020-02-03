using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;

namespace HelppoLasku.PDF
{
    public class PdfCreator
    {
        public PdfCreator()
        {

        }

        public void Create(string hello)
        {
            PdfDocument document = new PdfDocument();

            document.Info.Title = "Invoice";
            document.Info.Author = "HelppoLasku";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Verdana", 20, XFontStyle.Bold);

            for (int i = 0; i < 50; i++)
            {
                XRect rect = new XRect(0, i * 20, page.Width, page.Height);
                gfx.DrawString(i + " " + hello, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            }

            string filename = "Hello.pdf";

            document.Save(filename);

            Process.Start(filename);
        }
    }
}
