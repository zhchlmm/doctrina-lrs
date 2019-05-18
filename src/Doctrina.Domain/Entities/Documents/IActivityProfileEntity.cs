using System;

namespace Doctrina.Domain.Entities.Documents
{
    public interface IActivityProfileEntity
    {
        ActivityEntity Activity { get; set; }

        string ActivityHash { get; set; }

        string ProfileId { get; set; }

        Guid? RegistrationId { get; set; }
    }
}