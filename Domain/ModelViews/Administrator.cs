using _NET_MinimalAPI.Domain.Enuns;

namespace _NET_MinimalAPI.Domain.ModelViews
{
    public record AdministratorMV
    {
        public int Id { get; set; } = default!;
        public string Mail { get; set; } = default!;
        public string? Profile { get; set; } = default!;
    }

    public record AdministratorMVTk
    {
        public int Id { get; set; } = default!;
        public string Mail { get; set; } = default!;
        public string? Profile { get; set; } = default!;
        public string? Token { get; set; } = default!;
    }
}
