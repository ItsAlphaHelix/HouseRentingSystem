using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Infrastructure.Data.Repositories;

namespace HouseRentingSystem.Web.Extensions
{
    public static class HouseRentingServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHouseService, HouseService>();
            services.AddScoped<IAgentService, AgentsService>();

            return services;
        }
    }
}
