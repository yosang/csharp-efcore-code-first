using EFCoreDemo.Models;

namespace EFCoreDemo.Repository;

public interface IToolRepository
{
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
}