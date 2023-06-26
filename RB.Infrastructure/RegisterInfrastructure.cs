using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RB.Application.Interfaces;
using RB.Infrastructure.Persistence;

namespace RB.Infrastructure
{
    public static class RegisterInfrastructure
    {
        public static IServiceCollection AddInfrastrucutre(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddDbContext<IRBDbContext, RBDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("RoomBooking"));
            });

            return services;
        }
    }
}
