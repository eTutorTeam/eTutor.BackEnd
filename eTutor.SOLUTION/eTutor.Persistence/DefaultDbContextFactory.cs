using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace eTutor.Persistence
{
    public sealed class DefaultDbContextFactory : IDesignTimeDbContextFactory<ETutorContext>
    {
        private readonly IConfigurationRoot _configuration;

        public DefaultDbContextFactory()
        {
            string basePath = AppContext.BaseDirectory;

            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public ETutorContext CreateDbContext(string[] args)
        {
            return Create(_configuration.GetConnectionString("Main  Connection"));
        }

        ETutorContext Create(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));
            }

            var optionsBuilder = new DbContextOptionsBuilder<ETutorContext>();

            optionsBuilder.UseMySql(connectionString);

            return new ETutorContext(optionsBuilder.Options);
        }
        
    }
}
