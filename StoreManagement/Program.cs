global using AdditionalExtensionMethods;

namespace StoreManagement;

internal class Program
{
    static void Main()
    {
        Product p1 = Product.GetInstance("БАНАН");
        Product p2 = Product.GetInstance("бАнАн");
        Console.WriteLine($"{p1}\n{p2}");
    }
}
