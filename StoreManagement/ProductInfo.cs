namespace StoreManagement;

internal class ProductInfo
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
                throw new ArgumentException("Количество товара не может быть отрицательным.");
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
                throw new ArgumentException("Стоимость товара не может быть отрицательной.");
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
