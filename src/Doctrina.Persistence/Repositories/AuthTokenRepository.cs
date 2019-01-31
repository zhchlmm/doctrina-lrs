using System.Linq;

namespace Doctrina.Persistence.Repositories
{
    public class AuthTokenRepository
    {
        private readonly DoctrinaDbContext dbContext;

        public AuthTokenRepository(DoctrinaDbContext dbContext)
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
