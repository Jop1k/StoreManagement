namespace StoreManagement;

public class ProductInfo
{
    private int _quantity;
    private decimal _price;

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("The quantity of products cannot be negative.");
            }
            else
            {
                _quantity = value;
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

    public ProductInfo(Product product, decimal price, int quantity)
    {
        Product = product;
        Price = price;
        Quantity = quantity;
    }

    public override string ToString() => $"{Product} | Price: {Price} | Quantity: {Quantity}";
}
