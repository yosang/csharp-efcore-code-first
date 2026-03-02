// This class inherits from DbContext class that comes from Microsoft.EntityFrameworkCore

using EFCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace EFCoreDemo.Data;

// This class inhertis from DbContext
// A DbContext instance represents a session with the database
// It allows us to query and save instances of entities.
// 1. We first set the models using DbSet<TEntity> for each model
// 2. We then override the DbContext.OnConfiguring method to configure the database
// 3. The models are then automatically discovered however, 
// for further configuration of models, we can override the DbContext.OnConfiguring method
public class HardwareStoreContext : DbContext
{
    public DbSet<Tool> Tools { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }

    // Connction configuration
    // This method is called for each instnace of HardwareStoreContext, and configures the database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Provides connection details for a MySQL server
        optionsBuilder.UseMySQL("server=localhost;database=testdb;user=testuser;password=p@ssword");
    }

    // Allows us to furhter configure the models with specific constraints such as:
    // Explicitly using a specific property as Primary Key
    // Make properties required / max length / column name
    // Configure relationships (HasOne / WithMany / HasForeignKey)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Initial data seeds
        List<Brand> brands = new()
        {
            new Brand() { ID = 1, Name = "Bosch"},
            new Brand() { ID = 2, Name = "Samsung"}
        };

        List<Category> categories = new()
        {
          new Category() { ID = 1, Name = "Hand Tool"},
          new Category() { ID = 2, Name = "Power Tool"},
        };

        List<Tool> tools = new()
        {
         new Tool() { ID = 1, Name = "Hammer", Price = 99.9, BrandID = 1, CategoryID = 1},
            new Tool() { ID = 2, Name = "Drill", Price = 99.9, BrandID = 1, CategoryID = 2},
        };

        // Entity Configuration
        modelBuilder.Entity<Brand>(e =>
        {
            e.HasKey(e => e.ID); // Here we are explictly requiring the brand entity to have an id (PK)
            e.Property(e => e.Name).IsRequired(); // A Brand name must be set
            e.HasData(brands); // Seeds the Brands table
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(e => e.ID);
            e.Property(e => e.Name).IsRequired();
            e.HasData(categories);
        });

        modelBuilder.Entity<Tool>(e =>
        {
            e.HasKey(e => e.ID);
            e.Property(e => e.Name).IsRequired();
            e.Property(e => e.Price).IsRequired();

            // Here we are explicitly making this a one-to-many relationship
            e.HasOne(e => e.Brand) // Tool belongs to one Brand
            .WithMany(e => e.Tools) // Brands can have many Tools
            .HasForeignKey(e => e.BrandID); // Specifies which key we want to use as Foreign Key

            e.HasOne(e => e.Category)
            .WithMany(e => e.Tools)
            .HasForeignKey(e => e.CategoryID);

            e.HasData(tools); // Finally seed the tools, they must be seeded last because Brands and Categories must exist first (Tools depend on foreign keys)

        });


    }
}