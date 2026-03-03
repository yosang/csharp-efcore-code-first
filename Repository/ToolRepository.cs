using EFCoreDemo.Data;
using EFCoreDemo.Models;
using EFCoreDemo.Repository;
using Microsoft.EntityFrameworkCore;

public class ToolRepository : IToolRepository
{
    readonly HardwareStoreContext? _db;

    public ToolRepository(HardwareStoreContext db)
    {
        _db = db;
    }

    #region CRUD Methods

    // SQL Query equivalent: INSERT INTO Tools (...) VALUES (...)
    public void AddTool(Tool tool)
    {
        if (_db == null) return;

        _db.Add(tool);
        _db.SaveChanges();
    }

    // SQL Query equivalent: INSERT INTO Tools (...) VALUES (...)
    public void AddTools(List<Tool> tools)
    {
        if (_db == null) return;

        _db.AddRange(tools);
        _db.SaveChanges();
    }

    // SQL Query equivalent: UPDATE Tools SET ATTR1 = VAL, ATTR2 = VAL... WHERE ID = toolId
    public void UpdateTool(int toolId, Tool updatedTool)
    {
        if (_db == null) return;

        var tool = _db.Find<Tool>(toolId); // Finds an entity by its primary key (id) and starts tracking it
        if (tool == null) return;

        // DTO - Data Transfer Object
        // A DTO carries data, such as ID and values, however it is a detached simple object
        // An entity, however, is loaded from the DbContext and is tracked / attached for each DbContext session.

        // This marks the whole entity Modified, EF updates ALL columns, not just changed ones.
        // updatedTool.ID = toolId; // We must set the ID first
        // tool = updatedTool;

        // Here EFCore will only replace the diff
        tool.Name = updatedTool.Name;
        tool.Price = updatedTool.Price;
        tool.BrandID = updatedTool.BrandID;
        tool.CategoryID = updatedTool.CategoryID;

        _db.SaveChanges();

    }

    // SQL Query equivalent: SELECT * FROM Tools, LEFT JOIN Brands, LEFT JOIN categories...
    public List<Tool>? GetAllTools()
    {
        if (_db == null) return null;
        return _db.Tools.Include(t => t.Brand).Include(t => t.Category).ToList(); // Eager loading, using the nav props to include associated entities
    }

    // SQL Query equivalent: SELECT * FROM Tools WHERE ID = id;
    public Tool? GetToolById(int id)
    {
        if (_db == null) return null;

        return _db.Tools.Find(id);

    }

    // SQL Query equivalent: SELECT with JOIN and WHERE BrandId = brandid;
    public List<Tool>? GetToolsForBrand(int brandId)
    {
        if (_db == null) return null;

        return _db.Tools.Include(t => t.Brand).Where(t => t.BrandID == brandId).ToList();
    }

    public void DeleteTool(Tool tool)
    {
        if (_db == null) return;

        _db.Tools.Remove(tool);
        _db.SaveChanges();
    }

    // SQL Query equivalent: DELETE FROM Tools WHERE Id = id
    public void DeleteToolById(int id)
    {
        if (_db == null) return;

        Tool? tool = _db.Tools.Find(id);

        if (tool != null)
        {
            _db.Remove(tool);
            _db.SaveChanges();
        }
    }

    // SQL Query equivalent: DELETE FROM Tools WHERE ID IN (1, 2, 3);
    public void DeleteTools(List<Tool> tools)
    {
        if (_db == null) return;

        _db.RemoveRange(tools);
        _db.SaveChanges();
    }

    #endregion

    #region LINQ methods
    // Filters
    public List<Tool> GetToolsCheaperThan(double price)
    {
        if (_db == null) return new List<Tool>();

        return _db.Tools
                .Where(t => t.Price < price) // Filte rby price lower than price
                .OrderBy(t => t.Price) // Order by the price column
                .ToList(); // Convert to a list
    }
    public List<Tool> GetToolsByPriceRange(double min, double max)
    {
        if (_db == null) return new List<Tool>();
        return _db.Tools
                .Where(t => t.Price < max && t.Price > min)
                .OrderBy(t => t.Price)
                .ToList();
    }
    public List<Tool> SearchToolsByName(string pattern) // Partial search
    {
        if (_db == null || string.IsNullOrWhiteSpace(pattern)) return new List<Tool>();

        pattern = pattern.Trim().ToLower(); // Trim any potential spaces and make it case insensitive

        return _db.Tools
                .Where(t => EF.Functions.Like(t.Name.ToLower(), $"%{pattern}%")) // Similar to SQL LIKE = %..% - where the pattern can be in the middle
                .OrderBy(t => t.Price) // Order by price
                .ToList();
    }

    // Sorting

    public List<Tool> GetAllToolsByPriceSorted(bool descending = false)
    {
        if (_db == null) return new List<Tool>();

        var result = descending ? _db.Tools.OrderByDescending(t => t.Price).ToList() : _db.Tools.OrderBy(t => t.Price).ToList();

        return result;
    }
    public List<Tool> GetToolsForBrandSorted(int brandId, string sortBy = "Name")
    {
        if (_db == null) return new List<Tool>();

        var result = _db.Tools
                      .Include(t => t.Brand)
                      .Where(t => t.BrandID == brandId);

        // Here we can apply based on string passed
        result = sortBy?.Trim().ToLower() switch
        {
            "price" => result.OrderBy(t => t.Price),
            "price desc" => result.OrderByDescending(t => t.Price),
            "name desc" => result.OrderByDescending(t => t.Name),
            _ => result.OrderBy(t => t.Name)
        };

        return result.ToList();
    }
    // Projection

    public double GetAveragePriceForCategory(int categoryId)
    {
        if (_db == null) return 0;

        return _db.Tools
                .Where(t => t.CategoryID == categoryId) // First we join the tables
                .Average(t => t.Price); // Then we find the average price
    }

    // Aggregates
    public int CountToolsInCategory(int categoryId)
    {
        if (_db == null) return 0;
        return _db.Tools
                .Where(t => t.CategoryID == categoryId)
                .Count();
    }
    public Tool? GetMostExpensiveTool()
    {
        if (_db == null) return null;

        return _db.Tools
                .OrderByDescending(t => t.Price)
                .FirstOrDefault();
    }

    // Pagination
    public List<Tool> GetTopThreeMostExpensive()
    {
        if (_db == null) return new List<Tool>();
        return _db.Tools
                .OrderByDescending(t => t.Price)
                .Take(3) // Limits the result to 3 items
                .ToList();
    }
    #endregion
}