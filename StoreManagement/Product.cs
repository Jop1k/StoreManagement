namespace StoreManagement;

internal class Product
{
    private static List<Product> _existingProducts = [];
    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; }

    private Product(string name)
    {
        Name = name;
        _existingProducts.Add(this);
    }

    public static Product GetInstance(string name)
    {
        foreach (var product in _existingProducts)
        {
            if (product.Name == name)
            {
                return product;
            }
        }

        return new Product(name);
    }

    public override string ToString() => $"Product: {Name} | ID: {Id}";
}
