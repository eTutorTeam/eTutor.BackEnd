using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eTutor.Core.Models;
using eTutor.Persistence;
using eTutor.Persistence.Seeders;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eTutor.ServerApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (IServiceScope scope = host.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var provider = scope.ServiceProvider;
                try
                {
                    IPasswordHasher<User> hasher = provider.GetService<IPasswordHasher<User>>();
                    ETutorContext context = provider.GetService<ETutorContext>();
                    ApplicationDbInitializer.SeedUsers(hasher, context);
                    Task.WaitAll();

                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine(e.Message);
                    return;
                }
            }
                
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
