namespace StoreManagement;

internal class Product
{
    private static Dictionary<int, Product> _existingCodes = [];

    public IReadOnlyDictionary<int, Product> ExistingCodes => _existingCodes.AsReadOnly();

    public int Code { get; }

    public string Name { get; }

    public Product(string name, int code)
    {
        if (_existingCodes.ContainsKey(code))
        {
            throw new ArgumentException("Товар с данным кодом уже существует.");
        }

        Code = code;
        Name = name.Сapitalize();
        _existingCodes.Add(code, this);
    }

    public override string ToString() => $"Product: {Name} | Code: {Code}";
}
