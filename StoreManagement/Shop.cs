namespace StoreManagement;

public class Shop
{
    private Dictionary<int, ProductInfo> _products = [];

    public IReadOnlyDictionary<int, ProductInfo> Products => _products.AsReadOnly();

    public int Code { get; }

    public string Name { get; }

    public Address Address { get; }

    public Shop(string name, int code, Address address)
    {
        Code = code;
        Name = name;
        Address = address;
    }

    public void AddProduct(Product product, decimal price = 0, int quantity = 0)
    {
        if (_products.ContainsKey(product.Code))
        {
            throw new ArgumentException($"Product with code {product.Code} already exists in the shop.");
        }

        _products.Add(product.Code, new ProductInfo(product, price, quantity));
    }

    public void RemoveProduct(int productCode)
    {
        if (!_products.ContainsKey(productCode))
        {
            throw new ArgumentException($"Product with code {productCode} not found in the shop.");
        }

        _products.Remove(productCode);
    }

    public void ChangePrice(int productCode, decimal newPrice)
    {
        if (!_products.ContainsKey(productCode))
        {
            throw new ArgumentException($"Product with code {productCode} not found in the shop.");
        }

        _products[productCode].Price = newPrice;
    }

    public override string ToString() => $"Shop: {Name} | Code: {Code} | {Address}";
}
