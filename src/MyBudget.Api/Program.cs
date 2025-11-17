using Microsoft.EntityFrameworkCore;
using MyBudget.Infrastructure.Data;
using MyBudget.Core.Services;
using MyBudget.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Логи
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

// Сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Коннект-строка с надёжным фолбэком
var conn =
    builder.Configuration.GetConnectionString("db") ??
    Environment.GetEnvironmentVariable("ConnectionStrings__db") ??
    "Data Source=/home/vagrant/app/data/mybudget.db";

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(conn));

builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IBudgetService,    BudgetService>();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Swagger ВСЕГДА
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBudget API v1");
    c.RoutePrefix = "swagger";
});

// Без HTTPS в Вагранте
// app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

// Авто-міграция (не валим приложение, если что-то пошло не так)
if (!app.Environment.IsEnvironment("Testing"))
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine("[WARN] DB migrate failed: " + ex.Message);
    }
}

// Редирект с корня на Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();

public partial class Program { }
