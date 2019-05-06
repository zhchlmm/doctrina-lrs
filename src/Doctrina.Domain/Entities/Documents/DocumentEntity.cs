using System;
using System.Security.Cryptography;
using System.Text;

namespace Doctrina.Domain.Entities.Documents
{
    /// <summary>
    /// Represents a stored document
    /// </summary>
    public class DocumentEntity : IDocumentEntity
    {
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

        public DateTimeOffset CreateDate { get; set; }

        public void GenerateChecksum()
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Content);
                Checksum = Encoding.UTF8.GetString(hash);
            }
        }

        public static DocumentEntity Create(byte[] content, string contentType)
        {
            var doc = new DocumentEntity()
            {
                Content = content,
                ContentType = contentType,
                CreateDate = DateTimeOffset.UtcNow,
            };

            doc.GenerateChecksum();

            return doc;
        }
    }
}
