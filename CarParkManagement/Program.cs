using CarParkManagement.Database.DbContext;
using CarParkManagement.Database.Seeds;
using CarParkManagement.Features.Parking.ExitParking;
using CarParkManagement.Features.Parking.ExitParking.Helpers;
using CarParkManagement.Features.Parking.FetchParkingSpaces.Helpers;
using CarParkManagement.Features.Parking.ParkCar;
using CarParkManagement.Features.Parking.ParkCar.Helpers;
using CarParkManagement.Infrastructure.Exceptions;
using CarParkManagement.Infrastructure.Validation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

    builder.Services.AddOpenApi();
    builder.Services.AddControllers();

    builder.Services.AddDbContext<CarParkManagementDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString(CarParkManagementDbContext.ConnectionStringName)));

    builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<Program>());
    builder.Services.AddScoped<IValidator<ExitParkingCommand>, ExitParkingCommandValidator>();
    builder.Services.AddScoped<IValidator<ParkCarCommand>, ParkCarCommandValidator>();
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

    builder.Services.AddScoped<IParkingChargeCalculator, ParkingChargeCalculator>();
    builder.Services.AddScoped<IAvailableParkingSpaceService, AvailableParkingSpaceService>();
    builder.Services.AddScoped<IParkingSpacesInfoService, ParkingSpacesInfoService>();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();


    var app = builder.Build();

    if (!app.Environment.IsEnvironment("Testing"))
    {
        await app.MigrateAndSeedAsync();
    }

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();
    app.UseExceptionHandler();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}
