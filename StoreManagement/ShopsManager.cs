namespace StoreManagement;

public class ShopsManager
{
    private Dictionary<int, Product> _products = [];

    private Dictionary<int, Shop> _shops = [];

    public IReadOnlyDictionary<int, Product> Products => _products.AsReadOnly();

    public IReadOnlyDictionary<int, Shop> Shops => _shops.AsReadOnly();

    public Shop FindShopWithLowestPriceForCart(params (int productCode, int quantity)[] products)
    {
        Shop? store = null;
        decimal lowPrice = decimal.MaxValue;

        foreach (var shop in Shops.Values)
        {
            if (ShopService.CanBuyProducts(shop, products))
            {
                decimal cost = ShopService.CalculateTheCost(shop, products);

                if (lowPrice > cost)
                {
                    lowPrice = cost;
                    store = shop;
                }
            }
        }

        return store;
    }

    public Shop FindShopWithLowestPriceForProduct(int productCode)
    {
        Shop? store = null;
        decimal lowPrice = decimal.MaxValue;

        foreach (var shop in Shops.Values)
        {
            if (shop.Products.ContainsKey(productCode))
            {
                decimal productPrice = shop.Products[productCode].Price;

                if (lowPrice > productPrice && shop.Products[productCode].Quantity > 0)
                {
                    lowPrice = productPrice;
                    store = shop;
                }
            }
        }

        return store;
    }

    public void DeliverProducts(Shop shop, params (int productCode, int quantity, decimal price)[] receivedProducts)
    {
        foreach (var (productCode, quantity, price) in receivedProducts)
        {
            if (!_products.ContainsKey(productCode))
            {
                throw new ArgumentException($"There is no product with code {productCode}.");
            }

            ShopService.ReceiveProduct(shop, _products[productCode], quantity, price);
        }
    }

    public void CreateShop(string name, int code, Address address)
    {
        if (_shops.ContainsKey(code))
        {
            throw new ArgumentException($"Shop with code {code} already exists.");
        }

        _shops.Add(code, new Shop(name, code, address));
    }

    public void CreateProduct(string name, int code)
    {
        if (_products.ContainsKey(code))
        {
            throw new ArgumentException($"Product with code {code} already exists.");
        }

        _products.Add(code, new Product(name, code));
    }
}
