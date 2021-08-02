using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TodoApp.Database.Models;

#nullable disable

namespace TodoApp.Database.Data
{
    public partial class TodoContext : DbContext
    {
        public TodoContext()
        {
        }

        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.ToTable("TodoItem");

                entity.HasIndex(e => e.UserId, "IX_TodoItem_UserId");

                entity.HasIndex(e => new { e.UserId, e.CreatedUnixTicks }, "IX_TodoItem_UserId_Created_UnixTicks");

                entity.HasIndex(e => new { e.UserId, e.DueUnixTicks }, "IX_TodoItem_UserId_Due_UnixTicks");

                entity.HasIndex(e => new { e.UserId, e.Title }, "IX_TodoItem_UserId_Title");

                entity.Property(e => e.CreatedUnixTicks).HasColumnName("Created_UnixTicks");

                entity.Property(e => e.CreatedZoneId)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Created_ZoneId");

                entity.Property(e => e.DueUnixTicks).HasColumnName("Due_UnixTicks");

                entity.Property(e => e.DueZoneId)
                    .HasMaxLength(256)
                    .HasColumnName("Due_ZoneId");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(1024);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
