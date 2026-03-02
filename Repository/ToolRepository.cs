using EFCoreDemo.Data;
using EFCoreDemo.Models;

namespace EFCoreDemo.Repository;

public class ToolRepository : ITool
{
    private readonly HardwareStoreContext _db;

    public ToolRepository(HardwareStoreContext db)
    {
        _db = db;
    }
    public void addTool(Tool newTool)
    {
        _db.Tools.Add(newTool);
        _db.SaveChanges();
    }

    public void addTools(List<Tool> newTools)
    {
        _db.Tools.AddRange(newTools);
        _db.SaveChanges();
    }
}