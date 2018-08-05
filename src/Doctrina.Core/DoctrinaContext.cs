using Doctrina.Core.Data;
using Doctrina.Core.Data.Documents;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Core
{
    public partial class DoctrinaContext : DbContext
    {
        public DbSet<DoctrinaUser> Users { get; set; }
        public DbSet<VerbEntity> Verbs { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<AgentEntity> Agents { get; set; }
        public DbSet<GroupMemberEntity> GroupMembers { get; set; }
        public DbSet<StatementEntity> Statements { get; set; }
        public DbSet<ContextEntity> StatementContexts { get; set; }
        public DbSet<AgentProfileEntity> AgentProfiles { get; set; }
        public DbSet<ActivityProfileEntity> ActivityProfiles { get; set; }
        public DbSet<ActivityStateEntity> ActivityStates { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }

        //public DbSet<ActivityProfile> ActivityProfiles { get; set; }

        public DoctrinaContext(DbContextOptions<DoctrinaContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Agent Indexes
            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.Mbox)
                .HasFilter("[Mbox] IS NOT NULL")
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.Mbox_SHA1SUM)
                .HasFilter("[Mbox_SHA1SUM] IS NOT NULL")
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.OpenId)
                .HasFilter("[OpenId] IS NOT NULL")
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(agent => new { agent.Account_HomePage, agent.Account_Name })
                .HasFilter("[Account_HomePage] IS NOT NULL AND [Account_Name] IS NOT NULL")
                .IsUnique();

            #endregion

            #region Activity
            modelBuilder.Entity<ActivityEntity>()
                .HasIndex(ac => ac.ActivityId);
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
