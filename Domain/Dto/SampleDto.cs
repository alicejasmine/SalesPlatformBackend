namespace Domain.Dto;

public class SampleDto
{
    public SampleDto(Guid id, string name, string description, int price, DateTime created, DateTime modified)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Created = created;
        Modified = modified;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}