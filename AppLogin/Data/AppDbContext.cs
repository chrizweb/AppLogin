using AppLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLogin.Data {
	public class AppDbContext : DbContext {
		public AppDbContext(DbContextOptions<AppDbContext> options):base(options) {

		}

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(tb => {
				tb.HasKey(col => col.UserId);
				tb.Property(col => col.UserId)
				.UseIdentityColumn()
				.ValueGeneratedOnAdd();

				tb.Property(col => col.Name).HasMaxLength(50);
				tb.Property(col => col.Email).HasMaxLength(50);
				tb.Property(col => col.Password).HasMaxLength(50);
			});

			modelBuilder.Entity<User>().ToTable("User");
		}
	}
}
