using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using System;

namespace Doctrina.Persistence.Services
{
    public interface IAttachmentService
    {
        AttachmentEntity CreateAttachment(Guid statementId, Attachment attachment);
    }
}
