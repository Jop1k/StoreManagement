namespace StoreManagement;

internal class Address
{
    public string Country { get; set; }

    public string City { get; set; }

    public string Street { get; set; }

    public string Building { get; set; }

    public Address(string county = "Unknown", string city = "Unknown", string street = "Unknown", string building = "Unknown")
    {
        Country = county;
        City = city;
        Street = street;
        Building = building;
    }

    public override string ToString() => $"Address: country {Country}, city {City}, street {Street}, building {Building}";
}
