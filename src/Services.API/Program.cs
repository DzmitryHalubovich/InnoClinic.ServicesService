using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.API.Extentions;
using Services.Domain.Interfaces;
using Services.Infrastructure.Data;
using Services.Infrastructure.Repositories;
using Services.Presentation.Validators;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Commands.Specializations;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

ValidatorOptions.Global.LanguageManager.Enabled = false;

var app = builder.Build();

app.UseExceptionHandler(opt => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
        typeof(Services.Services.Abstractions.Queries.Services.GetAllServicesQuery).Assembly,
        typeof(Services.Services.Handlers.Services.GetAllServicesQueryHandler).Assembly));
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
        opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}
