namespace Services.Contracts.Service;

public class ServiceCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public TimeSpan TimeSlotSize { get; set; }
}
