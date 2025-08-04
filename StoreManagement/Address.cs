namespace StoreManagement;

public class Address
{
    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string Building { get; set; }

    public Address(string county, string city, string street, string building)
    {
        Country = county;
        City = city;
        Street = street;
        Building = building;
    }

    public override string ToString() => $"Address: country {Country}, city {City}, street {Street}, building {Building}";
}
