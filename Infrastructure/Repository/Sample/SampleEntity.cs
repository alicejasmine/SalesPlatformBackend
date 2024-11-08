namespace Infrastructure.Repository.Sample;

public class SampleEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }

    public SampleEntity(Guid id, string name, string description, int price, DateTime created, DateTime modified) : base(id, created, modified)
    {
        Name = name;
        Description = description;
        Price = price;
    }
}