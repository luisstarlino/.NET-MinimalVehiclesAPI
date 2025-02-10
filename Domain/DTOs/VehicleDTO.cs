using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _NET_MinimalAPI.Domain.DTOs
{
    public record VehicleDTO
    {
        public string Name { get; set; } = default!;       
        public string Branch { get; set; } = default!;
        public int Year { get; set; } = default!;
    }
}
