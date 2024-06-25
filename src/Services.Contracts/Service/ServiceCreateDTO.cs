namespace Services.Contracts.Service;

public class ServiceCreateDTO
{
    public string Name { get; set; }

    public decimal Price { get; set; }

    public int Status { get; set; }

    public int ServiceCategoryId { get; set; }

    public int SpecializationId { get; set; }
}
