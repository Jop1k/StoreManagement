namespace StoreManagement.Tests;

public class StoreManagement
{
    [Fact]
    public void CalculateTheCost_CalculateThePriceOfGoods_CorrectPrice()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.CreateProduct("банан", 1);
        shopService.CreateProduct("дынька", 2);
        shopService.DeliverProducts(0, [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        decimal cost = shopService.Shops[0].CalculateTheCost([(0, 4), (1, 12), (2, 3)]);
        ShopService.Clear();

        Assert.Equal(5000, cost);
    }

    [Fact]
    public void CalculateTheCost_SendingAnInvalidProductCode_ArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Shop("5opka", 0, new Address("", "", "", "")).CalculateTheCost((456, 456)));
        ShopService.Clear();
    }

    [Fact]
    public void CanBuyProducts_SendingAnInvalidProductCode_False()
    {
        Assert.False(new Shop("5opka", 0, new Address("", "", "", "")).CanBuyProducts((0, 456)));
        ShopService.Clear();
    }

    [Fact]
    public void CanBuyProducts_CheckingThePossibilityOfPurchasing_True()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.CreateProduct("банан", 1);
        shopService.CreateProduct("дынька", 2);
        shopService.DeliverProducts(0, [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        bool result = shopService.Shops[0].CanBuyProducts([(0, 100), (1, 100), (2, 100)]);
        ShopService.Clear();

        Assert.True(result);
    }

    [Fact]
    public void CanBuyProducts_CheckingThePossibilityOfPurchasing_False()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.CreateProduct("банан", 1);
        shopService.CreateProduct("дынька", 2);
        shopService.DeliverProducts(0, [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        bool result = shopService.Shops[0].CanBuyProducts([(0, 101), (1, 101), (2, 101)]);
        ShopService.Clear();

        Assert.False(result);
    }

    [Fact]
    public void BuyProducts_BuyExistingProducts_CorrectResult()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.CreateProduct("банан", 1);
        shopService.CreateProduct("дынька", 2);
        shopService.DeliverProducts(0, [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        (bool, decimal) result = shopService.Shops[0].BuyProducts([(0, 10), (1, 10), (2, 10)]);
        ShopService.Clear();

        Assert.Equal((true, 12000), result);
    }

    [Fact]
    public void BuyProducts_BuyNonExistentProducts_CorrectResult()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.CreateProduct("банан", 1);
        shopService.CreateProduct("дынька", 2);
        shopService.DeliverProducts(0, [(0, 100, 500), (1, 100, 100), (2, 100, 600)]);

        (bool, decimal) result = shopService.Shops[0].BuyProducts([(0, 101), (1, 101), (2, 101)]);
        ShopService.Clear();

        Assert.Equal((false, 0), result);
    }

    [Fact]
    public void BuyProducts_BuyProduct_NewAmountOfProduct()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("5opka", 0, new Address("", "", "", ""));
        shopService.CreateProduct("арбуз", 0);
        shopService.DeliverProducts(0, (0, 10, 500));

        shopService.Shops[0].BuyProducts((0, 5));
        ShopService.Clear();

        Assert.Equal(5, shopService.Shops[0].Products[0].Amount);
    }

    [Fact]
    public void ReceiveProducts_DeliverNonExistingInShopProduct_CorrectProductInfo()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        var product = new Product("Тумблер", 0);

        shop.ReceiveProduct(product, 10, 100);
        ShopService.Clear();

        Assert.Equal((product, 10, 100), (shop.Products[0].Product, shop.Products[0].Amount, shop.Products[0].Price));
    }

    [Fact]
    public void ReceiveProducts_DeliverExistingInShopProduct_CorrectProductInfo()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        var product = new Product("Тумблер", 0);
        shop.ReceiveProduct(product, 5, 100);

        shop.ReceiveProduct(product, 5, 100);
        ShopService.Clear();

        Assert.Equal((product, 10, 100), (shop.Products[0].Product, shop.Products[0].Amount, shop.Products[0].Price));
    }

    [Fact]
    public void FindPurchasableProducts_SearchForPossibleProductsToBuy_CorrectProductList()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateProduct("Тумблер", 0);
        shopService.CreateProduct("Шоколадка", 1);
        shopService.CreateProduct("Шокочервячки", 2);
        shopService.CreateProduct("Вафля", 3);
        shopService.DeliverProducts(0, [(0, 10, 100), (1, 100, 80), (2, 5, 220), (3, 0, 10)]);
        Dictionary<Product, int> productList = [];
        productList.Add(shopService.Products[0], 10);
        productList.Add(shopService.Products[1], 12);
        productList.Add(shopService.Products[2], 4);
        productList.Add(shopService.Products[3], 0);

        var result = shopService.Shops[0].FindPurchasableProducts(1000);
        ShopService.Clear();

        Assert.Equal(productList, result);
    }

    [Fact]
    public void ChangePrice_ChangeProductPrice_CorrectNewPrice()
    {
        var shop = new Shop("50PKA", 0, new Address("", "", "", ""));
        shop.ReceiveProduct(new Product("Ананас", 0), 10, 200);

        shop.ChangePrice(0, 300);
        ShopService.Clear();

        Assert.Equal(300, shop.Products[0].Price);
    }

    [Fact]
    public void DeliverProduct_DeliverProductToTheShop_CorrectProductInfo()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateProduct("Зубастик", 0);

        shopService.DeliverProducts(0, (0, 10, 100));
        ShopService.Clear();

        Assert.Equal((shopService.Products[0], 10, 100),
            (shopService.Shops[0].Products[0].Product, shopService.Shops[0].Products[0].Amount, shopService.Shops[0].Products[0].Price));
    }

    [Fact]
    public void DeliverProduct_SendingInvalidShopCode_ArgumentException()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);

        Assert.Throws<ArgumentException>(() => shopService.DeliverProducts(1, (0, 10, 100)));
        ShopService.Clear();
    }

    [Fact]
    public void DeliverProduct_SendingInvalidProductCode_ArgumentException()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);

        Assert.Throws<ArgumentException>(() => shopService.DeliverProducts(0, (1, 10, 100)));
        ShopService.Clear();
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingExistentProduct_ShopWithLowestProductPrice()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);
        shopService.DeliverProducts(0, (0, 10, 100));
        shopService.DeliverProducts(1, (0, 10, 200));

        var shop = shopService.FindShopWithLowestPrice(0);
        ShopService.Clear();

        Assert.Equal(shopService.Shops[0], shop);
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingExistingProductThatNotInTheRange_Null()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);
        shopService.DeliverProducts(0, (0, 0, 100));
        shopService.DeliverProducts(1, (0, 0, 200));

        var shop = shopService.FindShopWithLowestPrice(0);
        ShopService.Clear();

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestPrice_SendingNonExistentProduct_Null()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);
        shopService.CreateProduct("Зубастик", 1);
        shopService.DeliverProducts(0, (1, 10, 100));
        shopService.DeliverProducts(1, (1, 10, 200));

        var shop = shopService.FindShopWithLowestPrice(0);
        ShopService.Clear();

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingExistentProducts_ShopWithLowestProductsPrice()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Зубастик", 0);
        shopService.DeliverProducts(0, (0, 10, 100));
        shopService.DeliverProducts(1, (0, 10, 200));

        var shop = shopService.FindShopWithLowestCartTotal((0, 10));
        ShopService.Clear();

        Assert.Equal(shopService.Shops[0], shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingExistingProductThatNotInTheRange_Null()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Зубастик", 0);
        shopService.DeliverProducts(0, (0, 1, 100));
        shopService.DeliverProducts(1, (0, 5, 200));

        var shop = shopService.FindShopWithLowestCartTotal((0, 10));
        ShopService.Clear();

        Assert.Null(shop);
    }

    [Fact]
    public void FindShopWithLowestCartTotal_SendingNonExistentProduct_Null()
    {
        var shopService = ShopService.GetInstance();
        shopService.CreateShop("50PKA", 0, new Address("", "", "", ""));
        shopService.CreateShop("БЫЧОК", 1, new Address("", "", "", ""));
        shopService.CreateProduct("Золотое яйцо", 0);
        shopService.CreateProduct("Зубастик", 1);
        shopService.DeliverProducts(0, (1, 10, 100));
        shopService.DeliverProducts(1, (1, 10, 200));

        var shop = shopService.FindShopWithLowestCartTotal((0, 10));
        ShopService.Clear();

        Assert.Null(shop);
    }
}
