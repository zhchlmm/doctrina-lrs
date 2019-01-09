using System;

namespace Doctrina.Core.Data.Documents
{
    public interface IActivityProfileEntity
    {
        ActivityEntity Activity { get; set; }
        Guid ActivityKey { get; set; }
        DocumentEntity Document { get; set; }
        Guid DocumentId { get; set; }
        Guid Key { get; set; }
        string ProfileId { get; set; }
        Guid? RegistrationId { get; set; }
        DateTime UpdateDate { get; set; }
    }
}