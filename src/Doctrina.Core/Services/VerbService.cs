using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Newtonsoft.Json;
using System;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public class VerbService : IVerbService
    {
        private readonly IVerbRepository _verbsRepo;

        public VerbService(DoctrinaContext dbContext, IVerbRepository verbRepository)
        {
            _verbsRepo = verbRepository;
        }

        public VerbEntity MergeVerb(Verb verb)
        {
            if (verb == null)
                throw new ArgumentNullException("verb");

            var curr = _verbsRepo.GetByVerbId(verb.Id);
            if(curr != null)
            {
                // Update canonical data
                var jsonString = curr.CanonicalData;
                var langMaps = JsonConvert.DeserializeObject<LanguageMap>(jsonString);
                // TODO: Update maps
            }
            else
            {
                curr = new VerbEntity()
                {
                    Key = Guid.NewGuid(),
                    CanonicalData = verb.Display.ToJson(),
                    Id = verb.Id.ToString()
                };
                this._verbsRepo.CreateVerb(curr);
            }

            return curr;
        }
    }
}
