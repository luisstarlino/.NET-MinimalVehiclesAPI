using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;

namespace _NET_MinimalAPI.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);
        Administrator Add(Administrator administrator);
        List<Administrator> GetAll(int? page);
        Administrator? GetUniqueById(int id);


    }
}
