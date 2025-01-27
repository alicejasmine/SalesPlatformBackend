namespace Domain.ValueObject;

public readonly record struct DocumentIdentifier(Guid EnvironmentId, DateOnly Date)
{
    public string Value { get; } = $"{EnvironmentId}-{Date.Year}-{Date.Month}";

    public DocumentIdentifier(Guid environmentId, DateOnly? inputDate)
        : this(environmentId, inputDate ?? DateOnly.FromDateTime(DateTime.UtcNow))
    {
    }

    public DocumentIdentifier(Guid environmentId)
        : this(environmentId, DateOnly.FromDateTime(DateTime.UtcNow))
    {
    }

    public override string ToString() => Value;
}