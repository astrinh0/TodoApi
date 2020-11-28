using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TodoApi.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Add DBContext postgres
            services.AddDbContext<TodoContext>(opt =>
                                               opt.UseNpgsql(configuration.GetConnectionString("DbContexto")));
        }
    }
}
