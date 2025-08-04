namespace StoreManagement;

public class ShopService
{
    private static ShopService _instance;

    private Dictionary<int, Product> _products = [];

    private Dictionary<int, Shop> _shops = [];

    public IReadOnlyDictionary<int, Product> Products => _products.AsReadOnly();

    public IReadOnlyDictionary<int, Shop> Shops => _shops.AsReadOnly();

    private ShopService() { }

    public static ShopService GetInstance()
    {
        if (_instance == null)
        {
            return new ShopService();
        }

        return _instance;
    }

    public Shop FindShopWithLowestCartTotal(params (int productCode, int amount)[] products)
    {
        Shop store = null!;
        decimal lowPrice = decimal.MaxValue;

        foreach (var shop in Shops.Values)
        {
            if (shop.CanBuyProducts(products))
            {
                if (lowPrice > shop.CalculateTheCost(products))
                {
                    lowPrice = shop.CalculateTheCost(products);
                    store = shop;
                }
            }
        }

        return store;
    }

    public Shop FindShopWithLowestPrice(int productCode)
    {
        Shop store = null!;
        decimal lowPrice = decimal.MaxValue;

        foreach (var shop in Shops.Values)
        {
            if (shop.Products.ContainsKey(productCode))
            {
                if (lowPrice > shop.Products[productCode].Price && shop.Products[productCode].Amount > 0)
                {
                    lowPrice = shop.Products[productCode].Price;
                    store = shop;
                }
            }
        }

        return store;
    }

    public void DeliverProducts(int shopCode, params (int productCode, int amount, decimal price)[] receivedProducts)
    {
        foreach (var product in receivedProducts)
        {
            if (!Shops.ContainsKey(shopCode))
            {
                throw new ArgumentException("Магазин с таким кодом не найден.");
            }
            if (!_products.ContainsKey(product.productCode))
            {
                throw new ArgumentException("Товара с таким кодом не существует.");
            }

            Shops[shopCode].ReceiveProduct(_products[product.productCode], product.amount, product.price);
        }
    }

    public void CreateShop(string name, int code, Address address) => _shops.Add(code, new Shop(name, code, address));

    public void CreateProduct(string name, int code) => _products.Add(code, new Product(name, code));

    public static void Clear()
    {
        _instance = null;
        Shop.ClearExistingCodes();
        Product.ClearExistingCodes();
    }
}
