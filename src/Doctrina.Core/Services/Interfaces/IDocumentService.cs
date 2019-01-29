using System;
using System.Net.Mime;
using Doctrina.Persistence.Entities;
using Doctrina.xAPI.Documents;

namespace Doctrina.Persistence.Services
{
    public interface IDocumentService
    {
        IDocumentEntity CreateDocument(string contentType, byte[] content);
        IDocumentEntity GetDocument(Guid documentId);

        string ComputeHash(byte[] buffer);
        IDocumentEntity UpdateDocument(IDocumentEntity entity, string contentType, byte[] content);

        void DeleteDocument(IDocumentEntity entity);
    }
}