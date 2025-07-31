using System.Text;

namespace StoreManagement;

internal class Shop
{
    private static Dictionary<int, Shop> ExistingCodes { get; } = [];

    private Dictionary<int, ProductInfo> Products { get; } = [];

    public int Code { get; }

    public string Name { get; }

    public Address Address { get; }

    public Shop(string name, int code, Address address)
    {
        if (ExistingCodes.ContainsKey(code))
        {
            throw new ArgumentException("Магазин с таким кодом уже существует."); // return null + warn?
        }

        Code = code;
        Name = name;
        Address = address;
        ExistingCodes.Add(code, this);
    }

    public string BuyProducts(params (int code, int amount)[] products)
    {
        var result = CanBuyProducts(products);

        if (result.canBuy)
        {
            return $"Стоимость покупки {result.price}";
        }

        return $"Сделка невозможна, не хватает товара.";
    }

    private (decimal price, bool canBuy) CanBuyProducts(params (int code, int amount)[] products)
    {
        decimal cost = 0;

        foreach (var product in products)
        {
            if (Products[product.code].Amount < product.amount)
            {
                return (0, false);
            }

            cost += Products[product.code].Price;
        }

        return (cost ,true);
    }

    public void ReceiveProducts(params (int code, int amount)[] products)
    {
        foreach (var product in products)
        {
            Products[product.code].Amount += product.amount;
        }
    }

    public void ChangePrice(int productCode, decimal newPrice)
    {
        Products[productCode].Price = newPrice;
    }

    public void CreateProduct(string name, int code, decimal price)
    {
        Products.Add(code, new ProductInfo(new Product(name, code), price, 0));
    }

    public List<ProductInfo> GetAllProducts()
    {
        List<ProductInfo> products = [];

        foreach (var product in Products.Values)
        {
            products.Add(product);
        }

        return products;
    }

    public override string ToString() => $"Shop: {Name} | Code: {Code} | {Address}";
}
