using EFCoreDemo.Repository;
using EFCoreDemo.Models;
using EFCoreDemo.Data;
public class BrandRepository : IBrand
{
    private readonly HardwareStoreContext _db;

    public BrandRepository(HardwareStoreContext db)
    {
        _db = db;
    }
    public void seedDefaultBrands()
    {
        string[] defaultBrands = new[] { "Bosch", "Samsung" };

        bool altered = false;

        foreach (string name in defaultBrands)
        {
            if (!_db.Brands.Any(e => e.Name == name))
            {
                _db.Brands.Add(new Brand { Name = name });
                altered = true;
            }
        }

        if (altered) _db.SaveChanges();
    }
    public void addBrand(Brand newBrand)
    {

        _db.Brands.Add(newBrand);
        _db.SaveChanges();
    }

    public void addBrands(List<Brand> newBrands)
    {
        _db.Brands.AddRange(newBrands);
        _db.SaveChanges();
    }

    public Brand getBrandById(int id)
    {
        Brand? brand = _db.Brands.Find(id);

        return brand;
    }
}