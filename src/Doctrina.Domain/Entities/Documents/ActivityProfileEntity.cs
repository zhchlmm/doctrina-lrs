using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityProfileEntity : IActivityProfileEntity
    {
        public Guid Key { get; set; }

        public string ProfileId { get; set; }

        public DateTime UpdateDate { get; set; }

        public Guid ActivityKey { get; set; }

        public Guid? RegistrationId { get; set; }

        public Guid DocumentId { get; set; }

        #region Navigation Properties
        public virtual ActivityEntity Activity { get; set; }

        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
