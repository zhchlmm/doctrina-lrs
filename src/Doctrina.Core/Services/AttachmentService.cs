using Doctrina.Core.Data;
using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Doctrina.Core.Services
{
    public class AttachmentService : IAttachmentService
    {
        private void AddAttachment(StatementEntity entity, Attachment attachment, byte[] payload = null)
        {

            // Signature Requirements
            if (attachment.ContentType.StartsWith(MediaTypes.Application.OctetStream)
                && attachment.UsageType == new Iri("http://adlnet.gov/expapi/attachments/signature"))
            {
                /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#signature-requirements
                // TODO: Create attachment using documentService?
                throw new NotImplementedException();
            }

            // 

            var current = JObject.FromObject(attachment);
            var canonicalData = new JObject();
            if (current["display"] != null)
            {
                canonicalData["display"] = current["display"];
            }
            if (current["description"] != null)
            {
                canonicalData["description"] = current["description"];
            }
            var attachmentEntity = new AttachmentEntity()
            {
                SHA2 = attachment.SHA2,
                ContentType = attachment.ContentType,
                StatementId = entity.StatementId,
                CanonicalData = canonicalData.ToString(Newtonsoft.Json.Formatting.None),
            };

            if (attachment.FileUrl != null)
            {
                // Attachment is located on a remote resource
                attachmentEntity.FileUrl = attachment.FileUrl.ToString();
            }
            else
            {
                // Attachment was part of the post body
                attachmentEntity.Content = payload;
            }

            entity.Attachments.Add(attachmentEntity);
        }
    }
}
