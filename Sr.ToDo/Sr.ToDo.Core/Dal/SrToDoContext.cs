using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sr.ToDo.Core.Dal
{
    public partial class SrToDoContext : DbContext
    {
        public SrToDoContext()
        {
        }

        public SrToDoContext(DbContextOptions<SrToDoContext> options)
            : base(options)
        {
        }
		public virtual DbSet<ToDo> ToDo { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=SrToDo;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<ToDo>(entity =>
			{
				entity.HasIndex(e => e.Name);
				
				entity.Property(e => e.Name).IsFixedLength();
			});

			OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
