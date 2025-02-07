using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace _NET_MinimalAPI.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DBContext _dbContext;

        public VehicleService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Vehicle vehicle)
        {
            _dbContext.Vehicles.Add(vehicle);
            _dbContext.SaveChanges();
            
        }

        public void Delete(Vehicle vehicle)
        {
            _dbContext.Vehicles.Remove(vehicle);
            _dbContext.SaveChanges();
        }

        public List<Vehicle> GetAll(int page = 1, string? name = null, string? year = null)
        {
            var query = _dbContext.Vehicles.AsQueryable();

            if(!String.IsNullOrEmpty(name) )
            {
                query = query.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name.ToLower()}%"));
            }

            var dataPerPage = 10;

            query = query.Skip((dataPerPage-1) * dataPerPage).Take(dataPerPage);

            return query.ToList();

        }

        public Vehicle? GetUniqueById(int id)
        {
            return _dbContext.Vehicles.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Vehicle vehicle)
        {
            _dbContext.Vehicles.Update(vehicle);
            _dbContext.SaveChanges();
        }
    }
}
