using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.test.Tests.Mocks.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace _NET_MinimalAPI.test
{
    public class SetupTest
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            // --- Add DB Test (using the appsettings from the .test project)
            builder.Services.AddDbContext<DBContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("MySql");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            // --- DI for the Mocked ServicesRegistra os serviços MOCADOS
            builder.Services.AddScoped<IAdministratorService, AdministratorServiceMock>();
            //builder.Services.AddScoped<IVehicleService, VehicleServiceTest>();

            //// Outros serviços essenciais
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
        }

        public static void ConfigureApp(WebApplication app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }
    }
}
