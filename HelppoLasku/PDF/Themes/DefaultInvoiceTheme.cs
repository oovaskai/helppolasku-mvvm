using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace HelppoLasku.PDF.Themes
{
    public class DefaultInvoiceTheme : Theme
    {
        internal override void Define(Document document)
        {
            var style = document.Styles["Normal"];
            style.Font.Name = "Calibri";
            style.Font.Size = 11;
            style.Font.Color = Colors.Black;

            style = document.Styles.AddStyle("PageHeader", "Normal");

            style = document.Styles.AddStyle("InvoiceTitle", "Normal");
            style.Font.Size = 13;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("CompanyName", "InvoiceTitle");

            style = document.AddStyle("BodyColumnHeader", "Normal");
            style.Font.Bold = true;

            style = document.AddStyle("BodyTitle", "BodyColumnHeader");

            style = document.AddStyle("BodyItem", "Normal");

            style = document.Styles.AddStyle("BottomTaxes", "BodyColumnHeader");

            style = document.Styles.AddStyle("BottomTotal", "BodyColumnHeader");
            style.Font.Size = 12;

            style = document.Styles.AddStyle("BottomHeader", "Normal");
            style.Font.Size = 10;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("BottomValue", "BottomTotal");
            style.Font.Bold = false;

            style = document.Styles.AddStyle("BottomInfo", "Normal");
            style.Font.Size = 8;
            style.Font.Color = Colors.DarkGray;
        }
    }
}
