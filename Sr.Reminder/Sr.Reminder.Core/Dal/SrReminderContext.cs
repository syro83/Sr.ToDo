using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sr.Reminder.Core.Dal
{
    public partial class SrReminderContext : DbContext
    {
        public SrReminderContext()
        {
        }

        public SrReminderContext(DbContextOptions<SrReminderContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Reminder> Reminder { get; set; }
        public virtual DbSet<ReminderCategory> ReminderCategory { get; set; }
        public virtual DbSet<Task> Task { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=SrReminder;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<Reminder>(entity =>
            {
                entity.HasIndex(e => e.Name);

                entity.HasIndex(e => new { e.DueDate, e.Priority })
                    .HasName("IX_Reminder_DueDate");

                entity.Property(e => e.Name).IsFixedLength();
            });

            modelBuilder.Entity<ReminderCategory>(entity =>
            {
                entity.HasKey(e => new { e.ReminderId, e.CategoryId });

                entity.HasIndex(e => new { e.CategoryId, e.ReminderId })
                    .HasName("IX_ReminderCategory");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ReminderCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReminderCategory_Category");

                entity.HasOne(d => d.Reminder)
                    .WithMany(p => p.ReminderCategory)
                    .HasForeignKey(d => d.ReminderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReminderCategory_Reminder");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasIndex(e => e.ReminderId)
                    .HasName("IX_Task");

                entity.HasOne(d => d.Reminder)
                    .WithMany(p => p.Task)
                    .HasForeignKey(d => d.ReminderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Reminder");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
