using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Models;

namespace ManagementEmployeeEnterprise.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<FraudAlert> FraudAlerts { get; set; }
        public DbSet<WorkspaceBooking> WorkspaceBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Department Configuration
            builder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);
                entity.Property(e => e.DepartmentId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Employee Configuration
            builder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId);
                entity.Property(e => e.EmployeeId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.IdentityUserId).IsRequired().HasMaxLength(450);
                entity.Property(e => e.BaseLocationLat).HasColumnType("decimal(9, 6)");
                entity.Property(e => e.BaseLocationLon).HasColumnType("decimal(9, 6)");
                
                entity.HasOne(e => e.Department)
                      .WithMany()
                      .HasForeignKey(e => e.DepartmentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Workspace Configuration
            builder.Entity<Workspace>(entity =>
            {
                entity.HasKey(e => e.WorkspaceId);
                entity.Property(e => e.WorkspaceId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CostPerSqFt).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });

            // Inside OnModelCreating:
            builder.Entity<WorkspaceBooking>(entity =>
            {
                entity.HasKey(e => e.BookingId);
                entity.Property(e => e.BookingId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.TimeSlot).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).HasDefaultValue("Confirmed").HasMaxLength(20);

                entity.HasOne(e => e.Workspace)
                      .WithMany()
                      .HasForeignKey(e => e.WorkspaceId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Employee)
                      .WithMany()
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CheckIn Configuration
            builder.Entity<CheckIn>(entity =>
            {
                entity.HasKey(e => e.CheckInId);
                entity.Property(e => e.CheckInId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.LocationLat).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(e => e.LocationLon).HasColumnType("decimal(9, 6)").IsRequired();
                entity.Property(e => e.CheckInMethod).IsRequired().HasMaxLength(20);
                entity.Property(e => e.IsFlagged).HasDefaultValue(false);

                entity.HasOne(e => e.Employee)
                      .WithMany(emp => emp.CheckIns)
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // FraudAlert Configuration
            builder.Entity<FraudAlert>(entity =>
            {
                entity.HasKey(e => e.AlertId);
                entity.Property(e => e.AlertId).HasDefaultValueSql("NEWID()");
                entity.Property(e => e.RuleName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Status).HasDefaultValue("Pending").HasMaxLength(20);
                
                entity.HasOne(e => e.CheckIn)
                      .WithOne(c => c.FraudAlert)
                      .HasForeignKey<FraudAlert>(e => e.CheckInId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
