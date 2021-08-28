using Application.Managers;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;

namespace Application
{
    public static class DependencyInjection
    {
        public static void ConfigureApplication(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationManager, AuthenticationManager>();
            services.AddTransient<INoteManager, NoteManager>();
        }
    }
}
