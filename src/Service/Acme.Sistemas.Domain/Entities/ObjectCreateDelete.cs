namespace Acme.Sistemas.Domain.Entities;

public sealed class ObjectCreateDelete
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }
}
