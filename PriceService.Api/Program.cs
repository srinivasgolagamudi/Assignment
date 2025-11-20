using Microsoft.EntityFrameworkCore;
using PriceService.Application;
using PriceService.Application.Interfaces;
using PriceService.Domain;
using PriceService.Infrastructure;
using PriceService.Infrastructure.Clients;
using PriceService.Infrastructure.Data;
using PriceService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server
var conn = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));

builder.Services.AddHttpClient<IBitstampClient, BitstampClient>();
builder.Services.AddHttpClient<IBitfinexClient, BitfinexClient>();

builder.Services.AddScoped<IPriceProvider, BitstampPriceProvider>();
builder.Services.AddScoped<IPriceProvider, BitfinexPriceProvider>();
builder.Services.AddScoped<IPriceAggregator, AveragePriceAggregator>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<PriceService.Application.PriceService>();

var app = builder.Build();

// Ensure database created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/api/prices", async (DateTime? time, PriceService.Application.PriceService svc) =>
{
    if (time == null)
        return Results.BadRequest(new { error = "time is required" });

    var t = time.Value.ToUniversalTime();
    if (t.Minute != 0 || t.Second != 0 || t.Millisecond != 0)
        return Results.BadRequest(new { error = "Time must be hour-accurate" });

    var result = await svc.GetAggregatedPriceAsync(t);
    return Results.Ok(result);
});

app.MapGet("/api/prices/history", async (DateTime? start, DateTime? end, IPriceRepository repo) =>
{
    if (start == null || end == null)
        return Results.BadRequest(new { error = "start and end required" });

    var list = await repo.GetRangeAsync(start.Value.ToUniversalTime(), end.Value.ToUniversalTime());
    return Results.Ok(list);
});

app.Run();
