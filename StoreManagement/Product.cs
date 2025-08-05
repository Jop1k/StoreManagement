namespace StoreManagement;

public class Product
{
    public int Code { get; }

    public string Name { get; }

    public Product(string name, int code)
    {
        Code = code;
        Name = name.Сapitalize();
    }

    public override string ToString() => $"Product: {Name} | Code: {Code}";
}
