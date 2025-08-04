namespace StoreManagement;

public class ProductInfo
{
    private int _amount;
    private decimal _price;

    public int Amount
    {
        get => _amount;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("The quantity of products cannot be negative.");
            }
            else
            {
                _amount = value;
            }
        }
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("The cost of a product cannot be negative.");
            }
            else
            {
                _price = value;
            }
        }
    }

    public Product Product { get; }

    public ProductInfo(Product product, decimal price, int amount)
    {
        Product = product;
        Price = price;
        Amount = amount;
    }

    public override string ToString() => $"{Product} | Price: {Price} | Amount: {Amount}";
}
