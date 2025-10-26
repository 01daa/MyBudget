using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBudget.Infrastructure.Data;

public sealed class CustomWebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            foreach (var d in services.Where(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>)).ToList())
                services.Remove(d);

            var dbPath = Path.Combine(Path.GetTempPath(), $"mybudget_tests_{Guid.NewGuid():N}.db");
            var cs = new SqliteConnectionStringBuilder { DataSource = dbPath }.ToString();

            services.AddDbContext<AppDbContext>(o => o.UseSqlite(cs));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
