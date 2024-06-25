using Services.Contracts.Service;

namespace Services.Contracts.Specialization;

public class SpecializationResponseDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Status { get; set; }

    public IEnumerable<ServiceResponseDTO>? Services { get; set; }
}
