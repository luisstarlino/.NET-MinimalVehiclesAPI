using Microsoft.Extensions.Configuration;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.EntityFrameworkCore;


namespace _NET_MinimalAPI.test.Tests.IntegrationTests.Services
{
    /// <summary>
    /// USE A REAL DB TO CHECK THE PERSISTENCE
    /// </summary>
    [TestClass]
    public class AdministratorServiceIntegrationTest
    {

        [TestMethod]
        public void SaveAdminTest()
        {
            // --- Arrange
            var context = CreateTestContext();
            var admService = new AdministratorService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administrators");

            var adm = new Administrator()
            {
                Id = 1,
                Mail = "mail@mail.com",
                Password = "password",
                Profile = "adm"
            };

            // --- Act
            admService.Add(adm);

            // --- Assert
            Assert.AreEqual(1, admService.GetAll(1).Count());
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            // --- Arrange
            var context = CreateTestContext();
            var admService = new AdministratorService(context);
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administrators");

            var adm = new Administrator()
            {
                Id = 1,
                Mail = "mail@mail.com",
                Password = "password",
                Profile = "adm"
            };

            // --- Act
            admService.Add(adm);
            var foundAdm = admService.GetUniqueById(adm.Id);

            // --- Assert
            Assert.IsNotNull(foundAdm);
            Assert.AreEqual(1, foundAdm.Id);
        }

        private DBContext CreateTestContext()
        {
            // --- Builder settings
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();

            // --- Get Connection String 
            var connectionString = configuration.GetConnectionString("MySql");

            // --- Configure
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            return new DBContext(options);
        }
    }
}
