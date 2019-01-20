using Doctrina.Core.Data;
using Doctrina.xAPI;
using System;

namespace Doctrina.Core.Services
{
    public interface IAttachmentService
    {
        AttachmentEntity CreateAttachment(Guid statementId, Attachment attachment);
    }
}
