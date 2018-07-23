using Doctrina.Core.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace Doctrina.Core
{
    public partial class DoctrinaDbContext : DbContext
    {
        public DbSet<VerbEntity> Verbs { get; set; }
        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<AgentEntity> Agents { get; set; }
        public DbSet<GroupMemberEntity> GroupMembers { get; set; }
        public DbSet<StatementEntity> Statements { get; set; }
        public DbSet<ActivityProfileEntity> ActivityProfiles { get; set; }
        public DbSet<AgentProfileEntity> AgentProfiles { get; set; }
        public DbSet<ActivityStateEntity> ActivityStates { get; set; }
        public DbSet<DocumentEntity> Documents { get; set; }

        //public DbSet<ActivityProfile> ActivityProfiles { get; set; }

        public DoctrinaDbContext(DbContextOptions<DoctrinaDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Agent Indexes
            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.Mbox)
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.Mbox_SHA1SUM)
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.OpenId)
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(x => x.Account_Name)
                .IsUnique();

            modelBuilder.Entity<AgentEntity>()
                .HasIndex(agent => new { agent.Account_HomePage, agent.Account_Name });

            #endregion

            //this.FixOnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

}
