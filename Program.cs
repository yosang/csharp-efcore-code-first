using System.Text;
using EFCoreDemo.Data;
using EFCoreDemo.Models;
using EFCoreDemo.Repository;
using Microsoft.EntityFrameworkCore;
public class Program
{
    public static void Main()
    {
        using var db = new HardwareStoreContext();
        db.Database.EnsureCreated();

        BrandRepository brandRepo = new BrandRepository(db);
        CategoryRepository categoryRepo = new CategoryRepository(db);
        ToolRepository toolRepo = new ToolRepository(db);

        brandRepo.seedDefaultBrands();
        categoryRepo.seedDefaultCategories();

        toolRepo.addTools(new List<Tool>()
        {
            new Tool() { ID=Guid.NewGuid().ToString(), Name = "Hammer", Price = 99.9, BrandID = 1, CategoryID = 1},
            new Tool() { ID=Guid.NewGuid().ToString(), Name = "Drill", Price = 99.9, BrandID = 1, CategoryID = 2},
        });

    }

}