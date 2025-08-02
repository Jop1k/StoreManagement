global using AdditionalExtensionMethods;

namespace StoreManagement;

internal class Program // исключения: сущ. код магазина/товара | обращение по несущ. ключу | передача null в ctor адрес шопа | 
{
    static void Main()
    {
        var shopService = new ShopService();

        shopService.CreateShop("SOSO4NIK", 1, new Address());
        shopService.CreateShop("Jop1k", 2, new Address());

        shopService.CreateProduct("Банан", 0);
        shopService.CreateProduct("Арбуз", 1);
        shopService.CreateProduct("Яблоко", 2);
        shopService.CreateProduct("Чеснок", 3);
        shopService.CreateProduct("Дыня", 4);
        shopService.CreateProduct("Лук", 5);

        shopService.DeliverProduct(1, [(0, 15, 100), (2, 75, 300), (1, 9, 200), (5, 17, 50)]);
        shopService.DeliverProduct(2, [(1, 46, 300), (5, 22, 200), (2, 11, 1565), (4, 54, 103)]);

        foreach (var shop in shopService.Shops.Values)
        {
            Console.WriteLine($"Магазин: {shop.Name} | Продукты:");
            foreach (var product in shop.Products.Values)
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
        }

        Shop store = shopService.FindShopWithLowestCartTotal([(1, 10), (5, 10)]);

        Console.WriteLine($"{store}\n{store.Products[1]}\n{store.Products[5]}");
    }
}
