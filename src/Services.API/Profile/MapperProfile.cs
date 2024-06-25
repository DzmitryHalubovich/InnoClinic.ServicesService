using AutoMapper;
using Services.Contracts.Service;
using Services.Contracts.Specialization;
using Services.Domain.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<SpecializationCreateDTO, Specialization>();

        CreateMap<Specialization, SpecializationResponseDTO>();

        CreateMap<SpecializationUpdateDTO, Specialization>()
            .ForMember(dest => dest.Services, opt => opt.Ignore());

        CreateMap<ServiceCreateDTO, Service>();

        CreateMap<Service, ServiceResponseDTO>();

        CreateMap<ServiceCategory, ServiceCategoryDTO>();

        CreateMap<ServiceUpdateDTO, Service>();

        CreateMap<SpecializationChangeStatusDTO, Specialization>();
    }
}
