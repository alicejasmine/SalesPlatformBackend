using System.ComponentModel.DataAnnotations;

namespace Domain;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime Modified { get; set; }

    protected BaseModel(Guid id, DateTime created, DateTime modified)
    {
        Id = id;
        Created = created;
        Modified = modified;
    }
}