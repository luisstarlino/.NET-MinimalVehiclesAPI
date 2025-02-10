namespace _NET_MinimalAPI.Domain.Interfaces
{
    public interface IAdministratorService
    {
        Administrator? Login(LoginDTO loginDTO);

    }
}
