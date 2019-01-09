using Doctrina.Core.Data;
using Doctrina.xAPI.Models;
using System;
using System.Linq;

namespace Doctrina.Core.Services
{
    public interface IAttachmentService
    {
        void AddAttachment(StatementEntity statement, Attachment attachment, byte[] payload = null);
    }
}
