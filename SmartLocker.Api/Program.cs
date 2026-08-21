using Microsoft.EntityFrameworkCore;
using SmartLocker.Api.Data;
using SmartLocker.Api.Services;
using SmartLocker.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SmartLockerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SmartLockerDb")));

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ILockerService, LockerService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
