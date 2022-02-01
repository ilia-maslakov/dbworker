using dbworker.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dbworker.Configure
{
    /// <summary>
    /// Configure controllers
    /// </summary>
    public static class ConfigureServicesDbContext
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            string consrt = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DBworkerContext>(o => o.UseNpgsql(consrt));
        }
    }
}
