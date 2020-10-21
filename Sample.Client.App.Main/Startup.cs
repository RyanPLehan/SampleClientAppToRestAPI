using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Client.App.Domain;
using Sample.Client.App.Infrastructure;

namespace Sample.Client.App.Main
{
    internal class Startup
    {
        public IServiceCollection Services { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        internal Startup()
        { }

        public void Initialize()
        {
            Services = new ServiceCollection();
            ConfigureServices(Services);
            ServiceProvider = Services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Build Configuation
            IConfiguration configuration = LoadConfiguration(GetConfigurationFileName());
            services.Configure<ApplicationOption>(configuration.GetSection("Application"));
            services.Configure<CustomerApiOption>(configuration.GetSection("CustomerApi"));
            services.Configure<AzureADOption>(configuration.GetSection("AzureAd"));

            // Add Infrastructure services
            services.AddSingleton<ICustomerApi, CustomerApiService>();

            // Add Domain Services
            services.AddSingleton<ICustomerService, CustomerService>();

            // Add Http Clients
            services.AddHttpClient<ICustomerApi, CustomerApiService>()
                    .ConfigureHttpClient((serviceProvider, httpClient) =>
                    {
                        httpClient.Timeout = TimeSpan.FromSeconds(30);
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    });
        }


        private string GetConfigurationFileName()
        {
            string envValue = null;

            // .net core defaults to production if no ASPNETCORE_ENVIRONMENT environment variable is set
            // this overriding .net cores default behvoir
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1
            // envValue = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
            //           System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
            //           String.Empty;

            return (String.IsNullOrWhiteSpace(envValue) ? "appsettings.local.json" : $"appsettings.{envValue}.json");
        }

        private IConfiguration LoadConfiguration(string appSettingsFileName)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ConfigurationBuilder builder = new ConfigurationBuilder();
            // builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.SetBasePath(assemblyPath);
            builder.AddJsonFile(appSettingsFileName, false, true);

            return builder.Build();
        }

        /*
        private ILoggerFactory CreateLoggerFactory(IConfiguration configuration)
        {
            LoggerConfiguration loggerConfig = new LoggerConfiguration();
            loggerConfig.ReadFrom.Configuration(configuration);

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(loggerConfig.CreateLogger());
            });

            return loggerFactory;
        }
        */
    }
}
