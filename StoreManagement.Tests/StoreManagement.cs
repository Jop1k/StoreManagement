namespace StoreManagement.Tests;

public class StoreManagement
{
    [Fact]
    public void CalculateTheCost_CalculateThePriceOfGoods_CorrectPrice()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.CreateProduct("банан", 1);
        shopManager.CreateProduct("дынька", 2);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        decimal cost = ShopService.CalculateTheCost(shopManager.Shops[0], [(0, 4), (1, 12), (2, 3)]);

        Assert.Equal(5000, cost);
    }

    [Fact]
    public void CalculateTheCost_SendingAnInvalidProductCode_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ShopService.CalculateTheCost(new Shop("5opka", 0, new Address("", "", "", "")), (456, 456)));
    }

    [Fact]
    public void CanBuyProducts_SendingAnInvalidProductCode_False()
    {
        Assert.False(ShopService.CanBuyProducts(new Shop("5opka", 0, new Address("", "", "", "")), (0, 456)));
    }

    [Fact]
    public void CanBuyProducts_CheckingThePossibilityOfPurchasing_True()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.CreateProduct("банан", 1);
        shopManager.CreateProduct("дынька", 2);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        bool result = ShopService.CanBuyProducts(shopManager.Shops[0], [(0, 100), (1, 100), (2, 100)]);

        Assert.True(result);
    }

    [Fact]
    public void CanBuyProducts_CheckingThePossibilityOfPurchasing_False()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.CreateProduct("банан", 1);
        shopManager.CreateProduct("дынька", 2);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        bool result = ShopService.CanBuyProducts(shopManager.Shops[0], [(0, 101), (1, 101), (2, 101)]);

        Assert.False(result);
    }

    [Fact]
    public void BuyProducts_BuyExistingProducts_CorrectResult()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.CreateProduct("банан", 1);
        shopManager.CreateProduct("дынька", 2);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        (bool, decimal) result = ShopService.BuyProducts(shopManager.Shops[0], [(0, 10), (1, 10), (2, 10)]);

        Assert.Equal((true, 12000), result);
    }

    [Fact]
    public void BuyProducts_BuyNonExistentProducts_CorrectResult()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.CreateProduct("банан", 1);
        shopManager.CreateProduct("дынька", 2);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        (bool, decimal) result = ShopService.BuyProducts(shopManager.Shops[0], [(0, 101), (1, 101), (2, 101)]);

        Assert.Equal((false, 0), result);
    }

    [Fact]
    public void BuyProducts_BuyProduct_NewAmountOfProduct()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("арбуз", 0);
        shopManager.DeliverProducts(shopManager.Shops[0], (0, 10, 500));

        ShopService.BuyProducts(shopManager.Shops[0], (0, 5));

        Assert.Equal(5, shopManager.Shops[0].Products[0].Quantity);
    }

    [Fact]
    public void ReceiveProducts_DeliverNonExistingInShopProduct_CorrectProductInfo()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        var product = new Product("Тумблер", 0);

        ShopService.ReceiveProduct(shop, product, 10, 100);

        Assert.Equal((product, 10, 100), (shop.Products[0].Product, shop.Products[0].Quantity, shop.Products[0].Price));
    }

    [Fact]
    public void ReceiveProducts_DeliverExistingInShopProduct_CorrectProductInfo()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        var product = new Product("Тумблер", 0);
        ShopService.ReceiveProduct(shop, product, 5, 100);

        ShopService.ReceiveProduct(shop, product, 5, 100);

        Assert.Equal((product, 10, 100), (shop.Products[0].Product, shop.Products[0].Quantity, shop.Products[0].Price));
    }

    [Fact]
    public void FindPurchasableProducts_SearchForPossibleProductsToBuy_CorrectProductList()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("Тумблер", 0);
        shopManager.CreateProduct("Шоколадка", 1);
        shopManager.CreateProduct("Шокочервячки", 2);
        shopManager.CreateProduct("Вафля", 3);
        shopManager.DeliverProducts(shopManager.Shops[0], [(0, 10, 100), (1, 100, 80), (2, 5, 220), (3, 0, 10)]);
        Dictionary<Product, int> productList = [];
        productList.Add(shopManager.Products[0], 10);
        productList.Add(shopManager.Products[1], 12);
        productList.Add(shopManager.Products[2], 4);
        productList.Add(shopManager.Products[3], 0);

        var result = ShopService.FindPurchasableProducts(shopManager.Shops[0], 1000);

        Assert.Equal(productList, result);
    }

    [Fact]
    public void ChangePrice_ChangeProductPrice_CorrectNewPrice()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        var product = new Product("Ананас", 0);
        ShopService.ReceiveProduct(shop, product, 10, 200);

        shop.ChangePrice(product.Code, 300);

        Assert.Equal(300, shop.Products[0].Price);
    }

    [Fact]
    public void DeliverProduct_DeliverProductToTheShop_CorrectProductInfo()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("Зубастик", 0);

        shopManager.DeliverProducts(shopManager.Shops[0], (0, 10, 100));

        Assert.Equal((shopManager.Products[0], 10, 100),
            (shopManager.Shops[0].Products[0].Product, shopManager.Shops[0].Products[0].Quantity, shopManager.Shops[0].Products[0].Price));
    }

    [Fact]
    public void DeliverProduct_SendingInvalidProductCode_ArgumentException()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateProduct("Золотое яйцо", 0);

        Assert.Throws<ArgumentException>(() => shopManager.DeliverProducts(shopManager.Shops[0], (1, 10, 100)));
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingExistentProduct_ShopWithLowestProductPrice()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Золотое яйцо", 0);
        shopManager.DeliverProducts(shopManager.Shops[0], (0, 10, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (0, 10, 200));

        var shop = shopManager.FindShopWithLowestPriceForProduct(0);

        Assert.Equal(shopManager.Shops[0], shop);
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingExistingProductThatNotInTheRange_Null()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Золотое яйцо", 0);
        shopManager.DeliverProducts(shopManager.Shops[0], (0, 0, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (0, 0, 200));

        var shop = shopManager.FindShopWithLowestPriceForProduct(0);

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingNonExistentProduct_Null()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Золотое яйцо", 0);
        shopManager.CreateProduct("Зубастик", 1);
        shopManager.DeliverProducts(shopManager.Shops[0], (1, 10, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (1, 10, 200));

        var shop = shopManager.FindShopWithLowestPriceForProduct(0);

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingExistentProducts_ShopWithLowestProductsPrice()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Зубастик", 0);
        shopManager.DeliverProducts(shopManager.Shops[0], (0, 10, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (0, 10, 200));

        var shop = shopManager.FindShopWithLowestPriceForCart((0, 10));

        Assert.Equal(shopManager.Shops[0], shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingExistingProductThatNotInTheRange_Null()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Зубастик", 0);
        shopManager.DeliverProducts(shopManager.Shops[0], (0, 1, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (0, 5, 200));

        var shop = shopManager.FindShopWithLowestPriceForCart((0, 10));

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingNonExistentProduct_Null()
    {
        var shopManager = new ShopsManager();
        shopManager.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopManager.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopManager.CreateProduct("Золотое яйцо", 0);
        shopManager.CreateProduct("Зубастик", 1);
        shopManager.DeliverProducts(shopManager.Shops[0], (1, 10, 100));
        shopManager.DeliverProducts(shopManager.Shops[1], (1, 10, 200));

        var shop = shopManager.FindShopWithLowestPriceForCart((0, 10));

        Assert.Null(shop);
    }

    [Fact]
    public void ProductInfo_1()
    {
        Assert.Throws<ArgumentException>(() => new ProductInfo(new Product("test", 0), -1, 0));
    }

    [Fact]
    public void ProductInfo_2()
    {
        Assert.Throws<ArgumentException>(() => new ProductInfo(new Product("test", 0), 0, -1));
    }
}
