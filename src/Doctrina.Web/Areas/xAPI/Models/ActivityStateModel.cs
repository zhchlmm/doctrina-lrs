using Doctrina.Web.Areas.xAPI.Mvc.ModelBinders;
using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.Web.Areas.xAPI.Models
{
    [ModelBinder(typeof(ActivityStateModelBinder))]
    public class StateDocumentModel
    {
        [Required]
        public Iri ActivityId { get; set; }
        [Required]
        public Agent Agent { get; set; }
        [Required]
        public string StateId { get; set; }
        public Guid? Registration { get; set; }

        // POST
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
