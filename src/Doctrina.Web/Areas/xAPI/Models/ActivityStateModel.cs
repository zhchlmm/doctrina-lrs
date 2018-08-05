using Doctrina.Web.Areas.xAPI.Mvc.ModelBinders;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.Web.Areas.xAPI.Models
{
    [ModelBinder(typeof(StateDocumentModelBinder))]
    public class StateDocumentModel
    {
        [Required]
        public Uri ActivityId { get; set; }
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
