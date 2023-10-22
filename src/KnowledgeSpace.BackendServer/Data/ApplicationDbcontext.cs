using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.BackendServer.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KnowledgeSpace.BackendServer.Data
{
    public class ApplicationDbcontext : IdentityDbContext<User,Role,string>
    {
        public ApplicationDbcontext(DbContextOptions option) : base(option)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);

            builder.Entity<Role>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);

            builder.Entity<CommandInFunction>().HasKey(x => new { x.CommandId, x.FunctionId });
            builder.Entity<CommandInFunction>().HasOne(x => x.Function).WithMany(x => x.CommandInFunctions).HasForeignKey(x => x.FunctionId);
            builder.Entity<CommandInFunction>().HasOne(x => x.Command).WithMany(x => x.CommandInFunctions).HasForeignKey(x => x.CommandId);

            builder.Entity<Vote>().HasKey(x => new { x.KnowledgeBaseId, x.UserId });
            builder.Entity<Vote>().HasOne(x => x.User).WithMany(x => x.Votes).HasForeignKey(x => x.UserId);
            builder.Entity<Vote>().HasOne(x => x.KnowledgeBase).WithMany(x => x.Votes).HasForeignKey(x => x.KnowledgeBaseId);

            builder.Entity<LabelInKnowledgeBase>().HasKey(x => new { x.KnowledgeBaseId, x.LabelId });
            builder.Entity<LabelInKnowledgeBase>().HasOne(x => x.Label).WithMany(x => x.LabelInKnowledgeBases).HasForeignKey(x => x.LabelId);
            builder.Entity<LabelInKnowledgeBase>().HasOne(x => x.KnowledgeBase).WithMany(x => x.LabelInKnowledgeBases).HasForeignKey(x => x.KnowledgeBaseId);
            
            builder.Entity<Permission>().HasKey(x => new { x.FunctionId, x.CommandId, x.RoleId });
            builder.Entity<Permission>().HasOne(x => x.Command).WithMany(x => x.Permissions).HasForeignKey(x => x.CommandId);
            builder.Entity<Permission>().HasOne(x => x.Role).WithMany(x => x.Permissions).HasForeignKey(x => x.RoleId);
            builder.Entity<Permission>().HasOne(x => x.Function).WithMany(x => x.Permissions).HasForeignKey(x => x.FunctionId);

            builder.Entity<Function>().Property(x => x.ParentId).IsRequired(false);
            builder.Entity<Function>().Property(x => x.Icon).IsRequired(false);

            builder.Entity<ActivityLog>().HasOne(x => x.User).WithMany(x => x.ActivityLogs).HasForeignKey(x => x.UserId);
            builder.Entity<KnowledgeBase>().HasOne(x => x.Category).WithMany(x => x.KnowledgeBases).HasForeignKey(x => x.CategoryId);
            builder.Entity<Comment>().HasOne(x => x.KnowledgeBase).WithMany(x => x.Comments).HasForeignKey(x => x.KnowledgeBaseId);
            builder.Entity<Report>().HasOne(x => x.KnowledgeBase).WithMany(x => x.Reports).HasForeignKey(x => x.KnowledgeBaseId);
            builder.Entity<Attachment>().HasOne(x => x.KnowledgeBase).WithMany(x => x.Attachments).HasForeignKey(x => x.KnowledgeBaseId);
            builder.Entity <Comment>().Property(x => x.OwnerUserId).IsRequired(false);
            
            builder.HasSequence("KnowledgeBaseSequence");

            builder.Seed();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IDateTracking changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreateDate = DateTime.Now;
                    }
                    else
                    {
                        changedOrAddedItem.LastModifiedDate = DateTime.Now;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<CommandInFunction> CommandInFunctions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBases { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<LabelInKnowledgeBase> LabelInKnowledgeBases { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
