using _NET_MinimalAPI.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NET_MinimalAPI.test.Domain.Services
{
    [TestClass]
    public class AdministratorServiceTest
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



        private DBContext CreateTestContext ()
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
