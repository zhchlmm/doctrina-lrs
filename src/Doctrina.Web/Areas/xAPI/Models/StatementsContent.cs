using Doctrina.Web.Areas.xAPI.Mvc.ModelBinders;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doctrina.Web.Areas.xAPI.Models
{
    [ModelBinder(BinderType = typeof(StatementsPostContentModelBinder))]
    public class StatementsPostContent
    {
        public Statement[] Statements { get; set; }

        public AttachmentContent[] Attachments { get; set; }
    }

    public class AttachmentContent {
        public string ContentType { get; set; }
        public string Hash { get; set; }
        public byte[] Content { get; set; }
    }
}
