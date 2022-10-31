using Microsoft.EntityFrameworkCore;
using PracticaViamatica.Model;

namespace PracticaViamatica.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public  DbSet<Producto> producto { get; set; }
        public DbSet<Persona> persona { get; set; }
        public DbSet<Credenciales> credenciales { get; set; }
    }
}
