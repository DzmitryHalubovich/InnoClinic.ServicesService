using Services.Contracts.Service;

namespace Services.Contracts.Specialization;

public class SpecializationUpdateDTO
{
    public string Name { get; set; }

    public int Status { get; set; }

    public IEnumerable<ServiceCreateDTO>? Services { get; set; }
}
