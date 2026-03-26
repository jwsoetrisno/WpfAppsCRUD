using Microsoft.EntityFrameworkCore;
using WpfApps.Models;

namespace WpfApps.Data;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}