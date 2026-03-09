namespace Domain.Entities.Common;

public abstract class BaseEntity<T>
{
    public T Id { get; set; }
    public DateTime CreatedAt { get; set; }
}