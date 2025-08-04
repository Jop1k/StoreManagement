namespace StoreManagement;

public class Product
{
    private static Dictionary<int, Product> _existingCodes = [];

    public static IReadOnlyDictionary<int, Product> ExistingCodes => _existingCodes.AsReadOnly();

    public int Code { get; }

    public string Name { get; }

    public Product(string name, int code)
    {
        if (_existingCodes.ContainsKey(code))
        {
            throw new ArgumentException($"Product with code {code} already exists.");
        }

        Code = code;
        Name = name.Сapitalize();
        _existingCodes.Add(code, this);
    }

    public static void ClearExistingCodes() => _existingCodes.Clear();

    public override string ToString() => $"Product: {Name} | Code: {Code}";
}
