namespace Doctrina.Persistence
{
    public class DoctrinaInitializer
    {
        public static void Initialize(DoctrinaDbContext context)
        {
            var initializer = new DoctrinaInitializer();
            initializer.SeedEverything(context);
        }

        public void SeedEverything(DoctrinaDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
