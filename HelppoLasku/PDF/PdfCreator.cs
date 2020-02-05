using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace HelppoLasku.PDF
{
    public class PdfCreator
    {
        public PdfCreator()
        {
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public string Subject { get; set; }

        public Theme Theme { get; set; }
        public Template Template { get; set; }

        public void CreateDocument(string filename, bool openFile)
        {
            if (Template == null)
                throw new NullReferenceException("Template");

            var document = new Document();
            Theme?.Define(document);
            document.Add(Template.Create());

            try
            {
                var renderer = new PdfDocumentRenderer(true);
                renderer.Document = document;
                renderer.RenderDocument();

                if (filename.LastIndexOf(".pdf") != filename.Length - 4)
                    filename += ".pdf";

                renderer.PdfDocument.Save(filename);

                if (openFile)
                    Process.Start(filename);
            }
            catch (Exception ex)
            {
                Views.MainWindow.Message(ex.Message, "Virhe", System.Windows.MessageBoxImage.Error);
            }      
        }
    }
}
