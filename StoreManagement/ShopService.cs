namespace StoreManagement;

internal class ShopService
{
    private Dictionary<int, Product> _products = [];

    public Dictionary<int, Shop> Shops { get; } = [];

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

    public Shop FindShopWithLowestPrice(int productCode, int amount = 1)
    {
        Shop store = null!;
        decimal lowPrice = decimal.MaxValue;

        foreach (var shop in Shops.Values)
        {
            if (shop.Products.ContainsKey(productCode))
            {
                if (lowPrice > shop.Products[productCode].Price && shop.Products[productCode].Amount >= amount)
                {
                    lowPrice = shop.Products[productCode].Price * amount;
                    store = shop;
                }
            }
        }

        return store;
    }

    public void DeliverProduct(int shopCode, params (int productCode, int amount, decimal price)[] receivedProducts)
    {
        foreach (var productInfo in receivedProducts)
        {
            Shops[shopCode].ReceiveProducts((_products[productInfo.productCode], productInfo.amount, productInfo.price));
        }
    }

    public void CreateShop(string name, int code, Address address)
    {
        Shops.Add(code, new Shop(name, code, address)); // exception
    }

    public void CreateProduct(string name, int code)
    {
        _products.Add(code, Product.GetInstance(name, code));
    }
}
