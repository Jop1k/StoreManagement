namespace StoreManagement;

public class Shop
{
    private static Dictionary<int, Shop> _existingCodes = [];

    private Dictionary<int, ProductInfo> _products = [];

    public static IReadOnlyDictionary<int, Shop> ExistingCodes => _existingCodes.AsReadOnly();

    public IReadOnlyDictionary<int, ProductInfo> Products => _products.AsReadOnly();

    public int Code { get; }

    public string Name { get; }

    public Address Address { get; }

    public Shop(string name, int code, Address address)
    {
        if (_existingCodes.ContainsKey(code))
        {
            throw new ArgumentException($"Shop with code {code} already exists.");
        }

        Code = code;
        Name = name;
        Address = address;
        _existingCodes.Add(code, this);
    }

    public decimal CalculateTheCost(params (int code, int amount)[] products)
    {
        decimal costs = 0;

        foreach (var product in products)
        {
            if (!_products.ContainsKey(product.code))
            {
                throw new ArgumentException($"Product with code {product.code} not found in the shop.");
            }

            costs += _products[product.code].Price * product.amount;
        }

        return costs;
    }

    public bool CanBuyProducts(params (int code, int amount)[] products)
    {
        foreach (var product in products)
        {
            if (!_products.ContainsKey(product.code))
            {
                return false;
            }

            if (_products[product.code].Amount < product.amount)
            {
                return false;
            }
        }

        return true;
    }

    public (bool, decimal) BuyProducts(params (int code, int amount)[] products)
    {
        if (CanBuyProducts(products))
        {
            foreach (var product in products)
            {
                _products[product.code].Amount -= product.amount;
            }

            return (true, CalculateTheCost(products));
        }

        return (false, 0);
    }

    public void ReceiveProduct(Product product, int amount, decimal price)
    {
        if (_products.ContainsKey(product.Code))
        {
            _products[product.Code].Amount += amount;
            ChangePrice(product.Code, price);
            return;
        }

        _products.Add(product.Code, new ProductInfo(product, price, amount));
    }

    public Dictionary<Product, int> FindPurchasableProducts(decimal budget)
    {
        Dictionary<Product, int> affordableProducts = [];

        foreach (var product in _products.Values)
        {
            if (product.Price <= budget)
            {
                affordableProducts.Add(product.Product, (int)(budget / product.Price) > product.Amount ? product.Amount : (int)(budget / product.Price));
            }
        }

        return affordableProducts;
    }

    public void ChangePrice(int productCode, decimal newPrice) => _products[productCode].Price = newPrice;

    public static void ClearExistingCodes() => _existingCodes.Clear();

    public override string ToString() => $"Shop: {Name} | Code: {Code} | {Address}";
}
