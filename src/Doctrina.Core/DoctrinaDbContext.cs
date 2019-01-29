using Doctrina.Domain.Entities;
using Doctrina.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Persistence
{
    public partial class DoctrinaDbContext : DbContext
    {
        public DbSet<DoctrinaUser> Users { get; set; }
        public DbSet<VerbEntity> Verbs { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<AgentEntity> Agents { get; set; }
        public DbSet<GroupMemberEntity> GroupMembers { get; set; }
        public DbSet<StatementEntity> Statements { get; set; }
        public DbSet<SubStatementEntity> SubStatements { get; set; }
        public DbSet<ContextEntity> StatementContexts { get; set; }
        public DbSet<ContextActivitiesEntity> ContextActivities { get; set; }
        public DbSet<AgentProfileEntity> AgentProfiles { get; set; }
        public DbSet<ActivityProfileEntity> ActivityProfiles { get; set; }
        public DbSet<ActivityStateEntity> ActivityStates { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }
        public DbSet<ResultEntity> Results { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }

        //public DbSet<ActivityProfile> ActivityProfiles { get; set; }

        public DoctrinaDbContext(DbContextOptions<DoctrinaDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();

            #region Activity
            
            #endregion

            //this.FixOnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

}
