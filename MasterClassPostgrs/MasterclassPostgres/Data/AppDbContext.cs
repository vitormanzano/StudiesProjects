using MasterclassPostgres.Models;
using Microsoft.EntityFrameworkCore;

namespace MasterclassPostgres.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly); //Procura todas as configs do EF no Program
    }
}