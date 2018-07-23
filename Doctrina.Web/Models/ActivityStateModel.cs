using ExperienceAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using UmbracoLRS.Core.ModelBinders;

namespace UmbracoLRS.Core.Models
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
        public string MediaType { get; set; }
        public string JsonDocument { get; set; }
        public byte[] BinaryDocument { get; set; }
    }
}
