using Microsoft.EntityFrameworkCore;

namespace YourProjectNamespace.Models
{
    public class PhongUserDbContext : DbContext
    {
        public PhongUserDbContext(DbContextOptions<PhongUserDbContext> options)
            : base(options)
        {
        }

        public DbSet<Phong> Phongs { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Đặt tên bảng chính xác
            modelBuilder.Entity<Phong>().ToTable("Phong");
            modelBuilder.Entity<User>().ToTable("User"); // Chú ý: "User" không có "s"
        }
    }
}
