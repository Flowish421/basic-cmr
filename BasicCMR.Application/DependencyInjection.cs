using BasicCMR.Application.Interfaces;
using BasicCMR.Application.Mappings;
using BasicCMR.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BasicCMR.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();
            services.AddAutoMapper(typeof(ApplicationMapper));
            return services;
        }
    }
}
