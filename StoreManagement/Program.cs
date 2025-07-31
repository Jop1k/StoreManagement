global using AdditionalExtensionMethods;

namespace StoreManagement;

internal class Program
{
    static void Main()
    {
        var shop = new Shop("5OPKA", 0, new Address("Russia", "Irkutsk", "Pushkina", "4"));
        Console.WriteLine(shop);

        shop.CreateProduct("банан", 0, 100);
        shop.CreateProduct("арбуз", 1, 200);

        foreach (var product in shop.GetAllProducts())
        {
            Console.WriteLine(product);
        }

        shop.ChangePrice(0, 120);
        shop.ChangePrice(1, 205);

        foreach (var product in shop.GetAllProducts())
        {
            Console.WriteLine(product);
        }

        Console.WriteLine("Введите код продукта и число, на которое хотите пополнить продукт");
        (int code, int amount) tup = (int.Parse(Console.ReadLine()), int.Parse(Console.ReadLine()));
        shop.ReceiveProducts((tup.code, tup.amount));

        foreach (var product in shop.GetAllProducts())
        {
            Console.WriteLine(product);
        }
    }
}
