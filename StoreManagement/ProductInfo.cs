namespace StoreManagement;

internal class ProductInfo
{
    public Product Product { get; set; }

    public decimal Price { get; set; }

    public int Amount { get; set; }

    public ProductInfo(Product product, decimal price, int amount)
    {
        Product = product;
        Price = price;
        Amount = amount;
    }

    public override string ToString() => $"{Product} | Price: {Price} | Amount: {Amount}";
}
