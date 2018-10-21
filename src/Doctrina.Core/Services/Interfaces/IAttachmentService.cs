using Doctrina.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Services
{
    public interface IAttachmentService
    {
        AttachmentEntity GetAttachments(Guid statementId);
    }
}
