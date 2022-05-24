using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<StudentEvent> EventParticipations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Image> Images { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);

            builder.Entity<StudentCommunity>()
                .HasKey(sc => new { sc.StudentId, sc.CommunityId });  
            builder.Entity<StudentCommunity>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.Communities)
                .HasForeignKey(sc => sc.StudentId);  
            builder.Entity<StudentCommunity>()
                .HasOne(sc => sc.Community)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.CommunityId);

            builder.Entity<StudentEvent>()
                .HasKey(se => new { se.StudentId, se.EventId });  
            builder.Entity<StudentEvent>()
                .HasOne(se => se.Student)
                .WithMany(s => s.Events)
                .HasForeignKey(se => se.StudentId);  
            builder.Entity<StudentEvent>()
                .HasOne(se => se.Event)
                .WithMany(e => e.Students)
                .HasForeignKey(se => se.EventId);
        }
    }
}
