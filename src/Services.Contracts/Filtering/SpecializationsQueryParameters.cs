namespace Services.Contracts.Filtering;

public class SpecializationsQueryParameters
{
    public bool IncludeServices { get; set; } = true;

    public bool OnlyActive { get; set; } = true;
}
