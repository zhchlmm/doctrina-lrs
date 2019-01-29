using Doctrina.xAPI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Doctrina.xAPI.LRS.Models
{
    public class ActivityProfileModel
    {
        [Required]
        [FromQuery(Name ="activityId")]
        public Iri ActivityId { get; set; }

        [Required]
        [FromQuery(Name = "profileId")]
        public string ProfileId { get; set; }

        [Required]
        [FromBody]
        public string Document { get; set; }

        [FromQuery(Name = "registration")]
        public Guid? Registration { get; set; }

        [FromHeader(Name = "Content-Type")]
        public string ContentType { get; set; }
    }
}
