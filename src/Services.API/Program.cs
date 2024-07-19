using FluentValidation;
using Services.API.Extentions;

var builder = WebApplication.CreateBuilder(args);

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.ConfigureServices();

var app = builder.Build();

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization("ApiScope");

app.Run();

