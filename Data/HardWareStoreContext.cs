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

    // Connction configurationa
    // This method is called for each instnace of HardwareStoreContext, and configures the database
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Provides connection details for a MySQL server
        optionsBuilder.UseMySQL("server=localhost;database=testdb;user=testuser;password=p@ssword");

        // optionsBuilder.LogTo(Console.WriteLine); // Enables logging
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
            new Brand { ID = 1, Name = "DeWalt" },
            new Brand { ID = 2, Name = "Milwaukee" },
            new Brand { ID = 3, Name = "Makita" },
            new Brand { ID = 4, Name = "Bosch" },
            new Brand { ID = 5, Name = "Ryobi" },
            new Brand { ID = 6, Name = "Craftsman" },
            new Brand { ID = 7, Name = "Ridgid" },
            new Brand { ID = 8, Name = "Black+Decker" },
        };

        List<Category> categories = new()
        {
            new Category { ID = 1, Name = "Hand Tools" },
            new Category { ID = 2, Name = "Power Drills" },
            new Category { ID = 3, Name = "Saws" },
            new Category { ID = 4, Name = "Sanders & Grinders" },
            new Category { ID = 5, Name = "Measuring Tools" },
            new Category { ID = 6, Name = "Fastening Tools" },
        };

        List<Tool> tools = new()
        {
        // Hand Tools (Category 1)
        new Tool { ID = 1,  Name = "Claw Hammer 16oz",               Price = 24.99,  BrandID = 1, CategoryID = 1 },
        new Tool { ID = 2,  Name = "Claw Hammer 20oz",               Price = 34.99,  BrandID = 2, CategoryID = 1 },
        new Tool { ID = 3,  Name = "Adjustable Wrench",         Price = 19.99,  BrandID = 6, CategoryID = 1 },
        new Tool { ID = 4,  Name = "Pliers Set (3-piece)",           Price = 39.99,  BrandID = 4, CategoryID = 1 },
        new Tool { ID = 5,  Name = "Screwdriver Set (8-piece)",      Price = 29.99,  BrandID = 8, CategoryID = 1 },
        new Tool { ID = 6,  Name = "Utility Knife Folding",          Price = 14.99,  BrandID = 5, CategoryID = 1 },

        // Power Drills (Category 2)
        new Tool { ID = 7,  Name = "20V Cordless Drill/Driver",      Price = 129.00, BrandID = 1, CategoryID = 2 },
        new Tool { ID = 8,  Name = "M18 Fuel Hammer Drill",          Price = 229.00, BrandID = 2, CategoryID = 2 },
        new Tool { ID = 9,  Name = "18V LXT Brushless Drill",        Price = 189.00, BrandID = 3, CategoryID = 2 },
        new Tool { ID = 10, Name = "18V Compact Drill",              Price = 99.00,  BrandID = 4, CategoryID = 2 },
        new Tool { ID = 11, Name = "ONE+ 18V Cordless Drill",        Price = 79.00,  BrandID = 5, CategoryID = 2 },

        // Saws (Category 3)
        new Tool { ID = 12, Name = "7-1/4 Circular Saw",             Price = 149.00, BrandID = 1, CategoryID = 3 },
        new Tool { ID = 13, Name = "M18 Fuel Circular Saw 7-1/4",    Price = 279.00, BrandID = 2, CategoryID = 3 },
        new Tool { ID = 14, Name = "6-1/2 Circular Saw Compact",     Price = 119.00, BrandID = 3, CategoryID = 3 },
        new Tool { ID = 15, Name = "Jigsaw Variable Speed",          Price = 89.99,  BrandID = 4, CategoryID = 3 },
        new Tool { ID = 16, Name = "Reciprocating Saw 18V",          Price = 129.00, BrandID = 5, CategoryID = 3 },

        // Sanders & Grinders (Category 4)
        new Tool { ID = 17, Name = "Random Orbital Sander",      Price = 79.99,  BrandID = 1, CategoryID = 4 },
        new Tool { ID = 18, Name = "M18 Orbital Sander",             Price = 149.00, BrandID = 2, CategoryID = 4 },
        new Tool { ID = 19, Name = "4-1/2 Angle Grinder",            Price = 69.99,  BrandID = 4, CategoryID = 4 },
        new Tool { ID = 20, Name = "Cordless Angle Grinder 18V",     Price = 119.00, BrandID = 3, CategoryID = 4 },

        // Measuring Tools (Category 5)
        new Tool { ID = 21, Name = "25ft Tape Measure",              Price = 19.99,  BrandID = 1, CategoryID = 5 },
        new Tool { ID = 22, Name = "Laser Distance Measurer 165ft",  Price = 89.99,  BrandID = 4, CategoryID = 5 },
        new Tool { ID = 23, Name = "Digital Caliper",            Price = 39.99,  BrandID = 6, CategoryID = 5 },

        // Fastening Tools (Category 6)
        new Tool { ID = 24, Name = "Impact Driver 20V",              Price = 149.00, BrandID = 1, CategoryID = 6 },
        new Tool { ID = 25, Name = "M18 Fuel Impact Driver",         Price = 199.00, BrandID = 2, CategoryID = 6 },
        new Tool { ID = 26, Name = "Brad Nailer 18Ga Cordless",      Price = 269.00, BrandID = 3, CategoryID = 6 },
        new Tool { ID = 27, Name = "18V Cordless Stapler",           Price = 99.00,  BrandID = 5, CategoryID = 6 },
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