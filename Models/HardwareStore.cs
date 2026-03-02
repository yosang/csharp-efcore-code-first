namespace EFCoreDemo.Models;

public class Tool
{
    public int ID { get; set; }// This is the primary key attribute
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }

    // Explicit FK's: used when creating / updating
    public int BrandID { get; set; } // This is a explicit foreign key attribute, so we can add Brands by int its int ID
    public int CategoryID { get; set; }

    // Navigation properties: used when reading / querying
    public virtual Brand? Brand { get; set; }
    public virtual Category? Category { get; set; }
}
public class Brand
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Tool>? Tools { get; set; }
}

public class Category
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual List<Tool>? Tools { get; set; }
}

