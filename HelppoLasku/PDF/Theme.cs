using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace HelppoLasku.PDF
{
    public abstract class Theme
    {
        public string Name { get; private set; }

        internal abstract void Define(Document document);
    }
}
