using _NET_MinimalAPI.Domain.Entities;

namespace _NET_MinimalAPI.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> GetAll(int? page = 1, string? name = null, string? year = null );
        Vehicle? GetUniqueById(int id);
        void Add(Vehicle vehicle);
        void Update(Vehicle vehicle);
        void Delete(Vehicle vehicle);

    }
}
