using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace HelppoLasku.PDF
{
    public abstract class Template
    {
        public string Name { get; protected set; }

        protected abstract void Setup(Section section);

        protected abstract void CreateContent(Section section);

        internal Section Create()
        {
            var section = new Section();
            Setup(section);
            CreateContent(section);
            return section;
        }
    }
}
