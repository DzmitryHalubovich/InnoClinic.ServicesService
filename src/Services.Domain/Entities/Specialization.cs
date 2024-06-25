using System.ComponentModel.DataAnnotations;

namespace Services.Domain.Entities;

public class Specialization
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public Status Status { get; set; }

    public ICollection<Service>? Services { get; set; }
}
