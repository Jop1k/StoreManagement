namespace StoreManagement;

internal class Shop
{
    private static Dictionary<int, Shop> _existingCodes = [];

    private Dictionary<int, ProductInfo> _products = [];

    public IReadOnlyDictionary<int, Shop> ExistingCodes => _existingCodes.AsReadOnly();

    public IReadOnlyDictionary<int, ProductInfo> Products => _products.AsReadOnly();

    public int Code { get; }

    public string Name { get; }

    public Address Address { get; }

    public Shop(string name, int code, Address address)
    {
        if (_existingCodes.ContainsKey(code))
        {
            throw new ArgumentException("Магазин с таким кодом уже существует.");
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
            if (!_existingCodes.ContainsKey(product.code))
            {
                throw new ArgumentException("Товар с таким кодом не найден.");
            }

            costs += _products[product.code].Price * product.amount;
        }

        return costs;
    }

    public bool CanBuyProducts(params (int code, int amount)[] products)
    {
        foreach (var product in products)
        {
            if (!_existingCodes.ContainsKey(product.code))
            {
                throw new ArgumentException("Товар с таким кодом не найден.");
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

    public void ReceiveProducts((Product product, int amount, decimal price) receivedProduct) // возможность установить или изменить цену
    {
        if (_products.ContainsKey(receivedProduct.product.Code))
        {
            _products[receivedProduct.product.Code].Amount += receivedProduct.amount;
            ChangePrice(receivedProduct.product.Code, receivedProduct.price);
            return;
        }

        _products.Add(receivedProduct.product.Code, new ProductInfo(receivedProduct.product, receivedProduct.price, receivedProduct.amount));
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

    public override string ToString() => $"Shop: {Name} | Code: {Code} | {Address}";
}
