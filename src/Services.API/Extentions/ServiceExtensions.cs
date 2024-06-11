using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Interfaces;
using Services.Infrastructure.Data;
using Services.Infrastructure.Repositories;
using Services.Presentation.MessageProducer;
using Services.Presentation.Validators;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Commands.Specializations;
using Services.Services.Abstractions.Contracts;
using Services.Services.Abstractions.RabbitMQ;
using Services.Services.RabbitMQ;

namespace Services.API.Extentions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection());
        services.AddScoped<IMessageProducer, MessageProducer>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(Services.Abstractions.Queries.Services.GetAllServicesQuery).Assembly,
            typeof(Services.Handlers.Services.GetAllServicesQueryHandler).Assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddProblemDetails();
        services.AddAutoMapper(typeof(MapperProfile));
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddScoped<IValidator<CreateSpecializationCommand>, CreateSpecializationCommandValidator>();
        services.AddScoped<IValidator<UpdateSpecializationCommand>, UpdateSpecializationCommandValidator>();
        services.AddScoped<IValidator<CreateServiceCommand>, CreateServiceCommandValidator>();
        services.AddScoped<IValidator<UpdateServiceCommand>, UpdateServiceCommandValidator>();
        services.AddScoped<IServicesRepository, ServicesRepository>();
        services.AddScoped<ISpecializationsRepository, SpecializationsRepository>();
        services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
        services.AddDbContext<ServicesDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection")));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
