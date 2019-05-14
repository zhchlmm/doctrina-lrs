using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityProfileEntity : IActivityProfileEntity
    {
        public Guid Key { get; set; }

        public string ProfileId { get; set; }

        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// MD5 checksum of Iri
        /// </summary>
        public string ActivityEntityId { get; set; }

        public Guid? RegistrationId { get; set; }

        public DocumentEntity Document { get; set; }


        #region Navigation Properties
        public virtual ActivityEntity Activity { get; set; }
        #endregion
    }
}
