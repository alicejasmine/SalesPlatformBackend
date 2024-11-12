namespace Domain;

public class BaseModel
{
    public Guid Id { get; }
    public DateTime Created { get; }

    public DateTime Modified { get; }

    protected BaseModel(Guid id, DateTime created, DateTime modified)
    {
        Id = id;
        Created = created;
        Modified = modified;
    }
}