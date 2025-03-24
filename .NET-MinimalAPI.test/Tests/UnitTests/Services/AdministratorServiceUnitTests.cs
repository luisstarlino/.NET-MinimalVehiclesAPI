using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.test.Tests.Setup;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NET_MinimalAPI.test.Tests.UnitTests.Services
{
    [TestClass]
    public class AdministratorServiceUnitTests
    {
        private IAdministratorService _administratorService;

        [TestInitialize]
        public void Setup()
        {
            _administratorService = TestStartup.ServiceProvider!.GetRequiredService<IAdministratorService>();
        }

        [TestMethod]
        public void AddAdministrator_Should_AddSuccessfully()
        {
            // --- Arrange
            var admin = new Administrator { Id = 1, Mail = "test@mail.com", Password = "1234", Profile = "admin" };

            // --- Act
            _administratorService.Add(admin);
            var result = _administratorService.GetUniqueById(1);

            // --- Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}
