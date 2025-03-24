using _NET_MinimalAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NET_MinimalAPI.test.Tests.Mocks.Services
{
    /// <summary>
    /// MOCK IN MEMORY TO UNIT TESTS 
    /// </summary>
    public class AdministratorServiceMock : IAdministratorService
    {
        // --- Mock in memory
        List<Administrator> administratorInMemory = new List<Administrator>()
        {
            new Administrator { Id = 1, Mail = "memory@mail.com", Password = "password", Profile = "adm" }
        };

        public Administrator Add(Administrator administrator)
        {
            administrator.Id = administratorInMemory.Count() + 1;
            administratorInMemory.Add(administrator);
            return administrator;
        }

        public List<Administrator> GetAll(int? page)
        {
            return administratorInMemory;
        }

        public Administrator? GetUniqueById(int id)
        {
            var found = administratorInMemory.FirstOrDefault(x => x.Id == id);
            if (found != null) return found;
            return null;
        }

        public Administrator? Login(LoginDTO loginDTO)
        {
            var logged = administratorInMemory.FirstOrDefault(adm => adm.Mail == loginDTO.Mail && adm.Password == loginDTO.Password);
            if (logged != null) return logged;
            return null;
        }
    }
}
