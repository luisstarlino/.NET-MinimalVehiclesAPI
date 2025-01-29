using Microsoft.EntityFrameworkCore;


public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
        
    }

    public DbSet<Administrator> Administrators { get; set; } = default!;

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     if (!optionsBuilder.IsConfigured)
    //     {
    //         var connectionString = _appSettingsConfig.GetConnectionString("myql")?.ToString();

    //         if (!string.IsNullOrEmpty(connectionString))
    //         {
    //             optionsBuilder.UseMySql(
    //                 connectionString,
    //                 ServerVersion.AutoDetect(connectionString)
    //             );
    //         }
    //     }
    // }

}