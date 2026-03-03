using EFCoreDemo.Models;

namespace EFCoreDemo.Repository;

public interface IToolRepository
{
    #region CRUD methods
    // Here we define the CRUD capabilities of our repository
    // Must be implemented by the repository class

    // CREATE
    void AddTool(Tool tool);
    void AddTools(List<Tool> tools);

    // UPDATE
    void UpdateTool(int toolId, Tool updatedTool);

    // READ
    Tool? GetToolById(int id);
    List<Tool>? GetAllTools();
    List<Tool>? GetToolsForBrand(int brandId);

    // DELETE
    void DeleteTool(Tool tool);
    void DeleteToolById(int id);
    void DeleteTools(List<Tool> tools);

    #endregion

    #region Linq methods

    // Filters
    List<Tool> GetToolsCheaperThan(double price);
    List<Tool> GetToolsByPriceRange(double min, double max);
    List<Tool> SearchToolsByName(string pattern); // Partial search

    // Sorting
    List<Tool> GetAllToolsByPriceSorted(bool descending = false);
    List<Tool> GetToolsForBrandSorted(int brandId, string sortBy = "Name");

    // Projection
    double GetAveragePriceForCategory(int categoryId);

    // Aggregates
    int CountToolsInCategory(int categoryId);
    Tool? GetMostExpensiveTool();

    // Pagination
    List<Tool> GetTopThreeMostExpensive();

    #endregion
}