using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace _NET_MinimalAPI.Domain.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly DBContext _dbContext;
        public AdministratorService(DBContext context)
        {
            _dbContext = context;
        }

        public Administrator Add(Administrator administrator)
        {
            _dbContext.Administrators.Add(administrator);
            _dbContext.SaveChanges();

            return administrator;
        }

        public List<Administrator> GetAll(int? page)
        {
            var query = _dbContext.Administrators.AsQueryable();

            var dataPerPage = 10;

            if (page > 1)
            {
                query = query.Skip(((int)page - 1) * dataPerPage).Take(dataPerPage);
            }

            return query.ToList();
        }

        public Administrator Login(LoginDTO loginDTO)
        {
            var admin = _dbContext.Administrators.Where(adm => adm.Mail.Equals(loginDTO.Mail) && adm.Password.Equals(loginDTO.Password)).FirstOrDefault();
            return admin;
        }

        public Administrator? GetUniqueById(int id)
        {
            return _dbContext.Administrators.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
