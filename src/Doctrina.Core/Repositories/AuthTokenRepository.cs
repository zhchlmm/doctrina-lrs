using System.Linq;

namespace Doctrina.Core.Repositories
{
    public class AuthTokenRepository
    {
        private readonly DoctrinaContext dbContext;

        public AuthTokenRepository(DoctrinaContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //internal AuthTokenEntity GetByApiUser(string apiUser)
        //{
        //    var match = this.Where(x => x.APIUser == apiUser).FirstOrDefault();
        //    return match;
        //}
    }
}
