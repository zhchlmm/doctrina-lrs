using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmbracoLRS.Core.Models
{
    public class Document : IDocument
    {
        public string VirtualPath { get; internal set; }
    }
}
