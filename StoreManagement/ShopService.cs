namespace StoreManagement;

public class ShopService
{
    public static decimal CalculateTheCost(Shop shop, params (int code, int quantity)[] products)
    {
        decimal costs = 0;

        foreach (var (code, quantity) in products)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("The quantity of products cannot be negative.");
            }

            if (!shop.Products.ContainsKey(code))
            {
                throw new ArgumentException($"Product with code {code} not found in the shop.");
            }

            costs += shop.Products[code].Price * quantity;
        }

        return costs;
    }

    public static bool CanBuyProducts(Shop shop, params (int code, int quantity)[] products)
    {
        foreach (var (code, quantity) in products)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("The quantity of products cannot be negative.");
            }

            if (!shop.Products.ContainsKey(code) || shop.Products[code].Quantity < quantity)
            {
                return false;
            }
        }

        return true;
    }

    public static (bool, decimal) BuyProducts(Shop shop, params (int code, int quantity)[] products)
    {
        if (CanBuyProducts(shop, products))
        {
            foreach (var (code, quantity) in products)
            {
                shop.Products[code].Quantity -= quantity;
            }

            return (true, CalculateTheCost(shop, products));
        }

        return (false, 0);
    }

    public static void ReceiveProduct(Shop shop, Product product, int quantity, decimal price)
    {
        if (quantity < 0)
        {
            throw new ArgumentException("The quantity of products cannot be negative.");
        }

        if (shop.Products.ContainsKey(product.Code))
        {
            shop.Products[product.Code].Quantity += quantity;
            shop.ChangePrice(product.Code, price);
            return;
        }

        shop.AddProduct(product, price, quantity);
    }

    public static Dictionary<Product, int> FindPurchasableProducts(Shop shop, decimal budget)
    {
        if (budget < 0)
        {
            throw new ArgumentException("Budget cannot be negative.");
        }

        Dictionary<Product, int> purchasableProducts = [];

        foreach (var product in shop.Products.Values)
        {
            if (product.Price <= budget)
            {
                int maxAmount = (int)(budget / product.Price);

                purchasableProducts.Add(product.Product, maxAmount > product.Quantity ? product.Quantity : maxAmount);
            }
        }

        return purchasableProducts;
    }
}
