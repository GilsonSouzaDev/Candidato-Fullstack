using candidatos.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Candidato.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _dbName = $"test_db_{Guid.NewGuid()}.db";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<CandidatoContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<CandidatoContext>(options =>
                {
                    // Use a unique physical SQLite database for each test run to avoid migration crashes
                    options.UseSqlite($"Data Source={_dbName}");
                });

                // Program.cs will call Migrate() and Seed data automatically
            });

            builder.UseEnvironment("Testing");
        }
        
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (System.IO.File.Exists(_dbName))
            {
                try { System.IO.File.Delete(_dbName); } catch { }
            }
        }
    }
}
