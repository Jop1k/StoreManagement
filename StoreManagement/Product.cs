namespace StoreManagement;

internal class Product // исключения: попытка создать объект с сущ. кодом
{
    private static Dictionary<int, Product> ExistingCodes { get; } = [];

    public int Code { get; }

    public string Name { get; }

    private Product(string name, int code)
    {
        Code = code;
        Name = name.Сapitalize();
        ExistingCodes.Add(code, this);
    }

    public static Product GetInstance(string name, int code)
    {
        if (ExistingCodes.ContainsKey(code))
        {
            return ExistingCodes[code];
        }

        return new Product(name, code);
    }

    public override string ToString() => $"Product: {Name} | Code: {Code}";
}
