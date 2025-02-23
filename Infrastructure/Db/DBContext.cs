using _NET_MinimalAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;


public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<Administrator> Administrators { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>().HasData(
            new Administrator{
                Id = 1,
                Mail = "admin@test.com",
                Password = "123456",
                Profile = "adm"
            }
        );
    }

}