namespace StoreManagement;

internal class ProductInfo
{
    private int _amount;

    public Product Product { get; }

    public decimal Price { get; set; }

    public int Amount
    {
        get => _amount;
        set
        {
            if (value < 0)
            {
                Console.WriteLine("количество продукта не может быть меньше 0");
                _amount = 0;
            }
            else
            {
                _amount = value;
            }
        }
    }

    public ProductInfo(Product product, decimal price, int amount)
    {
        Product = product;
        Price = price;
        Amount = amount;
    }

    public override string ToString() => $"{Product} | Price: {Price} | Amount: {Amount}";
}
