using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;

namespace Repository
{
    public static class DependencyInjection
    {
        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate(); // Do migration automatically.
        }
    }
}
