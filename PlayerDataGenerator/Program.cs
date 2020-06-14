using System;
using System.IO;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayerDataGenerator.Data;

namespace PlayerDataGenerator
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            RegisterServices();
            IServiceScope scope = _serviceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<ConsoleApplication>().Run();
            DisposeServices();
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var generalSettings = _configuration.GetSection("GeneralSettings").Get<GeneralSettings>();
            _configuration.Bind("GeneralSettings", generalSettings);
            var connectionString = _configuration.GetConnectionString("PlayerContext");
            services.AddDbContext<PlayerContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IPlayerExtractor, PlayerExtractor>();
            services.AddSingleton(generalSettings);
            services.AddScoped<ConsoleApplication>();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
