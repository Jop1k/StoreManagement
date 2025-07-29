namespace StoreManagement;

internal class Program
{
    static void Main()
    {
        Product p1 = Product.GetInstance("Банан");
        Product p2 = Product.GetInstance("Банан");
        Console.WriteLine($"{p1}\n{p2}");
    }
}
