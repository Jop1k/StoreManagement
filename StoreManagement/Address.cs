namespace StoreManagement;

internal class Address
{
    public string Country { get; set; } = "Unknown";

    public string City { get; set; } = "Unknown";

    public string Street { get; set; } = "Unknown";

    public string Building { get; set; } = "Unknown";

    public Address(string county, string city, string street, string building)
    {
        Country = county;
        City = city;
        Street = street;
        Building = building;
    }

    public override string ToString() => $"Address: country {Country}, city {City}, street {Street}, building {Building}";
}
