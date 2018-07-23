using Doctrina.Core.Persistence.Models;
using Doctrina.Core.Repositories;
using Newtonsoft.Json;
using System;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public class VerbService : IVerbService
    {
        private readonly VerbRepository Verbs;
        private readonly IVerbRepository verbRepository;

        public VerbService(DoctrinaDbContext dbContext, IVerbRepository verbRepository)
        {
            this.verbRepository = verbRepository;
        }

        public VerbEntity MergeVerb(Verb verb)
        {
            if (verb == null)
                throw new ArgumentNullException("verb");

            var curr = this.Verbs.GetByVerbId(verb.Id);
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
                    CanonicalData = verb.Display.ToJson(),
                    VerbId = verb.Id.ToString()
                };
                this.Verbs.CreateVerb(curr);
            }

            return curr;
        }
    }
}
