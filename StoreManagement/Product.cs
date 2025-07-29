namespace StoreManagement;

internal class Product
{
    private static Dictionary<string, Product> ExistingProduct { get; } = [];

    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; }

    private Product(string name)
    {
        Name = name.Сapitalize();
        ExistingProduct.Add(name.ToLower(), this);
    }

    public static Product GetInstance(string name)
    {
        if (ExistingProduct.TryGetValue(name.ToLower(), out Product product))
        {
            return product;
        }

        return new Product(name);
    }

    public override string ToString() => $"Product: {Name} | ID: {Id}";
}
