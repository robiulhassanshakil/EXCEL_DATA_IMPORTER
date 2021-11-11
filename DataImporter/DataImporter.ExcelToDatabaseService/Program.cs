using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataImporter.Common;
using DataImporter.Importing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using DataImporter.Importing.Contexts;

namespace DataImporter.ExcelToDatabaseService
{
    public class Program
    {
        public static IConfiguration _configuration { get; set; }
        public static ILifetimeScope AutofacContainer { get; set; }

        public static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
            try
            {
                Log.Information("Application Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    var connectionInfo = GetConnectionStringAndMigrationAssemblyName();
                    builder.RegisterModule(new WorkerModule(connectionInfo.connectionString,
                        connectionInfo.migrationAssemblyName));
                    builder.RegisterModule(new ImportingModule(connectionInfo.connectionString,
                        connectionInfo.migrationAssemblyName));
                    builder.RegisterModule(new CommonModule());

                })
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionInfo = GetConnectionStringAndMigrationAssemblyName();

                    services.AddAutoMapper(connectionInfo.migrationAssembly);
                    services.AddDbContext<ImportingDbContext>(option =>
                        option.UseSqlServer(connectionInfo.connectionString, m => m.MigrationsAssembly(connectionInfo.migrationAssemblyName)));
                    services.AddHostedService<Worker>();
                });

        public static (string connectionString, string migrationAssemblyName, Assembly migrationAssembly) GetConnectionStringAndMigrationAssemblyName()
        {
            var connectionStringName = "DefaultConnection";
            var connectionString = _configuration.GetConnectionString(connectionStringName);
            var migrationAssemblyName = typeof(Worker).Assembly.FullName;
            var migrationAssembly = typeof(Worker).Assembly;

            return (connectionString, migrationAssemblyName, migrationAssembly);
        }
    }
}
