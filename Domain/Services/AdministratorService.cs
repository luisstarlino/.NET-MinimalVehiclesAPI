using _NET_MinimalAPI.Domain.Interfaces;

namespace _NET_MinimalAPI.Domain.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly DBContext _dbContext;
        public AdministratorService(DBContext context)
        {
            _dbContext = context;
        }
        public Administrator Login(LoginDTO loginDTO)
        {
            var admin = _dbContext.Administrators.Where(adm => adm.Mail.Equals(loginDTO.Mail) && adm.Password.Equals(loginDTO.Password)).FirstOrDefault();
            return admin;
        }
    }
}
