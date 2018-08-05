using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;

namespace Doctrina.Web.Mvc
{
    public class MultipartMixedResult : Collection<MultipartContent>, IActionResult
    {
        private readonly System.Net.Http.MultipartContent content;

        public MultipartMixedResult(string subtype = "byteranges", string boundary = null)
        {
            if (boundary == null)
            {
                this.content = new MultipartContent(subtype);
            }
            else
            {
                this.content = new MultipartContent(subtype, boundary);
            }
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            //foreach (var item in this)
            //{
            //    if (item.Headers != null)
            //    {
            //        var content = new StreamContent(item.ReadAsStreamAsync().Result);

            //        if (item.ContentType != null)
            //        {
            //            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(item.ContentType);
            //        }

            //        if (item.FileName != null)
            //        {
            //            var contentDisposition = new ContentDispositionHeaderValue("attachment");
            //            contentDisposition.SetHttpFileName(item.FileName);
            //            content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //            content.Headers.ContentDisposition.FileName = contentDisposition.FileName;
            //            content.Headers.ContentDisposition.FileNameStar = contentDisposition.FileNameStar;
            //        }

            //        this.content.Add(content);
            //    }
            //}

            context.HttpContext.Response.ContentLength = content.Headers.ContentLength;
            context.HttpContext.Response.ContentType = content.Headers.ContentType.ToString();

            await content.CopyToAsync(context.HttpContext.Response.Body);
        }
    }
}
