using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NET_MinimalAPI.test.Domain.Entities
{
    [TestClass]
    public class AdministratorTest
    {
        [TestMethod]
        public void TestProperties()
        {
            // --- Arrange
            var adm = new Administrator();

            // --- Act
            adm.Id = 1;
            adm.Password = "password";
            adm.Profile = "adm";
            adm.Mail = "mail@mail.com";

            // --- Assert
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("password", adm.Password);
            Assert.AreEqual("adm", adm.Profile);
            Assert.AreEqual("mail@mail.com", adm.Mail);
        }
    }
}
