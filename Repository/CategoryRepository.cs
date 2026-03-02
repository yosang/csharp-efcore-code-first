using EFCoreDemo.Repository;
using EFCoreDemo.Models;
using EFCoreDemo.Data;
public class CategoryRepository : ICategory
{
    private readonly HardwareStoreContext _db;

    public CategoryRepository(HardwareStoreContext db)
    {
        _db = db;
    }
    public void seedDefaultCategories()
    {

        string[] defaultCategories = new[] { "Hand Tool", "Power Tools" };

        bool altered = false;

        foreach (string name in defaultCategories)
        {
            if (!_db.Categories.Any(e => e.Name == name))
            {
                _db.Categories.Add(new Category { Name = name });
                altered = true;
            }
        }

        if (altered) _db.SaveChanges();
    }
    public void addCategory(Category newCategory)
    {
        _db.Categories.Add(newCategory);
        _db.SaveChanges();
    }

    public void addCategories(List<Category> newCategories)
    {
        _db.Categories.AddRange(newCategories);
        _db.SaveChanges();
    }

    public Category getCategoryById(int id)
    {

        Category? category = _db.Categories.Find(id);

        return category;
    }
}