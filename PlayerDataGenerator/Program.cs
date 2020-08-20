using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PlayerDataGenerator.Data;
using AutoMapper;
using System.Reflection;

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
            var generalSettings = _configuration.GetSection(Constants.GENERAL_SETTINGS).Get<GeneralSettings>();
            _configuration.Bind(Constants.GENERAL_SETTINGS, generalSettings);
            var connectionString = _configuration.GetConnectionString(Constants.DBConnections.CRICKET_DB);
            services.AddDbContext<CricketContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ICricketDataIngestor, CricketDataIngestor>();
            services.AddScoped<IPlayerScriptGenerator, PlayerScriptGenerator>();
            services.AddSingleton(generalSettings);
            services.AddScoped<ConsoleApplication>();
            services.AddAutoMapper(new[] { Assembly.GetExecutingAssembly() });
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
