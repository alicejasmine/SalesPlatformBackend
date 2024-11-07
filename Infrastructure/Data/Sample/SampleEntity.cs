namespace Infrastructure.Data.Sample;

public class SampleEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public SampleEntity(string name, string description, int price)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}