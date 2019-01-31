using System;

namespace Doctrina.Domain.Entities.Documents
{
    public class ActivityProfileEntity : IActivityProfileEntity
    {
        public Guid Key { get; set; }

        public string ProfileId { get; set; }

        public DateTime UpdateDate { get; set; }

        public string ActivityId { get; set; }

        public Guid? RegistrationId { get; set; }

        #region Navigation Properties
        public virtual ActivityEntity Activity { get; set; }

        public virtual DocumentEntity Document { get; set; }
        #endregion
    }
}
