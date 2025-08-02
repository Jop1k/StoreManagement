namespace StoreManagement;

internal class Shop // исключения: попытка создать объект с сущ. кодом | пытаемся обратиться по не существующему ключу у Products
{
    private Dictionary<int, ProductInfo> _products = [];

    private static Dictionary<int, Shop> ExistingCodes { get; } = [];

    public IReadOnlyDictionary<int, ProductInfo> Products
    {
        get => _products.AsReadOnly();
    }

    public int Code { get; }

    public string Name { get; }

    public Address Address { get; }

    public Shop(string name, int code, Address address)
    {
        if (ExistingCodes.ContainsKey(code))
        {
            throw new ArgumentException("Магазин с таким кодом уже существует.");
        }

        Code = code;
        Name = name;
        Address = address;
        ExistingCodes.Add(code, this);
    }

    public decimal CalculateTheCost(params (int code, int amount)[] products)
    {
        decimal costs = 0;

        foreach (var product in products)
        {
            costs += _products[product.code].Price * product.amount;
        }

        return costs;
    }

    public bool CanBuyProducts(params (int code, int amount)[] products)
    {
        foreach (var product in products)
        {
            if (_products[product.code].Amount < product.amount)
            {
                return false;
            }
        }

        return true;
    }

    public string BuyProducts(params (int code, int amount)[] products)
    {
        if (CanBuyProducts(products))
        {
            foreach (var product in products)
            {
                _products[product.code].Amount -= product.amount;
            }

            return $"Успешно! Стоимость всего товара: {CalculateTheCost(products)}.";
        }

        return $"Ошибка! Не хватает товара.";
    }

    public void ReceiveProducts((Product product, int amount, decimal price) receivedProduct) // написать метод для получения товара из кода?
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

    public void ChangePrice(int productCode, decimal newPrice)
    {
        _products[productCode].Price = newPrice;
    }

    public override string ToString() => $"Shop: {Name} | Code: {Code} | {Address}";
}
