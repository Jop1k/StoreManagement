namespace StoreManagement;

internal class Product
{
    private static Dictionary<int, Product> ExistingCodes { get; } = [];

    public int Code { get; }

    public string Name { get; }

    public Product(string name, int code)
    {
        if (ExistingCodes.ContainsKey(code))
        {
            Console.WriteLine("Товар с таким кодом уже существует.");
            //throw new ArgumentException("Товар с таким кодом уже существует."); // return null + warn?
        }

        Code = code;
        Name = name.Сapitalize();
        ExistingCodes.Add(code, this);
    }

    public override string ToString() => $"Product: {Name} | Code: {Code}";
}
