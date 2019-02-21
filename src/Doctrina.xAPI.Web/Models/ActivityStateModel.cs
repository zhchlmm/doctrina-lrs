using Doctrina.xAPI.LRS.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace Doctrina.xAPI.LRS.Models
{
    [ModelBinder(typeof(ActivityStateModelBinder))]
    public class StateDocumentModel
    {
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
