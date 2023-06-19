using Microsoft.EntityFrameworkCore;

namespace PocarStore.Models 
{
    public class Pocar
    {
          public int Id { get; set; }
          public string? Marca { get; set; }
          public string? Modelo { get; set; }
          public string? Placa { get; set; }
          public int Ano { get; set; }

    }

    class PocarDb : DbContext
{
    public PocarDb(DbContextOptions options) : base(options) { }
    public DbSet<Pocar> PocarC { get; set; } = null!;
}
}