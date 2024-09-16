using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using System;
using System.Linq;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Client> Clients { get; set; }
    public DbSet<City> Citys { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<ItemOrder> ItensOrder { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Installment> Installments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    public override int SaveChanges()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseModel && 
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChanges();
    }
}
