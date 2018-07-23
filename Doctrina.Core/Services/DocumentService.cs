using Doctrina.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using xAPI.Core;
using xAPI.Core.Documents;

namespace Doctrina.Core.Services
{
    public sealed class DocumentService : IDocumentService
    {
        private readonly DoctrinaDbContext dbContext;

        public DocumentService(DoctrinaDbContext context)
        {
            this.dbContext = context;
        }

        public IDocumentEntity GetDocument(Guid documentId)
        {
            return dbContext.Documents.Find(documentId);
        }

        public IDocumentEntity CreateDocument(string contentType, byte[] buffer)
        {
            var entity = new DocumentEntity()
            {
                Id = Guid.NewGuid(),
                ContentType = contentType,
                Content = buffer,
                ETag = ComputeHash(buffer),
                Timestamp = DateTime.UtcNow
            };
            dbContext.Documents.Add(entity);
            dbContext.Entry(entity).State = EntityState.Added;
            return entity;
        }

        public IDocumentEntity UpdateDocument(IDocumentEntity entity, string contentType, byte[] content)
        {
            if ((entity.Content != null
                && entity.ContentType.StartsWith(MIMETypes.Application.Json))
                && contentType.StartsWith(MIMETypes.Application.Json) && content != null)
            {
                // Do a merge
                string newJson = Encoding.UTF8.GetString(content);
                JToken newDocument = JToken.Parse(newJson);

                // Merge property values
                var savedJson = Encoding.UTF8.GetString(entity.Content);
                var savedDocument = JToken.Parse(savedJson);
                // Merge if both values are Json Objects
                if (savedDocument.Type == JTokenType.Object && newDocument.Type == JTokenType.Object)
                {
                    (savedDocument as JObject).Merge(newDocument, new JsonMergeSettings()
                    {
                        MergeNullValueHandling = MergeNullValueHandling.Merge,
                        MergeArrayHandling = MergeArrayHandling.Replace
                    });

                    // Both are objects, and have been merged, now replace
                    entity.Content = Encoding.UTF8.GetBytes(savedDocument.ToString());
                }
                else
                {
                    // Both are not objects, just replace with new doc
                    entity.Content = content;
                }
            }
            else
            {
                entity.Content = content;
            }

            // Update etag as the last thing
            entity.ContentType = contentType;
            entity.Timestamp = DateTime.UtcNow;
            entity.ETag = ComputeHash(content);

            this.dbContext.Documents.Update((DocumentEntity)entity);
            dbContext.Entry((DocumentEntity)entity).State = EntityState.Modified;

            return entity;
        }

        #region Helpers
        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public string ComputeHash(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Compute hash for bytes
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public string ComputeHash(byte[] bytes)
        {
            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        private string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
        #endregion
    }
}
