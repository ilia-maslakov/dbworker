using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using dbworker.Configure;
using AutoMapper;

namespace dbworker
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesBase.ConfigureServices(services);
            ConfigureServicesDbContext.ConfigureServices(services, Configuration);
            ConfigureServicesSwagger.ConfigureServices(services);
            ConfigureServicesControllers.ConfigureServices(services);
            ConfigureServicesMassTransit.ConfigureServices(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            ConfigureCommon.Configure(app, env, mapper);
            ConfigureEndpoints.Configure(app);
        }
    }
}
