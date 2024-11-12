using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repository.Sample;

public class SampleEntity : BaseEntity
{
    [Required]  
    [MaxLength(50)]
    public string Name { get; set; }
   
    [Required]  
    [MaxLength(250)]
    public string Description { get; set; }
    
    [Required]  
    public int Price { get; set; }

    public SampleEntity(Guid id, string name, string description, int price, DateTime created, DateTime modified) : base(id, created, modified)
    {
        Name = name;
        Description = description;
        Price = price;
        Created = created;
        Modified = modified;
    }
    
}