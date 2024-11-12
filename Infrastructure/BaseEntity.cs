using System.ComponentModel.DataAnnotations;

namespace Infrastructure;

public abstract class BaseEntity

{
    [Key]  public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    protected BaseEntity()
    {
    }

    protected BaseEntity(Guid id, DateTime created)
    {
        Id = id;
        Created = created;
    }

    protected BaseEntity(Guid id, DateTime created, DateTime modified)
    {
        Id = id;
        Created = created;
        Modified = modified;
    }
}