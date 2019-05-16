using Doctrina.xAPI.LRS.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Doctrina.xAPI.LRS.Models
{
    [ModelBinder(typeof(ActivityStateModelBinder))]
    public class StateDocumentModel
    {

        public string StateId { get; set; }
        public Iri ActivityId { get; set; }
        public Agent Agent { get; set; }
        public Guid? Registration { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
