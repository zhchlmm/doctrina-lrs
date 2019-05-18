using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Doctrina.Domain.Entities.Documents
{
    /// <summary>
    /// Represents a stored document
    /// </summary>
    [Owned]
    public abstract class DocumentBaseEntity
    {
        public DocumentBaseEntity() { }

        public DocumentBaseEntity(byte[] content, string contentType)
        {
            Content = content;
            ContentType = contentType;
            LastModified = DateTimeOffset.UtcNow;
            CreateDate = DateTimeOffset.UtcNow;
            Checksum = GenerateChecksum();
        }

        public Guid Key { get; set; }

        /// <summary>
        /// Representation of the Content-Type header received
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// The byte array of the document content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// MD5 Checksum
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// UTC Date when the document was last modified
        /// </summary>
        public DateTimeOffset? LastModified { get; set; }

        /// <summary>
        /// UTC Date when the document was created
        /// </summary>
        public DateTimeOffset CreateDate { get; set; }

        // Methods:
        private string GenerateChecksum()
        {
            using (var md5 = MD5.Create())
            {
                byte[] checksum = md5.ComputeHash(Content);
                return BitConverter.ToString(checksum).Replace("-", string.Empty).ToLower(); ;
            }
        }

        // Factories:
        public void UpdateDocument(byte[] content, string contentType)
        {
            this.Content = content;
            this.ContentType = contentType;
            this.LastModified = DateTimeOffset.UtcNow;
            Checksum = this.GenerateChecksum();
        }
    }
}
