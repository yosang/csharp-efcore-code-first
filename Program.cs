using EFCoreDemo.Data;
using EFCoreDemo.Models;
public class Program
{
    public static void Main()
    {
        using var db = new HardwareStoreContext(); // Once we create a new instance of DbContext, everything should be configured
        db.Database.EnsureCreated(); // EFCore ensures tables are created if the database is empty of tables

        ToolRepository toolRepo = new(db);

        #region CRUD operations
        // Create - Adding a new tool
        toolRepo.AddTool(new Tool() { Name = "Circle Saw", Price = 99.9, BrandID = 1, CategoryID = 2 });

        // Create - Adding multiple tools
        toolRepo.AddTools(new List<Tool>()
        {
            new Tool() { Name = "Screwdriver", Price = 99.99, BrandID = 6, CategoryID = 1 },
            new Tool() { Name = "Umbracco", Price = 99.99, BrandID = 6, CategoryID = 1 },
        });

        // Read - Get a tool/'s
        Tool? umbracco = toolRepo.GetToolById(5);
        // List<Tool>? tools = toolRepo.GetAllTools();
        // if (tools != null)
        // {
        //     foreach (var t in tools)
        //     {
        //         Console.WriteLine(t.ID);
        //         Console.WriteLine(t.Name);
        //         Console.WriteLine(t.Brand?.Name);
        //         Console.WriteLine(t.Category?.Name);
        //     }
        // }

        // Read - Get Tools for a specific Brand
        List<Tool>? tools = toolRepo.GetToolsForBrand(2);
        // if (tools != null)
        // {
        //     foreach (var t in tools)
        //     {
        //         Console.WriteLine(t.ID);
        //         Console.WriteLine(t.Name);
        //     }
        // }

        // Update a tool
        if (umbracco != null)
        {
            umbracco.Name = "Umbracco key";
            toolRepo.UpdateTool(5, umbracco);
        }

        // Delete a tool
        if (umbracco != null)
        {
            toolRepo.DeleteTool(umbracco);
        }

        // delete by id
        toolRepo.DeleteToolById(1);

        // Multiple deletes
        Tool? drill = toolRepo.GetToolById(2);
        Tool? circleSaw = toolRepo.GetToolById(3);

        if (drill != null && circleSaw != null)
        {
            toolRepo.DeleteTools(new List<Tool>()
        {
            drill,
            circleSaw
        });
        }
        ;
        #endregion

        #region LINQ methods

        // var cheapTools = toolRepo.GetToolsCheaperThan(50.20);
        // foreach (var t in cheapTools) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var toolsByPriceRange = toolRepo.GetToolsByPriceRange(0, 20);
        // foreach (var t in toolsByPriceRange) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var toolsByPartialName = toolRepo.SearchToolsByName("saw");
        // foreach (var t in toolsByPartialName) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var toolsSortedAsc = toolRepo.GetAllToolsByPriceSorted();
        // foreach (var t in toolsSortedAsc) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var toolsSortedDesc = toolRepo.GetAllToolsByPriceSorted(true);
        // foreach (var t in toolsSortedDesc) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var toolsForBrandSortedByName = toolRepo.GetToolsForBrandSorted(1);
        // var toolsForBrandSortedByNameDesc = toolRepo.GetToolsForBrandSorted(1, "name desc");
        // var toolsForBrandSortedByPrice = toolRepo.GetToolsForBrandSorted(1, "price");
        // var toolsForBrandSortedByPriceDesc = toolRepo.GetToolsForBrandSorted(1, "price desc");
        // foreach (var t in toolsForBrandSortedByName) Console.WriteLine($"Id: {t.ID} - Name: {t.Name} - Price: {t.Price}");

        // var averagePriceForCategory = toolRepo.GetAveragePriceForCategory(1);
        // Console.WriteLine((decimal)averagePriceForCategory); // Here we are casting it to a decimal

        // var totalToolsInCategory = toolRepo.CountToolsInCategory(1);
        // Console.WriteLine(totalToolsInCategory);

        // var mostExpensiveTool = toolRepo.GetMostExpensiveTool();
        // Console.WriteLine($"Name: {mostExpensiveTool?.Name} - Price {mostExpensiveTool?.Price}");

        var topThree = toolRepo.GetTopThreeMostExpensive();
        foreach (var t in topThree) Console.WriteLine($"Name {t.Name} - Price {t.Price}");

        #endregion
    }

}