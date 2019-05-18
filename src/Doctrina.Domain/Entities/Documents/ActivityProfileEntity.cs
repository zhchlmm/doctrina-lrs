using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityProfileEntity : DocumentBaseEntity, IActivityProfileEntity, IDocumentEntity
    {
        public ActivityProfileEntity()
        {
        }

        public ActivityProfileEntity(byte[] content, string contentType) : base(content, contentType)
        {
        }

        public string ProfileId { get; set; }

        /// <summary>
        /// MD5 checksum of Iri
        /// </summary>
        public string ActivityHash { get; set; }

        public Guid? RegistrationId { get; set; }

        #region Navigation Properties
        public virtual ActivityEntity Activity { get; set; }
        #endregion
    }
}
