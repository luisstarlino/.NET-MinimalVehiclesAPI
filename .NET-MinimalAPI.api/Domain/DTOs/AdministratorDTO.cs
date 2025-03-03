using _NET_MinimalAPI.Domain.Enuns;

namespace _NET_MinimalAPI.Domain.DTOs
{
    public class AdministratorDTO
    {
        public string Mail { get; set; } = default!;
        public string Password { get; set; } = default!;
        public ProfileENUM? Profile { get; set; } = default!;

    }
}
