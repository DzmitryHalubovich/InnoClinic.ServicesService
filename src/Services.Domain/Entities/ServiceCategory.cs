using System.ComponentModel.DataAnnotations;

namespace Services.Domain.Entities;

public class ServiceCategory
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    public TimeSpan TimeSlotSize { get; set; }
}
