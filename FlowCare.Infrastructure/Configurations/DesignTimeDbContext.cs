using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FlowCare.Infrastructure
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            // Path to the Presentation project where appsettings.json is
            string presentationPath = Path.Combine(Directory.GetCurrentDirectory(), "..\\FlowCare");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(presentationPath)                   
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection")!;

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(
                connectionString,
                npgsqlOptions => npgsqlOptions.MigrationsAssembly("FlowCare.Infrastructure"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}