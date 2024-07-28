using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Services.Domain.Interfaces;
using Services.Infrastructure;
using Services.Infrastructure.Data;
using Services.Infrastructure.Repositories;
using Services.Presentation.Validators;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Commands.Specializations;

namespace Services.API.Extentions;

public static class WebApplicationBuilderExtention
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<CreateSpecializationCommand>, CreateSpecializationCommandValidator>();
        builder.Services.AddScoped<IValidator<UpdateSpecializationCommand>, UpdateSpecializationCommandValidator>();
        builder.Services.AddScoped<IValidator<CreateServiceCommand>, CreateServiceCommandValidator>();
        builder.Services.AddScoped<IValidator<UpdateServiceCommand>, UpdateServiceCommandValidator>();
        builder.Services.AddScoped<IServicesRepository, ServicesRepository>();
        builder.Services.AddScoped<ISpecializationsRepository, SpecializationsRepository>();
        builder.Services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        builder.Host.UseSerilog((ctx, lc) =>
            lc.WriteTo.Console()
            .ReadFrom.Configuration(ctx.Configuration));

        builder.Logging.ClearProviders();

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5005";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "services.api");
            });
        });

        var MTRabbitMqOptions = builder.Configuration
        .GetSection("MassTransitRabbitMq")
        .Get<MassTransitRabbitMqConfiguration>();

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(MTRabbitMqOptions.Host, "/", h =>
                {
                    h.Username(MTRabbitMqOptions.Username);
                    h.Password(MTRabbitMqOptions.Password);
                });

                cfg.ConfigureEndpoints(context);

                cfg.AutoDelete = true;
            });
        });

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(Services.Abstractions.Queries.Services.GetAllServicesQuery).Assembly,
            typeof(Services.Handlers.Services.GetAllServicesQueryHandler).Assembly));

        builder.Services.AddProblemDetails();
        builder.Services.AddAutoMapper(typeof(MapperProfile));
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        builder.Services.AddDbContext<ServicesDbContext>(opt =>
           opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
        });
    }
}
