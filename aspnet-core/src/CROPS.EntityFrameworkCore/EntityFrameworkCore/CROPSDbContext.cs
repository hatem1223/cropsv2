using Abp.EntityFrameworkCore;
using CROPS.ALM;
using CROPS.Contracts;
using CROPS.Projects;
using CROPS.Reports;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CROPS.EntityFrameworkCore
{
    public class CROPSDbContext : AbpDbContext
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Account> Account { get; set; }

        public virtual DbSet<Assignee> Assignee { get; set; }

        public virtual DbSet<AssigneeLevel> AssigneeLevels { get; set; }

        public virtual DbSet<AssigneeRole> AssigneeRoles { get; set; }

        public virtual DbSet<Bug> Bug { get; set; }

        public virtual DbSet<Dashboard> Dashboard { get; set; }

        public virtual DbSet<Epic> Epic { get; set; }

        public virtual DbSet<Feature> Feature { get; set; }

        public virtual DbSet<Iteration> Iteration { get; set; }

        public virtual DbSet<Project> Project { get; set; }

        public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }

        public virtual DbSet<Report> Report { get; set; }

        public virtual DbSet<Task> Task { get; set; }

        public virtual DbSet<UserStory> UserStory { get; set; }

        public virtual DbSet<Workspace> Workspace { get; set; }

        public virtual DbSet<UserFavorite> UserFavorite { get; set; }

        public CROPSDbContext(DbContextOptions<CROPSDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // foreach (var property in typeof(IHasName).GetProperties())
            //  modelBuilder.Types().Configure(m => m.Ignore(property.Name));

            var propertyNames = typeof(IHasName).GetProperties().Select(p => p.Name).ToList();

            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(t => typeof(IHasName).IsAssignableFrom(t.ClrType));
            foreach (var entityType in entityTypes)
            {
                var entityTypeBuilder = modelBuilder.Entity(entityType.ClrType);
                foreach (var propertyName in propertyNames)
                {
                    entityTypeBuilder.Ignore(propertyName);
                }
            }
        }
    }
}
