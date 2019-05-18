using System;

namespace Doctrina.Domain.Entities
{
    public class AttachmentEntity
    {
        public Guid Id { get; set; }

        public string UsageType { get; set; }

        public string CanonicalData { get; set; }

        public string ContentType { get; set; }

        public byte[] Payload { get; set; }

        public string FileUrl { get; set; }

        public string SHA2 { get; set; }

        public long Length { get; set; }

        public Guid StatementId { get; set; }

        public virtual StatementBaseEntity Statement { get; set; }
    }
}
